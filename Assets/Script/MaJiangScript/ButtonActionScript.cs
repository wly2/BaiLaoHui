﻿using UnityEngine;

public class ButtonActionScript : MonoBehaviour
{
	public GameObject huBtn;
	public GameObject gangBtn;
	public GameObject pengBtn;
	public GameObject chiBtn;
	public GameObject passBtn;

	/// <summary>
	/// Shows the button.
	/// </summary>
	/// <param name="type">Type.</param> 1-胡，2-杠，3-碰,4-吃
	public void ShowBtn(int type)
	{
		passBtn.SetActive(true);
		if (type == 1)
		{
			huBtn.SetActive(true);
			/**			huBtn.transform.localPosition = new Vector3 (341f,-115f);
			passBtn.SetActive (true);			*/
		}

		if (type == 2)
		{
			gangBtn.SetActive(true);
			/*gangBtn.transform.localPosition = new Vector3 (370f,-128f);
	if (huBtn.activeSelf)
	{
		huBtn.transform.localPosition = new Vector3 (254f, -115f);
	    }
	if (!passBtn.activeSelf) {
		passBtn.SetActive (true);
	}*/
		}

		if (type == 3)
		{
			pengBtn.SetActive(true);
			/*			pengBtn.transform.localPosition = new Vector3 (370f,-128f);
			if (gangBtn.activeSelf) {
				gangBtn.transform.localPosition = new Vector3 (257f,-128f);
				if (huBtn.activeSelf) {
					huBtn.transform.localPosition = new Vector3 (124f, -115f);
				}
			} else {
				if (huBtn.activeSelf) {
					huBtn.transform.localPosition = new Vector3 (240f, -115f);
				}
			}
			if (passBtn.activeSelf == false) {
				passBtn.SetActive (true);
			}*/
		}

		if (type == 4)
		{
			chiBtn.SetActive(true);
		}
	}

	public void CleanBtnShow()
	{
		huBtn.SetActive(false);
		gangBtn.SetActive(false);
		pengBtn.SetActive(false);
		chiBtn.SetActive(false);
		passBtn.SetActive(false);
	}
}