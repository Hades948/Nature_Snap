using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* A MonoBehavior script with a callback function to switch scenes to the correct area.
*/
public class StartGameButtonController : MonoBehaviour {
    public void OnButtonPress() {
        Game.loadGame();
        Game.switchScenes(Game.areaNameFromSaveData);
    }
}
