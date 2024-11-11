using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandIKTargets : MonoBehaviour
{

    public Transform leftHandPos;
    public Transform rightHandPos;

    public Transform leftHandIkTarget;
    public Transform rightHandIkTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leftHandIkTarget.position = leftHandPos.position;
        leftHandIkTarget.rotation = leftHandPos.rotation;

        rightHandIkTarget.position = rightHandPos.position;
        rightHandIkTarget.rotation = rightHandPos.rotation;
    }
}
