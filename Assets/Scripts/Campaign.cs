using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Nom : Campaign.cs
    Description : Script g�rant les diff�rentes campagnes jouables depuis le menu principal.
     */

public class Campaign : MonoBehaviour
{
    //========================
    public string CampaignName;
    public Objective CampaignObjective;
    public float TimeLimit;
    //========================

    private void Start()
    {
        CampaignName = GameObject.Find("DataTracker").GetComponent<DataTracker>().Objective_Save.ObjectiveName; //On r�cup�re le nom de l'objectif
        CampaignObjective = GameObject.Find("DataTracker").GetComponent<DataTracker>().Objective_Save; //On r�cup�re l'objectif dans DataTracker
        string campaigntype = CampaignObjective.GetType().Name; //On r�cup�re le type d'objectif

        if(campaigntype == "Assassinate") //Si l'objectif est de type "assassinate"
        {
            GameObject[] enemytanks = GameObject.FindGameObjectsWithTag("EnemyTank"); //On r�cup�re TOUT les tanks ennemis
            int RandomChoice = Random.Range(0, enemytanks.Length); //On s�lectionne un tank al�atoire
            ((Assassinate)CampaignObjective).Target = enemytanks[RandomChoice]; //On assigne la cible
            enemytanks[RandomChoice].GetComponent<EnemyTank>().AssassinateLight.SetActive(true); //On active la lumi�re d'assassinat
        }

        if(campaigntype == "Destroy") //Si l'objectif est de type "destroy
        {
            GameObject[] enemytanks = GameObject.FindGameObjectsWithTag("EnemyTank"); //On r�cup�re TOUT les tanks ennemis
            int RandomAmount = Random.Range(1, enemytanks.Length); //On choisis un nombre al�atoire de tanks � d�truire, au minimum 1
            ((Destroy)CampaignObjective).Amount = RandomAmount;
        }

        CampaignObjective.Completed = false; //Emp�che que l'objectif soit gard� comme "complet�" entre chaque sc�nes
    }

    private void Update()
    {
        TimeLimit -= Time.deltaTime; // R�duit le temps limite � chaque frame qui passe, jusqu'a l'�chec
        if (TimeLimit <= 0) EndCampaign(false); //termine la campagne avec une d�faite

        if (CampaignObjective.Completed) EndCampaign(true); //Si l'objectif est accompli, on termine la campagne avec une victoire.
    }

    public void EndCampaign(bool status) //Termine la campagne actuelle, et v�rifie si c'est une vitoire ou une d�faite. Le status est �gal � victoire ou d�faite. True = victoire, false = d�faite
    {
        UIManager uimanager = GameObject.Find("Canvas").GetComponent<UIManager>(); //On r�cup�re l'UIMANAGER
        if(status) //Si c'est une victoire
        {
            uimanager.EndScreenText.text = "VICTOIRE";
            
        }
        else //Pour la d�faite
        {
            uimanager.EndScreenText.text = "DEFAITE";
        }
        uimanager.EndScreenBG.SetActive(true);

    }
}
