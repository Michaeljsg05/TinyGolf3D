using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwapText : MonoBehaviour
{

    public string initialText = "||";
    

    public void SwapTxT(string desText)
    {
        // get the tmptext
        TMP_Text btnText = GetComponentInChildren<TMP_Text>();
        if (btnText.text == desText)
            btnText.text = initialText;
        else
            btnText.text = desText;
    }
}
