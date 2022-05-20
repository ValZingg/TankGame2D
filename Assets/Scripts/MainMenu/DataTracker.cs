using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    Nom : DataTracker.cs
    Description : Ce script sert à enregistrer les données que le joueur a selectionné dans le menu principal, et à les transferer dans la partie.
     */

public class DataTracker : MonoBehaviour
{
    //===============================
    public Objective Objective_Save;
    //===============================

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject); //Signale à Unity que l'objet ne doit PAS être détruit quand le changement de niveau aura lieu.
    }
}
