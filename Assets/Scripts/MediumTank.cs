using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumTank : Tank
{
    public MediumTank()
    {
        //Les points de vie
        MaxHealth = 50;
        Health = MaxHealth;

        //Les dégâts et tirs
        FiringRate = 1.10f;
        DamagePerShell = 25f;

        //La vitesse
        Speed = 8f;
        Acceleration = 2f;
        TurnRate = 30f;
        CanonTurnRate = 1.8f;
    }
}
