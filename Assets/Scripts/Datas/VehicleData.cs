using System;
using Unity.Mathematics;

[Serializable]
public struct VehicleData
{
    public VehicleMovementData MovementData;
    public VehiclePhysicData PhysicData;
}

[Serializable]
public struct VehicleMovementData
{
    public float VehicleSpeed;
    public float VehicleAcceleration;
    public float SidewaysSpeed;

    public VehicleMovementData(float vehicleSpeed, float vehicleAcceleration, float sidewaysSpeed)
    {
        VehicleSpeed = vehicleSpeed;
        VehicleAcceleration = vehicleAcceleration;
        SidewaysSpeed = sidewaysSpeed;
    }
}
    
[Serializable]
public struct VehiclePhysicData
{
    public float3 ForceParameters;
    public float2 CameraShake;
    public float3 BarrierOpenParameters;
    public float3 BarrierCloseParameters;

    public VehiclePhysicData(float3 forceParameters, float2 cameraShake, float3 barrierOpenParameters, float3 barrierCloseParameters)
    {
        ForceParameters = forceParameters;
        CameraShake = cameraShake;
        BarrierOpenParameters = barrierOpenParameters;
        BarrierCloseParameters = barrierCloseParameters;
    }
}