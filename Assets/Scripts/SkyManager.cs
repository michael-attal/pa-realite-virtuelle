using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour
{
    public float skySpeed;
    private static readonly int Rotation = Shader.PropertyToID("_Rotation");

    void Update()
    {
        RenderSettings.skybox.SetFloat(Rotation, Time.time * skySpeed);
    }
}
