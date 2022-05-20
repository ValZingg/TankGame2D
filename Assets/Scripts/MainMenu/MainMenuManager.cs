using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Nom : MainMenuManager.cs
    Description : Ce script sert à gérer les différents boutons dans le menu principal.
     */

public class MainMenuManager : MonoBehaviour
{
    //==========================
    [Header("MenuElements")]
    public GameObject CampaignBG; //L'objet "fenêtre" de campagne
    //==========================

    private void Start()
    {
        CampaignBG = GameObject.Find("CampaignBG");
        CampaignBG.SetActive(false); //On cache la fenêtre au début du programme
    }

    public void ToggleCampaignWindow() //Active / Désactive l'affichage de la fenêtre de campagne
    {
        CampaignBG.SetActive(!CampaignBG.activeSelf);
    }
}
