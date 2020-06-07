using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryLoader : MonoBehaviour {
    void Start() {
        foreach (Transform t in transform) {
            if (t.gameObject.name == "Panel") continue;
            if (t.gameObject.name == "ReturnToGameButton") continue;
            if (t.gameObject.name == "Gallery") continue;
            
            t.Find("Image").gameObject.SetActive(Game.photosTaken.Contains(t.gameObject.name));
        }
    }
}
