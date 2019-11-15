using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test_Me : MonoBehaviour
{
    private Image obj;
    public Image preserveAspect;//image preserveAspect测试
    string path = "https://image.baidu.com/search/detail?z=0&ipn=false&word=%E5%A4%B4%E5%83%8F%20%E5%A5%B3%E7%94%9F&step_word=&hs=0&pn=5&spn=0&di=195509071450&pi=&tn=baiduimagedetail&is=0%2C0&istype=0&ie=utf-8&oe=utf-8&cs=1489918960%2C4194737111&os=234503379%2C407972743&simid=0%2C0&adpicid=0&lpn=0&fm=&sme=&cg=head&bdtype=0&simics=3464856078%2C1921535939&oriquery=&objurl=http%3A%2F%2Fimg5q.duitang.com%2Fuploads%2Fitem%2F201503%2F19%2F20150319092719_PZfmx.jpeg&fromurl=ippr_z2C%24qAzdH3FAzdH3Fooo_z%26e3B17tpwg2_z%26e3Bv54AzdH3Frj5rsjAzdH3F4ks52AzdH3Fnd09blba9AzdH3F1jpwtsAzdH3F&gsm=0&cardserver=1";

    // Use this for initialization
    void Start()
    {
        //imagePreserveTure();
        //imagePreserveFalse();
        StartCoroutine(wwwLoad());
    }

    

// Update is called once per frame
void Update()
    {

    }

    public void Load()
    {
        ResourcesLoader.Load<Sprite>("baseassets/ui_online/ui_public/BG_Popup", (sprite) =>
        {
            obj.overrideSprite = sprite;
        });
    }

    private void imagePreserveFalse()
    {
        preserveAspect.preserveAspect = false;
        Debug.Log("preserveFalse" + preserveAspect);
    }

    private void imagePreserveTure()
    {
        preserveAspect.preserveAspect = true;
        Debug.Log("preserveTrue" + preserveAspect);
    }

    private IEnumerator wwwLoad()
    {
        Texture2D texture;
        WWW www = new WWW(path);
        yield return www;
        texture = www.texture;
        preserveAspect.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        Debug.Log(preserveAspect.sprite);
        preserveAspect.preserveAspect = true;
    }

    public void forTest()
    {
       // List<int> list = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("===============" + i);
        }
    }
}
