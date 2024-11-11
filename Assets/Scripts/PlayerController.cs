using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerTracker playerTracker;
    public Transform wheel;
    float turnSpeed = 5f;
    float accelValue = 0;
    Action<float> onSpeedChange;
    public float playerSpeed;
    public static Action onSpeedUpStart;
    public static Action onSlowDownStart;
    public static Action onStop;

    // Left/Right input handler
    float turnValue = 0;
    float turnDir = 0;
    Action<float> onTurnChange;

    // Wheel rotation
    public float wheelTurnSpeed = 3;
    Quaternion startRot;
    Quaternion targetRot;

    Vector3 neutralDir;
    Vector3 leftDir;
    Vector3 rightDir;
    float turnTime = 1; // start at one to prevent turning in the beginning

    // Restarting
    Vector3 startPos;

    public Action onStart; // sync with other player scripts

    bool firstTimePlay = true;

    void OnEnable(){
        onTurnChange += TurnHandler;
        onSpeedChange += SpeedHandler;

        GameManager.onReset += OnStart;
    }

    void OnDisable(){
        onTurnChange -= TurnHandler;
        onSpeedChange -= SpeedHandler;

        GameManager.onReset -= OnStart;
    }

    void Start(){
        neutralDir = wheel.forward;
        leftDir = (-wheel.right + wheel.forward).normalized;
        rightDir = (wheel.right + wheel.forward).normalized;

        startPos = transform.position;
        wheel.rotation = Quaternion.LookRotation(neutralDir, wheel.up);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isGameStarted || firstTimePlay){
            PauseHandler();
            if(!GameManager.Instance.isPaused && !firstTimePlay){
                SpeedInputHandler();
                TurnInputHandler();
                WheelSmoothTurn();

                // Sideways Movement
                if(playerTracker.currentSpeed.magnitude > 0){
                    transform.position += transform.right * turnDir * Time.deltaTime;
                    if(transform.position.x > 5){
                        transform.position = new Vector3(5, transform.position.y, transform.position.z);
                    }
                    if(transform.position.x < -5){
                        transform.position = new Vector3(-5, transform.position.y, transform.position.z);
                    }
                }
            }
        }
        
    }

    void OnStart(){
        // Reset wheel
        wheel.rotation = Quaternion.LookRotation(neutralDir, wheel.up);
        turnTime = 1f;
        accelValue = 0;

        transform.position = startPos;
        onStart?.Invoke();
        Debug.Log("restarting");
    }

    void SpeedHandler(float val){
        if(val > 0){
            onSpeedUpStart?.Invoke();
        }
        else if(val < 0){
            onSlowDownStart?.Invoke();
        }
        else{
            onStop?.Invoke();
        }
    }

    void SpeedInputHandler(){
        float newValue;
        if(Input.GetKey(KeyCode.W) && playerTracker.currentGas > 0){
            newValue = 1;
        }
        else if(Input.GetKey(KeyCode.S)){
            newValue = -1;
        }
        else{
            newValue = 0;
        }

        if(newValue != accelValue){
            accelValue = newValue;
            onSpeedChange?.Invoke(newValue);
        }
    }

    void WheelSmoothTurn(){
        if(turnTime < 1){
            wheel.rotation = Quaternion.Slerp(startRot, targetRot, turnTime);
            turnTime += Time.deltaTime * wheelTurnSpeed;
        }
    }

    void TurnHandler(float val){
        startRot = wheel.rotation;
        turnTime = 0;

        if(val > 0){
            // turning speed
            turnDir = turnSpeed;

            // wheel turning
            targetRot = Quaternion.LookRotation(rightDir, wheel.up);
        }
        else if(val < 0){
            // turning speed
            turnDir = -turnSpeed;
            

            // wheel turning
            targetRot = Quaternion.LookRotation(leftDir, wheel.up);
        }
        else{
            // turning speed
            turnDir = 0;

            // wheel turning
            targetRot = Quaternion.LookRotation(neutralDir, wheel.up);
        }
    }

    void TurnInputHandler(){
        float newValue;
        if(Input.GetKey(KeyCode.A)){
            newValue = -1;
        }
        else if(Input.GetKey(KeyCode.D)){
            newValue = 1;
        }
        else{
            newValue = 0;
        }

        if(newValue != turnValue){
            turnValue = newValue;
            onTurnChange?.Invoke(newValue);
        }
    }

    void PauseHandler(){
        if(Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.isTransitioning){
            if(firstTimePlay) firstTimePlay = false;
            Debug.Log("ATTEMPT PAUSE");
            GameManager.Instance.TogglePause();
        }
    }
}
