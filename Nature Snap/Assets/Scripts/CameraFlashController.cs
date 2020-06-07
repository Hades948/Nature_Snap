using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlashController : MonoBehaviour {
    private Vector3 scaleStart;
    private Vector3 scaleEnd;
    private Color colorStart;
    private Color colorEnd;

    private const float ZOOM_SPEED = 30.0f;
    private const float COLOR_SPEED = 10.0f;
    private State state;
    private enum State {
        INACTIVE, SCALE_IN, SCALE_OUT
    }

    public void Start() {
        scaleStart = new Vector3(0.25f, 0.26f, 1.0f);
        scaleEnd = new Vector3(0.24f, 0.25f, 1.0f);
        colorStart = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        colorEnd = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        transform.localScale = scaleStart;
        gameObject.GetComponent<SpriteRenderer>().color = colorStart;
        state = State.INACTIVE;
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    public void Update() {
        if (state == State.INACTIVE) return;

        if (state == State.SCALE_IN) {
            transform.localScale = Vector3.Lerp(transform.localScale, scaleEnd, ZOOM_SPEED * Time.deltaTime);
            gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(gameObject.GetComponent<SpriteRenderer>().color, colorEnd, COLOR_SPEED * Time.deltaTime);
            if (transform.localScale == scaleEnd) {
                state = State.SCALE_OUT;
            }
        } else if (state == State.SCALE_OUT) {
            transform.localScale = Vector3.Lerp(transform.localScale, scaleStart, ZOOM_SPEED * Time.deltaTime);
            gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(gameObject.GetComponent<SpriteRenderer>().color, colorStart, COLOR_SPEED * Time.deltaTime);
            if (transform.localScale == scaleStart) {
                state = State.INACTIVE;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
    
    public void doFlash() {
        gameObject.GetComponent<Renderer>().enabled = true;
        state = State.SCALE_IN;
    }
}
