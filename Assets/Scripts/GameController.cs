using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject[] spawnPoints;
    public GameObject[] potentialCharacters;
    public GameObject[] charactersInGame;

    int wantedCharacter = 0;

    [System.Serializable]
    public class sliders
    {
        public Slider playerOneHPBar;
        public Slider playerOneSuperBar;

        public Slider playerTwoHPBar;
        public Slider playerTwoSuperBar;
    }
	// Use this for initialization
	void Start () {
        charactersInGame = new GameObject[2];
        spawnPlayers();
        charactersInGame[0].GetComponent<CharacterController>().otherPlayer = charactersInGame[1];
        charactersInGame[1].GetComponent<CharacterController>().otherPlayer = charactersInGame[0];
    }
	
	// Update is called once per frame
	void Update () {

    }

    void spawnPlayers()
    {
        for (int spawns = 0; spawns < spawnPoints.Length; spawns++)
        {
            GameObject newPlayer = Instantiate(potentialCharacters[wantedCharacter], spawnPoints[spawns].transform.position, spawnPoints[spawns].transform.rotation) as GameObject;

            //If the player is player one, set the axes that it can use and store it for later
            if (spawns == 0)
            {
                charactersInGame[0] = newPlayer;
                newPlayer.GetComponent<CharacterController>().inputSettings.HORIZONTAL_INPUT = "gp_P1LSX";
                newPlayer.GetComponent<CharacterController>().inputSettings.VERTICAL_INPUT = "gp_P1LSY";
                newPlayer.GetComponent<CharacterController>().inputSettings.DASH_INPUT = "gp_P1A";
                newPlayer.GetComponent<CharacterController>().inputSettings.PROJECTILE_INPUT = "gp_P1X";
            }
            else
            {
                charactersInGame[1] = newPlayer;
                newPlayer.GetComponent<CharacterController>().inputSettings.HORIZONTAL_INPUT = "gp_P2LSX";
                newPlayer.GetComponent<CharacterController>().inputSettings.VERTICAL_INPUT = "gp_P2LSY";
                newPlayer.GetComponent<CharacterController>().inputSettings.DASH_INPUT = "gp_P2A";
                newPlayer.GetComponent<CharacterController>().inputSettings.PROJECTILE_INPUT = "gp_P2X";
            }
        }
    }
}
