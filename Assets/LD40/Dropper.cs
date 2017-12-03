using UnityEngine;

public class Dropper : Interactible {

    public override void Interact() {
        //Debug.LogFormat("Interacting with Dropper {0}", this);
        transform.Find("Item").GetComponent<Rigidbody2D>().simulated = true;
    }
}
