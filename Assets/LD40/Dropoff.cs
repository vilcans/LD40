using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dropoff : Interactible {

    [TextArea]
    public string address = "Unknown";

    private HashSet<Box> boxesDroppingOff;

    private float lastDrop;

    private float timeBetweenDrops = .5f;

    private Vector3 myPosition;

    void Awake() {
        boxesDroppingOff = new HashSet<Box>();
        myPosition = transform.position;
    }

    public override void Interact() {
        Box[] allBoxes = FindObjectsOfType<Box>();
        Vector3 myPosition = transform.position;
        //Debug.LogFormat("Found {0} boxes", allBoxes.Length);
        for(int i = 0, len = allBoxes.Length; i < len; ++i) {
            if(CanDropOff(allBoxes[i])) {
                if(boxesDroppingOff.Count == 0) {
                    lastDrop = Time.time;
                }
                boxesDroppingOff.Add(allBoxes[i]);
            }
        }
    }

    private bool CanDropOff(Box box) {
        if(box.destination != this) {
            return false;
        }
        GameObject obj = box.gameObject;
        float distance = Vector3.Distance(obj.transform.position, myPosition);
        if(distance >= 10) {
            //Debug.LogFormat("Strange, there is a box that is {0} m away: {1}", distance, obj);
            return false;
        }
        return true;
    }

    private bool CanDropOff() {
        Box[] allBoxes = FindObjectsOfType<Box>();
        for(int i = 0, len = allBoxes.Length; i < len; ++i) {
            if(CanDropOff(allBoxes[i])) {
                return true;
            }
        }
        return false;
    }

    public override string GetHelp() {
        if(CanDropOff()) {
            return "Honk to drop off packages at " + address;
        }
        else {
            return "No packages to drop off at " + address;
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
