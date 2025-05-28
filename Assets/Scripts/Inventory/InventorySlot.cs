using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int itemId;
    public Image icon;
    public GameObject lockedOverlay;
    public bool isPicked = false;

    public void Refresh()
    {
        bool collected = InventoryController.Instance.IsItemCollected(itemId);
        lockedOverlay.SetActive(!collected);
        GetComponent<Button>().interactable = collected && !isPicked;
    }

    public void LockSlot()
    {
        isPicked = true;
        GetComponent<Button>().interactable = false;
    }
}