using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarsManager : MonoBehaviour
{
    public LaneCarInfo[] laneCarInfos;
    [SerializeField] Material[] carMaterials;
    Material currentMaterial;
    // public float maxSpeed = Mathf.NegativeInfinity;

    public float startSpeed;

    List<Car> cars = new List<Car>();

    [Header("Debug")]
    [SerializeField] Car lane1Car;
    [SerializeField] Car lane2Car;
    [SerializeField] Car lane3Car;
    [SerializeField] SpeedDataSlot slot1;
    [SerializeField] SpeedDataSlot slot2;
    [SerializeField] SpeedDataSlot slot3;
    [SerializeField] TMP_Text playerSpeedData;

    public static CarsManager Instance {get; private set;}

    void Awake(){
        if (Instance != null && Instance != this){ 
            Destroy(this); 
        } 
        else{ 
            Instance = this; 
        } 
    }

    void OnEnable(){
        GameManager.onReset += OnRestart;
        // GameManager.onStart += OnGameStart;
        // GameManager.onSetup += OnRestart;
    }

    void OnDisable(){
        GameManager.onReset -= OnRestart;
        // GameManager.onStart += OnGameStart;
        // GameManager.onSetup -= OnRestart;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setup manager properties
        RandomCarMaterial();

        // SetupCarInfos();

        // LaneCarInfo[] laneCarInfos = GameManager.Instance.laneCarInfos;

        // Start speed should be the speed of the lane the player starts in
        // startSpeed = laneCarInfos[1].carMoveSpeed;

        // Configure cars
        GameObject[] carsGO = GameObject.FindGameObjectsWithTag("Car");
        foreach(GameObject carGO in carsGO){
            Car car = carGO.GetComponent<Car>();

            // maxSpeed = Mathf.Max(car.carMoveSpeed, maxSpeed);

            // LaneCarInfo laneCarInfo = laneCarInfos[car.lanePosition - 1];

            car.Initialize();

            cars.Add(car);
        }

        // maxSpeed *= 2f;

        // Debug.Log("Maxspeed: " + maxSpeed);
    }
    
    void Update(){
        CarSpeedInfo();
    }
    
    void RandomCarMaterial(){
        currentMaterial = carMaterials[UnityEngine.Random.Range(0, carMaterials.Length)];
    }

    public Material GetCarMaterial(){
        RandomCarMaterial();
        return currentMaterial;
    }

    void CarSpeedInfo(){
        playerSpeedData.text = GameManager.Instance.playerCarProperties.carSpeed.ToString();

        slot1.SetText(lane1Car.lanePosition.ToString(), lane1Car.carMoveSpeed.ToString(), lane1Car.targetSpeed.z.ToString(), lane1Car.currentSpeed.z.ToString());
        slot2.SetText(lane2Car.lanePosition.ToString(), lane2Car.carMoveSpeed.ToString(), lane2Car.targetSpeed.z.ToString(), lane2Car.currentSpeed.z.ToString());
        slot3.SetText(lane3Car.lanePosition.ToString(), lane3Car.carMoveSpeed.ToString(), lane3Car.targetSpeed.z.ToString(), lane3Car.currentSpeed.z.ToString());
    }

    /*
    void SetupCarInfos(){
        LaneCarInfo[] laneCarInfos = GameManager.Instance.laneCarInfos;

        for(int i = 0; i < laneCarInfos.Length; i++){
            laneCarInfos[i].playerCarProperties = GameManager.Instance.playerCarProperties;
        }
    }
    */

    public LaneCarInfo GetLaneCarInfo(int lanePos){
        return laneCarInfos[lanePos - 1];
    }

    public void OnRestart(){
        // LaneCarInfo[] laneCarInfos = GameManager.Instance.laneCarInfos;

        foreach(Car car in cars){

            // LaneCarInfo laneCarInfo = laneCarInfos[car.lanePosition - 1];

            //car.Initialize(laneCarInfo);

            car.OnRestart();
        }
    }

    public void OnGameStart(){
        // LaneCarInfo[] laneCarInfos = GameManager.Instance.laneCarInfos;

        foreach(Car car in cars){

            car.Initialize();

            car.OnGameStart();
        }
    }
    

}

[Serializable]
public struct LaneCarInfo {
    public int lanePosition;
    public float carMoveSpeed;
    // [HideInInspector] public Vector3 currentSpeed;
    // [HideInInspector] public float speedDelta;
    // [HideInInspector] public PlayerCarProperties playerCarProperties;

}