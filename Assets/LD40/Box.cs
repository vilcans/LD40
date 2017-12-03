using UnityEngine;

public class Box : MonoBehaviour {

    private int numberOfCollisions;
    private float timeInCollision;

    private void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.layer == 8) {
            --numberOfCollisions;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.layer == 8) {
            ++numberOfCollisions;
        }
    }

    void Update() {
        if(numberOfCollisions == 0) {
            timeInCollision = 0;
            return;
        }
        timeInCollision += Time.deltaTime;
        //Debug.LogFormat("{0} collided with {1}", this, collision.gameObject);
        if(timeInCollision > .1f) {
            GameObject particles = Instantiate(GameSettings.instance.destroyEffect);
            particles.transform.position = transform.position;
            AudioClip[] availableClips = GameSettings.instance.hitClips;
            AudioClip clip = availableClips[Random.Range(0, availableClips.Length)];
            PackageSounds.instance.Play(clip, transform.position);
            Destroy(gameObject);
        }
    }
}
