// Places the selected Objects on the surface of a terrain.

using UnityEngine;
using UnityEditor;

public class AreYouSureButton : ScriptableObject
{
    
    public static bool CreateWizard()
    {

        return EditorUtility.DisplayDialog("Do you wish to save your composition?",
            "Save your composition?", "Yes, Save", "Do Not Save");

    }
}