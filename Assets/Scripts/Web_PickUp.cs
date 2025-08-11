using System.Data.SqlTypes;
using UnityEngine;

public class Web_PickUp : MonoBehaviour
{
    private bool pickedUp = false;
    public AudioClip webPickupSound;
    
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameOver || !GameManager.Instance.gameStarted)
        {
            Destroy(gameObject);
            return;
        }
        float movement = GameManager.Instance.scrollSpeed * Time.deltaTime; // / 1.25f
        transform.position += Vector3.left * movement;

        // Optionally, you can destroy the obstacle if it moves off the screen (left side)
        // You can adjust this if you don't want the obstacle destroyed
        if (transform.position.x < Camera.main.ScreenToWorldPoint(Vector3.zero).x - 5)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!pickedUp && other.gameObject.tag == "Player"){
            pickedUp = true;
            GameManager.Instance.webCounter++;
            UIManager.Instance.webCounterText.text = "x"+GameManager.Instance.webCounter;
            AudioManager.Instance.PlaySound(AudioManager.Instance.soundEffects, webPickupSound);
            Destroy(gameObject);
        }
        
    }
}
