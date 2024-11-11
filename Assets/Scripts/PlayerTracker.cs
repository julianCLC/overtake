using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tracks how far player has driven
/// also how much gas has been used up
/// </summary>
/// 
public class PlayerTracker : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public float maxEngineVolume = 0.5f;
    public MyEngineAudio engineAudioOn;
    public MyEngineAudio engineAudioOff;
    public TMP_Text distanceDisplay;
    public TMP_Text speedDisplay;
    public Slider gasSlider;
    public Image gasFill;
    // public Image gasIndicator;
    public Animator gasAnim;
    public Color gasNeutral;
    public Color gasAccel;
    public Color gasFullSpeed;
    public float distancedTraveled {get; private set;}
    public float maxGas = 50;
    public float accelGasRate = 2f;
    public float fullSpeedGasRate = 0.5f;

    GasStates gasStates;
    float currentGasRate = 1f;
    public float currentGas;
    float moveSpeed;
    bool isAccelerating = true;

    // Handles rate of change
    float currentDelta;
    float speedCoastDelta;
    float speedUpDelta;
    float slowDownDelta;

    // For lerping speed
    Vector3 startSpeed;
    Vector3 targetSpeed;
    public Vector3 currentSpeed = Vector3.zero;
    float distance;
    float remainingDistance;

    public float fakeRPM;

    public static PlayerTracker Instance {get; private set;}

    void Awake(){
        if (Instance != null && Instance != this){ 
            Destroy(this); 
        } 
        else{ 
            Instance = this; 
        }
    
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = new Vector3(0, 0, -GameManager.Instance.playerStartSpeed);
        Initialize();
        engineAudioOn.rpm = (currentSpeed.magnitude / moveSpeed) + 1;

        fakeRPM = currentSpeed.magnitude / moveSpeed;
    }

    void Initialize(){
        PlayerCarProperties playerCarProperties = GameManager.Instance.playerCarProperties;
        speedCoastDelta = playerCarProperties.neutralSpeed;
        speedUpDelta    = playerCarProperties.accelerateSpeed;
        slowDownDelta   = playerCarProperties.decelerateSpeed;
        moveSpeed       = playerCarProperties.carSpeed;
    }

    void OnEnable(){
        // TODO: possibly change this to just recieve the input i.e. -1, 0 or 1
        // and determine from there if player is speeding up or slowing down
        PlayerController.onSpeedUpStart += SpeedUpStart;
        PlayerController.onSlowDownStart += SlowDownStart;
        PlayerController.onStop += StopChange;

        GameManager.onStart += OnStart;
        GameManager.onReset += OnRestart;
        GameManager.onDeath += OnDeath;
    }

    void OnDisable(){
        PlayerController.onSpeedUpStart -= SpeedUpStart;
        PlayerController.onSlowDownStart -= SlowDownStart;
        PlayerController.onStop -= StopChange;

        GameManager.onStart -= OnStart;
        GameManager.onReset -= OnRestart;
        GameManager.onDeath -= OnDeath;
    }

    // Update is called once per frame
    void Update()
    {
        

        if(GameManager.Instance.isGameStarted && !GameManager.Instance.isPaused){
            SpeedHandler();
            DistanceTracker();
            GasTracker();
            SpeedTracker();
            AudioHandler();
        }
        
    }

    void GasTracker(){
        currentGas -= Time.deltaTime * currentGasRate ;
        gasSlider.value = currentGas / maxGas;

        // Gas bar color
        switch (gasStates){
            case GasStates.Neutral:
                gasFill.color = gasNeutral;
                break;
            case GasStates.Accelerating:
                gasFill.color = gasAccel;
                break;
            case GasStates.FullSpeed:
                gasFill.color = gasFullSpeed;
                break;
        }

        if(gasSlider.value < .25f){
            if(!gasAnim.GetCurrentAnimatorStateInfo(0).IsName("Low")){
                gasAnim.Play("Low");
            }
            
        }
        else{
            if(!gasAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
                gasAnim.Play("Idle");
            }
        }
    }

    void DistanceTracker(){
        distancedTraveled += currentSpeed.magnitude * Time.deltaTime;
        distanceDisplay.text = "Distance: " + distancedTraveled.ToString("F0");

    }

    void SpeedHandler(){
        if(distance > 0){
            currentSpeed = Vector3.Lerp(startSpeed, targetSpeed, 1 - (remainingDistance / distance));
            remainingDistance -= moveSpeed * currentDelta * Time.deltaTime;
        }
        
        speedDisplay.text = "Speed: " + currentSpeed.magnitude.ToString("F0");

        
    }

    void AudioHandler(){
        float engineVol = (currentSpeed.magnitude / moveSpeed) + 1;
        engineAudioOn.rpm = engineVol;
        engineAudioOff.rpm = engineVol;

        // Crossfade
        if(isAccelerating){
            engineAudioOn.masterVolume = Mathf.Min(maxEngineVolume, engineAudioOn.masterVolume + Time.deltaTime);
            engineAudioOff.masterVolume = Mathf.Max(0, engineAudioOff.masterVolume - Time.deltaTime);
        }
        else{
            engineAudioOn.masterVolume = Mathf.Max(0, engineAudioOn.masterVolume - Time.deltaTime);
            engineAudioOff.masterVolume = Mathf.Min(maxEngineVolume, engineAudioOff.masterVolume + Time.deltaTime);
        }
    }

    void SpeedTracker(){
        // check if top speed
        if(currentSpeed.magnitude >= GameManager.Instance.playerCarProperties.carSpeed && isAccelerating){
            gasStates = GasStates.FullSpeed;
            currentGasRate = fullSpeedGasRate;
            
            vcam.m_Lens.FieldOfView = Mathf.Min(75, vcam.m_Lens.FieldOfView + Time.deltaTime * 30);
        }
        else{
            vcam.m_Lens.FieldOfView = Mathf.Max(70, vcam.m_Lens.FieldOfView - Time.deltaTime * 30);
        }


    }

    void SpeedUpStart(){
        targetSpeed = new Vector3(0, 0, -moveSpeed);
        currentDelta = speedUpDelta;

        currentGasRate = accelGasRate;
        gasStates = GasStates.Accelerating;
        isAccelerating = true;

        LerpConfigure();
    }

    void SlowDownStart(){
        StopChange();
        currentDelta = slowDownDelta;
        currentGasRate = 1;
    }

    void StopChange(){
        targetSpeed = Vector3.zero;
        currentDelta = speedCoastDelta;

        currentGasRate = 1;
        gasStates = GasStates.Neutral;
        isAccelerating = false;

        LerpConfigure();
    }

    void LerpConfigure(){
        startSpeed = currentSpeed;
        distance = Vector3.Distance(targetSpeed, startSpeed);
        remainingDistance = distance;
    }

    void OnRestart(){
        distancedTraveled = 0;
        currentGas = maxGas;
    }

    void OnStart(){
        Initialize();

        currentSpeed = new Vector3(0, 0, -GameManager.Instance.playerStartSpeed);
        StopChange();
    }

    void OnDeath(){
        engineAudioOff.masterVolume = 0;
        engineAudioOn.masterVolume = 0;
    }

    public float GetDistanceTravelled(){
        return distancedTraveled;
    }
}

public enum GasStates{
    Neutral,
    Accelerating,
    FullSpeed
}
