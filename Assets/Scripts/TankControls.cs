using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Nom : TankControls.cs
    Description : Script servant � contr�ler le tank, et tourner + Tirer
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
    public bool IsLoaded = true; //Est-ce qu'un obus est charg� ?

    [Header("Line renderer")]
    private LineRenderer Linerenderer_Aim;
    public LineRenderer Linerenderer_Canon;

    [Header("GameObjects")]
    public GameObject CenterPoint;
    public GameObject PointingToPoint;
    public GameObject CanonExitPoint;
    public GameObject Canon;
    public GameObject tankbody;

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

        //R�cup�re les donn�es du dit tank
        canonrotatespeed = TankScript.CanonTurnRate;
        tankspeed = TankScript.Speed;
        tankrotatespeed = TankScript.TurnRate;
        acceleration = TankScript.Acceleration;
        ShootCoolDown = TankScript.FiringRate;


        Linerenderer_Aim = GetComponent<LineRenderer>();
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
        Canon.transform.rotation = Quaternion.Slerp(Canon.transform.rotation, rotation, canonrotatespeed * Time.deltaTime * 2);

        //LIGNE DE VIS�E
        Linerenderer_Aim.SetPosition(0, Canon.transform.position);
        Linerenderer_Aim.SetPosition(1, MainCamera.ScreenToWorldPoint(Input.mousePosition));
        Linerenderer_Canon.SetPosition(0, Canon.transform.position);
        Linerenderer_Canon.SetPosition(1, PointingToPoint.transform.position);

        //Contr�le du tir
        if(Input.GetMouseButtonDown(0)) //Si le joueur utilise le clic gauche de la souris
        {
            if(IsLoaded) //Si un obus est charg�
            {
                var CreatedShell = Instantiate(ShellPrefab); //On cr�e l'obus

                //On assigne la m�me position et rotation que le bout du canon
                CreatedShell.transform.position = CanonExitPoint.transform.position;
                CreatedShell.transform.rotation = CanonExitPoint.transform.rotation;

                //On assigne les d�g�ts et statistiques
                CreatedShell.GetComponent<ShellObject>().Damage = TankScript.DamagePerShell;
                CreatedShell.GetComponent<ShellObject>().IsPlayerShell = true;

                //L'obus n'est plus charg�, vu qu'il a �t� tir�
                IsLoaded = false;

                //On d�bute le temps de recharge
                StartCoroutine(Reload(ShootCoolDown));
            }
            //Si un obus n'est pas charg�, rien ne se passe.
        }


        //Mouvement du tank
        if(!TankScript.RestrictMovement)
        {
            //Si une des touches de mouvement est appuy�e, on ajoute la rotation ou l'acc�leration
            if (Input.GetKey(KeyCode.A)) tankbody.transform.Rotate(Vector3.forward * tankrotatespeed * Time.deltaTime);
            else if (Input.GetKey(KeyCode.D)) tankbody.transform.Rotate(Vector3.back * tankrotatespeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.W)) currentspeed += acceleration * Time.deltaTime;
            else if(Input.GetKey(KeyCode.S)) currentspeed -= acceleration * Time.deltaTime;
         

            //Bouge le tank
            if (currentspeed != 0) transform.position += tankbody.transform.right * Time.deltaTime * currentspeed;

            //Corrige les erreurs de vitesse
            if(currentspeed > tankspeed)currentspeed = tankspeed;
            if (currentspeed < -tankspeed) currentspeed = -tankspeed;
        }

    }
    public void Explode() //Explosion du tank lorsqu'il est d�truit
    {
        var explosion = Instantiate(ExplosionEffect); //On cr�e l'effet d'explosion
        explosion.transform.position = transform.position;

        TankScript.RestrictMovement = true; //On emp�che tout mouvement
    }

    public IEnumerator Reload(float seconds) //Fonction chronom�trant le temps de recharge depuis le dernier tir
    {
        yield return new WaitForSeconds(seconds); //Apr�s x d�lai, l'obus sera � nouveau charg� et le joueur pourra tirer

        IsLoaded = true;
    }   

}
