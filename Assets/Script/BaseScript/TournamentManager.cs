using System.Collections.Generic;
using UnityEngine;

public class TournamentManager : MonoBehaviour
{
    public Dictionary<int, TournamentUI> dictionary = new Dictionary<int, TournamentUI>();

    void Awake()
    {
        for (int i = 0; i < gameObject.GetComponentsInChildren<TournamentUI>().Length; i++)
        {
        }
    }
}