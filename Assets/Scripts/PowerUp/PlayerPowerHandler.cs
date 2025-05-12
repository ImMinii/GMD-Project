using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerupHandler : MonoBehaviour
{
    public List<Powerup> collectedPowerups = new List<Powerup>();

    // Player state variables
    private float normalSpeed = 5f;
    private float boostedSpeed = 10f;
    private bool canDoubleJump = false;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        // Only activate one powerup at a time based on priority (1, 2, then 3)
        if (collectedPowerups.Count > 0 && Input.GetKey(KeyCode.Alpha1))
        {
            ApplyPowerup(collectedPowerups[0]);
        }
        else if (collectedPowerups.Count > 1 && Input.GetKey(KeyCode.Alpha2))
        {
            ApplyPowerup(collectedPowerups[1]);
        }
        else if (collectedPowerups.Count > 2 && Input.GetKey(KeyCode.Alpha3))
        {
            ApplyPowerup(collectedPowerups[2]);
        }
        else
        {
            // If no key is held, reset effects
            ResetAllEffects();
        }
    }
    
    private void ResetAllEffects()
    {
        playerMovement.moveSpeed = normalSpeed;
        canDoubleJump = false;
    }



    private void HandlePowerupInput(int index, KeyCode key)
    {
        if (index >= 0 && index < collectedPowerups.Count)
        {
            if (Input.GetKey(key))
            {
                ApplyPowerup(collectedPowerups[index]);
            }
            else
            {
                RemovePowerupEffect(collectedPowerups[index]);
            }
        }
    }

    private void ApplyPowerup(Powerup powerup)
    {
        switch (powerup.type)
        {
            case PowerupType.SpeedBoost:
                playerMovement.moveSpeed = boostedSpeed;
                break;
            case PowerupType.DoubleJump:
                canDoubleJump = true;
                break;
            default:
                Debug.LogWarning("Unknown powerup type.");
                break;
        }
    }

    private void RemovePowerupEffect(Powerup powerup)
    {
        switch (powerup.type)
        {
            case PowerupType.SpeedBoost:
                playerMovement.moveSpeed = normalSpeed;
                break;
            case PowerupType.DoubleJump:
                canDoubleJump = false;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PowerupPickup pickup = other.GetComponent<PowerupPickup>();
        if (pickup != null)
        {
            collectedPowerups.Add(pickup.powerup);
            Destroy(other.gameObject);
        }
    }
}
