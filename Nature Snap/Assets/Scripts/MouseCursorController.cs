using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseCursorController : MonoBehaviour {
    // TODO: Make this a singleton.
    public Sprite cameraCursor, defaultCursor;
    public static Sprite cameraCursorSprite, defaultCursorSprite;
    private static Sprite activeCursor;

    private static float offsetX, offsetY;

    public static void setCursorToCamera() {
        activeCursor = cameraCursorSprite;
        offsetX =  0.00f;
        offsetY = -0.01f;
    }
    
    public static void setCursorToDefault() {
        activeCursor = defaultCursorSprite;
        offsetX =  0.08f;
        offsetY = -0.08f;
    }

    void Start() {
        Cursor.visible = false;
        cameraCursorSprite = cameraCursor;
        defaultCursorSprite = defaultCursor;
        activeCursor = defaultCursor;
        offsetX =  0.08f;
        offsetY = -0.08f;
        GetComponent<SpriteRenderer>().sprite = activeCursor;
    }

    void Update() {
        Cursor.visible = false;
        GetComponent<SpriteRenderer>().sprite = activeCursor;

        float x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        transform.position = new Vector3(x + offsetX, y + offsetY, 0.0f);

        string sceneName = SceneManager.GetActiveScene().name; 
        if (sceneName == "Title" || sceneName == "Gallery") {
            transform.localScale = new Vector3(16.0f, 16.0f, 1.0f);
            transform.position = new Vector3(x + offsetX + 1.2f, y + offsetY- 1.2f, 0.0f);
        } else if (sceneName == "Credits" || sceneName == "Settings" || sceneName == "Map") {
            transform.localScale = new Vector3(3.8f, 3.8f, 1.0f);
            transform.position = new Vector3(x + offsetX + 0.23f, y + offsetY - 0.23f, 0.0f);
        } else {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            transform.position = new Vector3(x + offsetX, y + offsetY, 0.0f);
        }
    }
}
