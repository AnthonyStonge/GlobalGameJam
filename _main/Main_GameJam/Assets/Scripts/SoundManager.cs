﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    #region MonoSingleton
    private static SoundManager instance;

    public static SoundManager Instance { get { return instance; } }
    #endregion
    
    [Serializable]
    private struct audioFiller
    {
        public int id;
        public AudioClip clip;
    }
    [SerializeField] private audioFiller[] audioBiblio;
    public Dictionary<GameObject, AudioSource> audioSource;
    public Dictionary<int, AudioClip> dicSound;

    private void Awake()
    {
        
        #region MonoSingleton
        if (Instance != null)
        {
            Destroy(this.gameObject);
            //throw new System.Exception("An instance of this singleton already exists.");
            //On peut aussi faire un return ici
            //return;
        }
        else
        {
            instance = SoundManager.Instance;
            DontDestroyOnLoad(gameObject);
        }
        #endregion
        audioSource = new Dictionary<GameObject, AudioSource>();
        dicSound = new Dictionary<int, AudioClip>();
        foreach (audioFiller filler in audioBiblio)
        {
            dicSound.Add(filler.id, filler.clip);
        }
        //audioSource = GameObject.FindObjectsOfType<AudioSource>().ToDictionary(t => t.gameObject, t => t.GetComponent<AudioSource>());
        //Debug.Log("hh");
        instance = this;
    }

    public void AddAudioSource(GameObject obj, AudioSource source)
    {
        audioSource.Add(obj, source);
    }
    public void PlayLoopTimed(GameObject audioObj, int clipId, float playTime)
    {
        audioSource[audioObj].clip = dicSound[clipId];
        StartCoroutine(playTimed(audioSource[audioObj], playTime));
    }

    public void PlayOnce(GameObject audioObj, int clipId)
    {
        audioSource[audioObj].PlayOneShot(dicSound[clipId]);
    }

    public void PlayLoop(GameObject audioObj, int clipId)
    {
        audioSource[audioObj].clip = dicSound[clipId];
        audioSource[audioObj].loop = true;
        audioSource[audioObj].Play();
    }

    public void StopSound(GameObject audioObj)
    {
        audioSource[audioObj].Stop();
    }
    private IEnumerator playTimed(AudioSource source, float playTime)
    {
        source.Play();
        yield return new WaitForSeconds(playTime);
        source.Stop();
    }
    
}
