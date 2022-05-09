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
}
