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
}
