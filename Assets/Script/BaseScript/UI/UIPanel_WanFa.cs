using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System;
using LitJson;
using System.Runtime.InteropServices;

public class UIPanel_WanFa : UIWindow
{
    public GameObject img_sssPlay;
    public GameObject img_nnPlay;

    public void showNNPlay()
    {
        img_nnPlay.SetActive(true);
        img_sssPlay.SetActive(false);

    }

    public void showSSSPlay()
    {
        // img_nnPlay.SetActive(true);
        img_sssPlay.SetActive(true);
        img_nnPlay.SetActive(false);
    }
}

