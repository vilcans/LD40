using UnityEngine;
using UnityEngine.UI;

public class Dropper : Interactible {

    public string mailCenterName = "C?";
    public AnimationCurve shakeAnimation;

    private Transform mug;
    private Vector3 mugPosition;

    public Transform item;
    private float? timeToDrop;
    private Vector3 dropPoint;

    void Awake() {
        mug = transform.Find("Mug");
        mugPosition = mug.position;
        dropPoint = transform.Find("DropPoint").position + Vector3.forward * .01f;
    }

    public override void Interact() {
        //Debug.LogFormat("Interacting with Dropper {0}", this);
        if(!timeToDrop.HasValue) {
            if(item != null) {
                timeToDrop = 1.0f;
            }
        }
    }

    public override string GetHelp() {
        if(item == null) {
            return "There is no package to pick up at " + mailCenterName;
        }
        else if(timeToDrop.HasValue) {
            return "Deliver to " + item.GetComponent<Box>().destination.address;
        }
        else {
            return "Honk to pick up the package at " + mailCenterName;
        }
    }

    void Update() {
        if(timeToDrop.HasValue) {
            float t = timeToDrop.Value;
            float newT = t - Time.deltaTime;
            if(t > .5f && newT <= .5) {
                PackageSounds.instance.Play(GameSettings.instance.droppingSound, transform.position);
            }
            t = newT;
            float shake = shakeAnimation.Evaluate(1 - t);
            mug.position = mugPosition + Vector3.up * shake * .35f;
            if(t <= 0) {
                timeToDrop = null;
                item.GetComponent<Rigidbody2D>().simulated = true;
                item = null;
            }
            else {
                timeToDrop = t;
            }
        }
    }

    public void AddDelivery(Logistics.Delivery delivery) {
        if(item != null) {
            Debug.LogFormat("Can't add delivery - already have one");
            return;
        }
        GameObject obj = Instantiate(delivery.prefab, dropPoint, Quaternion.Euler(0, 0, Random.Range(0, 4) * 90f));
        obj.GetComponent<Rigidbody2D>().simulated = false;
        obj.GetComponentInChildren<Text>().text = delivery.to.GetComponent<Dropoff>().address;
        obj.GetComponent<Box>().destination = delivery.to;
        item = obj.transform;
    }
}
