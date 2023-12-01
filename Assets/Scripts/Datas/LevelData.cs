using System;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct LevelData
{
    public CameraData CameraData;
}

[Serializable]
public struct CameraData
{
    public float3 CameraPosition;
    public float3 CameraRotation;

    public CameraData(float3 cameraPosition, float3 cameraRotation)
    {
        CameraPosition = cameraPosition;
        CameraRotation = cameraRotation;
    }
}