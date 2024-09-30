using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerWinPage : MonoBehaviour
{
    private void OnEnable()
    {
        Education.EducationLayerOff();
        Destroy(this);
    }
    
}
