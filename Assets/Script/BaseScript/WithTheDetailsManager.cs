using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithTheDetailsManager
{
    private static WithTheDetailsManager _instance;

    public static WithTheDetailsManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new WithTheDetailsManager();
            }

            return _instance;
        }
    }

    private List<WithTheDetails> _list = new List<WithTheDetails>();

    public List<WithTheDetails> list
    {
        get
        {
            // if (_list.Count == 0)
            //InitTalkData();
            return _list;
        }
    }
}