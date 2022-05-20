using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


/*
 *  Nom : MainMenuButtonHover.cs
 *  Description : Simple script pour faire grandir le texte d'un bouton et le rendre coloré lorsqu'on le survole avec la souris.
 */

public class MainMenuButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Lorsque la souris entre dans la zone de l'objet
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Text>().fontSize = 90;
        GetComponent<Text>().color = Color.yellow;
    }

    //Lorsque la souris en sors
    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Text>().fontSize = 70;
        GetComponent<Text>().color = Color.white;
    }
}
