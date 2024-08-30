using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

using LeastSquares.Overtone;
using System.Collections;
using System.Collections.Generic;
using Assets.Overtone.Scripts;

public enum VoiceState
{
    idle,
    talking,
    finishedTalking
}


[Serializable]
public struct RhizomaSpeech
{
    public string text;
    public int voiceIndex;
    public float pitch;
    public float delay;

   
}



public class RhizomaVoices : MonoBehaviour
{
    public static VoiceState currentState;
    public TTSPlayer _player;
    public AudioSource source;

    public List<RhizomaSpeech> Speeches;

    private Task task;
    
    public delegate void StartTalkingDel(string text);
    public static StartTalkingDel StartTalkingDelegate;

    public delegate void FinishedTalkingDel();
    public static FinishedTalkingDel FinishedTalkingDelegate;

    
    string[] voices =
    {
        "en-gb-alan-medium",
        "en-gb-alba-medium",
        "en-gb-aru-medium",
        "en-gb-cori-high",
        "en-gb-jenny_dioco-medium",
        "en-gb-northern_english_male-medium",
        "en-gb-semaine-medium",
        "en-us-libritts-high",
        "fa-ir-amir-medium",
    };
    
    
    
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
        if (Input.GetKeyUp(KeyCode.L))
        {
           WriteToFile(ReadFromText("HadiText.txt"));
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            ReadFromFile();
            //StopCoroutine(ThinkIt());
            StartCoroutine(ThinkIt());
        }
    }

    IEnumerator ThinkIt()
    {
        // Prepare first audioclip
        PrepareIt(0);
        for (int i = 0; i < Speeches.Count; i++)
        {
            // Speak the perpared words
            SayIt(i);
            // Sync task with main thread
            yield return new WaitUntil(() => task.IsCompleted);
            
            // Talking has started, notify everyone!
            currentState = VoiceState.talking;
            if (StartTalkingDelegate != null)
                StartTalkingDelegate(Speeches[i].text);
            
            // Wait until this current clip has finished speaking
            yield return new WaitForSeconds(source.clip.length);
            currentState = VoiceState.finishedTalking;
            if (FinishedTalkingDelegate != null)
                FinishedTalkingDelegate();
            
            // Prepare next audioclip async before starting the delay
            if(i+1!=Speeches.Count)
                 PrepareIt(i+1);
            
            yield return new WaitForSeconds(Speeches[i].delay);
            currentState = VoiceState.idle;

        }

    }

    void PrepareIt(int id)
    {
        // Change voice according to speech requirement
        _player.Voice.voiceName = voices[Speeches[id].voiceIndex];
        _player.Voice.UpdateVoiceName();
        // Start async task to create the audioclip from tts
        task =  _player.Speak(Speeches[id].text);
    }
    async Task SayIt(int id)
    {
        // Sync task before moving onto setting the attributes
        await task;
        
        // Set audiosource clip and info from generated file
        AudioClip audioClip = _player.source.clip;
        source.clip = audioClip;
        source.pitch = Speeches[id].pitch;
        source.loop = false;
        source.Play();
    }



    void WriteToFile(string s)
    {
        Speeches.Clear();
        RhizomaSpeech rs = new RhizomaSpeech();
        rs.delay = 300;
        rs.pitch = 1;
        rs.voiceIndex = 3;
        foreach (var c in s)
        {

            if (c == '\n')
            {
                Speeches.Add(rs);
                rs = new RhizomaSpeech();
                rs.delay = 300;
                rs.pitch = 1;
                rs.voiceIndex = 3;
            }
            else
            {
                rs.text += c;
            }
                
        }

        // accomodating for edge cases
        if(s[s.Length-1] != '\n')
            Speeches.Add(rs);

       WriteListToJsonFile<RhizomaSpeech>(Speeches, "HadiSpeeches.json");

    }

    void ReadFromFile()
    {
        ReadFromJsonFile(ref Speeches,"HadiSpeeches.json");
    }
    
    void WriteListToJsonFile<T>(List<T> lst, string fileName)
    {
        string content = "";
        foreach (var speech in lst)
        {
            content += JsonUtility.ToJson(speech, true);
            content += "\n";
            content += ";\n";

        }
        content += "endl";
        
#if UNITY_EDITOR
        var folder = Application.streamingAssetsPath;

        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
#else
        var folder = Application.persistentDataPath;
#endif

        var filePath = Path.Combine(folder, fileName);

        using  (FileStream stream = new FileStream(filePath,FileMode.Create))
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(content);
            }
        }

        // Or just
        //File.WriteAllText(content);

        Debug.Log($"Json file written to \"{filePath}\"");

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
    
    void ReadFromJsonFile<T>(ref List<T> lst,string fileName)
    {
        
        // The target file path e.g.
#if UNITY_EDITOR
        var folder = Application.streamingAssetsPath;

        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
#else
        var folder = Application.persistentDataPath;
#endif
        lst.Clear();
        var filePath = Path.Combine(folder, fileName);
        string dataToLoad = "";
        using  (FileStream stream = new FileStream(filePath,FileMode.Open))
        {
            using (var reader = new StreamReader(stream))
            {
                while (true)
                {
                    string ln = reader.ReadLine();
                    if (ln == "endl")
                        break;
                    if (ln == ";")
                    {
                        lst.Add(JsonUtility.FromJson<T>(dataToLoad));
                        
                        dataToLoad = "";
                    }
                    else
                    {
                        dataToLoad += ln;
                    }
                    
                }
            }
        }

    }

    string ReadFromText(string fileName)
    {
        // The target file path e.g.
#if UNITY_EDITOR
        var folder = Application.streamingAssetsPath;

        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
#else
        var folder = Application.persistentDataPath;
#endif
        
        var filePath = Path.Combine(folder, fileName);
        string dataToLoad = "";
        using (FileStream stream = new FileStream(filePath, FileMode.Open))
        {
            using (var reader = new StreamReader(stream))
            {
                dataToLoad = reader.ReadToEnd();
                reader.Close();
            }
            
            stream.Close();

        }

        return dataToLoad;
    }
}