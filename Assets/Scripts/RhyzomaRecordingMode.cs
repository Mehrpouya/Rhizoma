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


    private List<RhizomaRetainer> RMemory = new List<RhizomaRetainer>();
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
public void SaveStringToText()
{
    // Use the CSV generation from before
    var content = TranslateMemoryToString();

    // The target file path e.g.
#if UNITY_EDITOR
    var folder = Application.streamingAssetsPath;

    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
#else
    var folder = Application.persistentDataPath;
#endif

    var filePath = Path.Combine(folder, "composition" + System.DateTime.Now + ".csv");

    using (var writer = new StreamWriter(filePath, false))
    {
        writer.Write(content);
    }

    // Or just
    //File.WriteAllText(content);

    Debug.Log($"CSV file written to \"{filePath}\"");

#if UNITY_EDITOR
    AssetDatabase.Refresh();
#endif
}
}
