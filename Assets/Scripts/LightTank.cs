using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTank : Tank
{
    public LightTank()
    {/* On assigne toutes les statistiques*/

        //Les points de vie
        MaxHealth = 30;
        Health = MaxHealth;

        //Les dégâts et tirs
        FiringRate = 0.90f;
        DamagePerShell = 15f;

        //La vitesse
        Speed = 10f;
        Acceleration = 3f;
        TurnRate = 40f;
        CanonTurnRate = 2.7f;
    }
}
