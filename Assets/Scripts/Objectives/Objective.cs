using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Nom : Objective.cs
    Description : Classe parente des objectifs assignés aux campagnes.
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

    public void CompleteObjective() => Completed = true; //Appelé quand l'objectif est accompli
}
