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
    public TTSPlayer _player;
    public AudioSource source;

    public List<RhizomaSpeech> Speeches;

    
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
           WriteToFile("");
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            ReadFromFile();
            StopCoroutine(ThinkIt());
            StartCoroutine(ThinkIt());
        }
    }

    IEnumerator ThinkIt(float delay = 0.0f)
    {
        for (int i = 0; i < Speeches.Count; i++)
        {
            SayIt(i);
            print("checking "+ source.clip.length);
            yield return new WaitForSeconds(Speeches[i].delay + source.clip.length);
            
        }

    }
    async void SayIt(int id)
    {
        _player.Voice.voiceName = voices[Speeches[id].voiceIndex];
        _player.Voice.UpdateVoiceName();
        await _player.Speak(Speeches[id].text);
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

       WriteListToJsonFile<RhizomaSpeech>(Speeches, "speeches.json");

    }

    void ReadFromFile()
    {
        ReadFromJsonFile(ref Speeches,"speeches.json");
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
}