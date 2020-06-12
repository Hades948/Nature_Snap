using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxController : MonoBehaviour {
    private Text text;
    private const float SPEED = 10.0f;
    private State state = State.STATIONARY;
    private enum State { STATIONARY, SLIDE_IN, SLIDE_OUT };
    private Vector3 slideInPosition = new Vector3(0.0f, 60.0f, 0.0f);
    private Vector3 slideOutPosition = new Vector3(0.0f, -60.0f, 0.0f);

    void Start() {
        text = GetComponent<Text>();

        // Start text box outside of camera's view
        ((RectTransform) transform).anchoredPosition = slideOutPosition;
    }

    void Update() {
        // Temporary.
        if (Input.GetKey(KeyCode.I)) {
            state = State.SLIDE_IN;
        } else if (Input.GetKey(KeyCode.O)) {
            state = State.SLIDE_OUT;
        }

        // Lerp to desired position based on state.
        if (state == State.SLIDE_IN) {
            ((RectTransform) transform).anchoredPosition = Vector3.Lerp(((RectTransform) transform).anchoredPosition, slideInPosition, SPEED * Time.deltaTime);
        } else if (state == State.SLIDE_OUT) {
            ((RectTransform) transform).anchoredPosition = Vector3.Lerp(((RectTransform) transform).anchoredPosition, slideOutPosition, SPEED * Time.deltaTime);
        }
    }
}
