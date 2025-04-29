using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicStarter : MonoBehaviour
{
    [SerializeField] private AudioClip menuTheme;  

    void Start()
    {
        if (menuTheme != null)
        {
            AudioSource src = AudioManager.Instance.GetComponent<AudioSource>();
            if (!src.isPlaying || src.clip != menuTheme)
            {
                src.clip = menuTheme;
                src.Play();
            }
        }
    }
}

