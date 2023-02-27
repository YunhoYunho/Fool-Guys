using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomizePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject PlayerModel;

    [SerializeField]
    private Slider R_Slider;

    [SerializeField]
    private Slider G_Slider;

    [SerializeField]
    private Slider B_Slider;

    private SkinnedMeshRenderer[] Skincolor;

    public Color playerColor;

    [SerializeField]
    private LobbyManager lobbyManager;

    private void Awake()
    {
        Skincolor = GetComponentsInChildren<SkinnedMeshRenderer>();
    }


    public void ColorChangeDetect()
    {
        playerColor = new Color(R_Slider.value, G_Slider.value, B_Slider.value);
        ColorChange();
    }

    public void ColorChange()
    {
        for (int i = 0; i < Skincolor.Length; i++)
        {
            Skincolor[i].material.color = playerColor;
        }
    }

    public void OnCustomizeConfirmButtonClicked()
    {
        lobbyManager.PlayerColor = playerColor;
        gameObject.SetActive(false);
    }

    public void OnCustomizeCancelButtonClicked()
    {
        R_Slider.value = 1;
        G_Slider.value = 1;
        B_Slider.value = 1;
        gameObject.SetActive(false);
    }




}
