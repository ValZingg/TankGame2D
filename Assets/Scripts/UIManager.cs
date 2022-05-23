using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
    Nom : UIManager.cs
    Description : Contrôle tout les éléments de l'interface utilisateur ( Points de vie, barre de vie, etc) et les actualise en fonction des besoins
     */

public class UIManager : MonoBehaviour
{
    //=====================
    [Header("UIElements")] // Les éléments graphiques
    public Text HealthText;
    public Image HealthBar;
    public Image HealthBarEmpty;

    public Image TargetX, TargetY;
    public Text TargetText;

    public Image DamageOverlay; //Fait rougir l'écran quand le joueur reçois un obus

    public Text TimeLimitText; //Texte indiquant le temps restant

    public Text EndScreenText; //Le texte qui affichera "Victoire" ou "Défaite"
    public GameObject EndScreenBG; //La fenêtre de fin de partie

    [Header("Parameters")]
    public float DamageOverlayDecayRate = 0.05f; //La vitesse à laquelle l'écran passe de rouge à normal

    [Header("External")]
    public TankControls PlayerTankScript; //Script de tank du joueur

    [Header("Booleans")]
    bool EnableTarget = true; //Activer/désactiver la cible TargetXY
    //=====================

    private void Start()
    {
        HealthText = GameObject.Find("HealthText").GetComponent<Text>();
        HealthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        HealthBarEmpty = GameObject.Find("HealthBarEmpty").GetComponent<Image>();

        TargetX = GameObject.Find("TargetX").GetComponent<Image>();
        TargetY = GameObject.Find("TargetY").GetComponent<Image>();
        TargetText = GameObject.Find("TargetText").GetComponent<Text>();

        if(!EnableTarget) //Si l'option est désactivée, on n'affiche pas la cible
        {
            TargetX.enabled = false;
            TargetY.enabled = false;
            TargetText.enabled = false;
        }

        DamageOverlay = GameObject.Find("DamageOverlay").GetComponent<Image>();
        DamageOverlay.color = new Color(80, 0, 0, 0); //Rend trensparent

        TimeLimitText = GameObject.Find("TimeLimitText").GetComponent<Text>();

        EndScreenBG = GameObject.Find("EndScreenBG");
        EndScreenText = GameObject.Find("EndScreenTitle").GetComponent<Text>();
        EndScreenBG.SetActive(false); //On cache l'écran de fin

    }


    private void Update()
    {
        HealthText.text = PlayerTankScript.TankScript.Health.ToString(); //Récupère les pv actuels du tank du joueur
        HealthBar.fillAmount = ((100 * PlayerTankScript.TankScript.Health) / PlayerTankScript.TankScript.MaxHealth) / 100; //Remplis la barre de vie en fonction du pourcentage de points de vie restant

        //Animation de healthbarempty
        float TargetFillAmount = HealthBar.fillAmount;
        if (HealthBarEmpty.fillAmount != TargetFillAmount && HealthBarEmpty.fillAmount > TargetFillAmount) HealthBarEmpty.fillAmount -= 0.002f;

        if(EnableTarget)
        {
            //Cible X, Y
            var MousePos = Input.mousePosition; // On récupère la position du curseur
            TargetX.transform.position = MousePos;
            TargetY.transform.position = MousePos;
            TargetText.transform.position = new Vector3(MousePos.x + 75f, MousePos.y - 75f, MousePos.z);
            TargetText.text = "X : " + MousePos.x.ToString() + "\nY : " + MousePos.y.ToString();
            if (!PlayerTankScript.IsLoaded) TargetText.text = TargetText.text + "\nRECHARGE...";
            else TargetText.text = TargetText.text + "\nCANON PRÊT";
        }

        if(DamageOverlay.color.a > 0) DamageOverlay.color = new Color(80, 0, 0, DamageOverlay.color.a - DamageOverlayDecayRate); //Si le calque rouge n'est pas transparent, il le deviens petit à petit

        //Affiche le temps restant
        TimeLimitText.text = GameObject.Find("CampaignManager").GetComponent<Campaign>().TimeLimit.ToString("0:00");
    }

    public void ActivateRedOverlay() //Active le calque rouge sur l'écran, puis le fait disparaitre petit à petit
    {
        DamageOverlay.color = new Color(80, 0, 0, 0.7f);
    }

    public void ReturnToMenuButtonClick() //Quand le joueur clique sur le bouton "Retour au menu"
    {
        SceneManager.LoadScene(0); //On retourne au menu principal
    }
}
