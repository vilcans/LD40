using System.Collections.Generic;
using UnityEngine;

public class Logistics : MonoBehaviour {

    public class Delivery {
        public Dropper from;
        public Dropoff to;
        public GameObject prefab;
    }

    private List<GameObject> boxPrefabs;

    private Dropoff[] dropoffs;
    private Dropper[] droppers;

    private float timeToNextPickup = 5;

    private float timeBetween = 8.0f;
    private float timeBetweenVariation = 4.0f;

    void Awake() {
        dropoffs = FindObjectsOfType<Dropoff>();
        droppers = FindObjectsOfType<Dropper>();
        Random.InitState(42);

        boxPrefabs = new List<GameObject>();

        for(int i = 0; i < 100; ++i) {
            string path = string.Format("Boxes/Box{0,2:D2}", i);
            GameObject prefab = Resources.Load<GameObject>(path);
            if(prefab != null) {
                //Debug.LogFormat("Loaded {0}", path);
                boxPrefabs.Add(prefab);
            }
        }
    }

    void FixedUpdate() {
        timeToNextPickup -= Time.deltaTime;
        if(timeToNextPickup > 0) {
            return;
        }
        timeToNextPickup = Random.Range(timeBetween - timeBetweenVariation * .5f, timeBetween + timeBetweenVariation * .5f);

        Delivery delivery = new Delivery {
            from = droppers[Random.Range(0, droppers.Length)],
            to = dropoffs[Random.Range(0, dropoffs.Length)],
            prefab = boxPrefabs[Random.Range(0, boxPrefabs.Count)],
        };
        delivery.from.AddDelivery(delivery);
    }
}
