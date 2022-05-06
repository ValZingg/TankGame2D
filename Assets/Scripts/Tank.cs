using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Nom : Tank.cs
    Description : Script servant à garder toutes les données et variable d'un tank. (munitions, vie,etc )
     
     */


public class Tank
{
    //===================
    [Header("Attributes")]
    public string TankName;

    public float Health;
    public float MaxHealth;

    public float DamagePerShell;
    public float FiringRate;

    public float Acceleration = 1.0f;
    public float Speed = 5.0f;
    public float TurnRate = 1.0f;
    public float CanonTurnRate = 1.0f; //Vitesse de rotation du canon

    [Header("Booleans")]
    public bool RestrictMovement = false; //Restreint le mouvement du tank
    public bool OnFire = false;
    //===================

}
