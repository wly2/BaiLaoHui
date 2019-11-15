using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPanel_Rank : UIWindow
{
    public Dictionary<int, RankItem> win;
    public List<int> list;
    public Dictionary<int, RankItem> card;
    [SerializeField] ScrollRectList myScrollRect;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject rankItem;

    GameObject Rank;
    

    void Start()
    {
        Win();
    }

    void OnValueChange(Vector2 pos)
    {
        myScrollRect.OnValueChange(pos);
    }

    void Win()
    {
        myScrollRect.gameObject.transform.localPosition = new Vector3(myScrollRect.gameObject.transform.localPosition.x,
            0, myScrollRect.gameObject.transform.localPosition.z);
        scrollRect.inertia = false;
        win = RankManager.Instance.Win;
        list = new List<int>(win.Keys);
        scrollRect.onValueChanged.AddListener(OnValueChange);
        myScrollRect.createitemobject = delegate(int index, UnityAction<GameObject> action)
        {
            if (rankItem != null)
            {
                GameObject cellObj = Instantiate(rankItem.gameObject);
                action(cellObj);
            }
        };
        myScrollRect.updateItem = delegate(ItemCell o, int index)
        {
            switch (index)
            {
                case 0:
                     ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/UIPanel_Rank/rank_first", (sprite)=> {
                         o.item.GetComponentsInChildren<Image>()[1].overrideSprite = sprite;
        });

                    //o.item.GetComponentsInChildren<Image>()[1].sprite =
                    //    Resources.Load("Image/rank_first", typeof(Sprite)) as Sprite;
                    o.item.GetComponentsInChildren<Image>()[1].enabled = true;
                    o.item.GetComponentsInChildren<Text>()[0].enabled = false;
                    break;
                case 1:
                    ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/UIPanel_Rank/rank_scend", (sprite) => {
                        o.item.GetComponentsInChildren<Image>()[1].overrideSprite = sprite;
                    });

                    //o.item.GetComponentsInChildren<Image>()[1].sprite =
                    //Resources.Load("Image/rank_scend", typeof(Sprite)) as Sprite;
                    o.item.GetComponentsInChildren<Image>()[1].enabled = true;
                    o.item.GetComponentsInChildren<Text>()[0].enabled = false;
                    break;
                case 2:
                    ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/UIPanel_Rank/rank_third", (sprite) => {
                        o.item.GetComponentsInChildren<Image>()[1].overrideSprite = sprite;
                    });

                    //o.item.GetComponentsInChildren<Image>()[1].sprite =
                    //Resources.Load("Image/rank_third", typeof(Sprite)) as Sprite;
                    o.item.GetComponentsInChildren<Image>()[1].enabled = true;
                    o.item.GetComponentsInChildren<Text>()[0].enabled = false;
                    break;
                default:
                    o.item.GetComponentsInChildren<Image>()[1].enabled = false;
                    o.item.GetComponentsInChildren<Text>()[0].enabled = true;
                    break;
            }

            o.item.GetComponentsInChildren<Text>()[0].text = list[index].ToString();
            o.item.GetComponentsInChildren<Text>()[1].text = win[list[index]].name;
            o.item.GetComponentsInChildren<Text>()[2].text = win[list[index]].winCount.ToString();
        };
        myScrollRect.Init(win.Count);
        scrollRect.inertia = true;
    }

    void Card()
    {
        myScrollRect.gameObject.transform.localPosition = new Vector3(myScrollRect.gameObject.transform.localPosition.x,
            0, myScrollRect.gameObject.transform.localPosition.z);
        scrollRect.inertia = false;
        card = RankManager.Instance.Roomcard;
        list = new List<int>(card.Keys);
        scrollRect.onValueChanged.AddListener(OnValueChange);
        myScrollRect.createitemobject = delegate(int index, UnityAction<GameObject> action)
        {
            if (rankItem != null)
            {
                GameObject cellObj = Instantiate(rankItem.gameObject);
                action(cellObj);
            }
        };
        myScrollRect.updateItem = delegate(ItemCell o, int index)
        {
            switch (index)
            {
                case 0:
                      ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/UIPanel_Rank/rank_first", (sprite)=> {
                         o.item.GetComponentsInChildren<Image>()[1].overrideSprite = sprite;
        });

                    //o.item.GetComponentsInChildren<Image>()[1].sprite =
                    //    Resources.Load("Image/rank_first", typeof(Sprite)) as Sprite;
                    o.item.GetComponentsInChildren<Image>()[1].enabled = true;
                    break;
                case 1:
                    ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/UIPanel_Rank/rank_scend", (sprite) => {
                        o.item.GetComponentsInChildren<Image>()[1].overrideSprite = sprite;
                    });
                    //o.item.GetComponentsInChildren<Image>()[1].sprite =
                    //    Resources.Load("Image/rank_scend", typeof(Sprite)) as Sprite;
                    o.item.GetComponentsInChildren<Image>()[1].enabled = true;
                    break;
                case 2:
                    ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/UIPanel_Rank/rank_third", (sprite) => {
                        o.item.GetComponentsInChildren<Image>()[1].overrideSprite = sprite;
                    });

                    //o.item.GetComponentsInChildren<Image>()[1].sprite =
                    //    Resources.Load("Image/rank_third", typeof(Sprite)) as Sprite;
                    o.item.GetComponentsInChildren<Image>()[1].enabled = true;
                    break;
                default:
                    o.item.GetComponentsInChildren<Image>()[1].enabled = false;
                    break;
            }

            o.item.GetComponentsInChildren<Text>()[0].text = list[index].ToString();
            o.item.GetComponentsInChildren<Text>()[1].text = card[list[index]].name;
            o.item.GetComponentsInChildren<Text>()[2].text = card[list[index]].roomCardCount.ToString();
        };
        myScrollRect.Init(card.Count);
        scrollRect.inertia = true;
    }

    public void WinBtn()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        Win();
    }

    public void CardBtn()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        Card();
    }
}