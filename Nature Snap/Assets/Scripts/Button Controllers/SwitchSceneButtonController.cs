using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
* A MonoBehavior script with a callback function to switch to a given scene.
*/
public class SwitchSceneButtonController : MonoBehaviour {
    /**
    * The scene to switch to.
    */
    public string sceneName;

    /**
    * Should the game be saved before this switch?
    */
    public bool saveGame;
    
    /**
    * Should the game be loaded before this switch?
    */
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
