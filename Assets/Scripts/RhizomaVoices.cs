using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using LeastSquares.Overtone;

    
    public class RhizomaVoices : MonoBehaviour
    {
    public TTSPlayer _player;
    public AudioSource source;




    // Start is called before the first frame update
    void Start()

        {
        //async void Speak(string text, TTSVoiceNative voice)
        //{
        //    try
        //    {
        //        // Asynchronous operation
                if (!_player.Engine.Loaded) return;
                _player.Speak("I always struggle with the start to be honest");
                //AudioClip audioClip = _player.source.clip;
                
                //source.clip = audioClip;
                //source.loop = false;
                //source.Play();
            //}
            //catch (Exception e)
            //{
            //    Debug.Log($"error : {e.Message}");
            //}
        }

        //voice.Dispose();
        ////async Task<AudioClip> Speak(string text, TTSVoiceNative voice);
        //if (!_player.Engine.Loaded) return;

        //string text = "Hello World!";
        //TTSVoiceNative voice = TTSVoiceNative.LoadVoiceFromResources("en-gb-semaine-medium");
        //await _player.Speak("Hi Hadi dear!" ?? string.Empty);
        //AudioClip audioClip = _player.source.clip;

        //source.clip = audioClip;
        //source.loop = false;
        //source.Play();

        //_player.Dispose();
        

    // Update is called once per frame
    void Update()
        {

        }
    }