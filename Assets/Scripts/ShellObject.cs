using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    Nom : ShellObject.cs
    Description : Script faisant avan�er continuellement l'obus, et la d�t�ction de collisions
     */

public class ShellObject : MonoBehaviour
{
    //=======================
    [Header("Attributes")]
    public float ProjectileSpeed = 10f; //Vitesse du projectile
    public bool IsPlayerShell = true; //Est-ce l'obus du joueur ou d'un ennemi ?
    public float Damage; //D�g�ts que l'obus va infliger
    public float LifeTime = 5.0f; //dur�e avant la destruction automatique du projectile

    [Header("Prefabs")]
    public GameObject SmokeEffect;
    //=======================

    private void Start()
    {
        StartCoroutine(DestroyAfterXSeconds(LifeTime)); //D�truit l'obus apr�s x secondes si rien n'est touch�
    }

    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * ProjectileSpeed;
    }

    public IEnumerator DestroyAfterXSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds); //On attend d'abord x secondes
        Destroy(gameObject); //Puis on autod�truit l'objet
    }

    private void OnCollisionEnter2D(Collision2D collision) //D�t�ction des collisions + destruction
    {
        var smoke = Instantiate(SmokeEffect); //On fait apparaitre de la fum�e
        smoke.transform.position = transform.position; //On s'assure que la fum�e a la m�me position que l'endroit de l'impact

        if(collision.gameObject.tag == "EnemyTank") //On v�rifie si l'impact est un tank ennemi
        {
            if(IsPlayerShell)//Si l'obus a �t� tir� par le joueur
            {
                if(collision.gameObject.GetComponent<EnemyTank>().TankScript.Health > 0) //Si il poss�de toujours un peu de vie
                {
                    collision.gameObject.GetComponent<EnemyTank>().TankScript.Health -= Damage; //On inflige les d�g�ts
                    Debug.Log("Life remaining : " + collision.gameObject.GetComponent<EnemyTank>().TankScript.Health);
                    if (collision.gameObject.GetComponent<EnemyTank>().TankScript.Health <= 0)
                    {
                        collision.gameObject.GetComponent<EnemyTank>().Explode(); //On joue l'animation d'explosion
                        Destroy(collision.gameObject); //On supprime le tank ennemi d�truit
                    }
                }
                
            }
        }
        else if(collision.gameObject.tag == "PlayerTank")// Si le tank touch� est le tank du joueur
        {
            if(!IsPlayerShell) //Alors on v�rifie que l'obus ne viens pas du joueur lui m�me
            {

            }
        }

        Destroy(gameObject); // On d�truit cet obus.
    }
}
