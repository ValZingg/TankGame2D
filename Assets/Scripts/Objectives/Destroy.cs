using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Nom : Destroy.cs
    Description : Classe h�rit�e de Objective. Le but d'un objectif "destroy" est de d�truire un certain nombre de tanks avant le temps imparti.
     */

public class Destroy : Objective
{
    //==================
    public int Amount; // Nombre de tanks � d�truire
    //==================

    public Destroy(string objectivename) : base(objectivename)
    {
        ObjectiveName = objectivename;
    }
}
