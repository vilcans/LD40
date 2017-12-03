using System.Collections.Generic;
using UnityEngine;

public class Dropoff : Interactible {

    private List<Box> boxesDroppingOff;

    private float lastDrop;

    private float timeBetweenDrops = .5f;

    void Awake() {
        boxesDroppingOff = new List<Box>();
    }

    public override void Interact() {
        Box[] allBoxes = FindObjectsOfType<Box>();
        Vector3 myPosition = transform.position;
        Debug.LogFormat("Found {0} boxes", allBoxes.Length);
        for(int i = 0, len = allBoxes.Length; i < len; ++i) {
            GameObject obj = allBoxes[i].gameObject;
            float distance = Vector3.Distance(obj.transform.position, myPosition);
            if(distance >= 5) {
                Debug.LogFormat("Strange, there is a box that is {0} m away: {1}", distance, obj);
                continue;
            }
            boxesDroppingOff.Add(allBoxes[i]);
        }
    }

    void Update() {
        float now = Time.time;
        int numberOfBoxes = boxesDroppingOff.Count;
        if(now - lastDrop > timeBetweenDrops && numberOfBoxes != 0) {
            int i = numberOfBoxes - 1;
            Box box = boxesDroppingOff[i];
            boxesDroppingOff.RemoveAt(i);
            box.DropOff();
            lastDrop = now;
        }
    }
}
