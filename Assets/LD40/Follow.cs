using UnityEngine;

public class Follow : MonoBehaviour {
    public Transform followTransform;
    public Vector3 offset = Vector3.back * 10;

    private new Transform transform;

    private void Awake() {
        transform = base.transform;
    }

    void Update() {
        if(followTransform != null) {
            transform.position = followTransform.position + offset;
        }
    }
}
