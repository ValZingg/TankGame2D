using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    Nom : UIManager.cs
    Description : Contr�le tout les �l�ments de l'interface utilisateur ( Points de vie, barre de vie, etc) et les actualise en fonction des besoins
     */

public class UIManager : MonoBehaviour
{
    //=====================
    [Header("UIElements")] // Les �l�ments graphiques
    public Text HealthText;
    public Image HealthBar;
    public Image HealthBarEmpty;

    public Image TargetX, TargetY;
    public Text TargetText;

    public Image DamageOverlay; //Fait rougir l'�cran quand le joueur re�ois un obus

    public Text TimeLimitText; //Texte indiquant le temps restant

    [Header("Parameters")]
    public float DamageOverlayDecayRate = 0.05f; //La vitesse � laquelle l'�cran passe de rouge � normal

    [Header("External")]
    public TankControls PlayerTankScript; //Script de tank du joueur

    [Header("Booleans")]
    bool EnableTarget = true; //Activer/d�sactiver la cible TargetXY
    //=====================

    private void Start()
    {
        HealthText = GameObject.Find("HealthText").GetComponent<Text>();
        HealthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        HealthBarEmpty = GameObject.Find("HealthBarEmpty").GetComponent<Image>();

        TargetX = GameObject.Find("TargetX").GetComponent<Image>();
        TargetY = GameObject.Find("TargetY").GetComponent<Image>();
        TargetText = GameObject.Find("TargetText").GetComponent<Text>();

        if(!EnableTarget) //Si l'option est d�sactiv�e, on n'affiche pas la cible
        {
            TargetX.enabled = false;
            TargetY.enabled = false;
            TargetText.enabled = false;
        }

        DamageOverlay = GameObject.Find("DamageOverlay").GetComponent<Image>();
        DamageOverlay.color = new Color(80, 0, 0, 0); //Rend trensparent

        TimeLimitText = GameObject.Find("TimeLimitText").GetComponent<Text>();

    }


    private void Update()
    {
        HealthText.text = PlayerTankScript.TankScript.Health.ToString(); //R�cup�re les pv actuels du tank du joueur
        HealthBar.fillAmount = ((100 * PlayerTankScript.TankScript.Health) / PlayerTankScript.TankScript.MaxHealth) / 100; //Remplis la barre de vie en fonction du pourcentage de points de vie restant

        //Animation de healthbarempty
        float TargetFillAmount = HealthBar.fillAmount;
        if (HealthBarEmpty.fillAmount != TargetFillAmount && HealthBarEmpty.fillAmount > TargetFillAmount) HealthBarEmpty.fillAmount -= 0.002f;

        if(EnableTarget)
        {
            //Cible X, Y
            var MousePos = Input.mousePosition; // On r�cup�re la position du curseur
            TargetX.transform.position = MousePos;
            TargetY.transform.position = MousePos;
            TargetText.transform.position = new Vector3(MousePos.x + 75f, MousePos.y - 75f, MousePos.z);
            TargetText.text = "X : " + MousePos.x.ToString() + "\nY : " + MousePos.y.ToString();
            if (!PlayerTankScript.IsLoaded) TargetText.text = TargetText.text + "\nRECHARGE...";
            else TargetText.text = TargetText.text + "\nCANON PR�T";
        }

        if(DamageOverlay.color.a > 0) DamageOverlay.color = new Color(80, 0, 0, DamageOverlay.color.a - DamageOverlayDecayRate); //Si le calque rouge n'est pas transparent, il le deviens petit � petit
        Debug.Log("Current Alpha : " + DamageOverlay.color.a);

        //Affiche le temps restant
        TimeLimitText.text = GameObject.Find("CampaignManager").GetComponent<Campaign>().TimeLimit.ToString("0:00");
    }

    public void ActivateRedOverlay() //Active le calque rouge sur l'�cran, puis le fait disparaitre petit � petit
    {
        DamageOverlay.color = new Color(80, 0, 0, 0.7f);
    }
}
