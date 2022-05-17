using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyTank : Tank
{
    public HeavyTank()
    {
        //Les points de vie
        MaxHealth = 80;
        Health = MaxHealth;

        //Les dégâts et tirs
        FiringRate = 1.70f;
        DamagePerShell = 40f;

        //La vitesse
        Speed = 6f;
        Acceleration = 1f;
        TurnRate = 15f;
        CanonTurnRate = 1f;

        //La taille
        SizeModifier = 1.5f;
    }
}
