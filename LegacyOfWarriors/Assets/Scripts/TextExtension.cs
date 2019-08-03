using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public static class TextExtension
{
    public static void SetRegularText(this Text textObj, string text)
    {
        textObj.color = Color.black;
        textObj.text = text;
    }

    public static void ResetText(this Text textObj)
    {
        textObj.text = "";
    }

    public static void SetSuccessText(this Text textObj, string text)
    {
        textObj.color = Color.green;
        textObj.text = text;
    }

    public static void SetErrorText(this Text textObj, string text)
    {
        textObj.color = Color.red;
        textObj.text = text;
    }
}
