using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* A MonoBehavior script that loads the gallery scene.  This is needed to set certain properties based on whether certain
* pictures have been taken.
*/
public class GallerySceneLoader : MonoBehaviour {
    void Start() {
        foreach (Transform t in transform) {
            if (t.gameObject.name == "Panel") continue;
            if (t.gameObject.name == "ReturnToGameButton") continue;
            if (t.gameObject.name == "Gallery") continue;
            
            // For each of the images, set whether they should be active based on save data.
            t.Find("Image").gameObject.SetActive(Game.photosTaken.Contains(t.gameObject.name));
        }
    }
}
