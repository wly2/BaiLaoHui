using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel_SignIn : UIWindow
{
    public static UIPanel_SignIn instance;
    public GameObject item;
    public List<Toggle> signIn = new List<Toggle>();
    public Image tiShi;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 1; i < 5; i++)
        {
            GameObject gameObject = Instantiate(item) as GameObject;
            gameObject.transform.SetParent(item.transform.parent);
            gameObject.transform.localScale = Vector3.one;
            signIn.Add(gameObject.GetComponent<Toggle>());
        }

        for (int i = 0; i < signIn.Count; i++)
        {
            signIn[i].gameObject.GetComponent<SiginItem>()
                .SetData(SiginDataManager.siginData.sign_config_list[(i + 1).ToString()]);
        }

        for (int i = 0; i < signIn.Count; i++)
        {
            if (i <= SiginDataManager.siginData.already_day - 1)
            {
                signIn[i].isOn = true;

                ResourcesLoader.Load<Sprite>("/BaseAssets/UI_Online/Login/UIPanel_SignIn/QianDaoEnd",(Sprite) => {
                    signIn[i].gameObject.GetComponentsInChildren<Image>()[1].overrideSprite = Sprite;
                });

                //signIn[i].gameObject.GetComponentsInChildren<Image>()[1].sprite =
                   // Resources.Load("Image/QianDaoEnd", typeof(Sprite)) as Sprite;
                signIn[i].enabled = false;
            }
            else
            {
                if (SiginDataManager.siginData.is_today_sign)
                {
                    signIn[i].enabled = false;
                }
                else
                {
                    if (i > SiginDataManager.siginData.already_day)
                    {
                        signIn[i].enabled = false;
                    }
                    else
                    {
                        signIn[i].enabled = true;
                    }
                }
            }
        }
    }
}