using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIMovementController : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Vector2 direction = Vector2.down;
    private Vector2 safeDirection = Vector2.zero; // Direction for avoiding bombs
    public float speed = 5f;
    private float idleTimer = 0f;
    public float idleTimeThreshold = 1f;
    public float avoidBombTimeThreshold = 1f;
    public bool GapBom = false;

    [Header("Sprites")]
    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer spriteRendererDeath;
    private AnimatedSpriteRenderer activeSpriteRenderer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeSpriteRenderer = spriteRendererDown;

        // Start the AI movement loop
        if(GapBom == false) InvokeRepeating(nameof(MoveRandomDirection), 0f, 2f);
    }

    private void MoveRandomDirection()
    {
        // Set a random direction every 2 seconds
        SetRandomDirection();
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = (safeDirection != Vector2.zero) ? safeDirection * speed * Time.fixedDeltaTime : direction * speed * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);

        // Check for hazards (bombs and explosions) and change direction if found
        CheckForHazards();

        // Check for idle state
        CheckForIdleState();
    }

    private void SetDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;

        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        activeSpriteRenderer = spriteRenderer;
        activeSpriteRenderer.idle = direction == Vector2.zero;
    }

    private void SetRandomDirection()
    {
        float randomValue = Random.value;

        if (randomValue < 0.25f)
            SetDirection(Vector2.up, spriteRendererUp);
        else if (randomValue < 0.5f)
            SetDirection(Vector2.down, spriteRendererDown);
        else if (randomValue < 0.75f)
            SetDirection(Vector2.left, spriteRendererLeft);
        else
            SetDirection(Vector2.right, spriteRendererRight);
    }

    private void CheckForHazards()
    {
        // Check for bombs in the vicinity
        Collider2D bombCollider = Physics2D.OverlapCircle(transform.position, 1.5f, LayerMask.GetMask("Bomb"));

        // Check for explosions in the vicinity
        Collider2D explosionCollider = Physics2D.OverlapCircle(transform.position, 1.5f, LayerMask.GetMask("Explosion"));

        if (bombCollider != null || explosionCollider != null)
        {
            // Change direction to move away from the hazards
            Vector2 awayFromHazards = (transform.position - (bombCollider != null ? bombCollider.transform.position : explosionCollider.transform.position)).normalized;
            SetSafeDirection(awayFromHazards);
            GapBom = true;
        }
        else
        {
            // If there are no hazards, reset the safe direction
            SetSafeDirection(Vector2.zero);
            GapBom = false;
        }
    }

    private void SetSafeDirection(Vector2 newSafeDirection)
    {
        safeDirection = newSafeDirection;

        // If a safe direction is set, start the idle timer to avoid bombs for a limited time
        if (safeDirection != Vector2.zero)
        {
            idleTimer = 0f;
        }
    }

    private void CheckForIdleState()
    {
        if (Mathf.Approximately(rigidbody.velocity.x, 0f) && Mathf.Approximately(rigidbody.velocity.y, 0f))
        {
            idleTimer += Time.fixedDeltaTime;

            if (idleTimer >= idleTimeThreshold)
            {
                SetDirection(Vector2.zero, activeSpriteRenderer); // Set to idle state
            }
        }
        else
        {
            idleTimer = 0f; // Reset the timer if there is movement
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            DeathSequence();
        }
    }

    private void DeathSequence()
    {
        enabled = false;

        spriteRendererUp.enabled = false;
        spriteRendererDown.enabled = false;
        spriteRendererLeft.enabled = false;
        spriteRendererRight.enabled = false;
        spriteRendererDeath.enabled = true;

        Debug.Log("Die");

        Invoke(nameof(OnDeathSequenceEnded), 1.25f);
    }

    private void OnDeathSequenceEnded()
    {
        gameObject.SetActive(false);
        // Add any additional logic here after the death sequence ends
    }
}
