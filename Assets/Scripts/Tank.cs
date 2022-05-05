using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Nom : Tank.cs
    Description : Script servant à garder toutes les données et variable d'un tank. (munitions, vie,etc )
     
     */

public class Tank : MonoBehaviour
{
    //===================
    [Header("Attributes")]
    public float BaseAcceleration = 1.0f;
    public float BaseSpeed = 5.0f;
    public float BaseTurnRate = 1.0f;
    public float BasicCanonTurnRate = 1.0f; //Vitesse de rotation du canon

    [Header("Booleans")]
    public bool RestrictTurretTurn = false; //Restreint la rotation du canon
    public bool RestrictMovement = false; //Restreint le mouvement du tank

    //===================
}
