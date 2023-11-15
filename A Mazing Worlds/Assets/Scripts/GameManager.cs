using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public List<Transform> players;

    private void Awake()
    {
        players = new();
        SetPlayers();
    }

    private void SetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player").Select(x => x.transform).ToList();
    }

    public List<Transform> GetPlayers()
    {
        return players;
    }
}
