using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using LeastSquares.Overtone;
using System.Collections;


public class RhizomaVoices : MonoBehaviour
    {
    public string S = "Hi";
    public TTSPlayer _player;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
        {
        //        // Asynchronous operation
        if (!_player.Engine.Loaded) {
            Debug.Log("error!");
            return; 
        }
        }

    // Update is called once per frame
    void Update()
        {
        if (Input.GetKeyUp(KeyCode.L)){
            StartCoroutine(ThinkIt());
        }
        }
    IEnumerator ThinkIt()
    {
        SayIt();
        yield return new WaitForSeconds(5f);

    }
    async void SayIt() {
        await _player.Speak(S);
        AudioClip audioClip = _player.source.clip;
        source.clip = audioClip;
        source.loop = true;
        source.Play();
    }
}