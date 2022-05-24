using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Nom : MainMenuManager.cs
    Description : Ce script sert � g�rer les diff�rents boutons dans le menu principal.
     */

public class MainMenuManager : MonoBehaviour
{
    //==========================
    [Header("MenuElements")]
    public GameObject CampaignBG; //L'objet "fen�tre" de campagne
    public GameObject InstructionsBG; //L'objet "fenetre" des instructions
    //==========================

    private void Start()
    {
        CampaignBG = GameObject.Find("CampaignBG");
        CampaignBG.SetActive(false); //On cache la fen�tre au d�but du programme

        InstructionsBG = GameObject.Find("InstructionsBG");
        InstructionsBG.SetActive(false);
    }

    public void ToggleCampaignWindow() //Active / D�sactive l'affichage de la fen�tre de campagne
    {
        CampaignBG.SetActive(!CampaignBG.activeSelf);
    }

    public void ToggleInstructionsWindow() //Affiche / cache la fen�tre des instructions
    {
        InstructionsBG.SetActive(!InstructionsBG.activeSelf);
    }

    public void QuitButton() //Appel�e quand on clique sur le bouton quitter
    {
        Application.Quit();
    }
}
