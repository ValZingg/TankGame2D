using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Nom : Assassinate.cs
    Description : Classe héritée de Objective. Le but d'un objectif "assassinate" est de détruire un tank en particulier pour réussir.
     */
public class Assassinate : Objective
{
    //==================
    public GameObject Target; // Cible à détruire
    //==================

    public Assassinate(string objectivename, GameObject target) : base(objectivename)
    {
        ObjectiveName = objectivename;
        Target = target;
    }
}
