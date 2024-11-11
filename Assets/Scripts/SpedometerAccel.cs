using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpedometerAccel : MonoBehaviour
{
    [Range(-112.548f, 108.146f)]
    public float overrideRot;
    public Transform speedNeedleTransform;
    public Transform rpmNeedleTransform;
    public PlayerTracker playerTracker;

    const float ZERO_SPEED_ANGLE = -112.548f;
    const float MAX_SPEED_ANGLE = 108.146f;

    // Update is called once per frame
    void Update()
    {
        // float currSpeedNormalized = GetNeedleRotation(PlayerTracker.Instance.currentSpeed.magnitude / GameManager.Instance.playerCarProperties.carSpeed);
        speedNeedleTransform.localEulerAngles = new Vector3(0, GetNeedleRotation(PlayerTracker.Instance.currentSpeed.magnitude / GameManager.Instance.playerCarProperties.carSpeed), 0);
    
        // float currRPMNormalized = GetNeedleRotation()
    
    }

    float GetNeedleRotation(float val){
        float totalSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;
        return ZERO_SPEED_ANGLE - val * totalSize;
    }
}
