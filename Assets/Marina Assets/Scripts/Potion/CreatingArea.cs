using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatingArea : MonoBehaviour
{
    private PotionCrafting potiongCrafting;

    private void Awake()
    {
        potiongCrafting = FindAnyObjectByType<PotionCrafting>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            potiongCrafting.canCreate = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            potiongCrafting.canCreate = false;
        }
    }
}
