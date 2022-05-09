using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Nom : TankControls.cs
    Description : Script servant à contrôler le tank, et tourner + Tirer
     */

public class EnemyTank : MonoBehaviour
{
    //====================================

    [Header("Base Tank Values")]
    public string TankToLoad;
    public Tank TankScript;
    public float tankspeed = 5.0f;
    public float currentspeed = 0f;
    public float acceleration = 0.1f;
    public float tankrotatespeed = 2.0f;
    public float canonrotatespeed = 2.0f;

    [Header("Shooting")]
    public float ShootCoolDown = 1.0f; //Temps de recharge entre chaque tir
    public bool IsLoaded = true; //Est-ce qu'un obus est chargé ?


    [Header("GameObjects")]
    public GameObject CenterPoint;
    public GameObject PointingToPoint;
    public GameObject CanonExitPoint;
    public GameObject Canon;
    public GameObject tankbody;

    [Header("Prefabs")]
    public GameObject ShellPrefab;
    public GameObject ExplosionEffect;

    //====================================

    void Start()
    {
        //Charge le bon tank suivant le choix
        if (TankToLoad == "LightTank") TankScript = new LightTank();
        else if (TankToLoad == "MediumTank") TankScript = new MediumTank();
        else if (TankToLoad == "HeavyTank") TankScript = new HeavyTank();

        //Récupère les données du dit tank
        canonrotatespeed = TankScript.CanonTurnRate;
        tankspeed = TankScript.Speed;
        tankrotatespeed = TankScript.TurnRate;
        acceleration = TankScript.Acceleration;
        ShootCoolDown = TankScript.FiringRate;

    }

    void Update()
    {
        //TODO : Intelligence artificielle
    }

    public void Explode() //Explosion du tank lorsqu'il est détruit
    {
        var explosion = Instantiate(ExplosionEffect); //On crée l'effet d'explosion
        explosion.transform.position = transform.position;

        TankScript.RestrictMovement = true; //On empêche tout mouvement
    }

    public IEnumerator Reload(float seconds) //Fonction chronométrant le temps de recharge depuis le dernier tir
    {
        yield return new WaitForSeconds(seconds); //Après x délai, l'obus sera à nouveau chargé et le joueur pourra tirer

        IsLoaded = true;
    }   

}
