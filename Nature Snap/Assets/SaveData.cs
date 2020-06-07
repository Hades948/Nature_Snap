using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData {
    public int playerTileX = Globals.SceneEnteranceLocations.InitialSpawn.X;
    public int playerTileY = Globals.SceneEnteranceLocations.InitialSpawn.Y;

    public string areaName = "Forest";
    public List<string> photosTaken = new List<string>();
}