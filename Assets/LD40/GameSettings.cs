using System;
using UnityEngine;

public class GameSettings : MonoBehaviour {
    public static GameSettings instance;

    public GameObject destroyEffect;

    public void Awake() {
        instance = this;
    }
}
