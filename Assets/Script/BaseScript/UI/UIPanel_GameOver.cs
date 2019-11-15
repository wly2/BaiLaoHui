using AssemblyCSharp;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel_GameOver : UIWindow
{
    public static UIPanel_GameOver _instance;
    public List<PlayerInformation> list;
    public List<Sprite> cardList;
    private Vector3 paiSize = new Vector3(0.8f, 0.8f, 0.8f);

    private void Awake()
    {
        _instance = this;
    }

    private void OnEnable()
    {
        OnhuReply();
    }

    public void Continue()
    {
        CloseUI();
        MyDebug.Log("GlobalDataScript.hupaiResponseVo.bAllGameEnd:" + GlobalDataScript.hupaiResponseVo.bAllGameEnd);
        if (GlobalDataScript.hupaiResponseVo.bAllGameEnd)
        {
            SocketEngine.Instance.SocketQuit();
            MySceneManager.instance.BackToMain();
            return;
        }

        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        var cgu = new CMD_GF_UserReady
        {
            wChairID = (ushort) GlobalDataScript.loginResponseData.chairID,
            bReady = 1
        };
        var temp = NetUtil.StructToBytes(cgu);
        //SocketSendManager.Instance.SendData((int) GameServer.MDM_GF_FRAME, (int) MDM_GF_FRAME.SUB_GF_USER_READY, temp,
        //    Marshal.SizeOf(cgu));
    }

    public void OnhuReply()
    {
        var hri = GlobalDataScript.hupaiResponseVo.avatarList;
        for (int i = 0; i < hri.Count; i++)
        {
            var posIndex = 0;
            var playerInfo = list[i];
            switch (MaJiangGameCtl.instance.GetDirection(hri[i].chairId))
            {
                case DirectionEnum.Bottom:
                    playerInfo.playerName.text = UIMaJiangPanel.instance.playerItems[0].nameText.text;
                    playerInfo.headIcon.sprite = UIMaJiangPanel.instance.playerItems[0].headerIcon.sprite;
                    break;
                case DirectionEnum.Right:
                    playerInfo.playerName.text = UIMaJiangPanel.instance.playerItems[1].nameText.text;
                    playerInfo.headIcon.sprite = UIMaJiangPanel.instance.playerItems[1].headerIcon.sprite;
                    break;
                case DirectionEnum.Top:
                    playerInfo.playerName.text = UIMaJiangPanel.instance.playerItems[2].nameText.text;
                    playerInfo.headIcon.sprite = UIMaJiangPanel.instance.playerItems[2].headerIcon.sprite;
                    break;
                case DirectionEnum.Left:
                    playerInfo.playerName.text = UIMaJiangPanel.instance.playerItems[3].nameText.text;
                    playerInfo.headIcon.sprite = UIMaJiangPanel.instance.playerItems[3].headerIcon.sprite;
                    break;
            }

            if (GlobalDataScript.loginResponseData.chairID == i)
            {
                ResourcesLoader.Load<Sprite>("MajiangAssets/UI/UI_mj/UIPanel_GameOver/Jiesuan_Yellow", (sprite) =>
                {
                    playerInfo.backGround.overrideSprite = sprite;
                });

                   // playerInfo.backGround.sprite = Resources.Load("Image/Yellow", typeof(Sprite)) as Sprite;
            }
            else
            {
                ResourcesLoader.Load<Sprite>("MajiangAssets/UI/UI_mj/UIPanel_GameOver/Jiesuan_Purple", (sprite) =>
                {
                    playerInfo.backGround.overrideSprite = sprite;
                });

                //playerInfo.backGround.sprite = Resources.Load("Image/Purple", typeof(Sprite)) as Sprite;
            }

            if (hri[i].chairId == 0)
            {
                playerInfo.banker.enabled = true;
            }

            if (hri[i].win)
            {
                playerInfo.hu.enabled = true;
                 playerInfo.hu.sprite = cardList[hri[i].huInfo.card];
                //ResourcesLoader.Load<Sprite>("Resources/UI/Panel_GamePlay/btnList/hu", (sprite) =>
                //{
                //    playerInfo.hu.overrideSprite = sprite;
                //});
            }

            else
                playerInfo.hu.enabled = false;

            if (hri[i].roundTotalScore > 0)
            {
                playerInfo.score.text = "﹢" + hri[i].roundTotalScore;
            }
            else
            {
                playerInfo.score.text = hri[i].roundTotalScore.ToString();
            }

            if (hri[i].gangInfos.Count > 0)
            {
                for (int j = 0; j < hri[i].gangInfos.Count; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        var go = Instantiate(playerInfo.pai);
                        go.transform.SetParent(playerInfo.transform);
                        go.transform.localScale = paiSize;
                        go.transform.localPosition =
                            playerInfo.pai.transform.localPosition + new Vector3(posIndex, 0, 0);
                        go.GetComponent<Image>().sprite = cardList[hri[i].gangInfos[j].cardIndex];
                        posIndex += 72;
                    }

                    posIndex += 20;
                }
            }

            if (hri[i].pengArray.Count > 0)
            {
                for (int j = 0; j < hri[i].pengArray.Count; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        var go = Instantiate(playerInfo.pai);
                        go.transform.SetParent(playerInfo.transform);
                        go.transform.localScale = paiSize;
                        go.transform.localPosition =
                            playerInfo.pai.transform.localPosition + new Vector3(posIndex, 0, 0);
                        go.GetComponent<Image>().sprite = cardList[hri[i].pengArray[j]];
                        posIndex += 72;
                    }

                    posIndex += 20;
                }
            }

            if (hri[i].centerChiArray.Count > 0)
            {
                for (int j = 0; j < hri[i].centerChiArray.Count; j++)
                {
                    var go = Instantiate(playerInfo.pai);
                    go.transform.SetParent(playerInfo.transform);
                    go.transform.localPosition = playerInfo.pai.transform.localPosition + new Vector3(posIndex, 0, 0);
                    go.GetComponent<Image>().sprite = cardList[hri[i].centerChiArray[j] - 1];
                    posIndex += 72;
                    go.transform.localScale = paiSize;
                    go = Instantiate(playerInfo.pai);
                    go.transform.SetParent(playerInfo.transform);
                    go.transform.localPosition = playerInfo.pai.transform.localPosition + new Vector3(posIndex, 0, 0);
                    go.GetComponent<Image>().sprite = cardList[hri[i].centerChiArray[j]];
                    posIndex += 72;
                    go.transform.localScale = paiSize;
                    go = Instantiate(playerInfo.pai);
                    go.transform.SetParent(playerInfo.transform);
                    go.transform.localPosition = playerInfo.pai.transform.localPosition + new Vector3(posIndex, 0, 0);
                    go.GetComponent<Image>().sprite = cardList[hri[i].centerChiArray[j] + 1];
                    posIndex += 92;
                    go.transform.localScale = paiSize;
                }
            }

            if (hri[i].rightChiArray.Count > 0)
            {
                for (int j = 0; j < hri[i].rightChiArray.Count; j++)
                {
                    var go = Instantiate(playerInfo.pai);
                    go.transform.SetParent(playerInfo.transform);
                    go.transform.localPosition = playerInfo.pai.transform.localPosition + new Vector3(posIndex, 0, 0);
                    go.GetComponent<Image>().sprite = cardList[hri[i].rightChiArray[j] - 2];
                    posIndex += 72;
                    go.transform.localScale = paiSize;
                    go = Instantiate(playerInfo.pai);
                    go.transform.SetParent(playerInfo.transform);
                    go.transform.localPosition = playerInfo.pai.transform.localPosition + new Vector3(posIndex, 0, 0);
                    go.GetComponent<Image>().sprite = cardList[hri[i].rightChiArray[j] - 1];
                    posIndex += 72;
                    go.transform.localScale = paiSize;
                    go = Instantiate(playerInfo.pai);
                    go.transform.SetParent(playerInfo.transform);
                    go.transform.localPosition = playerInfo.pai.transform.localPosition + new Vector3(posIndex, 0, 0);
                    go.GetComponent<Image>().sprite = cardList[hri[i].rightChiArray[j]];
                    posIndex += 92;
                    go.transform.localScale = paiSize;
                }
            }

            if (hri[i].leftChiArray.Count > 0)
            {
                for (int j = 0; j < hri[i].leftChiArray.Count; j++)
                {
                    var go = Instantiate(playerInfo.pai);
                    go.transform.SetParent(playerInfo.transform);
                    go.transform.localPosition = playerInfo.pai.transform.localPosition + new Vector3(posIndex, 0, 0);
                    go.GetComponent<Image>().sprite = cardList[hri[i].leftChiArray[j]];
                    posIndex += 72;
                    go.transform.localScale = paiSize;
                    go = Instantiate(playerInfo.pai);
                    go.transform.SetParent(playerInfo.transform);
                    go.transform.localPosition = playerInfo.pai.transform.localPosition + new Vector3(posIndex, 0, 0);
                    go.GetComponent<Image>().sprite = cardList[hri[i].leftChiArray[j] + 1];
                    posIndex += 72;
                    go.transform.localScale = paiSize;
                    go = Instantiate(playerInfo.pai);
                    go.transform.SetParent(playerInfo.transform);
                    go.transform.localPosition = playerInfo.pai.transform.localPosition + new Vector3(posIndex, 0, 0);
                    go.GetComponent<Image>().sprite = cardList[hri[i].leftChiArray[j] + 2];
                    posIndex += 92;
                    go.transform.localScale = paiSize;
                }
            }

            for (int j = 0; j < hri[i].paiArray.Count; j++)
            {
                var go = Instantiate(playerInfo.pai);
                go.transform.SetParent(playerInfo.transform);
                go.transform.localPosition = playerInfo.pai.transform.localPosition + new Vector3(posIndex, 0, 0);
                go.GetComponent<Image>().sprite = cardList[hri[i].paiArray[j]];
                posIndex += 72;
                go.transform.localScale = paiSize;
            }

            playerInfo.pai.SetActive(false);
        }
    }

    public void ScreenShot()
    {
        NetUtil.ShotSceneTexture();
    }
}