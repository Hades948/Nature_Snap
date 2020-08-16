using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* A MonoBehavior script that wraps a Direction property to be attached to GameObjects.
* Noteably used to specify which direction the player should face when entering an area.
*/
public class DirectionProperty : MonoBehaviour {
    public Direction direction;
}
