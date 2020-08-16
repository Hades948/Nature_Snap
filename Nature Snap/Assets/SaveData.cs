using UnityEngine;
using System.Collections.Generic;

/**
* A serializable class that stores all saveable data within the game.
*/
[System.Serializable]
public class SaveData {
    public int playerTileX = -1;
    public int playerTileY = -1;

    public string areaName = "Forest";
    public List<string> photosTaken = new List<string>();
}