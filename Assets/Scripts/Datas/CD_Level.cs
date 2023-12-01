using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CD_Level", menuName = "ParkingJam3D/CD_Level", order = 0)]
public class CD_Level : ScriptableObject
{
    public List<LevelData> Levels = new List<LevelData>();
}