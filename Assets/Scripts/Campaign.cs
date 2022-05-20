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

    private void Update()
    {
        TimeLimit -= Time.deltaTime; // R�duit le temps limite � chaque frame qui passe, jusqu'a l'�chec
        if (TimeLimit >= 0) EndCampaign();
    }

    public void EndCampaign() //Termine la campagne actuelle, et v�rifie si c'est une vitoire ou une d�faite
    {
        //TODO : R�uissite/D�faite
    }
}
