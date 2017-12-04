using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Logistics : MonoBehaviour {

    public static Logistics instance;

    public Text terminal;

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

    private int numberOfDeliveries;
    private int numberOfBreaks;

    void Awake() {
        instance = this;

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

        UpdateTerminal();
        Driving.instance.UpdateHelpText();
    }

    void UpdateTerminal() {
        var s = new StringBuilder();
        for(int i = 0, len = droppers.Length; i < len; ++i) {
            Dropper d = droppers[i];
            s.Append(d.mailCenterName);
            s.Append(":\n");
            if(d.item != null) {
                s.Append(d.item.GetComponentInChildren<Box>().destination.address);
            }
            else {
                s.Append("No pickup");
            }
            s.Append("\n\n");
        }

        s.Append("\nYour stats:\n-----------\nDelivered: ");
        s.Append(numberOfDeliveries);
        s.Append("\nUndelivered: ");
        s.Append(numberOfBreaks);

        terminal.text = s.ToString();
    }

    public void AddDelivery() {
        ++numberOfDeliveries;
        UpdateTerminal();
    }

    public void AddBreak() {
        ++numberOfBreaks;
        UpdateTerminal();
    }
}
