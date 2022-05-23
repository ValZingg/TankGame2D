using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Nom : Campaign.cs
    Description : Script gérant les différentes campagnes jouables depuis le menu principal.
     */

public class Campaign : MonoBehaviour
{
    //========================
    public string CampaignName;
    public Objective CampaignObjective;
    public float TimeLimit; //Temps limite (en seconde)
    public int TanksDestroyed = 0; //Nombre de tanks détruits par le joueur
    //========================

    private void Start()
    {
        CampaignName = GameObject.Find("DataTracker").GetComponent<DataTracker>().Objective_Save.ObjectiveName; //On récupère le nom de l'objectif
        CampaignObjective = GameObject.Find("DataTracker").GetComponent<DataTracker>().Objective_Save; //On récupère l'objectif dans DataTracker
        string campaigntype = CampaignObjective.GetType().Name; //On récupère le type d'objectif
        Debug.Log(CampaignObjective.GetType().Name);

        if(campaigntype == "Assassinate") //Si l'objectif est de type "assassinate"
        {
            GameObject[] enemytanks = GameObject.FindGameObjectsWithTag("EnemyTank"); //On récupère TOUT les tanks ennemis
            int RandomChoice = Random.Range(0, enemytanks.Length); //On sélectionne un tank aléatoire
            ((Assassinate)CampaignObjective).Target = enemytanks[RandomChoice]; //On assigne la cible
            enemytanks[RandomChoice].GetComponent<EnemyTank>().AssassinateLight.SetActive(true); //On active la lumière d'assassinat
        }

        if(campaigntype == "Destroy") //Si l'objectif est de type "destroy
        {
            GameObject[] enemytanks = GameObject.FindGameObjectsWithTag("EnemyTank"); //On récupère TOUT les tanks ennemis
            int RandomAmount = Random.Range(1, enemytanks.Length); //On choisis un nombre aléatoire de tanks à détruire, au minimum 1
            ((Destroy)CampaignObjective).Amount = RandomAmount;
        }

        CampaignObjective.Completed = false; //Empêche que l'objectif soit gardé comme "completé" entre chaque scènes
    }

    private void Update()
    {
        TimeLimit -= Time.deltaTime; // Réduit le temps limite à chaque frame qui passe, jusqu'a l'échec
        if (TimeLimit <= 0) EndCampaign(false); //termine la campagne avec une défaite

        if (CampaignObjective.Completed) EndCampaign(true); //Si l'objectif est accompli, on termine la campagne avec une victoire.
    }

    public void EndCampaign(bool status) //Termine la campagne actuelle, et vérifie si c'est une vitoire ou une défaite. Le status est égal à victoire ou défaite. True = victoire, false = défaite
    {
        UIManager uimanager = GameObject.Find("Canvas").GetComponent<UIManager>(); //On récupère l'UIMANAGER
        if(status) //Si c'est une victoire
        {
            uimanager.EndScreenText.text = "VICTOIRE";
            
        }
        else //Pour la défaite
        {
            uimanager.EndScreenText.text = "DEFAITE";
        }
        uimanager.EndScreenBG.SetActive(true);

    }

    public void AddTankDestruction() //appelé quand un tank ennemi est détruit. Cette fonction va vérifier si le type de mission et "destroy" et si oui, comparer le score avec l'objectif.
    {
        TanksDestroyed++;
        if(CampaignObjective.GetType().Name == "Destroy") //Si la campagne est de type "destroy"
        {
            if (TanksDestroyed == ((Destroy)CampaignObjective).Amount) CampaignObjective.Completed = true; //On complète l'objectif si le nombre de tanks à détruire est atteint.
        }
    }
}
