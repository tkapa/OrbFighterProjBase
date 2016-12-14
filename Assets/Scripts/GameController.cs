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
        public Slider[] playerHPBar;
        public Slider[] playerSuperBar;

        public Text timerText;
    }

    public UIElements uiElements = new UIElements();

	// Use this for initialization
	void Start () {
        //Set character length spawn the players, and set their other player variables
        charactersInGame = new GameObject[2];

        SpawnPlayers();

        charactersInGame[0].GetComponent<TwoDimensionalCharacterController>().otherPlayer = charactersInGame[1];
        charactersInGame[1].GetComponent<TwoDimensionalCharacterController>().otherPlayer = charactersInGame[0];
    }
	
	// Update is called once per frame
	void Update () {
        //Perform player bar updates and timer rundown.
        PlayerBarUpdate();
        Timer();
    }

    void SpawnPlayers()
    {
        for (int spawns = 0; spawns < spawnPoints.Length; spawns++)
        {
            GameObject newPlayer = potentialCharacters[wantedCharacter];
            TwoDimensionalCharacterController thisChar = newPlayer.GetComponent<TwoDimensionalCharacterController>();
            //If the player is player one, set the axes that it can use and store it for later

            //Create an information packet and set all relevant variables needed.
            SpawningInformation thisSpawningInformation = new SpawningInformation();
            thisSpawningInformation.characterToSpawn = charactersInGame[spawns] = newPlayer;
            thisSpawningInformation.spawnpoint = spawnPoints[spawns];

            if (spawns == 0)
            {
                thisChar.inputSettings.HORIZONTAL_INPUT = "Horizontal";
                thisChar.inputSettings.VERTICAL_INPUT = "Vertical";
                thisChar.inputSettings.DASH_INPUT = "gp_P1A";
                thisChar.inputSettings.PROJECTILE_INPUT = /*"gp_P1X"*/ "q";
                thisChar.inputSettings.SUPER_INPUT = "gp_P1Y";
            }
            else
            {
                thisChar.inputSettings.HORIZONTAL_INPUT = "gp_P2LSX";
                thisChar.inputSettings.VERTICAL_INPUT = "gp_P2LSY";
                thisChar.inputSettings.DASH_INPUT = "gp_P2A";
                thisChar.inputSettings.PROJECTILE_INPUT =/* "gp_P2X"*/"e";
                thisChar.inputSettings.SUPER_INPUT = "gp_P2Y";
            }

            //Invoke the event necessary to spawn each character.
            EventManager.instance.OnSpawnCharacter.Invoke(thisSpawningInformation);
            //Set max values of each bar
            uiElements.playerHPBar[spawns].maxValue = charactersInGame[spawns].GetComponent<HealthComponent>().maximumHealth;
            uiElements.playerSuperBar[spawns].maxValue = charactersInGame[spawns].GetComponent<TwoDimensionalCharacterController>().combatSettings.maxSuper;            
        }
    }

    void PlayerBarUpdate()
    {
        //Update each character's HP and Super Bar.
        for (int i = 0; i < charactersInGame.Length; i++)
        {
            //Set player one super and hp bar values
            uiElements.playerHPBar[i].value = charactersInGame[i].GetComponent<HealthComponent>().currentHealth;
            uiElements.playerSuperBar[i].value = charactersInGame[i].GetComponent<SuperComponent>().currentSuper;
        }        
    }

    void Timer()
    {
        //Countdown the timer until 0
        if (maximumGameTime > 0)
            maximumGameTime -= Time.deltaTime;
        //else
            //End Game

        uiElements.timerText.text = "" + (int)maximumGameTime;
    }
}
