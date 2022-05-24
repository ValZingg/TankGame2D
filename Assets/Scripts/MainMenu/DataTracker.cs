using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
    Nom : DataTracker.cs
    Description : Ce script sert � enregistrer les donn�es que le joueur a selectionn� dans le menu principal, et � les transferer dans la partie.
     */

public class DataTracker : MonoBehaviour
{
    //===============================
    public Objective Objective_Save;
    public string Tank_Save = "LightTank"; //Quel type de tank le joueur doit-il charger
    //===============================

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject); //Signale � Unity que l'objet ne doit PAS �tre d�truit quand le changement de niveau aura lieu.
    }

    public void AssassinateClick()
    {/* Quand le joueur clique sur la mission "assassinate" au menu principal*/
        Assassinate TempObj = new Assassinate("Assassiner"); //On cr�e l'objectif, et on le stocke dans DataTracker
        Objective_Save = TempObj;
        SceneManager.LoadScene("TestLevel"); //On charge le niveau
    }

    public void DestroyClick()
    {/* Quand le joueur clique sur la mission "Destroy" au menu principal*/
        Destroy TempObj = new Destroy("Destroy"); //On cr�e l'objectif, et on le stocke dans DataTracker
        Objective_Save = TempObj;
        SceneManager.LoadScene("TestLevel"); //On charge le niveau
    }

    //Ces trois fonctions sont appel�es lorsque le joueur clique sur un des boutons de choix de tank
    public void SelectLightTank() => Tank_Save = "LightTank";
    public void SelectMediumTank() => Tank_Save = "MediumTank";
    public void SelectHeavyTank() => Tank_Save = "HeavyTank";

}
