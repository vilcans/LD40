using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driving : MonoBehaviour {
    public float speedFactor = 500.0f;

    private WheelJoint2D[] wheels;

    private float speed;

    private void Awake() {
        wheels = GetComponentsInChildren<WheelJoint2D>();
    }

    private void Update() {
        speed = Input.GetAxis("Vertical") * -speedFactor;
        Debug.LogFormat("speed {0}", speed);
        JointMotor2D motor = new JointMotor2D {
            motorSpeed = speed,
            maxMotorTorque = 100000
        };
        for(int i = 0, len = wheels.Length; i < len; ++i) {
            wheels[i].motor = motor;
        }
    }
}
