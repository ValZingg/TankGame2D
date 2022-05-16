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

        //Récupère les données du dit tank
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

        if(CurrentMode == "Pursuit") //Si le tank est en mode "poursuite"
        {
            CurrentWayPoint = Target.transform.position; //On met à jour la destination chaque frame
        }

        if(CurrentWayPoint != null) //Tant que le tank a un point de destination
        {
            //On soustrais la position de la destination et celle du tank, puis on normalise
            Vector3 dirFromAtoB = (CurrentWayPoint - TankBody.transform.position).normalized;
            //On trouve le produit scalaire du résultat, et ceci nous permettra de savoir si le tank fait face à sa destination
            float dotProd = Vector3.Dot(dirFromAtoB, TankBody.transform.right);

            //On calcule aussi la distance qu'il reste entre le tank et sa destination
            float distance = Vector3.Distance(CurrentWayPoint, TankBody.transform.position);

            if (dotProd < 0.9 && distance > 3) //Tant que le tank n'est pas totalement aligné à sa destination, et pas trop proche de celle-ci
            {
                ControlTank("turnleft"); //Il tourne
            }

            if (distance >= 40) ControlTank("aimforspeed", tankspeed); //Si la distance est d'au moins 40, le tank accélère
            else if (distance > 10 && distance < 40) ControlTank("aimforspeed", tankspeed / 2); //Si il est entre 10 et 40 de distance, il va avancer à la moitié de sa vitesse maximum
            else if (distance <= 10) ControlTank("stop"); //Arrivé à 10 ou moins de distance, le tank va ralentir pour s'arrêter

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


        }

        //MOUVEMENT DU TANK
        //Bouge le tank
        if (currentspeed != 0) transform.position += TankBody.transform.right * Time.deltaTime * currentspeed;

        //Corrige les erreurs de vitesse
        if (currentspeed > tankspeed) currentspeed = tankspeed;
        if (currentspeed < -tankspeed) currentspeed = -tankspeed;
    }

    public void ControlTank(string order, float aimedspeed = 10) //Contrôle le tank suivant l'ordre (ex : accélérer, décélerer, tourner...)
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

    private void OnTriggerEnter2D(Collider2D collision)//Quand un tank entre dans le champ de vision de ce tank
    {
        if (collision.gameObject.tag == "PlayerTank") //Si ce tank est celui du joueur
        {
            StopAllCoroutines(); //Si le timer de recherche était lancé, on l'arrête

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
            StartCoroutine(SearchTimer()); //On commence le timer de recherche
        }
    }

    public IEnumerator SearchTimer()
    {
        yield return new WaitForSeconds(30f); //On attend 30 secondes
        Target = null; //Le tank du joueur sème ce tank
        CurrentMode = "Patrol"; //On retourne en patrouille
    }
}
