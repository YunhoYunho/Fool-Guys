using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupController : MonoBehaviour, IControllable
{
    [SerializeField] private List<ControlableObstacle> obstacleList;

    private void Awake()
    {
        ControlableObstacle[] obstacles = GetComponentsInChildren<ControlableObstacle>();
        foreach (ControlableObstacle obstacle in obstacles) 
        { 
            obstacleList.Add(obstacle);
        }
    }

    public void Control(float duration, float coolTime)
    {
        foreach (ControlableObstacle controlable in obstacleList)
        {
            controlable.Control(duration, coolTime);
        }
    }
}
