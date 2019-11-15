
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRoomOverItemNN : MonoBehaviour
{

    public Image headIcon;
    public Text nameText;
    public Text scoreText;   
    private int myId;
    public Image imgState;
    public Image img_Banker;

    public void showBanker()
    {
        img_Banker.gameObject.SetActive(true);
    }

    public void hideBanker()
    {
        img_Banker.gameObject.SetActive(false);
    }

    public void SetValue(int chairId)
    {
        long score = 0;
        if (GameDataSSS.Instance.isSss)
        {
            score = GameDataSSS.Instance.gameEnd.lGameScore[chairId];
            scoreText.text = score + "";
        }
        else
        {
            score = GameDataNN.Instance.gameEnd.lGameScore[chairId];
            scoreText.text = score + "";
        }

        //昵称
        nameText.text = GlobalDataScript.Instance.playersNameList[chairId].name;
#region
        //for (int i = 0; i < GlobalDataScript.Instance.playerInfos.Count; i++)
        //{
        //    ////昵称
        //    //if (GlobalDataScript.Instance.playerInfos[i].chairId == chairId)
        //    //{
        //    //    nameText.text = GlobalDataScript.Instance.playersNameList[i].name;
        //    //    myId = GlobalDataScript.Instance.playerInfos[i].userID;

        //    //}

        //    //庄家
        //    if (GameDataNN.Instance.gameStartInfo.wBankerUser == chairId
        //        && !GameDataSSS.Instance.isSss)
        //    {
        //        showBanker();
        //    }
        //    else
        //        hideBanker();

        //}
#endregion
        //庄家
        if (GameDataNN.Instance.gameStartInfo.wBankerUser == chairId
            && !GameDataSSS.Instance.isSss)
        {
            showBanker();
        }
        else
            hideBanker();

        if (GlobalDataScript.Instance.myGameRoomInfo.chairId != chairId)
            return;

        if (score > 0)//胜
        {
            ResourcesLoader.Load<Sprite>("ShareAssets/UI/UIPanel_GameOver/Victory", (sprite) =>
            {
                imgState.sprite = sprite;
            });

            SoundManager.Instance.PlaySoundBGM("win");
        }

        if (score == 0)//平
        {
            ResourcesLoader.Load<Sprite>("ShareAssets/UI/UIPanel_GameOver/peace", (sprite) =>
            {
                imgState.sprite = sprite;
            });
        }

        if (score < 0)//输
        {
            ResourcesLoader.Load<Sprite>("ShareAssets/UI/UIPanel_GameOver/Fail", (sprite) =>
            {
                imgState.sprite = sprite;
            });

            SoundManager.Instance.PlaySoundBGM("lose");
        }                                                                            

    }

    public void SetGameEndValue(int chairId)
    {
        long score = 0;
        if (GameDataSSS.Instance.isSss)
        {
            score = GameDataSSS.Instance.AllgameEnd.lScore[chairId];
            scoreText.text = score + "";

        }
        else
        {
            score = GameDataNN.Instance.AllgameEnd.lScore[chairId];
            scoreText.text = score + "";
        }

        nameText.text = GlobalDataScript.Instance.playersNameList[chairId].name;
        headIcon.sprite = GlobalDataScript.Instance.playersNameList[chairId].headIcon;

        //for (int i = 0; i < GlobalDataScript.Instance.playerInfos.Count; i++)
        //{
        //    if (GlobalDataScript.Instance.playerInfos[i].chairId == chairId)
        //    {
        //        nameText.text = GlobalDataScript.Instance.playersNameList[i].name;
        //        myId = GlobalDataScript.Instance.playerInfos[i].userID;
        //        headIcon.sprite = GlobalDataScript.Instance.playersNameList[i].headIcon;
        //    }
        //}                                                                                 

    }
}
