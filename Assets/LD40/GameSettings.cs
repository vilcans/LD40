using UnityEngine;

public class GameSettings : MonoBehaviour {
    public static GameSettings instance;

    public GameObject destroyEffect;

    public AudioClip[] hitClips;

    public void Awake() {
        instance = this;
    }
}
