using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] Transform carModelsParent;
    [Tooltip("Bounds of cars x position before they teleport to other side")]
    [SerializeField] Vector2 bounds;

    [Header("Properties")]
    public int lanePosition; // Manually input this
    public float carMoveSpeed;
    float speedCoastDelta;
    float speedUpDelta;
    float slowDownDelta;

    public Vector3 startSpeed;
    public Vector3 targetSpeed;
    public Vector3 currentSpeed = Vector3.zero;
    public float speedDelta; // determines rate of change at any point (depends on player car)

    float playerSpeed;

    float remainingDistance;
    float distance = 1;

    // for restarting
    Vector3 initPosition;

    // Components
    MeshRenderer[] meshRenderers;

    void Awake(){
        // Configuring mesh renderers
        int totalMeshRenderers = carModelsParent.childCount;
        foreach(Transform carModel in carModelsParent){
            totalMeshRenderers += carModel.childCount;
        }
        meshRenderers = new MeshRenderer[totalMeshRenderers];

        int index = 0;
        foreach(Transform carModel in carModelsParent){
            meshRenderers[index] = carModel.GetComponent<MeshRenderer>();
            index++;
            foreach(Transform subCarModel in carModel){
                meshRenderers[index] = subCarModel.GetComponent<MeshRenderer>();
                index++;
            }
        }


    }

    void Start(){
        
    }

    public void Initialize(){
        initPosition = transform.position;

        // Aesthetics
        RandomizeCar();

        // Set deltas and playerSpeed
        LaneCarInfo info = CarsManager.Instance.GetLaneCarInfo(lanePosition);
        PlayerCarProperties playerCarProperties = GameManager.Instance.playerCarProperties;
        speedCoastDelta = playerCarProperties.neutralSpeed;
        speedUpDelta    = playerCarProperties.accelerateSpeed;
        slowDownDelta   = playerCarProperties.decelerateSpeed;
        playerSpeed     = playerCarProperties.carSpeed;
        carMoveSpeed    = info.carMoveSpeed;

        Debug.Log("startspeed: " + GameManager.Instance.playerStartSpeed);
        targetSpeed = new Vector3(0, 0, -GameManager.Instance.playerStartSpeed + carMoveSpeed);
        speedDelta = speedUpDelta;

        LerpConfigure();
    }

    void OnEnable(){
        PlayerController.onSpeedUpStart += SpeedUpStart;
        PlayerController.onSlowDownStart += SlowDownStart;
        PlayerController.onStop += StopChange;
    }

    void OnDisable(){
        PlayerController.onSpeedUpStart -= SpeedUpStart;
        PlayerController.onSlowDownStart -= SlowDownStart;
        PlayerController.onStop -= StopChange;
    }

    void Update(){
        if(!GameManager.Instance.isPaused){
            SpeedHandler();
            OnReachBounds();
        }
    }

    #region Game Logic

    void SpeedHandler(){
        if(distance > 0){
            currentSpeed = Vector3.Lerp(startSpeed, targetSpeed, 1 - (remainingDistance / distance));
            remainingDistance -= carMoveSpeed * speedDelta * Time.deltaTime;
        }

        transform.position += currentSpeed * Time.deltaTime;
    }

    void SpeedUpStart(){
        targetSpeed = new Vector3(0, 0, -playerSpeed + carMoveSpeed);
        speedDelta = speedUpDelta;

        LerpConfigure();
    }

    void SlowDownStart(){
        speedDelta = slowDownDelta;

        LerpConfigure();
    }

    void StopChange(){
        targetSpeed = new Vector3(0, 0, carMoveSpeed);
        speedDelta = speedCoastDelta;

        LerpConfigure();
    }

    void LerpConfigure(){
        startSpeed = currentSpeed;
        distance = Vector3.Distance(targetSpeed, startSpeed);
        remainingDistance = distance;
    }

    void OnReachBounds(){
        // back bound
        if(transform.position.z <= bounds.y){
            Vector3 newPos = transform.position;
            newPos.z = bounds.x;
            transform.position = newPos;
            RandomizeCar();
        }
        // front bound
        else if(transform.position.z >= bounds.x){
            Vector3 newPos = transform.position;
            newPos.z = bounds.y;
            transform.position = newPos;
            RandomizeCar();
        }
    }

    #endregion

    #region Car model
    public void RandomizeCar(){
        HideAllCarMesh();

        Transform newCarModel = carModelsParent.GetChild(UnityEngine.Random.Range(0, carModelsParent.childCount));

        newCarModel.gameObject.SetActive(true);

        Material newMaterial = CarsManager.Instance.GetCarMaterial();
        foreach(MeshRenderer mr in meshRenderers){
            if(!mr.transform.CompareTag("PP")){
                mr.material = newMaterial;
            }
        }
    }

    public void HideAllCarMesh(){
        foreach(Transform carModel in carModelsParent){
            carModel.gameObject.SetActive(false);
        }
    }
    #endregion

    public void OnGameStart(){
        currentSpeed = new Vector3(0, 0, carMoveSpeed - GameManager.Instance.playerStartSpeed);
        StopChange();
    }

    public void OnRestart(){
        transform.position = initPosition;
        RandomizeCar();
        OnGameStart();
    }
}
