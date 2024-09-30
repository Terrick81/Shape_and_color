using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    [SerializeField] private Animation _animation;
    void Start()
    {
        
    }
    
    public void PlayAnimate()
    {
        _animation.Play();
    }
}
