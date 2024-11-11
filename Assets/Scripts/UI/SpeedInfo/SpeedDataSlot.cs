using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedDataSlot : MonoBehaviour
{
    public TMP_Text lane;
    public TMP_Text carSpeed;
    public TMP_Text targetSpeed;
    public TMP_Text currentSpeed;

    public void SetText(string _lane, string _carSpeed, string _target, string current){
        lane.text = _lane;
        carSpeed.text = _carSpeed;
        targetSpeed.text = _target;
        currentSpeed.text = current;
    }
}
