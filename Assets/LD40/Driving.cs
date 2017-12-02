using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driving : MonoBehaviour {
    private float speedFactor = 500.0f;
    private float rotationFactor = 900f;

    private WheelJoint2D[] wheels;

    private float speed;

    private Rigidbody2D body;

    private void Awake() {
        wheels = GetComponentsInChildren<WheelJoint2D>();
        body = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        speed = Input.GetAxis("Vertical") * -speedFactor;
        JointMotor2D motor = new JointMotor2D {
            motorSpeed = speed,
            maxMotorTorque = 100000
        };
        for(int i = 0, len = wheels.Length; i < len; ++i) {
            wheels[i].motor = motor;
        }

        float rotation = Input.GetAxis("Horizontal") * -rotationFactor;
        body.AddTorque(rotation);
    }
}
