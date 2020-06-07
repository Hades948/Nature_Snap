using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToGameButtonController : MonoBehaviour {
    public void OnButtonPress() {
        Game.loadGame();
        Game.switchScenes(Game.previousScene);
    }
}
