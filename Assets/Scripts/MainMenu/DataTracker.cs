using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    Nom : DataTracker.cs
    Description : Ce script sert � enregistrer les donn�es que le joueur a selectionn� dans le menu principal, et � les transferer dans la partie.
     */

public class DataTracker : MonoBehaviour
{
    //===============================
    public Objective Objective_Save;
    //===============================

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject); //Signale � Unity que l'objet ne doit PAS �tre d�truit quand le changement de niveau aura lieu.
    }
}
