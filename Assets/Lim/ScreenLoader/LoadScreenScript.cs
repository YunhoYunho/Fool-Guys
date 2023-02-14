using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreenScript : MonoBehaviour
{
    private Animator anim;
    private Canvas canvas;
    private string Loaded = "Loaded";
    private string Started = "Start";
    public bool isStarted;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        canvas = GetComponent<Canvas>();

    }

    private void Start()
    {
        LoadNextLevel();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
             LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        if (!isStarted)
        {
            canvas.planeDistance = 110;
            canvas.sortingLayerName = "Default";
            anim.SetTrigger(Loaded);
            anim.ResetTrigger(Started);
            isStarted = !isStarted;
        }
        else
        {
            canvas.planeDistance = 1;
            canvas.sortingLayerName = "On";
            anim.SetTrigger(Started);
            anim.ResetTrigger(Loaded);
            isStarted = !isStarted;
        }
        //SceneManager.LoadScene();
        //TODO : 로드 해야 할 씬 입력
    }


}