using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Projectile : MonoBehaviour
{
    //Capture Sound
    public AudioClip captureSound;
    private Rigidbody2D rb;
    private GameObject childSprite;
    void Start()
    {
        UIManager.Instance.webCounterText.text = "x"+GameManager.Instance.webCounter;
        childSprite = transform.GetChild(0).gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RotateProjectile();
        if(transform.position.x > 23 || transform.position.x < -23 || transform.position.y < -14){
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        //If "beast" handle it
        if(other.gameObject.tag == "Capturable"){
            //Capture logic here
            AudioManager.Instance.PlaySound(AudioManager.Instance.soundEffects, captureSound);
            BestiaryManager.Instance.AddCapturableCount(other.gameObject.GetComponent<MovingCapturable>().CapturableName);
            Sprite capturableSprite = other.gameObject.GetComponent<SpriteRenderer>().sprite;
            UIManager.Instance.last_Capture.color = Color.white;
            UIManager.Instance.last_Capture.sprite = capturableSprite;
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }

    void RotateProjectile()
    {
        // Get the velocity direction
        Vector2 direction = rb.linearVelocity;

        if (direction != Vector2.zero)
        {
            // Calculate the angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Adjust the angle because the sprite is facing diagonally (45 degrees)
            angle -= 45f;

            // Apply the rotation to the GameObject
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
