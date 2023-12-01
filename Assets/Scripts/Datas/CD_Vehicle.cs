using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CD_Vehicle", menuName = "ParkingJam3D/CD_Vehicle", order = 0)]
public class CD_Vehicle : ScriptableObject
{
    [FormerlySerializedAs("VehicleDataData")] public VehicleData VehicleData;
}