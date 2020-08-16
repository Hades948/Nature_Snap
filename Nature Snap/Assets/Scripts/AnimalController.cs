using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* A MonoBehavior script that controls functionality related to the animals.
* Really, it controls the mouse cursor and taking the picture of the animal.
* Not really the best in terms of software design but it seems like the most reasonable approach.
*/
public class AnimalController : MonoBehaviour {

    private CameraController cameraController;

    void Awake() {
        // Get handle to camera controller to use later on.
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    void OnMouseUp() {
        // At this point, the cursor is over this animal.  That means a picture can be taken.

        // Get the name of this animal and set it as the camera's new target.
        string name = gameObject.name;
        name = name.Substring(0, name.IndexOf(" "));
        cameraController.setTarget(name, true);

        // Save the fact that this animal has had its picture taken.
        name = name.Replace("_", " ");
        if (!Game.photosTaken.Contains(name)) {
            Game.photosTaken.Add(name);
            Game.saveGame(); // Autosave after every photo taken.
        }
    }

    void OnMouseEnter() {
        // Set cursor image to the camera image.
        MouseCursorController.setCursorToCamera();
    }

    void OnMouseExit() {
        // Set cursor image to the arrow image.
        MouseCursorController.setCursorToDefault();
    }
}
