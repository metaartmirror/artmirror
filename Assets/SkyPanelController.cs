using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SkyPanelController : NetworkBehaviour
{
    public GameObject obj;

    private NetworkVariable<float> red = new NetworkVariable<float>(1f);
    private NetworkVariable<float> green = new NetworkVariable<float>(1f);
    private NetworkVariable<float> blue = new NetworkVariable<float>(1f);

    private NetworkVariable<float> intensity = new NetworkVariable<float>();

    float _prevRed = 1f;
    float _prevGreen = 1f;
    float _prevBlue = 1f;
    float _prevIntensity = 1f;

    Light _light;
    Renderer _renderer;

    void Start()
    {
        _light = obj.GetComponent<Light>();
        _renderer = obj.GetComponent<Renderer>();
    }
    // public override void OnNetworkSpawn()
    // {
    //     _light = obj.GetComponent<Light>();
    //     _renderer = obj.GetComponent<Renderer>();
    // }

    public void ChangeRed(float value)
    {
        if (IsClient)
        {
            ChangeRedServerRpc(value);
        }
        else
        {
            red.Value = value;
            UpdateColor();
        }
    }

    [ServerRpc]
    void ChangeRedServerRpc(float value)
    {
        red.Value = value;
        UpdateColor();
    }

    public void ChangeBlue(float value)
    {
        if (IsClient)
        {
            ChangeBlueServerRpc(value);
        }
        else
        {
            blue.Value = value;
            UpdateColor();
        }
    }

    [ServerRpc]
    void ChangeBlueServerRpc(float value)
    {
        blue.Value = value;
        UpdateColor();
    }

    public void ChangeGreen(float value)
    {
        if (IsClient)
        {
            ChangeGreenServerRpc(value);
        }
        else
        {
            green.Value = value;
            UpdateColor();
        }
    }

    [ServerRpc]
    void ChangeGreenServerRpc(float value)
    {
        green.Value = value;
        UpdateColor();
    }

    public void ChangeIntensity(float value)
    {
        if (IsClient)
        {
            ChangeIntensityServerRpc(value);
        }
        else
        {
            intensity.Value = value;
            UpdateColor();
        }
    }

    [ServerRpc]
    void ChangeIntensityServerRpc(float value)
    {
        intensity.Value = value;
        UpdateColor();
    }

    private void UpdateColor()
    {
        _light.color = new Color(red.Value, green.Value, blue.Value);
        _renderer.material.SetVector("_EmissionColor", new Vector4(1f, 1f, 1f, 1f) * Mathf.Lerp(0, 10, intensity.Value / 10f));
    }

    private void Update()
    {
        if (_prevBlue != blue.Value)
        {
            _prevBlue = blue.Value;
            UpdateColor();
        }

        if (_prevGreen != green.Value)
        {
            _prevGreen = green.Value;
            UpdateColor();
        }

        if (_prevRed != red.Value)
        {
            _prevRed = red.Value;
            UpdateColor();
        }

        if (_prevIntensity != intensity.Value)
        {
            _prevIntensity = intensity.Value;
            UpdateColor();
        }
    }
}
