using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeLights : MonoBehaviour
{
    [SerializeField] Props props;
    [SerializeField] GameObject[] lights;
    int places;
    void Start(){
        places = lights.Length;
    }

    void OnEnable(){
        props.onLeavePlayArea += OnLeavePlayArea;
    }

    void OnDisable(){
        props.onLeavePlayArea -= OnLeavePlayArea;
    }

    void OnLeavePlayArea(){
        // get random assortment of numbers
        float seed = Random.Range(0, 1f);

        // prepare for truncating
        seed *= Mathf.Pow(10, places);

        foreach(GameObject light in lights){
            // isolate num
            float num = seed % 10;
            
            // set active based on given number
            if(num > 5){
                light.SetActive(true);
            }
            else{
                light.SetActive(false);
            }

            // remove 
            seed = seed / 10f;
        }
    }

    
}
