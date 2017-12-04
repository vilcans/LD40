using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dropoff : Interactible {

    [TextArea]
    public string address = "Unknown";

    private HashSet<Box> boxesDroppingOff;

    private float lastDrop;

    private float timeBetweenDrops = .5f;

    void Awake() {
        boxesDroppingOff = new HashSet<Box>();
    }

    public override void Interact() {
        Box[] allBoxes = FindObjectsOfType<Box>();
        Vector3 myPosition = transform.position;
        Debug.LogFormat("Found {0} boxes", allBoxes.Length);
        for(int i = 0, len = allBoxes.Length; i < len; ++i) {
            if(allBoxes[i].destination != this) {
                continue;
            }
            GameObject obj = allBoxes[i].gameObject;
            float distance = Vector3.Distance(obj.transform.position, myPosition);
            if(distance >= 5) {
                Debug.LogFormat("Strange, there is a box that is {0} m away: {1}", distance, obj);
                continue;
            }
            if(boxesDroppingOff.Count == 0) {
                lastDrop = Time.time;
            }
            boxesDroppingOff.Add(allBoxes[i]);
        }
    }

    void Update() {
        float now = Time.time;
        int numberOfBoxes = boxesDroppingOff.Count;
        if(now - lastDrop > timeBetweenDrops && numberOfBoxes != 0) {
            Box box = boxesDroppingOff.First();
            boxesDroppingOff.Remove(box);
            box.DropOff();
            Logistics.instance.AddDelivery();
            lastDrop = now;
        }
    }
}
