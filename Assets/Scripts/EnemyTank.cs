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
    public bool IsLoaded = true; //Est-ce qu'un obus est charg� ?

    [Header("Intelligence Artificielle")]
    public string CurrentMode = "Patrol"; //Comportement actual, ex : "Patrouille, poursuite"
    public Vector3 CurrentWayPoint; //Point dans laquelle le tank est en train de se diriger
    public GameObject Target; //Cible que le canon doit viser
    public bool InRangeToShoot = false; //Bonne distance pour tirer ?
    public float DistanceToTarget = 0f; //Distance jusqu'a la cible


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

        //R�cup�re les donn�es du dit tank
        canonrotatespeed = TankScript.CanonTurnRate;
        tankspeed = TankScript.Speed;
        tankrotatespeed = TankScript.TurnRate;
        acceleration = TankScript.Acceleration;
        ShootCoolDown = TankScript.FiringRate;

    }

    void Update()
    {
        //On calcule la distance jusqu'a la cible, si il y en a une.
        if (Target != null) DistanceToTarget = Vector3.Distance(transform.position, Target.transform.position);

        if(DistanceToTarget > 40f && CurrentMode == "Pursuit") //Si le tank du joueur s'�loigne trop
        {
            CurrentWayPoint = Target.transform.position;// On met une derni�re fois � jour la position du joueur, pour que le tank se d�place vers la derni�re position connue
            CurrentMode = "Search"; //Le tank passe en mode "recherche"
            StartCoroutine(SearchTimer()); //On commence le timer de recherche
        }

        if(CurrentMode == "Pursuit") //Si le tank est en mode "poursuite"
        {
            CurrentWayPoint = Target.transform.position; //On met � jour la destination chaque frame
        }

        if(CurrentWayPoint != null) //Tant que le tank a un point de destination
        {
            //On soustrais la position de la destination et celle du tank, puis on normalise
            Vector3 dirFromAtoB = (CurrentWayPoint - TankBody.transform.position).normalized;
            //On trouve le produit scalaire du r�sultat, et ceci nous permettra de savoir si le tank fait face � sa destination
            float dotProd = Vector3.Dot(dirFromAtoB, TankBody.transform.right);

            //On calcule aussi la distance qu'il reste entre le tank et sa destination
            float distance = Vector3.Distance(CurrentWayPoint, TankBody.transform.position);

            if (dotProd < 0.9 && distance > 3) //Tant que le tank n'est pas totalement align� � sa destination, et pas trop proche de celle-ci
            {
                ControlTank("turnleft"); //Il tourne
            }

            if (distance >= 40) ControlTank("aimforspeed", tankspeed); //Si la distance est d'au moins 40, le tank acc�l�re
            else if (distance > 10 && distance < 40) ControlTank("aimforspeed", tankspeed / 2); //Si il est entre 10 et 40 de distance, il va avancer � la moiti� de sa vitesse maximum
            else if (distance <= 10) ControlTank("stop"); //Arriv� � 10 ou moins de distance, le tank va ralentir pour s'arr�ter

            Debug.Log("DOTPROD = " + dotProd + " / DIST = " + distance);
        }

        if(Target != null && InRangeToShoot) //Si le tank a une cible
        {
            //ROTATION DU CANON
            var pos = gameObject.transform.position;
            var dir = Target.transform.position - pos;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
            Canon.transform.rotation = Quaternion.Slerp(Canon.transform.rotation, rotation, canonrotatespeed * Time.deltaTime / 2);

            if(CurrentMode == "Shoot")
            {
                if (IsLoaded) //Si un obus est charg�
                {
                    var CreatedShell = Instantiate(ShellPrefab); //On cr�e l'obus

                    //On assigne la m�me position et rotation que le bout du canon
                    CreatedShell.transform.position = CanonExitPoint.transform.position;
                    CreatedShell.transform.rotation = CanonExitPoint.transform.rotation;

                    //On assigne les d�g�ts et statistiques
                    CreatedShell.GetComponent<ShellObject>().Damage = TankScript.DamagePerShell;
                    CreatedShell.GetComponent<ShellObject>().IsPlayerShell = false;

                    //L'obus n'est plus charg�, vu qu'il a �t� tir�
                    IsLoaded = false;

                    //On d�bute le temps de recharge
                    StartCoroutine(Reload(ShootCoolDown));
                }
            }
        }

        //MOUVEMENT DU TANK
        //Bouge le tank
        if (currentspeed != 0) transform.position += TankBody.transform.right * Time.deltaTime * currentspeed;

        //Corrige les erreurs de vitesse
        if (currentspeed > tankspeed) currentspeed = tankspeed;
        if (currentspeed < -tankspeed) currentspeed = -tankspeed;
    }

    public void ControlTank(string order, float aimedspeed = 10) //Contr�le le tank suivant l'ordre (ex : acc�l�rer, d�c�lerer, tourner...)
    {
        if(order == "accelerate") currentspeed += acceleration * Time.deltaTime;
        if(order == "aimforspeed")
        {
            if(currentspeed > aimedspeed) currentspeed -= acceleration * Time.deltaTime;
            else currentspeed += acceleration * Time.deltaTime;
        }
        if(order == "reverse") currentspeed -= acceleration * Time.deltaTime;
        if(order == "stop")
        {
            if(currentspeed > 0) currentspeed -= acceleration * Time.deltaTime * 2;
        }
        if(order == "turnright") TankBody.transform.Rotate(Vector3.back * tankrotatespeed * Time.deltaTime);
        if(order == "turnleft") TankBody.transform.Rotate(Vector3.forward * tankrotatespeed * Time.deltaTime);
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

    public void MatchSpriteToColliderSize() //Fonction ajustant la taille d'un collider pour correspondre au sprite visible
    { /* Source : https://forum.unity.com/threads/changing-boxcollider2d-size-to-match-sprite-bounds-at-runtime.267964/ */

        Vector2 S = TankBody.GetComponent<SpriteRenderer>().sprite.bounds.size;
        TankBody.GetComponent<BoxCollider2D>().size = S;
    }

    private void OnTriggerEnter2D(Collider2D collision)//Quand un tank entre dans le champ de vision de ce tank
    {
        if (collision.gameObject.tag == "PlayerTank") //Si ce tank est celui du joueur
        {
            StopAllCoroutines(); //Si le timer de recherche �tait lanc�, on l'arr�te

            Target = collision.gameObject; //On vise le joueur
            InRangeToShoot = true; //Assez proche pour tirer
            CurrentMode = "Shoot"; //Entre en mode "Tir"
            CurrentWayPoint = collision.transform.position; //On se dirige vers lui
        }
    }

    private void OnTriggerExit2D(Collider2D collision)//Quand un tank sort du champ de vision de ce tank
    {
        if (collision.gameObject.tag == "PlayerTank") //Si ce tank est celui du joueur
        {
            InRangeToShoot = false; //Plus assez proche pour tirer
            CurrentMode = "Pursuit"; //Entre en mode "poursuite"
        }
    }

    public IEnumerator SearchTimer()
    {
        yield return new WaitForSeconds(30f); //On attend 30 secondes
        Target = null; //Le tank du joueur s�me ce tank
        CurrentMode = "Patrol"; //On retourne en patrouille
    }
}
