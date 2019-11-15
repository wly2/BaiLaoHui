using AssemblyCSharp;
using LitJson;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SiginItem : MonoBehaviour
{
    public Image headIconImg; //头像路径
    public Text title;
    public SignAward data;
    public Image bgSignItem;//签到界面背景

    public void SetData(SignAward _data)
    {
        data = _data;
        title.text = "第" + data.day + "天";
        StartCoroutine(LoadImg());
    }

    private IEnumerator LoadImg()
    {
        //开始下载图片
        WWW www = new WWW(data.thumb);
        yield return www;
        //下载完成，保存图片到路径filePath
        if (www != null)
        {
            Texture2D texture2D = www.texture;
            byte[] bytes = texture2D.EncodeToPNG();
            //将图片赋给场景上的Sprite
            headIconImg.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                new Vector2(0, 0));
            headIconImg.preserveAspect = true;
        }
        else
        {
            MyDebug.Log("没有加载到图片");
        }
    }

    public void Send()
    {
        HttpManager.instance.SentHttpRequre(HTTP_TYPE.SendSigin, GetSignInCallBack);
        ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Hall/UIPanel_QianDao/QianDaoEnd", (sprite) => {
            bgSignItem.overrideSprite = sprite;
        });
        
        // gameObject.GetComponent<Toggle>().enabled = false;
        UIPanel_SignIn.instance.tiShi.gameObject.SetActive(true);
    }

    private void GetSignInCallBack(WWW msg)
    {
        SiginResult result = JsonMapper.ToObject<SiginResult>(msg.text);
        gameObject.GetComponent<Toggle>().enabled = false;
        SiginDataManager.siginData.already_day = SiginDataManager.siginData.already_day + 1;
    }
}