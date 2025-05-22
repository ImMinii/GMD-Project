using UnityEngine;

public class PlayerPowerupHandler : MonoBehaviour
{
    private Powerup activePowerup;
    
    //-------------------------------------------------Sizing---------------------------------------------------------//
    [SerializeField] private Vector3 sizePowerupScale = new Vector3(1.5f, 1.5f, 1f);
    private Vector3 originalScale;
    private bool isSizeActive = false;

    //----------------------------------------------------------------------------------------------------------------// 
    
    // TESTING POWERUPS //
    [SerializeField] private PowerupType debugPowerupType;
    [SerializeField] private string debugDisplayName = "Test Powerup";

    private void Start()
    {
        //Sizing
        originalScale = transform.localScale;
        
        //Debug code start 
        activePowerup = new Powerup
        {
            type = debugPowerupType,
            displayName = debugDisplayName
        };

        Debug.Log("DEBUG: Assigned test powerup: " + debugPowerupType);
        //Debug code end
    }
    

    // Powerup state variables
    private bool canDoubleJump = false;

    public void UsePowerup()
    {
        if (activePowerup != null)
        {
            ApplyPowerup(activePowerup);
            activePowerup = null;
        }
    }





    private void ApplyPowerup(Powerup powerup)
    {
        // Reset all powerup-related states first
        ResetPowerupStates();

        switch (powerup.type)
        {
            case PowerupType.DoubleJump:
                canDoubleJump = true;
                Debug.Log("Double Jump Activated!");
                break;
            case PowerupType.Size:
                isSizeActive = !isSizeActive;
                if (isSizeActive)
                {
                    transform.localScale = sizePowerupScale;
                    Debug.Log("Size Increased!");
                }
                else
                {
                    transform.localScale = originalScale;
                    Debug.Log("Size Reverted!");
                }
                break;
            case PowerupType.Dash:
                Debug.Log("Dash activated – not yet implemented.");
                break;
            case PowerupType.Featherfall:
                Debug.Log("Featherfall activated – not yet implemented.");
                break;
            case PowerupType.Push:
                Debug.Log("Push activated – not yet implemented.");
                break;
            case PowerupType.TimeControl:
                Debug.Log("Time Control activated – not yet implemented.");
                break;
            default:
                Debug.LogWarning("Unknown powerup type.");
                break;
        }
    }

    private void ResetPowerupStates()
    {
        canDoubleJump = false;
        transform.localScale = originalScale;

        // TODO: Reset other powerup effects when implemented
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PowerupPickup pickup = other.GetComponent<PowerupPickup>();
        if (pickup != null)
        {
            activePowerup = pickup.powerup;
            Debug.Log("Picked up powerup: " + activePowerup.displayName);
            Destroy(other.gameObject);
        }
    }

    // To be used by other scripts (like Jumping)
    public bool CanDoubleJump() => canDoubleJump;
}
