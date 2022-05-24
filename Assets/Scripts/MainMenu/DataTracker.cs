using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
    Nom : DataTracker.cs
    Description : Ce script sert à enregistrer les données que le joueur a selectionné dans le menu principal, et à les transferer dans la partie.
     */

public class DataTracker : MonoBehaviour
{
    //===============================
    public Objective Objective_Save;
    public string Tank_Save = "LightTank"; //Quel type de tank le joueur doit-il charger
    //===============================

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject); //Signale à Unity que l'objet ne doit PAS être détruit quand le changement de niveau aura lieu.
    }

    public void AssassinateClick()
    {/* Quand le joueur clique sur la mission "assassinate" au menu principal*/
        Assassinate TempObj = new Assassinate("Assassiner"); //On crée l'objectif, et on le stocke dans DataTracker
        Objective_Save = TempObj;
        SceneManager.LoadScene("TestLevel"); //On charge le niveau
    }

    public void DestroyClick()
    {/* Quand le joueur clique sur la mission "Destroy" au menu principal*/
        Destroy TempObj = new Destroy("Destroy"); //On crée l'objectif, et on le stocke dans DataTracker
        Objective_Save = TempObj;
        SceneManager.LoadScene("TestLevel"); //On charge le niveau
    }

    //Ces trois fonctions sont appelées lorsque le joueur clique sur un des boutons de choix de tank
    public void SelectLightTank() => Tank_Save = "LightTank";
    public void SelectMediumTank() => Tank_Save = "MediumTank";
    public void SelectHeavyTank() => Tank_Save = "HeavyTank";

}
