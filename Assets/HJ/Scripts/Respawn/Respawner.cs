using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    [SerializeField] private int respawnIndex = 0;
    [SerializeField] private int myPlayerNum;

    public void UpdateCheckPoint(int respawnNum)
    {
        if(respawnIndex < respawnNum)
        {
            respawnIndex = respawnNum;
        }
    }

    public void Respawn()
    {
        //게임매니저로부터 리스폰 지역을 가져온다.
        Vector3 respawnPos = RespawnManager.Instance.GetRespawnPos(respawnIndex, 0/* 플레이어 번호 */);
        transform.position = respawnPos;

        //TODO:
        //그외 리스폰 로직
    }
}
