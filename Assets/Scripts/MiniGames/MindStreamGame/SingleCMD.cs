using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using MiniGames;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class SingleCMD : MonoBehaviour
{
    public const string Prefix = "<mindStream>:";
    
    public TMP_Text text;

    public CMDShowType CmdShowType;
    // Start is called before the first frame update
    
    public string baseString;

    public int totalNeedCharCount =>  baseString?.Length ?? 0;

    public int hasCurrectChar;
    
    public int point;

    public string inputString;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = GetFinalOutputString();
    }

    public void Setup(CMDShowType cmdShowType, string baseString)
    {
        this.CmdShowType = cmdShowType;
        this.baseString = baseString;
        hasCurrectChar = 0;
        point = 0;
        inputString = string.Empty;
    }

    string GetFinalOutputString()
    {
        switch (CmdShowType)
        {
            case CMDShowType.Input:
                //替换
                //在point前的被转换成input
                var middle =canSkipToNext() ?"":  $"<color=#66ccff>{GetEdittingChar(baseString[Math.Clamp(point,0,baseString.Length-1)])}</color>" ;
                //这里处理下下标
                var back =$"<alpha=#80>{baseString[Math.Clamp(point+1,0,baseString.Length)..]}";
                 //   ""; // 红色半透明警告
                return  $"<color=#007d3b>{ Prefix}</color>"+ ProcessInput()+middle+ $"<color=#007d3b>{ back}</color>";
            case CMDShowType.Output:
                //直接输出就好了
                return $"<color=#007d3b>{ Prefix + baseString}</color>";
            case CMDShowType.HoldOn:
                return $"<color=#007d3b>{Prefix + GetLoadingChar()}</color>" ;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Input(char inputChar)
    {
        if (CmdShowType == CMDShowType.Input && !canSkipToNext())
        {
            point++;
            inputString += inputChar;
        }
    }

    public void BackSpace()
    {
        if (CmdShowType == CMDShowType.Input && !canSkipToNext())
        {
            point = Math.Clamp(point - 1, 0, totalNeedCharCount);
            inputString = inputString.Remove(inputString.Length - 1, 1);
        }
    }

    public float GetPercentOfProgress()
    {
        int sam = 0;
        for (int i = 0; i < inputString.Length && i < baseString.Length && i < point; i++)
        {
            if (inputString[i] == baseString[i])
            {
                sam++;
            }
        }
        return sam / (float)totalNeedCharCount;
    }

    private string ProcessInput()
    {
        string result = "";
        for (int i = 0; i < inputString.Length && i < baseString.Length && i < point; i++)
        {
            if (inputString[i] == baseString[i])
            {
                result +=$"<color=#007d3b>{inputString[i]}</color>";
            }
            else
            {
                result += $"<color=#FF000080>{inputString[i]}</color>";
            }
            
        }
        return result;
    }


    private string GetLoadingChar()
    {
        char inputChar = (Time.time % 1f) switch
        {
            >= 0 and <= 0.25f => (char)('|'),
            > 0.25f and <= 0.5f => (char)('\\'),
            > 0.5f and <= 0.75f => (char)('-'),
            _ => (char)('/'),
        };
        return inputChar.ToString();
    }

    private string GetEdittingChar(char charToEdit)
    {
        return (Time.time % 1f) switch
        {
            >= 0 and <= 0.7f => charToEdit.ToString(),
            _ =>charToEdit == ' ' ? "░" : $"<u>{charToEdit}</u>",
        };
    }

    public bool canSkipToNext()
    {
        return point >= totalNeedCharCount;
    }
}
