using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

/**
* A MonoBehavior script that acts as a game controller.
* TODO: Cleanup.  This could all probably be cleaned up at least a bit.
*/
public class Game : MonoBehaviour {
    public static Tilemap path;
    public static TileBase[] pathTiles;
    public static string previousScene = "Title";
    public Sprite ForestMinimapSource, ArcticMinimapSource;
    public static Vector2Int playerTilePositionFromSaveData = new Vector2Int(-1, -1);
    public static string areaNameFromSaveData = "Forest";
    public static List<string> photosTaken = new List<string>();
    public static string[] forestAnimalRegistry = new string[] { "Bear", "Deer", "Moose", "Squirrel", "Wolf", "Woodpecker" };
    public static string[] arcticAnimalRegistry = new string[] { "Arctic Fox", "Arctic Hare", "Penguin", "Polar Bear", "Snowy Owl" };

    /**
    * Switch to any scene by name.
    */
    public static void switchScenes(string sceneName) {
        previousScene = SceneManager.GetActiveScene().name;
        GameObject.Find("Black Fade").GetComponent<BlackFadeController>().fadeToScene(sceneName);
    }

    /**
    * Save all of the current save data to the save file.
    */
    public static void saveGame() {
        SaveData data = new SaveData();
        data.playerTileX = PlayerController.targetTile.x;
        data.playerTileY = PlayerController.targetTile.y;
        data.areaName = SceneManager.GetActiveScene().name;
        data.photosTaken = photosTaken;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
        bf.Serialize(file, data);
        file.Close();
    }

    /**
    * Reset all of the game's save data.
    */
    public static void resetGameData() {
        SaveData data = new SaveData();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
        bf.Serialize(file, data);
        file.Close();
    }

    /**
    * Load all of the game's save data from the save file.
    */
    public static void loadGame() {
        if (File.Exists(Application.persistentDataPath + "/save.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            playerTilePositionFromSaveData = new Vector2Int(data.playerTileX, data.playerTileY);
            areaNameFromSaveData = data.areaName;
            photosTaken = data.photosTaken;
        } else {
            playerTilePositionFromSaveData = new Vector2Int(-1, -1);
            areaNameFromSaveData = "Forest";
        }
    }

    /**
    * Autosave game when the it's closed.
    */
    void OnApplicationQuit() {
        saveGame();
    }

    void Start() {
        // Get new path information.
        path = GameObject.Find("Path").GetComponent<Tilemap>();
        if (path) {
            BoundsInt bounds = path.cellBounds;
            pathTiles = path.GetTilesBlock(bounds);
        }
    }
}