using UnityEngine;
using System.Collections;

public class CharacterSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.instance.OnSpawnCharacter.AddListener();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CreateCharacter(SpawningInformation information)
    {

    }
}
