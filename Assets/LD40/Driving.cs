using UnityEngine;
using UnityEngine.UI;

public class Driving : MonoBehaviour {
    public static Driving instance;

    public float maxTorque = 10000;
    public float maxSpeed = 500.0f;

    public Text helpText;

    private float rotationTorquePerKg = 105;
    private float acceleration = 5;

    private WheelJoint2D[] wheels;

    private Rigidbody2D body;

    private float currentSpeed;

    private AudioSource audio;
    private AudioSource hornAudio;

    private float idleAudioPitch = .4f;
    private float maxSpeedPitch = 1.5f;
    private float engineSoundVolume;
    private float engineStartTime;
    private const float engineSoundFadeInTime = 5.5f;

    private Interactible currentInteractible;

    private void Awake() {
        instance = this;

        wheels = GetComponentsInChildren<WheelJoint2D>();
        body = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();

        hornAudio = transform.Find("Horn").GetComponent<AudioSource>();

        engineSoundVolume = audio.volume;
        audio.volume = 0;
        engineStartTime = Time.time;
    }

    void Update() {
        float time = Time.time - engineStartTime;
        audio.volume = Mathf.Pow(Mathf.Clamp01(time / engineSoundFadeInTime) * engineSoundVolume, .8f);

        if(Input.GetButtonDown("Fire1")) {
            hornAudio.Play();
            if(currentInteractible != null) {
                currentInteractible.Interact();
                UpdateHelpText();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Interactible i = collision.gameObject.GetComponentInParent<Interactible>();
        if(i != null) {
            //Debug.LogFormat("Entering interactible {0}", i);
            currentInteractible = i;
            UpdateHelpText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Interactible i = collision.gameObject.GetComponentInParent<Interactible>();
        if(i != currentInteractible) {
            Debug.LogFormat("Exiting not previously known trigger {0}", collision.gameObject);
        }
        else {
            //Debug.LogFormat("Exiting {0}", currentInteractible);
            currentInteractible = null;
        }
        UpdateHelpText();
    }

    public void UpdateHelpText() {
        if(currentInteractible == null) {
            helpText.CrossFadeColor(new Color(0, 0, 0, 0), 1.0f, true, true, false);
        }
        else {
            helpText.text = currentInteractible.GetHelp();
            helpText.CrossFadeColor(new Color(0, 0, 0, 1), .05f, true, true, false);
        }
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
            .8f * Mathf.SmoothStep(idleAudioPitch, maxSpeedPitch, Mathf.Abs(currentSpeed) / maxSpeed) +
            .2f * Mathf.LerpUnclamped(idleAudioPitch, maxSpeedPitch, Mathf.Abs(wheelSpeed * 10) / maxSpeed)
        );

        float rotationPower = Mathf.Abs(body.transform.up.y);
        float rotation = rotationPower * Input.GetAxis("Horizontal") * -rotationTorquePerKg * body.mass;
        body.AddTorque(rotation);
    }
}
