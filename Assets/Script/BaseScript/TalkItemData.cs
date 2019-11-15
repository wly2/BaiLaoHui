using UnityEngine;

public class TalkItemData
{
    public TalkItemData()
    {

    }

    public TalkItemData(int id,int sex, string message)
    {
        this.id = id;
        this.sex = sex;
        this.message = message;
    }

    public int id;
    public int sex;
    public string message;
    public string name;
    public int userId;
    public Sprite icon;
    public Sprite faceSprite;
    public AudioClip clip;
}