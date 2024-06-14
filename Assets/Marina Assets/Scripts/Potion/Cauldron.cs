using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    [Space(5)]
    [Header("——— CAULDRON COMPONENTS.")]
    [SerializeField] private GameObject cauludronRecipeSlots;
    [SerializeField] private GameObject cauldronInventorySlots;
    [SerializeField] private ParticleSystem cauldronSmoke;

    [Space(5)]
    [Header("——— ITEM DROP COMPONENTS.")]
    [SerializeField] private GameObject droppedItem;

    private void Awake()
    {
        cauludronRecipeSlots.SetActive(false);
        cauldronInventorySlots.SetActive(false);
    }

    public void DropPotion()
    {
        Instantiate(droppedItem, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cauludronRecipeSlots.SetActive(true);
            cauldronInventorySlots.SetActive(true);

            cauldronSmoke.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cauludronRecipeSlots.SetActive(false);
            cauldronInventorySlots.SetActive(false);

            cauldronSmoke.Stop();
        }
    }
}
