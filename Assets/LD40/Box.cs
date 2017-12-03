using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D collision) {
        //Debug.LogFormat("{0} collided with {1}", this, collision.gameObject);
        if(collision.gameObject.name.StartsWith("Ground")) {
            Destroy(gameObject);
        }
    }
}
