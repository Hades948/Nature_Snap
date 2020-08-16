using UnityEngine;
using UnityEngine.Tilemaps;
using static System.Math;

/**
* A class that contains various utility functions for the game which are used by multiple scripts.
* TODO: Cleanup.  There is probably a better way to accomplish this for Unity.
*/
public class Util {
    // Returns a Vector3 representing the absolute-world-space location of the given tile relative to the given path.
    public static Vector3 tileToAbsolute(Vector2Int tile) {
        Tilemap path = Game.path;
        return new Vector3(tileXToAbsoluteX(tile.x), tileYToAbsoluteY(tile.y), 0.0f);
    }

    public static float tileXToAbsoluteX(int tileX) {
        Tilemap path = Game.path;
        return tileX * path.cellSize.x + path.transform.position.x + path.cellSize.x / 2;
    }

    public static float tileYToAbsoluteY(int tileY) {
        Tilemap path = Game.path;
        return -tileY * path.cellSize.y + path.transform.position.y;
    }

    public static Vector2Int absoluteToTile(Vector3 absolute) {
        Tilemap path = Game.path;
        return new Vector2Int(absoluteXToTileX(absolute.x), absoluteYToTileY(absolute.y));
    } 

    public static int absoluteXToTileX(float absoluteX) {
        Tilemap path = Game.path;
        return (int) Round((absoluteX - path.transform.position.x - path.cellSize.x/2) / path.cellSize.x);
    }

    public static int absoluteYToTileY(float absoluteY) {
        Tilemap path = Game.path;
        return (int) Round(-((absoluteY - path.transform.position.y) / path.cellSize.y));
    }

    // Returns true if the path tilemap contains a tile at the given (tileX, tileY).
    // Returns false otherwise.
    public static bool isPathTile(int tileX, int tileY) {
        Tilemap path = Game.path;

        // FIXME For some reason, the top tile is at -1 on y.
        if (tileX < 0 || tileY < -1) return false;

        // Unity stores the Y-axis for the tilemap starting at the bottom, so we need to flip it here.
        // FIXME For some reason, this works for maps that have a tile at y=0 but those that don't need a "-1" instead of a "-2"
        //       This bug could be related to the above bug as well.  For now, this is a reasonable work-around.
        tileY = path.size.y - tileY - 2;

        if (tileY < 0) return false;

        return Game.pathTiles[tileX + tileY * path.cellBounds.size.x] != null;
    }

    // Returns true if given floats are "equal."  Or, in other words, within the tolerance range.
    public static bool equateFloats(float f1, float f2, float tolerance) {
        return Abs(f1 - f2) < tolerance;
    }
}