using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerCarProperties : ScriptableObject
{
    public float carSpeed = 1f;
    public float neutralSpeed = 0.0005f; // rate of change when not speeding up or slowing down
    public float accelerateSpeed = 0.004f; // rate of change when speeding up
    public float decelerateSpeed = 0.015f; // rate of change when slowing down

}
