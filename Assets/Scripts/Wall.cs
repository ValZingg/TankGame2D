using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Nom : Wall.cs
    Description : Ce script, appliqué sur les murs, stoppe les tanks qui rentrent dedant à trop grande vitesse.
     */

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "PlayerTank") //Quand le tank du joueur touche le mur
        {           
            if(collision.gameObject.GetComponent<TankControls>().currentspeed > collision.gameObject.GetComponent<TankControls>().tankspeed / 3)
            {/* Si le tank a atteint au moins un tier de sa vitesse max */
                collision.gameObject.GetComponent<TankControls>().currentspeed = 0f; //On stoppe le tank
            }
        }

        if(collision.gameObject.tag == "EnemyTank")
        {
            if (collision.gameObject.GetComponent<EnemyTank>().currentspeed > collision.gameObject.GetComponent<EnemyTank>().tankspeed / 3)
            {/* Si le tank a atteint au moins un tier de sa vitesse max */
                collision.gameObject.GetComponent<EnemyTank>().currentspeed = 0f; //On stoppe le tank
            }
        }
    }
}
