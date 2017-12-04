using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Interactible : MonoBehaviour {
    public abstract void Interact();

    public abstract string GetHelp();
}
