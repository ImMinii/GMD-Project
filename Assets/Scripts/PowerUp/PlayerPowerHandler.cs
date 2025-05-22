using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerPowerupHandler : MonoBehaviour
{
    private Powerup activePowerup;

    //-------------------------------------------------Sizing---------------------------------------------------------//
    [SerializeField] private Vector3 sizePowerupScale = new Vector3(1.5f, 1.5f, 1f);
    private Vector3 originalScale;
    private bool isSizeActive = false;

    //-------------------------------------------------Dashing--------------------------------------------------------//
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashCooldown = 1f;
    private bool canDash = true;
    private Rigidbody2D rb;

    //----------------------------------------------DoubleJump--------------------------------------------------------//
    private bool canDoubleJump = false;

    //--------------------------------------------------Phasing--------------------------------------------------------//
    [SerializeField] private LayerMask phaseableLayers;
    private bool isPhasing = false;
    private Collider2D[] playerColliders;
    private List<Collider2D> ignoredColliders = new List<Collider2D>();

    // TESTING POWERUPS //
    [SerializeField] private PowerupType debugPowerupType;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerColliders = GetComponentsInChildren<Collider2D>();
    }

    private void Start()
    {
        originalScale = transform.localScale;

        // Assign debug/test powerup
        activePowerup = new Powerup
        {
            type = debugPowerupType,
        };

        Debug.Log("DEBUG: Assigned test powerup: " + debugPowerupType);
        ApplyPassivePowerupEffects();  //Debug
    }

    public void UsePowerup()
    {
        if (activePowerup != null)
        {
            ApplyPowerup(activePowerup);
        }
    }

    private IEnumerator PerformDash()
    {
        canDash = false;
        Vector2 dashDirection = GetComponent<PlayerMovement>().IsFacingRight() ? Vector2.right : Vector2.left;
        rb.AddForce(dashDirection * dashForce, ForceMode2D.Impulse);
        Debug.Log("Dashed!");

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void ApplyPowerup(Powerup powerup)
    {
        ResetPowerupStates();

        switch (powerup.type)
        {
            case PowerupType.DoubleJump:
                canDoubleJump = true;
                Debug.Log("Double Jump Activated!");
                break;

            case PowerupType.Size:
                isSizeActive = !isSizeActive;
                transform.localScale = isSizeActive ? sizePowerupScale : originalScale;
                Debug.Log(isSizeActive ? "Size Increased!" : "Size Reverted!");
                break;

            case PowerupType.Dash:
                if (canDash)
                {
                    StartCoroutine(PerformDash());
                }
                else
                {
                    Debug.Log("Dash on cooldown.");
                }
                break;

            case PowerupType.Phasing:
                EnablePhasing(true);
                Debug.Log("Phasing activated.");
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

        if (isPhasing)
        {
            EnablePhasing(false);
        }
    }

    private void EnablePhasing(bool enable)
    {
        isPhasing = enable;

        if (enable)
        {
            Collider2D[] allColliders = GameObject.FindObjectsOfType<Collider2D>();
            foreach (Collider2D col in allColliders)
            {
                if (((1 << col.gameObject.layer) & phaseableLayers.value) != 0)
                {
                    foreach (Collider2D playerCol in playerColliders)
                    {
                        Physics2D.IgnoreCollision(playerCol, col, true);
                    }
                    ignoredColliders.Add(col);
                }
            }
        }
        else
        {
            foreach (Collider2D phaseable in ignoredColliders)
            {
                foreach (Collider2D playerCol in playerColliders)
                {
                    if (phaseable != null && playerCol != null)
                        Physics2D.IgnoreCollision(playerCol, phaseable, false);
                }
            }
            ignoredColliders.Clear();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PowerupPickup pickup = other.GetComponent<PowerupPickup>();
        if (pickup != null)
        {
            activePowerup = pickup.powerup;
            Debug.Log("Picked up powerup: " + activePowerup.displayName);
            ApplyPassivePowerupEffects();
            Destroy(other.gameObject);
        }
    }

    private void ApplyPassivePowerupEffects()
    {
        ResetPowerupStates();

        switch (activePowerup.type)
        {
            case PowerupType.DoubleJump:
                canDoubleJump = true;
                Debug.Log("Double Jump passively enabled.");
                break;

            case PowerupType.Phasing:
                EnablePhasing(true);
                Debug.Log("Passive Phasing enabled.");
                break;
        }
    }

    public bool CanDoubleJump() => canDoubleJump;
    public bool IsPhasing() => isPhasing;
}
