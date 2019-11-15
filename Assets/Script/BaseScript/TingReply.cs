using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TingReply : MonoBehaviour
{
    public int tingCount;
    public Image image;
    public GameObject prefab;
    private bool isShow;

    public void ShowTing()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        if (!isShow)
        {
            GameObject go = Instantiate(prefab, prefab.transform.position, Quaternion.identity);
            go.transform.parent = image.gameObject.transform;

            ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/admitDefeat", (sprite) => {
                go.GetComponent<Image>().overrideSprite = sprite;
            });

            // go.GetComponent<Image>().sprite = Resources.Load("Image/2/0", typeof(Sprite)) as Sprite;
            ShowSettingframe();
            isShow = true;
        }
        else
        {
            HideSettingFrame();
            isShow = false;
        }
    }

    private void ShowSettingframe()
    {
        image.gameObject.transform.DOLocalMove(new Vector3(517.5f, -195f), 0.4f);
    }

    private void HideSettingFrame()
    {
        image.gameObject.transform.DOLocalMove(new Vector3(1590, -195f), 0.4f);
    }
}