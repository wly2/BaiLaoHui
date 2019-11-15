using AssemblyCSharp;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0;
        Image image = GameObject.Find("Image").GetComponent<Image>();
        Tweener tweener = image.rectTransform.DOLocalMove(Vector3.zero, 1).SetEase(Ease.Linear);
        tweener.SetUpdate(true);
   }
}