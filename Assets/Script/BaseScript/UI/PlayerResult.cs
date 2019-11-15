using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System;
/**
* 单人投票结果
*/
namespace AssemblyCSharp
{
	public class PlayerResult :MonoBehaviour
	{
        public Image HeadIcon; 
		public Image result;
        public Sprite agreeSprite;
        public Sprite noSprite;
        public Text pName;
        public int userId;
		public PlayerResult ()
		{
		}

		public void  setInitVal(PlayerGameRoomInfo playerInfo){
            userId = playerInfo.userID;
            if (playerInfo.userHeadUrl == null || playerInfo.userHeadUrl.Length <= 0)
            {
                ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/UI_Public/head" + playerInfo.wFaceID, (sprite) =>
                {
                    HeadIcon.sprite = sprite;
                });
            }else
                StartCoroutine(LoadImg(playerInfo.userHeadUrl));

            pName.text = playerInfo.name;
		}

		public void changeResult(int resultstr){
            result.sprite = resultstr > 0 ? agreeSprite : noSprite;
            result.SetNativeSize();
		}
        private IEnumerator LoadImg(string headIconPath)
        {
            //开始下载图片
            if (headIconPath != null && headIconPath != "")
            {
                WWW www = new WWW(headIconPath);
                yield return www;
                //下载完成，保存图片到路径filePath
                try
                {
                    Texture2D texture2D = www.texture;
                    byte[] bytes = texture2D.EncodeToPNG();
                    //将图片赋给场景上的Sprite
                    Sprite tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
                    HeadIcon.sprite = tempSp;

                }
                catch (Exception e)
                {

                    MyDebug.Log("LoadImg" + e.Message);
                }
            }
        }

    }
}

