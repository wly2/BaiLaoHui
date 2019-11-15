using UnityEngine;

public class UIPanel_Withdrawal : UIWindow
{
    public GameObject kiting;
    public GameObject go;
    public GameObject game;

    public void ShowOne()
    {
        kiting.SetActive(true);
        go.SetActive(false);
        game.SetActive(false);
    }

    public void ShowTwo()
    {
        kiting.SetActive(false);
        go.SetActive(true);
        game.SetActive(false);
    }

    public void ShowThree()
    {
        kiting.SetActive(false);
        go.SetActive(false);
        game.SetActive(true);
    }
}