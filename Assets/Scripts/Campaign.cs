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
    public float TimeLimit;
    //========================

    private void Update()
    {
        TimeLimit -= Time.deltaTime; // Réduit le temps limite à chaque frame qui passe, jusqu'a l'échec
        if (TimeLimit >= 0) EndCampaign();
    }

    public void EndCampaign() //Termine la campagne actuelle, et vérifie si c'est une vitoire ou une défaite
    {
        //TODO : Réuissite/Défaite
    }
}
