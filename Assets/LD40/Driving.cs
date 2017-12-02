﻿using UnityEngine;

public class Driving : MonoBehaviour {
    public float maxTorque = 10000;
    public float maxSpeed = 500.0f;
    public float rotationSpeed = 900f;
    private float acceleration = 5;

    private WheelJoint2D[] wheels;

    private Rigidbody2D body;

    private float currentSpeed;

    private AudioSource audio;
    private float idleAudioPitch = .8f;
    private float maxSpeedPitch = 1.5f;

    private void Awake() {
        wheels = GetComponentsInChildren<WheelJoint2D>();
        body = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
    }

    private void FixedUpdate() {
        float wantedSpeed = Input.GetAxis("Vertical") * -maxSpeed;
        currentSpeed = Mathf.MoveTowards(currentSpeed, wantedSpeed, acceleration);

        if(false) {
            for(int i = 0, len = wheels.Length; i < len; ++i) {
                wheels[i].useMotor = false;
            }
        }
        else {
            JointMotor2D motor = new JointMotor2D {
                motorSpeed = currentSpeed,
                maxMotorTorque = maxTorque
            };
            for(int i = 0, len = wheels.Length; i < len; ++i) {
                wheels[i].motor = motor;
            }

        }

        audio.pitch = Mathf.LerpUnclamped(idleAudioPitch, maxSpeedPitch, Mathf.Abs(currentSpeed) / maxSpeed);

        float rotation = Input.GetAxis("Horizontal") * -rotationSpeed;
        body.AddTorque(rotation);
    }
}
