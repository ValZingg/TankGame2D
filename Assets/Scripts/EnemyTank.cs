using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Nom : TankControls.cs
    Description : Script pour les tanks ennemis. Controlant l'IA
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
    public GameObject TankBody;
    public GameObject TankTurret;

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

        //Charge les graphismes du tank
        TankBody.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("TankTex/" + TankToLoad + "/Hull"); //Texture du corps du tank
        TankBody.transform.localScale = new Vector3(1.5f, 1.5f, 1f); //Ajustement de la taille

        TankTurret.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("TankTex/" + TankToLoad + "/Turret"); //Texture de la tourelle du tank
        TankTurret.transform.localScale = new Vector3(0.7f, 0.7f, 1f);//Ajustement de la taille
        TankTurret.transform.position += new Vector3(0.25f, 0f, 0);//et de la position

        Canon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("TankTex/" + TankToLoad + "/Canon");
        Canon.transform.localScale = new Vector3(1f, 1f, 1f);
        Canon.transform.position = TankTurret.transform.position;

        MatchSpriteToColliderSize();

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

    public void MatchSpriteToColliderSize() //Fonction ajustant la taille d'un collider pour correspondre au sprite visible
    { /* Source : https://forum.unity.com/threads/changing-boxcollider2d-size-to-match-sprite-bounds-at-runtime.267964/ */

        Vector2 S = TankBody.GetComponent<SpriteRenderer>().sprite.bounds.size;
        TankBody.GetComponent<BoxCollider2D>().size = S;
    }

}
