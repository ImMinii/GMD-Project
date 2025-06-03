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

    //-------------------------------------------------Phasing--------------------------------------------------------//
    [SerializeField] private LayerMask phaseableLayers;
    private bool isPhasing = false;
    private Collider2D[] playerColliders;
    private List<Collider2D> ignoredColliders = new List<Collider2D>();
    
    //-------------------------------------------------Pushing--------------------------------------------------------//
    [SerializeField] private Transform wallCheck;              // Assign your wallCheck GameObject here
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    [SerializeField] private LayerMask pushableLayer;          // Create a PushableBox layer or use existing
    
    private PushableBox lastTouchedBox;
    private bool canPush = false;

    //---------------------------------------------TimeControl--------------------------------------------------------//
    private bool isTimeStopped = false;


    private AudioManager audioManager;
    // TESTING POWERUPS //
    //[SerializeField] private PowerupType debugPowerupType;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerColliders = GetComponentsInChildren<Collider2D>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        originalScale = transform.localScale;
        
/*
        // Assign debug/test powerup
        activePowerup = new Powerup
        {
            type = debugPowerupType,
        };

        Debug.Log("DEBUG: Assigned test powerup: " + debugPowerupType);
        ApplyPassivePowerupEffects();  //Debug
        */
        
    }
    
    private void FixedUpdate()
    {
        if (canPush)
        {
            HandlePushableBoxTouch();
        }
        else if (lastTouchedBox != null)
        {
            lastTouchedBox.FreezeXMovement();
            lastTouchedBox = null;
        }
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
                break;

            case PowerupType.TimeControl:
            ToggleTimeControl();
            break;

            default:
                Debug.LogWarning("Unknown powerup type.");
                break;
        }
    }

    private void ResetPowerupStates()
    {
        canDoubleJump = false;
        canPush = false;
        
        isSizeActive = false;
        transform.localScale = originalScale;
        
        if (isTimeStopped)
        {
            isTimeStopped = false;
            foreach (var platform in FindObjectsOfType<MovingPlatform>())
            {
                platform.isStopped = false;
            }
        }

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
        if (pickup != null && pickup.powerup != null)
        {
            int powerupId = (int)pickup.powerup.type; // use pickup.powerup
            if (!InventoryController.Instance.collectedItemIds.Contains(powerupId))
                InventoryController.Instance.collectedItemIds.Add(powerupId);

            var selector = FindObjectOfType<PlayerInventorySelector>();
            if (selector != null && InventoryToggle.isOpen)
            {
                selector.RefreshAllSlots();
            }
            
            Destroy(other.gameObject);
            audioManager.PlayPowerUpCollect(audioManager.powerUp);
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

            case PowerupType.Push:
                canPush = true;
                Debug.Log("Passive Push enabled.");
                break;
        }
    }

    
    private void HandlePushableBoxTouch()
    {
        Collider2D hit = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0f, pushableLayer);

        if (hit != null)
        {
            PushableBox box = hit.GetComponent<PushableBox>();
            if (box != null)
            {
                if (box != lastTouchedBox)
                {
                    lastTouchedBox?.FreezeXMovement(); // Freeze previous box if different
                    lastTouchedBox = box;
                }
                box.AllowXMovement();
            }
        }
        else if (lastTouchedBox != null)
        {
            lastTouchedBox.FreezeXMovement();
            lastTouchedBox = null;
        }
    }

    private void ToggleTimeControl()
{
    isTimeStopped = !isTimeStopped;
    foreach (var platform in FindObjectsOfType<MovingPlatform>())
    {
        platform.isStopped = isTimeStopped;
    }
    Debug.Log("Time Control " + (isTimeStopped ? "activated (platforms stopped)" : "deactivated (platforms resumed)"));
}
    
    public void SetActivePowerup(int itemId)
    {
        PowerupType type = (PowerupType)itemId;
        activePowerup = new Powerup { type = type, displayName = type.ToString() };
        Debug.Log("Active powerup set: " + type);
        ApplyPassivePowerupEffects();
    }




    public bool CanDoubleJump() => canDoubleJump;
    public bool IsPhasing() => isPhasing;
    
    public void ClearActivePowerup()
    {
        // Set activePowerup to null and reset all effects
        activePowerup = null;
        ResetPowerupStates();
        Debug.Log("Powerup deactivated.");
    }
}
