using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheels : MonoBehaviour
{
    public Car car;
    public Transform[] wheels;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(car != null && !GameManager.Instance.isPaused){
                foreach(Transform wheel in wheels){
                wheel.Rotate(car.carMoveSpeed * Time.deltaTime * 20, 0, 0);
            }
        }
        
    }
}
