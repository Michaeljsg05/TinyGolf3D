using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/*! 
 *  \brief     Level Screen Class
 *  \details   The Level Screen Class will handle instantiating all of the levels that are loaded through the SaveManagerSystem
 *  \author    Michael sansom
 *  \version   1.1a
 *  \date      19/11/20
 */
public class LevelScreen : MonoBehaviour
{
    public SaveManagement saveManager;
    public List<string> levels;
    public GameObject SavePanelPrefab;

    public List<LevelPanel> panels;

    bool work;
    // Start is called before the first frame update
    void Start()
    {
        saveManager = GameObject.Find("SaveManagement").GetComponent<SaveManagement>();
        // find and get the save management object script
        work = true;
        levels = saveManager.levels;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (work)
        {
            UpdatePanels();

            work = false;
        }
    }
    
    public void UpdatePanels()
    {
        Debug.Log("UpdatingPanels");
        levels = saveManager.levels;
        for (int i = 0; i < levels.Count; ++i)
        {
            if (i != 0)
            {
                string[] t_info = levels[i].Split(' ');
                // instantiate a prefab and use the information provided;
                // interested in iterators 1 and 3
                // 1 is the level number and 3 is the lock state
                GameObject t_newPanel = Instantiate(SavePanelPrefab);
                t_newPanel.transform.SetParent(this.transform);

                t_newPanel.GetComponent<LevelPanel>().levelNumber = int.Parse(t_info[1]);
                //6
                if(t_info[6] == "none")
                {
                    // we dont want to set the score to what is here
                    t_newPanel.GetComponent<LevelPanel>().bestScore = -1;
                }
                else
                {
                    Debug.Log("NewScoreFound!");
                    // we want to set it because it is not none
                    // parse the number
                    try
                    {
                        int t_BestScore = int.Parse(t_info[6]);
                        Debug.Log(t_BestScore);
                        // set the best score in the panel
                        t_newPanel.GetComponent<LevelPanel>().bestScore = t_BestScore;
                    }
                    catch(System.Exception e)
                    {
                        Debug.Log("<color=red> OPERATION FAILED: " + e + " | IF problem persists please contact a developer/programmer.");
                    }
                }

                if (t_info[3] == "false")
                {
                    t_newPanel.GetComponent<LevelPanel>().isLocked = true;
                }
                else if (t_info[3] == "true")
                {
                    t_newPanel.GetComponent<LevelPanel>().isLocked = false;
                }
                t_newPanel.transform.localScale = new Vector3(1, 1, 1);
                panels.Add(t_newPanel.GetComponent<LevelPanel>());
            }
        }
    }

}
