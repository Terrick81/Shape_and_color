using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public enum nameClip
{
    win,
    defeat,
    good,
    uncorrect,
}
public class SFXManager: MonoBehaviour
{



    [SerializeField] private AudioSource _audioSourse;
    [SerializeField] private AudioClip[] _clickClip;
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private AudioClip[] _drops;
    [SerializeField] private AudioClip _take;

    private int _lenghtArr;
    private static SFXManager _sfxManager;
    void Start()
    {
        _sfxManager = GetComponent<SFXManager>();
        _lenghtArr = _clickClip.Length;
    }
    
    public void PlayClickClip()
    {
        if (_audioSourse.isPlaying == false)
            _audioSourse.PlayOneShot(_clickClip[Random.Range(0, _lenghtArr)]);
    }

    public void PlayClipWithStop(nameClip name) 
    {
        _audioSourse.Stop();
        _audioSourse.PlayOneShot(_clips[(int)name]);
    }

    public static void Play(nameClip name)
    {
        _sfxManager.LocalPlayClip(name);
    }

    private void LocalPlayClip(nameClip name)
    {
        _audioSourse.PlayOneShot(_clips[(int)name]);
    }

    public void PlayTake()
    {
        _audioSourse.PlayOneShot(_take);
    }

    public void PlayDrop()
    {
        _audioSourse.PlayOneShot(_drops[Random.Range(0, _drops.Length)]);
    }
}
