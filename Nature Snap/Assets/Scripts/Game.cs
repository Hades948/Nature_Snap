using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

public class Game : MonoBehaviour {
    public static Tilemap path;
    public static TileBase[] pathTiles;
    public static string previousScene = "ENTRY";
    public Sprite ForestMinimapSource, ArcticMinimapSource;

    public static int playerTileXFromSaveData = -1;
    public static int playerTileYFromSaveData = -1;
    public static string areaNameFromSaveData = "Forest";
    public static List<string> photosTaken = new List<string>();

    public static void switchScenes(string sceneName) {
        previousScene = SceneManager.GetActiveScene().name;
        GameObject.Find("Black Fade").GetComponent<BlackFadeController>().fadeToScene(sceneName);
    }

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

    public static void resetGameData() {
        SaveData data = new SaveData();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
        bf.Serialize(file, data);
        file.Close();
    }

    public static void loadGame() {
        if (File.Exists(Application.persistentDataPath + "/save.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            playerTileXFromSaveData = data.playerTileX;
            playerTileYFromSaveData = data.playerTileY;
            areaNameFromSaveData = data.areaName;
            photosTaken = data.photosTaken;
        } else {
            playerTileXFromSaveData = -1;
            playerTileYFromSaveData = -1;
            areaNameFromSaveData = "Forest";
        }
    }

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

        // Set proper minimap.
        Image minimap = GameObject.Find("Minimap").GetComponent<Image>();
        if (SceneManager.GetActiveScene().name == "Forest") {
            minimap.sprite = ForestMinimapSource;
        } else if (SceneManager.GetActiveScene().name == "Arctic") {
            minimap.sprite = ArcticMinimapSource;
        }
    }
}