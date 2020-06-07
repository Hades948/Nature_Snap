using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour {

    private CameraController cameraController;

    void Awake() {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    void OnMouseUp() {
        string name = gameObject.name;
        name = name.Substring(0, name.IndexOf(" "));
        cameraController.setTarget(name, true);

        name = name.Replace("_", " ");
        if (!Game.photosTaken.Contains(name)) {
            Game.photosTaken.Add(name);
            Game.saveGame(); // Autosave after every photo taken.
        }
    }

    void OnMouseEnter() {
        MouseCursorController.setCursorToCamera();
    }

    void OnMouseExit() {
        MouseCursorController.setCursorToDefault();
    }
}
