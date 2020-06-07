using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButtonController : MonoBehaviour {
    public void OnButtonPress() {
        Game.loadGame();

        Game.switchScenes(Game.areaNameFromSaveData);
    }
}
