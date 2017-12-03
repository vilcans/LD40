using UnityEngine;

public class PackageSounds : MonoBehaviour {

    public static PackageSounds instance;

    private new AudioSource audio;

    void Awake() {
        instance = this;
        audio = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip, Vector3 position) {
        transform.position = position;
        audio.PlayOneShot(clip);
    }
}
