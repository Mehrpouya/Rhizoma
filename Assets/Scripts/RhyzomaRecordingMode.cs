using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RhyzomaRecordingMode :MonoBehaviour
{


    public List<RhizomaRetainer> RMemory = new List<RhizomaRetainer>();
    // Start is called before the first frame update

    public void Record_RMemory(Vector3 _position, float _volume) { 
      RMemory.Add(new RhizomaRetainer(_position, _volume));
    }

    public string TranslateMemoryToString()
    {
        var sb = new StringBuilder("Localtion,Volume");
        foreach (var frame in RMemory)
        {
            sb.Append('\n').Append(frame.memoryPlace.ToString()).Append(',').Append(frame.volume.ToString());
        }

        return sb.ToString();
    }

    public void ReadData(string fileName, ref float interval)
    {
        
        // The target file path e.g.
#if UNITY_EDITOR
        var folder = Application.streamingAssetsPath;

        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
#else
        var folder = Application.persistentDataPath;
#endif
        RMemory.Clear();
        var filePath = Path.Combine(folder, fileName);
        string dataToLoad = "";
        using  (FileStream stream = new FileStream(filePath,FileMode.Open))
        {
            using (var reader = new StreamReader(stream))
            {
                
                interval = float.Parse(reader.ReadLine());
                while (true)
                {
                    string ln = reader.ReadLine();
                    if (ln == "endl")
                        break;
                    if (ln == ";")
                    {
                        RMemory.Add(JsonUtility.FromJson<RhizomaRetainer>(dataToLoad));
                        
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
public void SaveStringToText(string fileName ,float interval)
{
    // Use the CSV generation from before
    string content="";
    content += interval.ToString();
    content += '\n';
    foreach (var con in RMemory)
    {
        content += JsonUtility.ToJson(con, true);
        content += "\n";
        content += ";\n";
    }
    
    content += "endl";
    // The target file path e.g.
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

    Debug.Log($"CSV file written to \"{filePath}\"");

#if UNITY_EDITOR
    AssetDatabase.Refresh();
#endif
}
}
