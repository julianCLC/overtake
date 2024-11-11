using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MasterVolume : MonoBehaviour
{
    public AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Mute(){
        mixer.SetFloat("Vol", -80f);
    }

    public void Unmute(){
        mixer.SetFloat("Vol", 0f);
    }
}
