using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;
using TMPro;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] TMP_Text[] names;
    [SerializeField] TMP_Text[] scores;
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] TMP_InputField _nameInputField;
    [SerializeField] Button submitButton;

    void OnEnable(){
        submitButton.onClick.AddListener(UploadEntry);
        GameManager.onDeath += GetEntries;
    }

    void OnDisable(){
        submitButton.onClick.RemoveListener(UploadEntry);
        GameManager.onDeath -= GetEntries;
    }

    void GetEntries(){
        LoadingScreen.SetActive(true);
        for(int i = 0; i < names.Length; i++){
            names[i].text = "";
            scores[i].text = "";
        }

        Leaderboards.OvertakeLeaderboard.GetEntries(entries => {
             LoadingScreen.SetActive(false);

            var length = Mathf.Min(names.Length, entries.Length);

            for(int i = 0; i < length; i++){
                names[i].text = entries[i].Username;
                scores[i].text = entries[i].Score.ToString() + "m";
            }
        });
    }

    void UploadEntry(){
        Debug.Log("Submitting: name: " + _nameInputField.text.Substring(0,3) + " | score: " + (int)PlayerTracker.Instance.GetDistanceTravelled());
        Leaderboards.OvertakeLeaderboard.UploadNewEntry(_nameInputField.text.Substring(0,3), (int)PlayerTracker.Instance.GetDistanceTravelled(), isSuccessful =>{
            if(isSuccessful){
                GetEntries();
            }
        });
    }
}
