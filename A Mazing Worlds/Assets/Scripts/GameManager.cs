using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> players;

    private void Awake()
    {
        players = new();
        SetPlayers();
    }

    private void SetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player").ToList();
    }

    public List<GameObject> GetPlayers()
    {
        return players;
    }
}
