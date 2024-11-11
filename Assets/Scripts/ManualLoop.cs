using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualLoop : MonoBehaviour
{
    AudioSource audioSource;
    // bool isPlaying = false;

    void Awake(){
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if(!isPlaying){
        // if(!audioSource.isPlaying && audioSource.time == 0f){
        //     audioSource.Play();
        //     // StartCoroutine(WaitForEnd());
        // }
    }

    IEnumerator WaitForEnd(){
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
    }
}
