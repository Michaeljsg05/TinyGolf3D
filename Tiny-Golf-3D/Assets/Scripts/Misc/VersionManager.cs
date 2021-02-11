using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*! 
 *  \brief    Version Manager Class
 *  \details   The version manager class will handle the game version, it will compare the current game version against the games latest version found on the pastebin link.
 *  \author    Michael sansom
 *  \version   1.0b
 *  \date      19/12/20
 */

public class VersionManager : MonoBehaviour
{
    public string URL = "";
    public string curVersion;
    string latestversion;
    public GameObject NewVersionAvaliable;
    public TMP_Text versionText;

    private void Start()
    {
#if UNITY_STANDALONE_WIN
        versionText.text = "ALPHA " + curVersion + " (WINDOWS BUILD)";
#endif
#if UNITY_ANDROID
        versionText.text = "ALPHA " + curVersion + " (Android BUILD)";
#endif

        StartCoroutine(LoadTxtData(URL));
        
    }

    public void CheckVersion()
    {
        if (curVersion != latestversion)
        {
            NewVersionAvaliable.SetActive(true);
        }
        else
        {
            NewVersionAvaliable.SetActive(false);
        }
#if UNITY_EDITOR
        Debug.Log("Current version: " + curVersion + " Latest version: " + latestversion);
#endif
    }

    IEnumerator LoadTxtData(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        latestversion = www.text;
        CheckVersion();
    }

    public void OpenURL(string url)
    {
        // chekc if user is on android or on windows

        // if on android
#if UNITY_ANDROID
        Application.OpenURL(url);
#endif
        // if on windows
#if UNITY_STANDALONE_WIN
        Application.OpenURL(url);
#endif
    }

    public void CloseUpdatePanel()
    {
        NewVersionAvaliable.SetActive(false);
    }

}
