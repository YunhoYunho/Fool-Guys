using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupController : MonoBehaviour, IControllable
{
    [SerializeField] private List<ConsoleObject> obstacleList;

    private void Awake()
    {
        ConsoleObject[] obstacles = GetComponentsInChildren<ConsoleObject>();
        foreach (ConsoleObject obstacle in obstacles) 
        { 
            obstacleList.Add(obstacle);
        }
    }

    public void Control(float duration, float coolTime)
    {
        foreach (ConsoleObject controlable in obstacleList)
        {
            controlable.Control(duration, coolTime);
        }
    }
}
