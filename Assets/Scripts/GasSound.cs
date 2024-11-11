using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSound : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayIndicator(){
        audioSource.Play();
    }
}
