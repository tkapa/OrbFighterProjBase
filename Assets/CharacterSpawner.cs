using UnityEngine;
using System.Collections;

public class CharacterSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager.instance.OnSpawnCharacter.AddListener(CreateCharacter);
	}

    void CreateCharacter(SpawningInformation information)
    {
        //Instantiate the character
        Instantiate(information.characterToSpawn, information.spawnpoint.transform.position, information.spawnpoint.transform.rotation);
    }
}
