using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject followTarget;
    public float moveSpeed = 6.0f;
    public bool doLerp = false;
    
    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        targetPosition = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);

        if (doLerp) {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        } else {
            transform.position = targetPosition;
        }
        
    }
}
