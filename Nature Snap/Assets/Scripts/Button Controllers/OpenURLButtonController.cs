using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURLButtonController : MonoBehaviour {
    public string url;
    public void OnButtonPress() {
        Application.OpenURL(url);
    }
}
