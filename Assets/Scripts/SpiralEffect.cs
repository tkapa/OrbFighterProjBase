using UnityEngine;
using System.Collections;

public class SpiralEffect : MonoBehaviour {
    //Variables
    [SerializeField] float
        rotationAmount,
        trailLifeTime = 0f,
        trailStartWidth = 0f,
        trailEndWidth = 0f;


	// Use this for initialization
	void Start () {
        TrailRenderer[] trails = GetComponentsInChildren<TrailRenderer>();
        foreach(TrailRenderer trail in trails)
        {
            trail.time = trailLifeTime;
            trail.startWidth = trailStartWidth;
            trail.endWidth = trailEndWidth;
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotationAmount * Time.deltaTime, 0f, 0f);
	}
}
