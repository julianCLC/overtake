using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props : MonoBehaviour
{
    [SerializeField] Vector2 bounds;
    [Tooltip("Used for modiying the bounds and prop is flipped 180")]
    [SerializeField] float flippedOffset;

    // Restart position
    Vector3 initPosition;

    public Action onLeavePlayArea;

    void OnEnable(){
        GameManager.onReset += OnGameStart;
    }

    void OnDisable(){
        GameManager.onReset -= OnGameStart;
    }

    void Awake(){
        initPosition = transform.position;

        // Modify bounds for props that are flipped
        if(Mathf.Abs(transform.rotation.eulerAngles.y) == 180){
            bounds -= new Vector2(flippedOffset, flippedOffset);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.isPaused){
            transform.position += PlayerTracker.Instance.currentSpeed * Time.deltaTime;
            OnReachBounds();
        }
        
    }

#region  Game Logic

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

#endregion

    void OnGameStart(){
        transform.position = initPosition;
    }
}
