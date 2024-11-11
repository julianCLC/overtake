using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// USE THIS AS BASE FOR PROPS AND CAR WHEN NEEDED
public class BaseObject : MonoBehaviour
{

    
    [SerializeField] Vector2 bounds;

    float moveSpeed;

    // Handles rate of change
    float currentDelta;
    float speedCoastDelta;
    float speedUpDelta;
    float slowDownDelta;

    // For lerping speed
    public Vector3 startSpeed;
    public Vector3 targetSpeed;
    public Vector3 currentSpeed = Vector3.zero;
    float distance;
    float remainingDistance;

    // Restart position
    Vector3 initPosition;

    public Action onLeavePlayArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Initialize(){
        initPosition = transform.position;

        PlayerCarProperties playerCarProperties = GameManager.Instance.playerCarProperties;

        speedCoastDelta = playerCarProperties.neutralSpeed;
        speedUpDelta    = playerCarProperties.accelerateSpeed;
        slowDownDelta   = playerCarProperties.decelerateSpeed;
        moveSpeed       = playerCarProperties.carSpeed;

        currentSpeed = new Vector3(0, 0, GameManager.Instance.playerStartSpeed);
        StopChange();
    }

    void OnEnable(){
        PlayerController.onSpeedUpStart += SpeedUpStart;
        PlayerController.onSlowDownStart += SlowDownStart;
        PlayerController.onStop += StopChange;

        // GameManager.onSetup += OnRestart;
    }

    void OnDisable(){
        PlayerController.onSpeedUpStart -= SpeedUpStart;
        PlayerController.onSlowDownStart -= SlowDownStart;
        PlayerController.onStop -= StopChange;

        // GameManager.onSetup -= OnRestart;
    }

    // Update is called once per frame
    void Update()
    {
        SpeedHandler();
        OnReachBounds();
    }

    void SpeedHandler(){
        if(distance > 0){
            currentSpeed = Vector3.Lerp(startSpeed, targetSpeed, 1 - (remainingDistance / distance));
            // remainingDistance -= carMoveSpeed * currentDelta * Time.deltaTime;
            remainingDistance -= moveSpeed * currentDelta * Time.deltaTime;

            transform.position += currentSpeed * Time.deltaTime;
        }
    }

    void SpeedUpStart(){
        targetSpeed = new Vector3(0, 0, -moveSpeed);
        currentDelta = speedUpDelta;

        LerpConfigure();
    }

    void SlowDownStart(){
        StopChange();
        currentDelta = slowDownDelta;
    }

    void StopChange(){
        targetSpeed = Vector3.zero;
        currentDelta = speedCoastDelta;

        LerpConfigure();
    }

    void LerpConfigure(){
        startSpeed = currentSpeed;
        distance = Vector3.Distance(targetSpeed, startSpeed);
        remainingDistance = distance;
    }

    void OnReachBounds(){
        if(transform.position.z <= bounds.y){
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, bounds.x);
            transform.position = newPos;
            onLeavePlayArea?.Invoke();
        }
        else if(transform.position.z > bounds.x){
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, bounds.y);
            transform.position = newPos;
            onLeavePlayArea?.Invoke();
        }
    }

    void OnRestart(){
        transform.position = initPosition;
        Initialize();

    }
}
