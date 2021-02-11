using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

/*! 
 *  \brief     Flush save files class
 *  \details   This is an editor tool that will help with removing and resetting save files.
 *  \author    Michael sansom
 *  \version   1.0a
 *  \date      10/12/2020
 */

public class FlushSaveFiles : EditorWindow
{
    [MenuItem("Window/FlushSaveFiles")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<FlushSaveFiles>("Flush Save Files");
    }

    private void OnGUI()
    {
        GUILayout.Label("Flush the save files:", EditorStyles.centeredGreyMiniLabel);
        GUILayout.Space(20);
        if (GUILayout.Button("Refresh asset database"))
        {
            UnityEditor.AssetDatabase.Refresh();
        }
        GUILayout.Space(15);
        if (GUILayout.Button("Delete all save files"))
        {
            // delete all the save files
            File.Delete(Application.dataPath + "/private_data/levels.txt");
            File.Delete(Application.dataPath + "/private_data/levels.meta");

            UnityEditor.AssetDatabase.Refresh();
        }
        GUILayout.Space(15);
        if (GUILayout.Button("Delete Level save files"))
        {
            // delete Level the save files
            File.Delete(Application.dataPath + "/private_data/levels.txt");
            File.Delete(Application.dataPath + "/private_data/levels.meta");

            UnityEditor.AssetDatabase.Refresh();
        }
        GUILayout.Space(15);
    }
}
