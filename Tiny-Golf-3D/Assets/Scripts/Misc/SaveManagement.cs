using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;

using TMPro;


/*! 
 *  \brief     Save Management Class
 *  \details   The save management class will handle all the save files input and output.
 *  \author    Michael sansom
 *  \version   1.1a
 *  \date      19/11/20
 */



public class SaveManagement : MonoBehaviour
{

    /*! \brief InitFiles will initiate all the required files.
   * 
   *  Initiate Files function will be used to check if the required files can be found and if not set them up with the base values that the starting player
   *  will require. 
   *  
   *  -- BASIC: InitFiles takes care of file set up and makes sure client can find files.
   */
    public List<string> levels;
    string levelPath;
    

    void InitFiles()
    {
        // start by getting our data subdirectory

#if UNITY_ANDROID
        string dataDirectory = Application.persistentDataPath + "/private_data";
#endif
#if UNITY_STANDALONE_WIN
        string dataDirectory = Application.dataPath + "/private_data";
#endif
        Debug.Log("Checking: <color=green>" + dataDirectory + "</color> At: <color=green>" + System.DateTime.Now + "</color>");
        if (!Directory.Exists(dataDirectory))
        {
            Debug.Log("<color=red>ERROR: directory " + dataDirectory + " could not be found. recreating now...</color>");
            // if the data directory does not exist create it...
            Directory.CreateDirectory(dataDirectory);
            Debug.Log("<color=green>Created</color>");
        }
        else
        {
            Debug.Log("Directory:<color=green> " + dataDirectory + "</color> was found!");
        }


        // file paths
        string levelDataPath = dataDirectory + "/levels.txt";

        if (!File.Exists(levelDataPath))
        {
            Debug.Log("<color=red>ERROR: file " + levelDataPath + "could not be found. attempting to create now...</color>");
            try
            {
                Debug.Log("WritingFiles");
                File.WriteAllText(levelDataPath, "- Level data information -\n");
                // setup the content of file
                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
                {
                    // first time setup so lock all levels but level one
                    if (i == 1) // levl 1
                    {
                        string t_content = "Level " + i + " = true BestScore = none \n";
                        File.AppendAllText(levelDataPath, t_content);
                    }
                    else if (i != 0)
                    {
                        string t_content = "Level " + i + " = false BestScore = none \n";
                        File.AppendAllText(levelDataPath, t_content);
                    }
                }

                Debug.Log("FilesWritten");
            }
            catch (System.Exception e)
            {
                Debug.Log("<color=red>Not working: " + e + "</color>");
            }
        }
        else
        {
            Debug.Log("File: <color=green>" + levelDataPath + "</color> was found!");
        }
        levelPath = levelDataPath;

        // create required files if they do not exist
        // if they do exist process the individual files
    }

    public void SaveLevels()
    {
        File.WriteAllLines(levelPath, levels);
    }

    // process level load file stuff
    void ProcessLevels()
    {
        string[] lines = File.ReadAllLines(levelPath);
        for(int i = 0; i < lines.GetLength(0); ++i)
        {
            Debug.Log(i + " | " + lines[i]);
            levels.Add(lines[i]);
        }
    }

    public void UnlockLevel(string a)
    {
        if(int.Parse(a) > levels.Count)
        {
            return;
        }
        for(int i = 0; i < levels.Count; ++i)
        {
            // split string
            string[] t_info = levels[i].Split(' ');
            // check string
            if(t_info[1] == a)
            {
                // set the false to true
                t_info[3] = "true";

                //string t_reBuilt = t_info[0] + " " + t_info[1] + " " + t_info[2] + " " + t_info[3] + " " + t_info[4] + " " + t_info[5] + " " + t_info[6];
                string t_reBuilt = "";
                for(int b = 0; b < t_info.Count(); b++)
                {
                    t_reBuilt += t_info[b] + " ";
                }
                Debug.Log("<color=green>Rebuild string: " + t_reBuilt + "</color>");

                levels[i] = t_reBuilt;
                File.WriteAllLines(levelPath, levels);

                break;
            }
            // if it is equal then put the string together 
            // re-set levels[i]
            // rewrite file
            // break loop
        }
    }

    //! awake function will be called when the object awakens
    public void Awake()
    {
        DontDestroyOnLoad(this);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }


    //! start function will be called on the scene start
    void Start()
    {
        InitFiles();
        // files are definetly there now process them
        ProcessLevels();
    }

    //! Update is called once per frame
    void Update()
    {

    }
}
