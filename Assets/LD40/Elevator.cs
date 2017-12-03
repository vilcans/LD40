using UnityEngine;

public class Elevator : MonoBehaviour {

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

    void Update() {
        float targetLength = shouldBeUp ? 0 : totalDistance;
        currentLength = Mathf.SmoothDamp(currentLength, targetLength, ref changeVelocity, 2.0f);
        for(int i = 0, len = ropes.Length; i < len; ++i) {
            ropes[i].joint.distance = ropes[i].startingLength - (totalDistance - currentLength);
            ropes[i].joint.connectedBody.WakeUp();
        }
    }
}
