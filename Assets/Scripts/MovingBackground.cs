using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    public static MovingBackground Instance;
    private BoxCollider2D backgroundCollider; // The BoxCollider2D component of the background
    private Vector2 startPosition;
    private float backgroundWidth;

    private float scrollPositionX = 0f; // Track the X position for smooth movement

    private void Awake() {
        Instance = this;
    }

    private void Start()
    {
        backgroundCollider = GetComponent<BoxCollider2D>();
        // Ensure the background collider is assigned
        if (backgroundCollider == null)
        {
            Debug.LogError("Background Collider is not assigned!");
            return;
        }

        startPosition = transform.position;
        backgroundWidth = backgroundCollider.size.x * transform.localScale.x;
    }

    private void Update()
    {
        if (GameManager.Instance.gameOver || !GameManager.Instance.gameStarted){
            return;
        }

        // Accumulate the scroll position over time
        scrollPositionX += GameManager.Instance.scrollSpeed * Time.deltaTime;

        // Use Mathf.Repeat to create the looping effect
        float newPositionX = Mathf.Repeat(scrollPositionX, backgroundWidth / 2);

        // Update the background's position
        transform.position = startPosition + Vector2.left * newPositionX;
    }

    public void ResetBackground(){
        scrollPositionX = 0;
        transform.position = startPosition;
    }
}

