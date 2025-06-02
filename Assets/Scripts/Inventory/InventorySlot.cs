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
    
    public void UnlockSlot()
    {
        isPicked = false;
        GetComponent<Button>().interactable = true;
    }

    
    // Add this in InventorySlot.cs
    public void OnSlotClicked()
    {
        if (InventoryController.Instance.IsItemCollected(itemId) && !isPicked)
        {
            LockSlot();
            // You can also call back to InventorySelector if needed
        }
    }
    
    public void ActivatePowerupForPlayer(GameObject playerObject)
    {
        var handler = playerObject.GetComponent<PlayerPowerupHandler>();
        if (handler != null)
        {
            handler.SetActivePowerup(itemId);
        }
    }


}