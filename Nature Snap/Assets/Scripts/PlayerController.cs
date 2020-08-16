using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

/**
* A MonoBehavior script that controls the player.
*/
public class PlayerController : MonoBehaviour {
    public static Direction direction;

    public float DEADZONE = 0.1f;
    public float moveSpeed;

    private Vector3 targetPosition;
    private TileBase[] pathTiles;
    private Vector2Int currentTile;
    public static Vector2Int targetTile; // TODO: Find another way to make this visible.
    private float initialMoveSpeed;
    private bool isWalking = false;
    public bool MovementLocked = false;
    private GameObject sceneChangeLocations;
    
    private Animator animator;
    private Text locationInfo;

    void Start() {
        initialMoveSpeed = moveSpeed;

        // Choose start location
        float startX = 0.0f;
        float startY = 0.0f;
        Vector2Int spawn = Vector2Int.zero;
        string currentScene = SceneManager.GetActiveScene().name;
        if (Game.previousScene == "Title" || Game.previousScene == "Gallery" || Game.previousScene == "Map") {
            if (Game.playerTilePositionFromSaveData.x == -1) { // Save data position not found.
                if (currentScene == "Forest") {
                    spawn = Util.absoluteToTile(GameObject.Find("Initial Spawn").transform.position);
                } else if (currentScene == "Arctic") { // For debug purposes only.
                    spawn = Util.absoluteToTile(GameObject.Find("Forest Spawn").transform.position);
                } else if (currentScene == "Farm") { // For debug purposes only.
                    spawn = Util.absoluteToTile(GameObject.Find("Forest Spawn").transform.position);
                }
            } else { // Load from save data
                spawn = Game.playerTilePositionFromSaveData;
            }
            direction = Direction.SOUTH;
        } else { // Switching between levels
            GameObject spawnLocation = GameObject.Find(Game.previousScene + " Spawn");
            spawn = Util.absoluteToTile(spawnLocation.transform.position);
            direction = spawnLocation.GetComponent<DirectionProperty>().direction;
        }
        currentTile.x = targetTile.x = spawn.x;
        currentTile.y = targetTile.y = spawn.y;
        startX = Util.tileXToAbsoluteX(currentTile.x);
        startY = Util.tileYToAbsoluteY(currentTile.y);
        transform.position = new Vector3(startX, startY, transform.position.z);
        targetPosition = transform.position;
        
        sceneChangeLocations = GameObject.Find("Scene Change Locations");

        // Set up animator.
        animator = GetComponent<Animator>();
        animator.SetInteger("direction", (int) direction);

        // Set up UI.
        locationInfo = GameObject.Find("Location Information").GetComponent<Text>();
    }

    void FixedUpdate() {
        // Don't move if movement is locked.
        if (MovementLocked) {
            animator.SetBool("isWalking", false);
            return;
        }

        // Debug speed boost.
        if (Globals.DO_DEBUG && Input.GetKey(KeyCode.LeftShift)) {
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
                direction = Direction.EAST;
                targetTile.x = ++currentTile.x;
                targetTile.y = currentTile.y;
            } else if (canMoveNorth && inputNorth) { // Walking North
                isWalking = true;
                direction = Direction.NORTH;
                targetTile.x = currentTile.x;
                targetTile.y = --currentTile.y;
            } else if (canMoveWest && inputWest) {   // Walking West
                isWalking = true;
                direction = Direction.WEST;
                targetTile.x = --currentTile.x;
                targetTile.y = currentTile.y;
            } else if (canMoveSouth && inputSouth) { // Walking South
                isWalking = true;
                direction = Direction.SOUTH;
                targetTile.x = currentTile.x;
                targetTile.y = ++currentTile.y;
            } else {                                 // Idle
                isWalking = false;
                if (inputEast) {
                    direction = Direction.EAST;
                } else if (inputNorth) {
                    direction = Direction.NORTH;
                } else if (inputWest) {
                    direction = Direction.WEST;
                } else if (inputSouth) {
                    direction = Direction.SOUTH;
                }
            }
        }

        // Move character towards target position.
        targetPosition = Util.tileToAbsolute(targetTile);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Update animation
        animator.SetInteger("direction", (int) direction);
        animator.SetBool("isWalking", isWalking);

        // Detect scene changes
        foreach (Transform t in sceneChangeLocations.transform) {
            if (Util.equateFloats(transform.position.x, t.position.x, 0.001f) && Util.equateFloats(transform.position.y, t.position.y, 0.001f)) {
                string sceneName = t.gameObject.name.Substring(0, t.gameObject.name.IndexOf(" "));
                MovementLocked = true;
                Game.switchScenes(sceneName);
            }
        }

        // Update UI
        locationInfo.text = "Current Tile Location: (" + currentTile.x + ", " + currentTile.y + ")\n"
                          + "Player Location: (" + transform.position.x + ", " + transform.position.y + ")\n";
    }
}
