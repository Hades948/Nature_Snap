using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {
    private const int EAST  = 0;
    private const int NORTH = 1;
    private const int WEST  = 2;
    private const int SOUTH = 3;
    private int direction = SOUTH;

    public float DEADZONE = 0.1f;
    public float moveSpeed;
    public Vector2Int startTile;

    private Vector3 targetPosition;
    private Tilemap path;
    private TileBase[] pathTiles;
    private Vector2Int currentTile;
    private Vector2Int targetTile;
    private float initialMoveSpeed;
    private bool isWalking = false;

    private Animator animator;

    void Start() {
        // Get path information.
        path = GameObject.Find("Path").GetComponent<Tilemap>();
        BoundsInt bounds = path.cellBounds;
        pathTiles = path.GetTilesBlock(bounds);

        // Set initial position values.
        currentTile.x = targetTile.x = startTile.x;
        currentTile.y = targetTile.y = startTile.y;
        initialMoveSpeed = moveSpeed;

        // Move player to start position.
        transform.position = tileToAbsolute(startTile);
        targetPosition = transform.position;

        // Set up animator.
        animator = GetComponent<Animator>();
        animator.SetInteger("direction", direction);
    }

    void FixedUpdate() {
        // TODO See if we can have a debug variable for this.
        // Debug speed boost.
        if (Input.GetKey(KeyCode.LeftShift)) {
            //moveSpeed = initialMoveSpeed * 10;
        } else {
            moveSpeed = initialMoveSpeed;
        }

        // If we're standing in the center of the current tile, check for input and update the target position.
        if (transform.position == targetPosition) {
            // Check if surrounding tiles are path tiles.
            bool canMoveEast  = isPathTile(currentTile.x + 1, currentTile.y);
            bool canMoveNorth = isPathTile(currentTile.x,     currentTile.y - 1);
            bool canMoveWest  = isPathTile(currentTile.x - 1, currentTile.y);
            bool canMoveSouth = isPathTile(currentTile.x,     currentTile.y + 1);

            // Get movement input.
            float horizontalAxis = Input.GetAxisRaw("Horizontal");
            float verticalAxis = Input.GetAxisRaw("Vertical");
            bool inputEast  = horizontalAxis >  DEADZONE;
            bool inputNorth = verticalAxis   >  DEADZONE;
            bool inputWest  = horizontalAxis < -DEADZONE;
            bool inputSouth = verticalAxis   < -DEADZONE;

            if (canMoveEast && inputEast) {          // Walking East
                isWalking = true;
                direction = EAST;
                targetTile.x = ++currentTile.x;
                targetTile.y = currentTile.y;
            } else if (canMoveNorth && inputNorth) { // Walking North
                isWalking = true;
                direction = NORTH;
                targetTile.x = currentTile.x;
                targetTile.y = --currentTile.y;
            } else if (canMoveWest && inputWest) {   // Walking West
                isWalking = true;
                direction = WEST;
                targetTile.x = --currentTile.x;
                targetTile.y = currentTile.y;
            } else if (canMoveSouth && inputSouth) { // Walking South
                isWalking = true;
                direction = SOUTH;
                targetTile.x = currentTile.x;
                targetTile.y = ++currentTile.y;
            } else {                                 // Idle
                isWalking = false;
                if (inputEast) {
                    direction = EAST;
                } else if (inputNorth) {
                    direction = NORTH;
                } else if (inputWest) {
                    direction = WEST;
                } else if (inputSouth) {
                    direction = SOUTH;
                }
            }
        }

        // Move character towards target position.
        targetPosition = tileToAbsolute(targetTile);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Update animation
        animator.SetInteger("direction", direction);
        animator.SetBool("isWalking", isWalking);
    }

    // Returns true if the path tilemap contains a tile at the given (tileX, tileY).
    // Returns false otherwise.
    private bool isPathTile(int tileX, int tileY) {
        // Unity stores the Y-axis for the tilemap starting at the bottom, so we need to flip it here.
        tileY = path.size.y - tileY - 1;

        return pathTiles[tileX + tileY * path.cellBounds.size.x] != null;
    }

    // Returns a Vector3 representing the absolute-world-space location of the given tile.
    private Vector3 tileToAbsolute(Vector2Int tile) {
        float absoluteX = tile.x * path.cellSize.x + path.transform.position.x + path.cellSize.x / 2;
        float absoluteY = -tile.y * path.cellSize.y + path.transform.position.y;
        return new Vector3 (absoluteX, absoluteY, 0.0f);
    }
}
