﻿using UnityEngine;

public class Driving : MonoBehaviour {
    public float maxTorque = 10000;
    public float maxSpeed = 500.0f;
    private float rotationTorquePerKg = 8.33f;
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

        float wheelSpeed = 0;
        if(wantedSpeed == 0) {
            // Put engine in neutral when buttons are released
            for(int i = 0, len = wheels.Length; i < len; ++i) {
                wheels[i].useMotor = false;
                wheelSpeed += wheels[i].GetComponent<Rigidbody2D>().angularVelocity;
            }
        }
        else {
            JointMotor2D motor = new JointMotor2D {
                motorSpeed = currentSpeed,
                maxMotorTorque = maxTorque
            };
            for(int i = 0, len = wheels.Length; i < len; ++i) {
                wheels[i].motor = motor;
                wheelSpeed += wheels[i].GetComponent<Rigidbody2D>().angularVelocity;
            }

        }

        audio.pitch = (
            .5f * Mathf.LerpUnclamped(idleAudioPitch, maxSpeedPitch, Mathf.Abs(currentSpeed) / maxSpeed) +
            .5f * Mathf.LerpUnclamped(idleAudioPitch, maxSpeedPitch, Mathf.Abs(wheelSpeed * 10) / maxSpeed)
        );

        float rotation = Input.GetAxis("Horizontal") * -rotationTorquePerKg * body.mass;
        body.AddTorque(rotation);
    }
}
