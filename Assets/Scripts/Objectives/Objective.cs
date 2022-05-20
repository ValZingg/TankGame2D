using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Nom : Objective.cs
    Description : Classe parente des objectifs assign�s aux campagnes.
     */

public class Objective
{
    //============================
    public string ObjectiveName;
    public bool Completed = false;
    //============================

    public Objective(string objectivename)
    {
        ObjectiveName = objectivename;
    }

    public void CompleteObjective() => Completed = true; //Appel� quand l'objectif est accompli
}
