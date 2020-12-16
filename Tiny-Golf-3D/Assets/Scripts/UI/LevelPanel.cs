using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
/*! 
 *  \brief     Level Panel Class
 *  \details   The Level Panel Class will handle the panels for each selectable level
 *  \author    Michael sansom
 *  \version   1.1a
 *  \date      19/11/20
 */
public class LevelPanel : MonoBehaviour
{
    public bool isLocked;
    public GameObject lockImage;
    public TMP_Text panelText;
    public TMP_Text BestScoreText;
    public int levelNumber = -1;
    public int bestScore = -1;
    public SaveManagement sve;

    private void Start()
    {
        panelText.text = levelNumber.ToString();
        sve = GameObject.Find("SaveManagement").GetComponent<SaveManagement>();

    }

    // Update is called once per frame
    void Update()
    {
        if (bestScore < 0)
        {
            BestScoreText.text = "Best Score: none";
        }
        else
        {
            BestScoreText.text = "Best Score: " + bestScore;
        }
        if (isLocked && !lockImage.activeSelf)
        {
            // if the panel should be locked and the lock is not active:
            // enable the lock image
            lockImage.SetActive(true);

        }
        if (!isLocked && lockImage.activeSelf)
        {
            lockImage.SetActive(false);
        }
        string[] t_split = sve.levels[levelNumber].Split(' ');
        if (t_split[3] == "false")
        {
            isLocked = true;
        }
        else
        {
            isLocked = false;
        }
    }
    public void LoadScene()
    {
        if (!isLocked)
        {
            GameObject.Find("Fader").GetComponent<Animator>().SetTrigger("FadeOut");
            StartCoroutine(Sleep(3, levelNumber));
        }
    }
    public void LoadScene(string sceneName)
    {
        if (!isLocked)
        {
            GameObject.Find("Fader").GetComponent<Animator>().SetTrigger("FadeOut");
            StartCoroutine(Sleep(3, sceneName));
        }

    }
    public IEnumerator Sleep(float delay, int i)
    {
        Debug.Log("Sleep started..");
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(i);
        Debug.Log("It has been " + delay + " seconds!");
    }
    public IEnumerator Sleep(float delay, string name)
    {
        Debug.Log("Sleep started..");
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(name);
        Debug.Log("It has been " + delay + " seconds!");
    }
}
