using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }

    [SerializeField]
    private List<RespawnPoints> respawnList;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GetRespawnPos(int index, int playerNum)
    {
        return respawnList[index].points[playerNum].position;
    }

    [Serializable]
    public struct RespawnPoints
    {
        public List<Transform> points;
    }
}
