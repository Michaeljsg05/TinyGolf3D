using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionManager : MonoBehaviour
{
    public string URL = "";
    public string curVersion;
    string latestversion;
    public GameObject NewVersionAvaliable;

    private void Start()
    {
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
