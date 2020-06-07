using UnityEngine;
using UnityEngine.Tilemaps;
using static System.Math;

public class Util {
    // Returns a Vector3 representing the absolute-world-space location of the given tile relative to the given path.
    public static Vector3 tileToAbsolute(Vector2Int tile) {
        Tilemap path = Game.path;
        float absoluteX = tile.x * path.cellSize.x + path.transform.position.x + path.cellSize.x / 2;
        float absoluteY = -tile.y * path.cellSize.y + path.transform.position.y;
        return new Vector3 (absoluteX, absoluteY, 0.0f);
    }

    public static float tileXToAbsoluteX(float tileX) {
        Tilemap path = Game.path;
        return tileX * path.cellSize.x + path.transform.position.x + path.cellSize.x / 2;
    }

    public static float tileYToAbsoluteY(float tileY) {
        Tilemap path = Game.path;
        return -tileY * path.cellSize.y + path.transform.position.y;
    }

    // Returns true if the path tilemap contains a tile at the given (tileX, tileY).
    // Returns false otherwise.
    public static bool isPathTile(int tileX, int tileY) {
        Tilemap path = Game.path;
        TileBase[] pathTiles = Game.pathTiles;

        if (tileY < 0 || tileX < 0) return false;

        // Unity stores the Y-axis for the tilemap starting at the bottom, so we need to flip it here.
        // FIXME For some reason, this works for maps that have a tile at y=0 but those that don't need a "-1" instead of a "-2"
        tileY = path.size.y - tileY - 2;

        if (tileY < 0) return false;

        return pathTiles[tileX + tileY * path.cellBounds.size.x] != null;
    }

    // Returns true if given floats are "equal."  Or, in other words, within the tolerance range.
    public static bool equateFloats(float f1, float f2, float tolerance) {
        return Abs(f1 - f2) < tolerance;
    }
}