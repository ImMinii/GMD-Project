using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public List<int> collectedItemIds = new List<int>(); // or your item data class

    public bool IsItemCollected(int itemId)
    {
        return collectedItemIds.Contains(itemId);
    }

    public static InventoryController Instance; // singleton, if useful

    private void Awake()
    {
        Instance = this;
    }
}