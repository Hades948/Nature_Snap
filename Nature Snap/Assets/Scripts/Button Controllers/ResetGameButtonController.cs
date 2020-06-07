using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGameButtonController : MonoBehaviour {
    public void OnButtonPressed() {
        Game.resetGameData();
    }
}
