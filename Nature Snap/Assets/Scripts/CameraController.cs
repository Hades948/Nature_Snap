using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
* A MonoBehavior script that controls the game's camera.
*/
public class CameraController : MonoBehaviour {
    /**
    * The GameObject that the camera should follow.
    */
    public GameObject followTarget;
    
    /**
    * The speed at which the camera will move.  This speed is later adjusted by Time.deltaTime.
    * Only has an effect if doLerp is true.
    */
    public float moveSpeed = 6.0f;

    /**
    * Should the camera gradually lerp to its target?
    * If false, camera will snap to target and moveSpeed will have no effect.
    */
    public bool doLerp = false;
   
    private GameObject ui;
    private GameObject mousePointer;
    private GameObject player;
    private PlayerController playerController;
    private float width;
    private float height;
    private Tilemap terrain;
    private Vector3 targetPosition;

    // Flash stuff
    // TODO: Move "Flash stuff" into an animator with callbacks if possible.
    public Animator flashAnimator;
    private long timeOfTargetChange;
    private State state = State.INACTIVE;
    private enum State {INACTIVE, PRE_FLASH, FLASH, POST_FLASH}

    void Start() {
        // Get handle on needed GameObjects.
        ui = GameObject.Find("UI");
        mousePointer = GameObject.Find("Mouse Pointer");
        player = GameObject.Find("player");
        playerController = player.GetComponent<PlayerController>();
        terrain = GameObject.Find("Terrain").GetComponent<Tilemap>();

        // Calculate the camera's size.
        height = gameObject.GetComponent<Camera>().orthographicSize;
        width = height * gameObject.GetComponent<Camera>().aspect;
    }

    void Update() {
        // Set up x, y variables to start at target's position.
        float x = followTarget.transform.position.x;
        float y = followTarget.transform.position.y;

        // Use terrain to clamp camera inside.
        x = Mathf.Clamp(x, width, terrain.localBounds.size.x - width);
        y = Mathf.Clamp(y, -(terrain.localBounds.size.y - height), -height);

        // To fix screen tearing.
        x += 0.0001f;
        y += 0.0001f;
        
        // Move camera.
        targetPosition = new Vector3(x, y, transform.position.z);
        if (doLerp) {
            // Lerp to target using moveSpeed.
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        } else {
            // Snap to target.
            transform.position = targetPosition;
        }

        // Flash animation
        // TODO: Move "Flash stuff" into an animator with callbacks if possible.
        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        long elapsed = now - timeOfTargetChange;
        // This is ugly; I know.  It needs to be moved to an animator if possible.
        if (state != State.INACTIVE) {
            if (state == State.FLASH && elapsed > 2000) {
                followTarget = GameObject.Find("player");

                state = State.POST_FLASH;
            } else  if (state == State.POST_FLASH && elapsed > 3000) {
                doLerp = false;
                ui.SetActive(true);
                mousePointer.SetActive(true);
                playerController.MovementLocked = false;

                state = State.INACTIVE;
            }
        }
    }

    /**
    * Set a new target for the camera. This should probably be an animal as the camera will automatically move back
    * to the player after the flash animation.
    */
    public void setTarget(string name, bool doLerp) {
        // Don't reset target if there's already one.  This prevents graphical glitches when the player clicks an animal
        // before the animation is finished.
        if (followTarget.name != "player") return;

        // Set properties for the lerp and flash animations.
        this.doLerp = doLerp;
        ui.SetActive(false);
        mousePointer.SetActive(false);
        playerController.MovementLocked = true;
        state = State.FLASH;
        flashAnimator.SetTrigger("Do Flash");

        followTarget = GameObject.Find(name);

        timeOfTargetChange = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
