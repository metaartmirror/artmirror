using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class ObjectMenuController : NetworkBehaviour
{
    public GameObject obj;

    [Header("Settings")]
    public GameObject marker;
    public GameObject objectPanel;

    NetworkVariable<float> intensity = new NetworkVariable<float>(10f);
    NetworkVariable<float> range = new NetworkVariable<float>(10f);

    float _prevIntensity = 10f;
    float _prevRange = 10f;

    Light _light;
    Renderer _renderer;

    private void Start()
    {
        objectPanel.SetActive(false);
        _light = obj.GetComponent<Light>();
        _renderer = obj.GetComponent<Renderer>();
    }

    private void Update()
    {
        if (_prevIntensity != intensity.Value)
        {
            _light.intensity = intensity.Value;
            _renderer.material.SetVector("_EmissionColor", new Vector4(1f, 1f, 1f, 1f) * Mathf.Lerp(0, 10, intensity.Value/10f));
            _prevIntensity = intensity.Value;
        }

        if (_prevRange != range.Value)
        {
            _light.range = range.Value;
            _prevRange = range.Value;
        }
    }

    // void FixedUpdate()
    // {
    //     // Bit shift the index of the layer (8) to get a bit mask
    //     int layerMask = 1 << 8;

    //     // This would cast rays only against colliders in layer 8.
    //     // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
    //     //layerMask = ~layerMask;

    //     RaycastHit hit;
    //     // Does the ray intersect any objects excluding the player layer
    //     if (Physics.Raycast(transform.position, -Vector3.up, out hit, Mathf.Infinity, layerMask))
    //     {
    //         Debug.DrawRay(transform.position, -Vector3.up * hit.distance, Color.yellow);
    //         //Debug.Log("Did Hit");

    //         marker.transform.position = hit.point;
    //     }
    //     else
    //     {
    //         Debug.DrawRay(transform.position, -Vector3.up * 1000, Color.white);
    //         //Debug.Log("Did not Hit");
    //     }
    // }

    public void ChangeIntensity(float value)
    {
        if (IsClient)
        {
            ChangeIntensityServerRpc(value * 10);
        }
        else
        {
            intensity.Value = value * 10;
        }
    }

    [ServerRpc]
    void ChangeIntensityServerRpc(float value)
    {
        intensity.Value = value;
    }

    public void ChangeRange(float value)
    {
        if (IsClient)
        {
            ChangeRangeServerRpc(value * 10);
        }
        else
        {
            range.Value = value * 10;
        }
    }

    [ServerRpc]
    void ChangeRangeServerRpc(float value)
    {
        range.Value = value;
    }

    public void RemoveObject()
    {
        if (IsClient)
        {
            RemoveObjectServerRpc();
        }
        else
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    [ServerRpc]
    void RemoveObjectServerRpc()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
