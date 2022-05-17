using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        if(!EnableTarget)
        {
            TargetX.enabled = false;
            TargetY.enabled = false;
            TargetText.enabled = false;
        }
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
    }
}
