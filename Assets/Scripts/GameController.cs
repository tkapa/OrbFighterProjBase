﻿using UnityEngine;
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
        SpawnPlayers();
        charactersInGame[0].GetComponent<CharacterController>().otherPlayer = charactersInGame[1];
        charactersInGame[1].GetComponent<CharacterController>().otherPlayer = charactersInGame[0];
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
            GameObject newPlayer = Instantiate(potentialCharacters[wantedCharacter], spawnPoints[spawns].transform.position, spawnPoints[spawns].transform.rotation) as GameObject;

            //If the player is player one, set the axes that it can use and store it for later
            if (spawns == 0)
            {
                charactersInGame[spawns] = newPlayer;

                //Set Input axes for each player
                newPlayer.GetComponent<CharacterController>().inputSettings.HORIZONTAL_INPUT = "gp_P1LSX";
                newPlayer.GetComponent<CharacterController>().inputSettings.VERTICAL_INPUT = "gp_P1LSY";
                newPlayer.GetComponent<CharacterController>().inputSettings.DASH_INPUT = "gp_P1A";
                newPlayer.GetComponent<CharacterController>().inputSettings.PROJECTILE_INPUT = /*"gp_P1X"*/ "e";

                //Set max values of each bar
                uiElements.playerOneHPBar.maxValue = charactersInGame[spawns].GetComponent<HealthComponent>().maximumHealth;
                uiElements.playerOneSuperBar.maxValue = charactersInGame[spawns].GetComponent<CharacterController>().combatSettings.maxSuper;
            }
            else
            {
                charactersInGame[spawns] = newPlayer;

                //Set input axes for each player
                newPlayer.GetComponent<CharacterController>().inputSettings.HORIZONTAL_INPUT = "gp_P2LSX";
                newPlayer.GetComponent<CharacterController>().inputSettings.VERTICAL_INPUT = "gp_P2LSY";
                newPlayer.GetComponent<CharacterController>().inputSettings.DASH_INPUT = "gp_P2A";
                newPlayer.GetComponent<CharacterController>().inputSettings.PROJECTILE_INPUT = /*"gp_P2X"*/ "q";

                //Set max values of each bar
                uiElements.playerTwoHPBar.maxValue = charactersInGame[spawns].GetComponent<HealthComponent>().maximumHealth;
                uiElements.playerTwoSuperBar.maxValue = charactersInGame[spawns].GetComponent<CharacterController>().combatSettings.maxSuper;
            }
        }
    }

    void PlayerBarUpdate()
    {
        //Set player one super and hp bar values
        uiElements.playerOneHPBar.value = charactersInGame[0].GetComponent<HealthComponent>().currentHealth;
        uiElements.playerOneSuperBar.value = charactersInGame[0].GetComponent<CharacterController>().combatSettings.currSuper;

        //Set player one super and hp bar valuesSet player two super and HP bar values
        uiElements.playerTwoHPBar.value = charactersInGame[1].GetComponent<HealthComponent>().currentHealth;
        uiElements.playerTwoSuperBar.value = charactersInGame[1].GetComponent<CharacterController>().combatSettings.currSuper;
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
