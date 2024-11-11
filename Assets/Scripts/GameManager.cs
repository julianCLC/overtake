using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public int playerLaneStart = 2;
    public float playerStartSpeed {get; private set;}
    // public LaneCarInfo[] laneCarInfos;
    public PlayerCarProperties playerCarProperties;

    public static Action onStart; // use this on game start
    public static Action onReset; // use this to reset values
    public static Action onDeath;
    // public static Action onSetup;

    public static GameManager Instance {get; private set;}

    public bool isGameStarted {get; private set;}
    public bool isPaused {get; private set;}
    public bool isTransitioning = true;

    bool firstTimePlay = true;

    void Awake(){
        if (Instance != null && Instance != this){ 
            Destroy(this); 
        } 
        else{ 
            Instance = this; 
        }

        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);

        isGameStarted = false;
        isPaused = false;

        SetPlayerStartSpeed();
    }

    void OnEnable(){
        Hitbox.onPlayerHit += OnDeath;
    }

    void OnDisable(){
        Hitbox.onPlayerHit -= OnDeath;
    }

    public void StartGame(){
        isGameStarted = true;
        onStart?.Invoke();
    }

    public void Restart(){
        isPaused = false;
        onReset?.Invoke();
        StartGame();
    }

    public void OnDeath(){
        isGameStarted = false;
        isPaused = true;
        MenuManager.Instance.OpenFailMenu();
        onDeath?.Invoke();
    }

    void SetPlayerStartSpeed(){
        playerStartSpeed = CarsManager.Instance.GetLaneCarInfo(playerLaneStart).carMoveSpeed;
    }

    public void TogglePause(){
        if(firstTimePlay){
            pauseMenu.SetActive(false);
            firstTimePlay = false;
            StartGame();
        }
        else{
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);
        }
        
    }
}
