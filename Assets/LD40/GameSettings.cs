using UnityEngine;

public class GameSettings : MonoBehaviour {
    public static GameSettings instance;

    public GameObject destroyEffect;

    public AudioClip[] hitClips;

    public GameObject dropOffEffect;
    public AudioClip dropOffSound;

    public void Awake() {
        instance = this;
    }
}
