using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour {
    public GameObject followTarget;
    public float moveSpeed = 6.0f;
    public bool doLerp = false;
   
    private GameObject ui;
    private GameObject mousePointer;
    private GameObject player;
    private PlayerController playerController;
    private float width;
    private float height;
    private Tilemap terrain;
    private Vector3 targetPosition;
    private long timeOfTargetChange;
    private State state = State.INACTIVE;
    private enum State {INACTIVE, PRE_FLASH, FLASH, POST_FLASH}

    void Start() {
        ui = GameObject.Find("UI");
        mousePointer = GameObject.Find("Mouse Pointer");
        player = GameObject.Find("player");
        playerController = player.GetComponent<PlayerController>();
        height = gameObject.GetComponent<Camera>().orthographicSize;
        width = height * gameObject.GetComponent<Camera>().aspect;
        terrain = GameObject.Find("Terrain").GetComponent<Tilemap>();
    }

    void Update() {
        float x = followTarget.transform.position.x;
        float y = followTarget.transform.position.y;

        // Use terrain to clamp camera inside.
        x = Mathf.Clamp(x, width, terrain.localBounds.size.x - width);
        y = Mathf.Clamp(y, -(terrain.localBounds.size.y - height), -height);

        // To fix screen tearing.
        x += 0.0001f;
        y += 0.0001f;
        
        // Move camera
        targetPosition = new Vector3(x, y, transform.position.z);
        if (doLerp) {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        } else {
            transform.position = targetPosition;
        }

        // Flash animation.  TODO: Use an animator for this instead.
        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        long elapsed = now - timeOfTargetChange;
        if (state != State.INACTIVE) {
            if (state == State.PRE_FLASH && elapsed > 1000) {
                GameObject.Find("Camera Flash").GetComponent<CameraFlashController>().doFlash();

                state = State.FLASH;
            } else if (state == State.FLASH && elapsed > 2000) {
                followTarget = GameObject.Find("player");
                ui.SetActive(true);
                mousePointer.SetActive(true);

                state = State.POST_FLASH;
            } else  if (state == State.POST_FLASH && elapsed > 3000) {
                playerController.MovementLocked = false;
                doLerp = false;

                state = State.INACTIVE;
            }
        }
    }

    public void setTarget(string name, bool doLerp) {
        // Don't reset target if there's already one.
        if (followTarget.name != "player") return;

        this.doLerp = doLerp;
        ui.SetActive(false);
        mousePointer.SetActive(false);
        playerController.MovementLocked = true;
        state = State.PRE_FLASH;

        followTarget = GameObject.Find(name);
        timeOfTargetChange = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
