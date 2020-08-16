using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* A MonoBehavior script with a callback function to open a given URL.
*/
public class OpenURLButtonController : MonoBehaviour {
    /** 
    * The URL to open.
    */
    public string url;

    public void OnButtonPress() {
        Application.OpenURL(url);
    }
}
