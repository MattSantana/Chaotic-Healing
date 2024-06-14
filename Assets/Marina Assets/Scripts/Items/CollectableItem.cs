using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [Space(5)]
    [Header("——— INVENTORY ITEM COMPONENTS.")]
    public ItemType itemType;
    public Sprite icon;
    public string itemName;

    public enum ItemType
    {
        SlimeBall,
        Leaf,
        Bone,
        Batwing,
        Egg,
        Stone,
        Eye,
        Fish,
        Flower
    }
}