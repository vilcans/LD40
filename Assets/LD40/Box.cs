using UnityEngine;

public class Box : MonoBehaviour {

    void OnCollisionEnter2D(Collision2D collision) {
        //Debug.LogFormat("{0} collided with {1}", this, collision.gameObject);
        if(collision.gameObject.name.StartsWith("Ground")) {
            GameObject particles = Instantiate(GameSettings.instance.destroyEffect);
            particles.transform.position = transform.position;
            AudioClip[] availableClips = GameSettings.instance.hitClips;
            AudioClip clip = availableClips[Random.Range(0, availableClips.Length)];
            PackageSounds.instance.Play(clip, transform.position);
            Destroy(gameObject);
        }
    }
}
