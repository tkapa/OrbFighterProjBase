using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject[] spawnPoints;
    public GameObject[] potentialCharacters;
    public GameObject[] charactersInGame;

    int wantedCharacter = 0;
    float maximumGameTime = 99.0f;

    [System.Serializable]
    public class UIElements
    {
        public Slider playerOneHPBar;
        public Slider playerOneSuperBar;

        public Slider playerTwoHPBar;
        public Slider playerTwoSuperBar;

        public Text timerText;
    }

    public UIElements uiElements = new UIElements();

	// Use this for initialization
	void Start () {
        //Set character length spawn the players, and set their other player variables
        charactersInGame = new GameObject[2];
        spawnPlayers();
        charactersInGame[0].GetComponent<CharacterController>().otherPlayer = charactersInGame[1];
        charactersInGame[1].GetComponent<CharacterController>().otherPlayer = charactersInGame[0];
    }
	
	// Update is called once per frame
	void Update () {
        playerBarUpdate();
        timer();
    }

    void spawnPlayers()
    {
        for (int spawns = 0; spawns < spawnPoints.Length; spawns++)
        {
            GameObject newPlayer = Instantiate(potentialCharacters[wantedCharacter], spawnPoints[spawns].transform.position, spawnPoints[spawns].transform.rotation) as GameObject;

            //If the player is player one, set the axes that it can use and store it for later
            if (spawns == 0)
            {
                charactersInGame[spawns] = newPlayer;
                newPlayer.GetComponent<CharacterController>().inputSettings.HORIZONTAL_INPUT = "gp_P1LSX";
                newPlayer.GetComponent<CharacterController>().inputSettings.VERTICAL_INPUT = "gp_P1LSY";
                newPlayer.GetComponent<CharacterController>().inputSettings.DASH_INPUT = "gp_P1A";
                newPlayer.GetComponent<CharacterController>().inputSettings.PROJECTILE_INPUT = "gp_P1X";

                uiElements.playerOneHPBar.maxValue = charactersInGame[spawns].GetComponent<CharacterController>().combatSettings.maxHealth;
                uiElements.playerOneSuperBar.maxValue = charactersInGame[spawns].GetComponent<CharacterController>().combatSettings.maxSuper;
            }
            else
            {
                charactersInGame[spawns] = newPlayer;
                newPlayer.GetComponent<CharacterController>().inputSettings.HORIZONTAL_INPUT = "gp_P2LSX";
                newPlayer.GetComponent<CharacterController>().inputSettings.VERTICAL_INPUT = "gp_P2LSY";
                newPlayer.GetComponent<CharacterController>().inputSettings.DASH_INPUT = "gp_P2A";
                newPlayer.GetComponent<CharacterController>().inputSettings.PROJECTILE_INPUT = "gp_P2X";

                uiElements.playerTwoHPBar.maxValue = charactersInGame[spawns].GetComponent<CharacterController>().combatSettings.maxHealth;
                uiElements.playerTwoSuperBar.maxValue = charactersInGame[spawns].GetComponent<CharacterController>().combatSettings.maxSuper;
            }
        }
    }

    void playerBarUpdate()
    {
        //Set player one super and hp bar values
        uiElements.playerOneHPBar.value = charactersInGame[0].GetComponent<CharacterController>().combatSettings.currHealth;
        uiElements.playerOneSuperBar.value = charactersInGame[0].GetComponent<CharacterController>().combatSettings.currSuper;

        //Set player one super and hp bar valuesSet player two super and HP bar values
        uiElements.playerTwoHPBar.value = charactersInGame[1].GetComponent<CharacterController>().combatSettings.currHealth;
        uiElements.playerTwoSuperBar.value = charactersInGame[1].GetComponent<CharacterController>().combatSettings.currSuper;
    }

    void timer()
    {
        //Countdown the timer until 0
        if (maximumGameTime > 0)
            maximumGameTime -= Time.deltaTime;

        uiElements.timerText.text = "" + (int)maximumGameTime;
    }
}
