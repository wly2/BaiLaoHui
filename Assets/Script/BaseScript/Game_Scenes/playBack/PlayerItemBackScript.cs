using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class PlayerItemBackScript : MonoBehaviour
{
    public Image headerIcon;
    public Image bankerImg;
    public Text nameText;
    public Image readyImg;
    public Text scoreText;
    public string dir;
    public GameObject HuFlag;
    public GameObject pengEffect;
    public GameObject gangEffect;

    public GameObject huEffect;

    // Use this for initialization
    private PlayerBackVO avatarvo;

    public void SetAvatarVo(PlayerBackVO value)
    {
        if (value != null)
        {
            avatarvo = value;
            nameText.text = avatarvo.accountName;
            scoreText.text = avatarvo.socre.ToString();
            StartCoroutine(LoadImg());

        }
        else
        {
            nameText.text = "";
            readyImg.enabled = false;
            bankerImg.enabled = false;

            ResourcesLoader.Load<Sprite>("Image/morentouxiang", (sprite) => {
                headerIcon.overrideSprite = sprite;
            });

           // headerIcon.overrideSprite = Resources.Load("Image/morentouxiang", typeof(Sprite)) as Sprite;
        }
    }

    /// <summary>
    /// 加载头像
    /// </summary>
    /// <returns>The image.</returns>
    private IEnumerator LoadImg()
    {
        //开始下载图片
        WWW www = new WWW(avatarvo.headIcon);
        yield return www;
        if (www != null)
        {
            Texture2D texture2D = www.texture;
            byte[] bytes = texture2D.EncodeToPNG();
            //将图片赋给场景上的Sprite
            Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                new Vector2(0, 0));
            headerIcon.overrideSprite = tempSp;
        }
        else
        {
            MyDebug.Log("没有加载到图片");
        }
    }

    /// <summary>
    /// Shows the hu effect.
    /// </summary>
    public void ShowHuEffect()
    {
        huEffect.SetActive(true);
        HuFlag.SetActive(true);
    }

    public void HideHuImage()
    {
        HuFlag.SetActive(false);
    }

    /// <summary>
    /// Shows the peng effect.
    /// </summary>
    public void ShowPengEffect()
    {
        pengEffect.SetActive(true);
    }

    /// <summary>
    /// Shows the gang effect.
    /// </summary>
    public void ShowGangEffect()
    {
        gangEffect.SetActive(true);
    }

    public int GetSex()
    {
        return avatarvo.sex;
    }
}