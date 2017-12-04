using UnityEngine;

public class Elevator : Interactible {

    public bool shouldBeUp;

    public float totalDistance = 5;

    private struct Rope {
        public SpringJoint2D joint;
        public float startingLength;
    }
    private Rope[] ropes;

    private float currentLength;

    private float changeVelocity;

    void Awake() {
        currentLength = totalDistance;

        SpringJoint2D[] joints = gameObject.GetComponentsInChildren<SpringJoint2D>();
        int numberOfRopes = joints.Length;
        ropes = new Rope[numberOfRopes];
        for(int i = 0; i < numberOfRopes; ++i) {
            Rope rope = new Rope {
                joint = joints[i],
            };
            rope.joint.autoConfigureDistance = false;
            rope.startingLength = rope.joint.distance;
            ropes[i] = rope;
        }
    }

    private void FixedUpdate() {
        float targetLength = shouldBeUp ? 0 : totalDistance;
        currentLength = Mathf.SmoothDamp(currentLength, targetLength, ref changeVelocity, 1.0f, 3.0f);
        for(int i = 0, len = ropes.Length; i < len; ++i) {
            ropes[i].joint.distance = ropes[i].startingLength - (totalDistance - currentLength);
            ropes[i].joint.connectedBody.WakeUp();
        }
    }

    public override void Interact() {
        shouldBeUp = !shouldBeUp;
    }

    public override string GetHelp() {
        return "Honk to make elevator go " + (shouldBeUp ? "down" : "up");
    }
}
