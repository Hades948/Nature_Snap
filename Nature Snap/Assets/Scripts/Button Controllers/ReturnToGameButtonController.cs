using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* A MonoBehavior script with a callback function to return to the previous scene.
* This can be used to return to the main game from a menu.
*/
public class ReturnToGameButtonController : MonoBehaviour {
    public void OnButtonPress() {
        Game.loadGame();
        Game.switchScenes(Game.previousScene);
    }
}
