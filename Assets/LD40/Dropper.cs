using UnityEngine;

public class Dropper : Interactible {

    private Transform item;
    private float? timeToDrop;

    public override void Interact() {
        //Debug.LogFormat("Interacting with Dropper {0}", this);
        if(!timeToDrop.HasValue) {
            item = transform.Find("Item");
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
                item.transform.SetParent(null);
                item.GetComponent<Rigidbody2D>().simulated = true;
            }
            else {
                timeToDrop = value;
            }
        }
    }
}
