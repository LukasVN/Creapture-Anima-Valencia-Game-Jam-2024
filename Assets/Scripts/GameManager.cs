using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject pointer;
    public GameObject chargebar;

    //Player variables
    private GameObject player;
    private Vector3 playerInitialPosition;
    public int webCounter;
    public GameObject webPickup;

    //Distance based variables
    private float currentDistance;
    private float currentDistance_float;
    private float distanceMultiplier = 8.5f;
    private float initial_DistanceMultiplier;
    public float scrollSpeed; // Speed at which the background scrolls
    private float initial_ScrollSpeed; // Speed at which the background scrolls
    [SerializeField] private TextMeshProUGUI currentDistanceTextValue;

    //Game Over varaibles
    public bool gameStarted = false;
    public bool gameOver = false;

    //Obstacle variables
    public bool canSpawnObstacle = true;
    public GameObject obstacleHit;
    [SerializeField] private GameObject[] ground_Obstacles;
    [SerializeField] private GameObject[] air_Obstacles;
    [SerializeField] private Transform ground_spawnpoint;
    [SerializeField] private Transform air_spawnpoint;

     // Capturable variables
    public GameObject[] defaultGroundCapturables;
    public GameObject[] defaultAirCapturables;
    // [SerializeField] private GameObject fantasmarillo;
    // [SerializeField] private GameObject Calpcol;
    // [SerializeField] private GameObject Kinzu;
    // [SerializeField] private GameObject komiche;
    private List<GameObject> currentGroundCapturablesPool = new List<GameObject>(); 
    private List<GameObject> currentAirCapturablesPool = new List<GameObject>(); 

    // Web Pickup spawn settings
    [SerializeField] private float minWebPickupSpawnInterval = 10f;
    [SerializeField] private float maxWebPickupSpawnInterval = 20f;
    private float timeToNextWebPickupSpawn;

    // Obstacle spawn settings
    [SerializeField] private float minObstacleSpawnInterval = 1f; // Min time between spawns
    [SerializeField] private float maxObstacleSpawnInterval = 3f; // Max time between spawns
    private float timeToNextObstacleSpawn;

    // Capturable spawn settings
    [SerializeField] private float minCapturableSpawnInterval = 5f; // Min time between capturable spawns
    [SerializeField] private float maxCapturableSpawnInterval = 15f; // Max time between capturable spawns
    private float timeToNextCapturableSpawn;

    // Scroll speed increase settings
    [SerializeField] private float distanceThreshold = 50f; // Increase speed every x meters
    [SerializeField] private float speedIncreaseAmount = 0.5f; // Amount to increase speed by


    private void Awake() {
        Instance = this;
    }

    void Start(){
        currentAirCapturablesPool = defaultAirCapturables.ToList<GameObject>();
        currentGroundCapturablesPool = defaultGroundCapturables.ToList<GameObject>();
        webCounter = 3;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 61;
        initial_ScrollSpeed = scrollSpeed;
        initial_DistanceMultiplier = distanceMultiplier;
        player = GameObject.FindWithTag("Player");
        playerInitialPosition = player.transform.position;
    }

    void Update()
    {
        if(gameOver && !gameStarted && !UIManager.Instance.bestiary_menu.activeSelf && !UIManager.Instance.settings_Menu.activeSelf){
            if(Input.GetKeyDown(KeyCode.R)){
                initializeGame();
            }
            return;
        }
        else if(!gameStarted){
            return;
        }

        if (canSpawnObstacle)
        {
            timeToNextObstacleSpawn -= Time.deltaTime;
            if (timeToNextObstacleSpawn <= 0)
            {
                SpawnObstacle();
                timeToNextObstacleSpawn = Random.Range(minObstacleSpawnInterval, maxObstacleSpawnInterval);
            }
        }

        // Web Pickup spawn logic
        timeToNextWebPickupSpawn -= Time.deltaTime;
        if (timeToNextWebPickupSpawn <= 0)
        {
            SpawnWebPickup();
            timeToNextWebPickupSpawn = Random.Range(minWebPickupSpawnInterval, maxWebPickupSpawnInterval);
        }

        timeToNextCapturableSpawn -= Time.deltaTime;
        if (timeToNextCapturableSpawn <= 0)
        {
            SpawnCapturable();
            timeToNextCapturableSpawn = Random.Range(minCapturableSpawnInterval, maxCapturableSpawnInterval);
        }

        // Update the current distance using the distance multiplier
        currentDistance_float += Time.deltaTime * distanceMultiplier;
        currentDistance = (int)currentDistance_float;

        // Increase scroll speed every x meters
        if (currentDistance >= distanceThreshold)
        {
            scrollSpeed += speedIncreaseAmount;
            distanceThreshold += 50f; // Reset the threshold for the next increase
        }

        // Format the distance as a 5-digit string and update the text
        currentDistanceTextValue.text = currentDistance.ToString("00000") +"m";

    }

    private void SpawnObstacle()
    {
        // Decide whether to spawn ground or air obstacle
        bool spawnGroundObstacle = Random.Range(0, 2) == 0;

        if (spawnGroundObstacle)
        {
            GameObject obstacle = ground_Obstacles[Random.Range(0, ground_Obstacles.Length)];
            Instantiate(obstacle, ground_spawnpoint.position, Quaternion.identity);
        }
        else
        {
            GameObject obstacle = air_Obstacles[Random.Range(0, air_Obstacles.Length)];
            Instantiate(obstacle, new Vector2(air_spawnpoint.position.x,Random.Range(air_spawnpoint.position.y+5.5f,air_spawnpoint.position.y+10)), Quaternion.identity);
        }
    }

    private void SpawnWebPickup()
    {
        // Randomly choose a position between the ground and air
        float randomY = Random.Range(ground_spawnpoint.position.y, air_spawnpoint.position.y);

        // Ensure no collision with obstacles or capturables
        if (IsPositionSafe(new Vector2(ground_spawnpoint.position.x, randomY)))
        {
            Instantiate(webPickup, new Vector2(ground_spawnpoint.position.x, randomY), Quaternion.identity);
        }
    }

    private void SpawnCapturable()
    {
        // Randomly decide whether to spawn on ground or air
        bool spawnOnGround = Random.Range(0, 2) == 0;
        float randomY = Random.Range(ground_spawnpoint.position.y, air_spawnpoint.position.y);
        if (IsPositionSafe(new Vector2(ground_spawnpoint.position.x, randomY))){

            if (spawnOnGround)
            {
                GameObject capturable = currentGroundCapturablesPool[Random.Range(0, currentGroundCapturablesPool.Count)];
                Instantiate(capturable, ground_spawnpoint.position, Quaternion.identity);
            }
            else
            {
                GameObject capturable = currentAirCapturablesPool[Random.Range(0, currentAirCapturablesPool.Count)];
                Instantiate(capturable, new Vector2(air_spawnpoint.position.x,Random.Range(air_spawnpoint.position.y,air_spawnpoint.position.y+4)), Quaternion.identity);
            }
        }

    }

    public void setGameOver(){
        UIManager.Instance.youLostUI((int)currentDistance);
        pointer.SetActive(false);
        chargebar.SetActive(false);
        gameOver = true;
        gameStarted = false;
    }

    public void initializeGame(){
        AudioManager.Instance.SetInGameMusic();
        pointer.SetActive(true);
        chargebar.SetActive(true);
        webCounter = 3;
        UIManager.Instance.gameStartUI();
        MovingBackground.Instance.ResetBackground();
        HandleObstacle();
        canSpawnObstacle = true;
        player.transform.position = playerInitialPosition;
        gameOver = false;
        gameStarted = true;
        currentDistanceTextValue.text = "00000";
        currentDistance = 0;
        currentDistance_float = 0;
        timeToNextObstacleSpawn = 0;
        timeToNextCapturableSpawn = 5f;
        timeToNextWebPickupSpawn = 10f;
        scrollSpeed = initial_ScrollSpeed;
        distanceMultiplier = initial_DistanceMultiplier;
        currentAirCapturablesPool = defaultAirCapturables.ToList();
        currentGroundCapturablesPool = defaultGroundCapturables.ToList();
    }

    private void HandleObstacle(){
        if(obstacleHit == null){
            return;
        }
        else{
            Destroy(obstacleHit);
        }
    }

    private bool IsPositionSafe(Vector2 position)
    {
        // Use a small radius to check for overlaps with obstacles and capturables
        float checkRadius = 0.5f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, checkRadius);

        // If any colliders are found in the given radius, the position is not safe
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Obstacle") || collider.CompareTag("Capturable"))
            {
                return false;
            }
        }
        return true;
    }

}
