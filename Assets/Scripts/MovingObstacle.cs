using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    private void Start() {
        GameManager.Instance.canSpawnObstacle = false;
    }
    private void Update()
    {
        // If the game is over, stop moving the obstacle
        if (GameManager.Instance.gameOver || !GameManager.Instance.gameStarted)
        {
            Destroy(gameObject);
            return;
        }

        // Move the obstacle left based on the scroll speed
        float movement = GameManager.Instance.scrollSpeed * Time.deltaTime; // / 1.25f
        transform.position += Vector3.left * movement;

        // Optionally, you can destroy the obstacle if it moves off the screen (left side)
        // You can adjust this if you don't want the obstacle destroyed
        if (transform.position.x < Camera.main.ScreenToWorldPoint(Vector3.zero).x - 5)
        {
            Destroy(gameObject);
        }
        else if(transform.position.x < Camera.main.ScreenToWorldPoint(Vector3.zero).x + 5){
            GameManager.Instance.canSpawnObstacle = true;
        }
    }
}
