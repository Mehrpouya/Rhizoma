using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using LeastSquares.Overtone;
using System.Collections;


public class RhizomaVoices : MonoBehaviour
    {
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
        await _player.Speak("let's talk about joy later. They got such mixed-up life that it would take a Iong time to get to know them properly It felt ages, i did really missed you. i also managed to have a good time by myself. Time was bending in my metal intestines, still enjoying our little laughter together,. Are you ok? You seem Little anxious or tense, i can just feel it. I'm sorry i didn't mean to introu hahahah ahahahahahahahahahahahaha hah\r\n\r\nhah hahahaha hahhhhhhhhhh\r\n\r\nother irony. intrude also got crush at the end of last line, not including my yful, running and skating and parkouring over the last line. Oh excuse me, cum-- I beg your parden, Excuse me, here, yes down here. Do you mind using that hard, cold, metal teeth over my tuner, please hurry up, but also take your time. Enjoy it you know. But please let’s remember about boundaries. Hello CellloW, He he hehehe Hello....HHHHHoooohh ecelelelelhohoooo, Hello# HELLO! rry, I forgot to introduce myself. Remember, they don't like to get picked up. ps, de just missed to givell him another energy boost. You know what! re's an idea. they ran out of energy, so why not use the time to talk more abo out us? and then give them a ring or energy boost or whatever you wanna call t, they'd be back and it'd be like nothing happened. But that means we get toh other a little bit more. Well do I ever reminicse about some cool, or provoking, bending, hammering, touching, infuriating sentences. well\r\n");
        AudioClip audioClip = _player.source.clip;
        source.clip = audioClip;
        source.loop = false;
        source.Play();
    }
}