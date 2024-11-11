using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWheels : MonoBehaviour
{
    public PlayerTracker car;
    public Transform[] wheels;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(car != null){
                foreach(Transform wheel in wheels){
                wheel.Rotate(car.currentSpeed.magnitude * Time.deltaTime * 20, 0, 0);
            }
        }
        
    }
}
