using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public PlayerController playerController;
    [SerializeField] Collider hitbox;
    [SerializeField] LayerMask carsLayer;
    Bounds playerBounds;
    bool isActive = true;

    public static Action onPlayerHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable(){
        playerController.onStart += OnStart;
    }

    void OnDisable(){
        playerController.onStart -= OnStart;

    }

    private void OnStart()
    {
        // prevent triggering hitbox before teleporting
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart(){
        yield return new WaitForSeconds(0.1f);
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive){
            CollisionDetection();
        }
    }

    
    void CollisionDetection(){
        playerBounds = hitbox.bounds;
        Collider[] colliders = Physics.OverlapBox(playerBounds.center, playerBounds.extents, transform.rotation, carsLayer, QueryTriggerInteraction.Ignore);
        if(colliders.Length > 0){
            foreach(Collider collider in colliders){
                Debug.Log("Hit " + collider.name);
                onPlayerHit?.Invoke();
                isActive = false;
                return;
            }
        }
    }

    /*void OnTriggerEnter(Collider other){
    if(other.CompareTag("Car")){
            Debug.Log("Hit Car! (" + other.name + ")");
        }
    }
    */

    /*
    void OnCollisionEnter(Collision collision){
        if(collision.transform.CompareTag("Car")){
            Debug.Log("Hit Car! (" + collision.transform.name + ")");
        }
    }
    */
    
}
