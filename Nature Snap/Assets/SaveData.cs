using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData {
    public int playerTileX = -1;
    public int playerTileY = -1;

    public string areaName = "Forest";
    public List<string> photosTaken = new List<string>();
}