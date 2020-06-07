using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject followTarget;
    public float moveSpeed = 6.0f;
    public bool doLerp = false;
   
    private GameObject ui;
    private GameObject mousePointer;
    private GameObject player;
    private PlayerController playerController;
    private Vector3 targetPosition;
    private long timeOfTargetChange;
    private bool isFlashActive;

    void Start() {
        ui = GameObject.Find("UI");
        mousePointer = GameObject.Find("Mouse Pointer");
        player = GameObject.Find("player");
        playerController = player.GetComponent<PlayerController>();
    }

    void Update() {
        targetPosition = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);

        if (doLerp) {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        } else {
            transform.position = targetPosition;
        }

        // TODO: This is kind of ugly.  See if it can be cleaned up.
        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        long elapsed = now - timeOfTargetChange;
        if (followTarget.name != "player") {
            if (elapsed > 1000 && !isFlashActive) {
                isFlashActive = true;
                GameObject.Find("Camera Flash").GetComponent<CameraFlashController>().doFlash();
            }
            if (elapsed > 2000) {
                followTarget = GameObject.Find("player");
                isFlashActive = false;
                ui.SetActive(true);
                mousePointer.SetActive(true);
            }
        } else if (playerController.MovementLocked && elapsed > 3000) {
            playerController.MovementLocked = false;
            doLerp = false;
        }
    }

    public void setTarget(string name, bool doLerp) {
        // Don't reset target if there's already one.
        if (followTarget.name != "player") return;

        this.doLerp = doLerp;
        ui.SetActive(false);
        mousePointer.SetActive(false);
        playerController.MovementLocked = true;

        followTarget = GameObject.Find(name);
        timeOfTargetChange = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
