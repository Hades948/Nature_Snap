using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSceneButtonController : MonoBehaviour {
    public string sceneName;
    public bool saveGame;
    public bool loadGame;
    public void OnButtonPress() {
        if (saveGame) {
            Game.saveGame();
        }
        if (loadGame) {
            Game.loadGame();
        }
        Game.switchScenes(sceneName);
    }
}
