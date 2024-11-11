using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEngineAudio : MonoBehaviour
{
    public EngineNote[] engineNotes;
    public float rpm;
    public float masterVolume;

    static float[] workingVolumes = new float[3]; // or maximum number of engine notes you need

    private void Update()
    {
        // The total volume calculated for all engine notes won't generally sum to 1.
        // Calculate what they do sum to and then scale the individual volumes to ensure
        // consistent volume across the RPM range.
        float totalVolume = 0f;
        for(int i = 0; i < engineNotes.Length; ++i)
        {
            totalVolume += workingVolumes[i] = engineNotes[i].SetPitchAndGetVolumeForRPM(rpm);
        }

        if (totalVolume > 0f)
        {
            for (int i = 0; i < engineNotes.Length; ++i)
            {
                engineNotes[i].SetVolume(masterVolume * workingVolumes[i] / totalVolume);
            }
        }
    }

}
