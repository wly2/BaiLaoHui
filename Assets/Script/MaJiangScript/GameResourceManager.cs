using AssemblyCSharp;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//结构定义
//用户信息
public class GameResourceManager : MonoBehaviour
{                                       
    public GameObject pengFx;
    public GameObject huFx;
    public GameObject gangFx;
    public GameObject yinying;
    private readonly string mjPath = "MajiangAssets/Prefab/MaJiangPerfab/mj";
    private readonly string fxPath = "Fx/";
    private static GameResourceManager _instance;

    public static GameResourceManager Instance
    {
        get { return _instance; }
    }

    private void OnEnable()
    {
        _instance = this;
    }

    [HideInInspector] public float mjSize1 = 0.68f;
    [HideInInspector] public float mjSize2 = 1;
    [HideInInspector] public float mjSize3 = 1.32f;

    private void OnDisable()
    {
        _instance = null;
    }

    /// <summary>
    ///创建牌
    /// </summary>
    /// <param name="cardPoint"></param>
    /// <param name="parent"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public GameObject CreateGameObjectAndReturn(int cardPoint, Transform parent, Vector3 position, Vector3 rotation)
    {
        MyDebug.Log(cardPoint);
        GameObject obj = null;
        ResourcesLoader.Load<GameObject>(mjPath+cardPoint, (goo) => {
            obj = Instantiate(goo) as GameObject;  
            obj.transform.SetParent(parent);
            obj.transform.localEulerAngles = rotation;
            obj.transform.localPosition = position;
            obj.transform.localScale = Vector3.one;    

        });

        return obj;

        //var obj = Instantiate(MjPerfabs[cardPoint]) as GameObject;
        
        //obj.transform.SetParent(parent);
        //obj.transform.localEulerAngles = rotation;
        //obj.transform.localPosition = position;
        //obj.transform.localScale = Vector3.one;
        //return obj;
    }
    public Sprite CreateSprite(string path)
    {
        MyDebug.Log(path);
        Sprite obj = null;
        ResourcesLoader.Load<Sprite>(path, (goo) => {
            obj = goo;    
        });

        return obj;

        //var obj = Instantiate(MjPerfabs[cardPoint]) as GameObject;

        //obj.transform.SetParent(parent);
        //obj.transform.localEulerAngles = rotation;
        //obj.transform.localPosition = position;
        //obj.transform.localScale = Vector3.one;
        //return obj;
    }
    UnityAction<GameObject> loadFinishHandler;
    public GameObject LoadObj(GameObject go)
    {

        return null;
    }

    public void PlayPengFx(Transform parent)
    {
        var obj = Instantiate(pengFx) as GameObject;
        obj.transform.SetParent(parent);
        obj.transform.localPosition = Vector3.zero;
        SoundManager.Instance.PlayFx("earthimpact");
    }

    public void PlayMGangFx(Transform parent)
    {
        var obj = Instantiate(gangFx) as GameObject;
        obj.transform.SetParent(parent);
        obj.transform.localPosition = Vector3.zero;
    }
    public void PlayAGangFx(Transform parent)
    {
        var obj = Instantiate(pengFx) as GameObject;
        obj.transform.SetParent(parent);
        obj.transform.localPosition = Vector3.zero;
    }

    public void PlayHuFx(Transform parent)
    {
        var obj = Instantiate(huFx) as GameObject;
        obj.transform.SetParent(parent);
        obj.transform.localPosition = Vector3.zero;
    }

    private void CreateFx(Transform parent, string name)
    {
        var obj = Instantiate(Resources.Load(fxPath + name)) as GameObject;
        obj.transform.SetParent(parent);
        obj.transform.localPosition = Vector3.zero;
    }
}