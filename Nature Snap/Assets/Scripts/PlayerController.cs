using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    public static int Direction;

    public float DEADZONE = 0.1f;
    public float moveSpeed;

    private Vector3 targetPosition;
    private TileBase[] pathTiles;
    private Vector2Int currentTile;
    public static Vector2Int targetTile;
    private float initialMoveSpeed;
    private bool isWalking = false;
    public bool MovementLocked = false;

    private Animator animator;
    private Text locationInfo;
    private Image minimapBlip;

    void Start() {
        initialMoveSpeed = moveSpeed;

        // Move player to start position.
        float startX = 0.0f;
        float startY = 0.0f;

        // TODO: UGLYYYYY
        if(Game.playerTileXFromSaveData != -1 && (Game.previousScene == "Title" || Game.previousScene == "ENTRY" || Game.previousScene == "Gallery")) {
            currentTile.x = targetTile.x = Game.playerTileXFromSaveData;
            currentTile.y = targetTile.y = Game.playerTileYFromSaveData;
            startX = Util.tileXToAbsoluteX(currentTile.x);
            startY = Util.tileYToAbsoluteY(currentTile.y);
            Direction = Directions.SOUTH;
        } else if (SceneManager.GetActiveScene().name == "Forest" && (Game.previousScene == "Title" || Game.previousScene == "ENTRY" || Game.previousScene == "Gallery")) {
            currentTile.x = targetTile.x = Globals.SceneEnteranceLocations.InitialSpawn.X;
            currentTile.y = targetTile.y = Globals.SceneEnteranceLocations.InitialSpawn.Y;
            startX = Util.tileXToAbsoluteX(currentTile.x);
            startY = Util.tileYToAbsoluteY(currentTile.y);
            Direction = Directions.SOUTH;
        } else if (SceneManager.GetActiveScene().name == "Arctic" && (Game.previousScene == "Title" || Game.previousScene == "ENTRY" || Game.previousScene == "Gallery")) {// This shouldn't run on release.
            currentTile.x = targetTile.x = Globals.SceneEnteranceLocations.ArcticFromForest.X;
            currentTile.y = targetTile.y = Globals.SceneEnteranceLocations.ArcticFromForest.Y;
            startX = Util.tileXToAbsoluteX(currentTile.x);
            startY = Util.tileYToAbsoluteY(currentTile.y);
            Direction = Directions.EAST;
        } else if (SceneManager.GetActiveScene().name == "Arctic" && Game.previousScene == "Forest") {
            currentTile.x = targetTile.x = Globals.SceneEnteranceLocations.ArcticFromForest.X;
            currentTile.y = targetTile.y = Globals.SceneEnteranceLocations.ArcticFromForest.Y;
            startX = Util.tileXToAbsoluteX(currentTile.x);
            startY = Util.tileYToAbsoluteY(currentTile.y);
            Direction = Directions.EAST;
        } else if (SceneManager.GetActiveScene().name == "Forest" && Game.previousScene == "Arctic") {
            currentTile.x = targetTile.x = Globals.SceneEnteranceLocations.ForestFromArctic.X;
            currentTile.y = targetTile.y = Globals.SceneEnteranceLocations.ForestFromArctic.Y;
            startX = Util.tileXToAbsoluteX(currentTile.x);
            startY = Util.tileYToAbsoluteY(currentTile.y);
            Direction = Directions.WEST;
        }
        transform.position = new Vector3(startX, startY, transform.position.z);
        targetPosition = transform.position;

        // Set up animator.
        animator = GetComponent<Animator>();
        animator.SetInteger("direction", Direction);

        // Set up UI.
        locationInfo = GameObject.Find("Location Information").GetComponent<Text>();
        minimapBlip = GameObject.Find("Minimap Blip").GetComponent<Image>();
    }

    void FixedUpdate() {
        // Don't move if movement is locked.
        if (MovementLocked)  {
            animator.SetBool("isWalking", false);
            return;
        }

        // Debug speed boost.
        if (Globals.Debug.DO_DEBUG && Input.GetKey(KeyCode.LeftShift)) {
            moveSpeed = initialMoveSpeed * 10;
        } else {
            moveSpeed = initialMoveSpeed;
        }

        // If we're standing in the center of the current tile, check for input and update the target position.
        if (Util.equateFloats(transform.position.x, targetPosition.x, 0.01f) && Util.equateFloats(transform.position.y, targetPosition.y, 0.01f)) {
            // Check if surrounding tiles are path tiles.
            bool canMoveEast  = Util.isPathTile(currentTile.x + 1, currentTile.y);
            bool canMoveNorth = Util.isPathTile(currentTile.x,     currentTile.y - 1);
            bool canMoveWest  = Util.isPathTile(currentTile.x - 1, currentTile.y);
            bool canMoveSouth = Util.isPathTile(currentTile.x,     currentTile.y + 1);

            // Get movement input.
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            float verticalAxis = Input.GetAxisRaw("Vertical");
            bool inputEast  = horizontalAxis >  DEADZONE;
            bool inputNorth = verticalAxis   >  DEADZONE;
            bool inputWest  = horizontalAxis < -DEADZONE;
            bool inputSouth = verticalAxis   < -DEADZONE;

            if (canMoveEast && inputEast) {          // Walking East
                isWalking = true;
                Direction = Directions.EAST;
                targetTile.x = ++currentTile.x;
                targetTile.y = currentTile.y;
            } else if (canMoveNorth && inputNorth) { // Walking North
                isWalking = true;
                Direction = Directions.NORTH;
                targetTile.x = currentTile.x;
                targetTile.y = --currentTile.y;
            } else if (canMoveWest && inputWest) {   // Walking West
                isWalking = true;
                Direction = Directions.WEST;
                targetTile.x = --currentTile.x;
                targetTile.y = currentTile.y;
            } else if (canMoveSouth && inputSouth) { // Walking South
                isWalking = true;
                Direction = Directions.SOUTH;
                targetTile.x = currentTile.x;
                targetTile.y = ++currentTile.y;
            } else {                                 // Idle
                isWalking = false;
                if (inputEast) {
                    Direction = Directions.EAST;
                } else if (inputNorth) {
                    Direction = Directions.NORTH;
                } else if (inputWest) {
                    Direction = Directions.WEST;
                } else if (inputSouth) {
                    Direction = Directions.SOUTH;
                }
            }
        }

        // Move character towards target position.
        targetPosition = Util.tileToAbsolute(targetTile);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Update animation
        animator.SetInteger("direction", Direction);
        animator.SetBool("isWalking", isWalking);

        // Detect scene changes
        // TODO: This will get messy.  See if it can be generalized.
        if (SceneManager.GetActiveScene().name == "Forest"
            && Util.equateFloats(transform.position.x, Util.tileXToAbsoluteX(Globals.SceneChangeLocations.ForestToArctic.X), 0.001f)
            && Util.equateFloats(transform.position.y, Util.tileYToAbsoluteY(Globals.SceneChangeLocations.ForestToArctic.Y), 0.001f)) {
             Game.switchScenes("Arctic");
        } else if (SceneManager.GetActiveScene().name == "Arctic"
                   && Util.equateFloats(transform.position.x, Util.tileXToAbsoluteX(Globals.SceneChangeLocations.ArcticToForest.X), 0.001f)
                   && Util.equateFloats(transform.position.y, Util.tileYToAbsoluteY(Globals.SceneChangeLocations.ArcticToForest.Y), 0.001f)) {
             Game.switchScenes("Forest");
        }

        // Update UI
        locationInfo.text = "Current Tile Location: (" + currentTile.x + ", " + currentTile.y + ")\n"
                          + "Player Location: (" + transform.position.x + ", " + transform.position.y + ")\n";

        // Update minimap
        // TODO: Try not to hard-code these values.
        minimapBlip.transform.localPosition = new Vector3(targetTile.x/2.0f - 47.75f, -targetTile.y/2.0f + 47.25f, 0.0f);
    }
}
