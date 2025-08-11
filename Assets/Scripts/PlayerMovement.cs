using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    //Sound variables
    public AudioClip crashSound;
    public AudioClip jumpSound;

    //Player variables
    [SerializeField] private float jumpForce = 10f; // Force applied when jumping
    [SerializeField] private Transform groundCheck; // Position to check if the player is grounded
    [SerializeField] private LayerMask groundLayer;  // Layer to determine what is considered ground
    [SerializeField] private Animator animator;      // Animator component for animations
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Color carColor;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool falling = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        // if (animator == null)
        // {
        //     Debug.LogError("Animator component is not assigned!");
        // }
    }

    private void Update()
    {
        if(GameManager.Instance.gameOver || !GameManager.Instance.gameStarted){
            if(spriteRenderer.color != Color.white){
                spriteRenderer.color = Color.white;
            }
            return;
        }
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if(falling && isGrounded){
            spriteRenderer.color = Color.white;
            falling = false;
        }

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            AudioManager.Instance.PlaySound(AudioManager.Instance.soundEffects,jumpSound);

            rb.linearVelocity = new Vector2(0, jumpForce); // Apply jump force
            // animator.SetTrigger("Jump"); // Trigger the jump animation
        }
        else if(Input.GetKeyDown(KeyCode.LeftControl) && !isGrounded && !falling){
            float initialeffectsPitch = AudioManager.Instance.soundEffects.pitch;
            AudioManager.Instance.soundEffects.pitch = Random.Range(AudioManager.Instance.soundEffects.pitch-0.6f, AudioManager.Instance.soundEffects.pitch - 0.3f);
            AudioManager.Instance.PlaySound(AudioManager.Instance.soundEffects, jumpSound);
            AudioManager.Instance.soundEffects.pitch = initialeffectsPitch;
            falling = true;
            spriteRenderer.color = carColor;
            rb.linearVelocity = new Vector2(0, -jumpForce/1.25f); // Apply fall force
        }

        // Set running animation
        // animator.SetBool("IsRunning", true);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Obstacle" && !GameManager.Instance.gameOver || other.gameObject.tag == "Capturable" && !GameManager.Instance.gameOver){
            AudioManager.Instance.PlaySound(AudioManager.Instance.soundEffects, crashSound);
            GameManager.Instance.obstacleHit = other.gameObject;
            GameManager.Instance.setGameOver();
        }
    }

}

