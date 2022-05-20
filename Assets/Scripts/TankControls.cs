using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Nom : TankControls.cs
    Description : Script servant à contrôler le tank, et tourner + Tirer. Ce script est uniquement pour le Tank du joueur
     */

public class TankControls : MonoBehaviour
{
    //##### POUR JOUEURS SEULEMENT #####
    //====================================
    [Header("Camera")]
    public Camera MainCamera;
    public float cameraspeed;
    public float smoothing = 0.001f;

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

    [Header("Line renderer")]
    public LineRenderer Linerenderer_Canon;

    [Header("GameObjects")]
    public GameObject CenterPoint; //Point central du tank  
    public GameObject PointingToPoint; //Le point où pointe le canon du tank    
    public GameObject CanonExitPoint; //Le bout du canon du tank, là ou va apparaître les obus
    public GameObject Canon; //Le canon
    public GameObject TankBody; //Le "corps" du tank
    public GameObject TankTurret; //La tourelle du tank

    [Header("Prefabs")]
    public GameObject ShellPrefab;
    public Texture2D CursorTexture;
    public GameObject ExplosionEffect;

    //====================================

    void Start()
    {
        //Ajuste le curseur
        Cursor.SetCursor(CursorTexture, new Vector2(CursorTexture.width / 2, CursorTexture.height / 2), CursorMode.Auto);

        //Charge le bon tank suivant le choix
        if (TankToLoad == "LightTank") TankScript = new LightTank();
        else if (TankToLoad == "MediumTank") TankScript = new MediumTank();
        else if (TankToLoad == "HeavyTank") TankScript = new HeavyTank();

        //Charge les graphismes du tank
        TankBody.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("TankTex/" + TankToLoad + "/Hull"); //Texture du corps du tank
        TankBody.transform.localScale = new Vector3(1.5f * TankScript.SizeModifier, 1.5f * TankScript.SizeModifier, 1f * TankScript.SizeModifier); //Ajustement de la taille

        TankTurret.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("TankTex/" + TankToLoad + "/Turret"); //Texture de la tourelle du tank
        TankTurret.transform.localScale = new Vector3(0.7f * TankScript.SizeModifier, 0.7f * TankScript.SizeModifier, 1f * TankScript.SizeModifier);//Ajustement de la taille
        TankTurret.transform.position += new Vector3(0.25f * TankScript.SizeModifier, 0f, 0);//et de la position

        Canon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("TankTex/" + TankToLoad + "/Canon");
        Canon.transform.localScale = new Vector3(1f * TankScript.SizeModifier, 1f * TankScript.SizeModifier, 1f * TankScript.SizeModifier);
        Canon.transform.position = TankTurret.transform.position;

        MatchSpriteToColliderSize(); //On ajuste la position du collider

        //Récupère les données du dit tank
        canonrotatespeed = TankScript.CanonTurnRate;
        tankspeed = TankScript.Speed;
        tankrotatespeed = TankScript.TurnRate;
        acceleration = TankScript.Acceleration;
        ShootCoolDown = TankScript.FiringRate;
    }

    void Update()
    {
        //MOUVEMENT DE LA CAMERA
        Rect screenrect = new Rect(0f, 0f, Screen.width, Screen.height);
        if(screenrect.Contains(Input.mousePosition))
        {
            var relativemousepos = CenterPoint.transform.position + (Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
            MainCamera.transform.position = Vector2.Lerp(CenterPoint.transform.position, relativemousepos, smoothing);
            MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, MainCamera.transform.position.y, -10);
        }

        //ROTATION DU CANON
        var pos = Camera.main.WorldToScreenPoint(Canon.transform.position);
        var dir = Input.mousePosition - pos;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
        Canon.transform.rotation = Quaternion.Slerp(Canon.transform.rotation, rotation, canonrotatespeed * Time.deltaTime);

        //LIGNE DE VISéE
        Linerenderer_Canon.SetPosition(0, Canon.transform.position);
        Linerenderer_Canon.SetPosition(1, PointingToPoint.transform.position);

        //Contrôle du tir
        if(Input.GetMouseButtonDown(0)) //Si le joueur utilise le clic gauche de la souris
        {
            if(IsLoaded) //Si un obus est chargé
            {
                var CreatedShell = Instantiate(ShellPrefab); //On crée l'obus

                //On assigne la même position et rotation que le bout du canon
                CreatedShell.transform.position = CanonExitPoint.transform.position;
                CreatedShell.transform.rotation = CanonExitPoint.transform.rotation;

                //On assigne les dégâts et statistiques
                CreatedShell.GetComponent<ShellObject>().Damage = TankScript.DamagePerShell;
                CreatedShell.GetComponent<ShellObject>().IsPlayerShell = true;

                //L'obus n'est plus chargé, vu qu'il a été tiré
                IsLoaded = false;

                //On débute le temps de recharge
                StartCoroutine(Reload(ShootCoolDown));
            }
            //Si un obus n'est pas chargé, rien ne se passe.
        }


        //Mouvement du tank
        if(!TankScript.RestrictMovement)
        {
            //Si une des touches de mouvement est appuyée, on ajoute la rotation ou l'accéleration
            if (Input.GetKey(KeyCode.A)) TankBody.transform.Rotate(Vector3.forward * tankrotatespeed * Time.deltaTime);
            else if (Input.GetKey(KeyCode.D)) TankBody.transform.Rotate(Vector3.back * tankrotatespeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.W)) currentspeed += acceleration * Time.deltaTime;
            else if(Input.GetKey(KeyCode.S)) currentspeed -= acceleration * Time.deltaTime;
         

            //Bouge le tank
            if (currentspeed != 0) transform.position += TankBody.transform.right * Time.deltaTime * currentspeed;

            //Corrige les erreurs de vitesse
            if(currentspeed > tankspeed)currentspeed = tankspeed;
            if (currentspeed < -tankspeed) currentspeed = -tankspeed;
        }

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
