using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* A MonoBehavior script with a callback function to quit the application.
*/
public class ExitGameButtonController : MonoBehaviour {
    public void OnButtonPress() {
        Application.Quit();
    }
}
