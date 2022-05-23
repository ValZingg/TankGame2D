using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    Nom : ShellObject.cs
    Description : Script faisant avançer continuellement l'obus, et la détéction de collisions
     */

public class ShellObject : MonoBehaviour
{
    //=======================
    [Header("Attributes")]
    public float ProjectileSpeed = 10f; //Vitesse du projectile
    public bool IsPlayerShell = true; //Est-ce l'obus du joueur ou d'un ennemi ?
    public float Damage; //Dégâts que l'obus va infliger
    public float LifeTime = 5.0f; //durée avant la destruction automatique du projectile

    [Header("Prefabs")]
    public GameObject SmokeEffect;
    //=======================

    private void Start()
    {
        StartCoroutine(DestroyAfterXSeconds(LifeTime)); //Détruit l'obus après x secondes si rien n'est touché
    }

    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * ProjectileSpeed;
    }

    public IEnumerator DestroyAfterXSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds); //On attend d'abord x secondes
        Destroy(gameObject); //Puis on autodétruit l'objet
    }

    private void OnCollisionEnter2D(Collision2D collision) //Détéction des collisions + destruction
    {
        var smoke = Instantiate(SmokeEffect); //On fait apparaitre de la fumée
        smoke.transform.position = transform.position; //On s'assure que la fumée a la même position que l'endroit de l'impact

        if(collision.gameObject.tag == "EnemyTank") //----On vérifie si l'impact est un tank ennemi
        {
            if(IsPlayerShell)//Si l'obus a été tiré par le joueur
            {
                if(collision.gameObject.GetComponent<EnemyTank>().TankScript.Health > 0) //Si il possède toujours un peu de vie
                {
                    collision.gameObject.GetComponent<EnemyTank>().TankScript.Health -= Damage; //On inflige les dégâts
                    if (collision.gameObject.GetComponent<EnemyTank>().TankScript.Health <= 0)
                    {
                        collision.gameObject.GetComponent<EnemyTank>().Explode(); //On joue l'animation d'explosion

                        if (collision.gameObject.GetComponent<EnemyTank>().AssassinateLight.activeSelf) //Si le tank a la lumière d'assassinat activé, alors on déduit que la mission actuelle est une mission d'assassinat, et que la cible vient de mourir.
                            GameObject.Find("CampaignManager").GetComponent<Campaign>().CampaignObjective.CompleteObjective(); //On accompli donc l'objectif.

                        Destroy(collision.gameObject); //On supprime le tank ennemi détruit
                    }
                }
                
            }
        }
        else if(collision.gameObject.tag == "PlayerTank")//----Si le tank touché est le tank du joueur
        {
            if(!IsPlayerShell) //Alors on vérifie que l'obus ne viens pas du joueur lui même
            {
                if (collision.gameObject.GetComponent<TankControls>().TankScript.Health > 0) //Si il possède toujours un peu de vie
                {
                    collision.gameObject.GetComponent<TankControls>().TankScript.Health -= Damage; //On inflige les dégâts
                    GameObject.Find("Canvas").GetComponent<UIManager>().ActivateRedOverlay(); //On active le calque rouge sur l'écran
                    if (collision.gameObject.GetComponent<TankControls>().TankScript.Health <= 0)
                    {
                        collision.gameObject.GetComponent<TankControls>().Explode(); //On joue l'animation d'explosion
                        GameObject.Find("CampaignManager").GetComponent<Campaign>().EndCampaign(false); //On montre l'écran de défaite
                        Destroy(collision.gameObject); //On supprime le tank du joueur

                        //TODO : GAME OVER
                    }
                }
            }
        }

        Destroy(gameObject); // On détruit cet obus.
    }
}
