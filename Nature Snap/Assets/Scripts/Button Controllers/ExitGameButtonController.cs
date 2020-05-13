using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameButtonController : MonoBehaviour {
    public void OnButtonPress() {
        Application.Quit();
    }
}
