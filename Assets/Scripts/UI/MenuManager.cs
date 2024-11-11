using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public CinemachineBrain cinemachineBrain;
    public CinemachineVirtualCamera menuCam;
    public CinemachineVirtualCamera gameCam;

    public PlayerTracker playerTracker;
    public GameObject failMenu;
    public TMP_Text distanceDisplay;

    public GameObject HUD;
    public GameObject pauseMenu;

    public static MenuManager Instance {get; private set;}

    void Awake(){
        if (Instance != null && Instance != this){ 
            Destroy(this); 
        } 
        else{ 
            Instance = this; 
        }
    }

    public void GameStartFromMenu(){
        menuCam.Priority = 0;
        gameCam.Priority = 10;

        StartCoroutine(WaitFinishBlend());
    }

    IEnumerator WaitFinishBlend(){
        yield return new WaitForEndOfFrame();
        Debug.Log("isBlending: " + cinemachineBrain.IsBlending);
        while(cinemachineBrain.IsBlending){
            yield return new WaitForEndOfFrame();
        }

        HUD.SetActive(true);
        pauseMenu.SetActive(true);
        GameManager.Instance.isTransitioning = false;
        // GameManager.Instance.StartGame();
    }

    public void OpenFailMenu(){
        HUD.SetActive(false);
        failMenu.SetActive(true);

        distanceDisplay.text = playerTracker.distancedTraveled.ToString("F0") + "m";
    }
}
