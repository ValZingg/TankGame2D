using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Nom : DestroyAfterXSeconds.cs
    Description : Simple script servant à détruire l'objet après x secondes
     */

public class DestroyAfterXSeconds : MonoBehaviour
{
    public float SecondsBeforeDestroy = 1.0f;

    private void Start()
    {
        StartCoroutine(DestroyAfterX(SecondsBeforeDestroy));
    }

    public IEnumerator DestroyAfterX(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
