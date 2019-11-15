using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityLogBtn : MonoBehaviour
{
	private bool isShow = false;
	public GameObject gameObject;

	public void BtnClick()
	{
		if (isShow)
		{
			gameObject.SetActive(false);
			isShow = false;
		}
		else
		{
			gameObject.SetActive(true);
			isShow = true;
		}
	}
}