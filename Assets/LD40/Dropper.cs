using UnityEngine;
using UnityEngine.UI;

public class Dropper : Interactible {

    private Transform item;
    private float? timeToDrop;
    private Vector3 dropPoint;

    void Awake() {
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

    void Update() {
        if(timeToDrop.HasValue) {
            float value = timeToDrop.Value;
            float newValue = value - Time.deltaTime;
            if(value > .5f && newValue <= .5) {
                PackageSounds.instance.Play(GameSettings.instance.droppingSound, transform.position);
            }
            value = newValue;
            if(value <= 0) {
                timeToDrop = null;
                item.GetComponent<Rigidbody2D>().simulated = true;
                item = null;
            }
            else {
                timeToDrop = value;
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
        item = obj.transform;
    }
}
