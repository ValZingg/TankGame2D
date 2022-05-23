using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Nom : Destroy.cs
    Description : Classe héritée de Objective. Le but d'un objectif "destroy" est de détruire un certain nombre de tanks avant le temps imparti.
     */

public class Destroy : Objective
{
    //==================
    public int Amount; // Nombre de tanks à détruire
    //==================

    public Destroy(string objectivename) : base(objectivename)
    {
        ObjectiveName = objectivename;
    }
}
