using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* A MonoBehavior script with a callback function to reset all of the game's save data.
*/
public class ResetGameButtonController : MonoBehaviour {
    public void OnButtonPressed() {
        Game.resetGameData();
    }
}
