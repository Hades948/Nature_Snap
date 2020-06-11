using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapSceneLoader : MonoBehaviour {
    public Sprite forestMap;
    public Sprite arcticMap;

    void Start() {
        GameObject map = GameObject.Find("Map");
        GameObject name = GameObject.Find("Name");
        GameObject playerBlip = GameObject.Find("Player Blip");

        // Set map and activate animal blips
        if (Game.previousScene == "Forest") {
            map.GetComponent<Image>().sprite = forestMap;
        } else if (Game.previousScene == "Arctic") {
            map.GetComponent<Image>().sprite = arcticMap;
            
        }

        // Set map name label
        name.GetComponent<Text>().text = Game.previousScene;

        // Place player blip
        float MAP_WIDTH = ((RectTransform) map.transform).rect.width;
        float MAP_HEIGHT = ((RectTransform) map.transform).rect.height;
        const float MAP_BOARDER_WIDTH = 2.0f;
        const float MAP_BOARDER_HEIGHT = 2.0f;
        float blipX =   Game.playerTilePositionFromSaveData.x - MAP_WIDTH/2  + MAP_BOARDER_WIDTH;
        float blipY = -(Game.playerTilePositionFromSaveData.y - MAP_HEIGHT/2 + MAP_BOARDER_HEIGHT);
        playerBlip.transform.localPosition = new Vector3(blipX, blipY, 0.0f);

        // Activate animal blips
        foreach (string animal in Game.forestAnimalRegistry) {
            GameObject.Find(animal + " Blip").SetActive(Game.previousScene == "Forest" ? Game.photosTaken.Contains(animal) : false);
        }
        foreach (string animal in Game.arcticAnimalRegistry) {
            GameObject.Find(animal + " Blip").SetActive(Game.previousScene == "Arctic" ? Game.photosTaken.Contains(animal) : false);
        }

        // Activate area blips
        GameObject.Find("Arctic From Forest Blip").SetActive(Game.previousScene == "Forest");
        GameObject.Find("Farm From Forest Blip").SetActive(Game.previousScene == "Forest");
        GameObject.Find("Forest From Arctic Blip").SetActive(Game.previousScene == "Arctic");
    }
}
