using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Nom : Assassinate.cs
    Description : Classe h�rit�e de Objective. Le but d'un objectif "assassinate" est de d�truire un tank en particulier pour r�ussir.
     */
public class Assassinate : Objective
{
    //==================
    public GameObject Target; // Cible � d�truire
    //==================

    public Assassinate(string objectivename) : base(objectivename)
    {
        ObjectiveName = objectivename;
    }
}
