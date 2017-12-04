using UnityEngine;

public class Game : MonoBehaviour {
    public Texture2D colors;

    private float startTime;

    private new Camera camera;

    void Awake() {
        camera = Camera.main;
        startTime = Time.time;
    }

    void Update() {
        float time = Time.time - startTime;

        float v = 1 - Mathf.PingPong(time / 60.0f, 1);
        Color c = colors.GetPixelBilinear(0.5f, v);
        camera.backgroundColor = c;

        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
