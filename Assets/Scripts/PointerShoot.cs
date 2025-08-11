using UnityEngine;
using UnityEngine.UI;

public class PointerShoot : MonoBehaviour
{
    //Shoot Sound
    public AudioClip shootSound;
    // Set the minimum and maximum angle for clamping (customizable in the Inspector)
    private Transform parentAxis;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform instantiateArea;
    [SerializeField] private float minForce; // Minimum force for the projectile
    [SerializeField] private float maxForce; // Maximum force for the projectile
    [SerializeField] private float chargeTime; // Time required to reach maximum force
    [SerializeField] private Slider shootBarSlider;
    [SerializeField] private RawImage chargeBar; // The UI Image for the bar
    [SerializeField] private float chargeLevel = 0f; // Charge level between 0 and 1
    private bool onCooldown = false;

    // Colors for the bar
    private Color startColor = Color.green; // Empty bar color
    private Color endColor = Color.red; // Full bar color

    private bool isCharging = false;
    private float chargeStartTime;

    private void Start() {
        parentAxis = transform.parent;

    }

    void Update()
    {
        if(GameManager.Instance.gameOver || !GameManager.Instance.gameStarted){
            return;
        }


        // Start charging the shot when the mouse button is pressed
        if (Input.GetMouseButtonDown(0) && !onCooldown && GameManager.Instance.webCounter > 0){
            GameManager.Instance.webCounter--;
            onCooldown = true;
            Invoke("ResetCooldown",0.5f);
            isCharging = true;
            chargeStartTime = Time.time; // Record the time when charging starts
        }

        // Update the charge bar slider and calculate force while charging
        if (Input.GetMouseButton(0) && isCharging){
            // Calculate the charge duration
            float chargeDuration = Time.time - chargeStartTime;

            // Calculate the force based on how long the button has been held
            float force = Mathf.Lerp(minForce, maxForce, chargeDuration / chargeTime);
            
            // Update the slider value to reflect the current charge level
            if (shootBarSlider != null){
                shootBarSlider.value = Mathf.Clamp01(chargeDuration / chargeTime); // Slider value should be between 0 and 1
            }

            // Update the charge bar (RawImage) color and charge level
            chargeLevel = Mathf.Clamp01(chargeDuration / chargeTime); // Normalized value between 0 and 1
            if (chargeBar != null){
                chargeBar.color = Color.Lerp(startColor, endColor, chargeLevel); // Lerp from red to green
            }
        }

        // Release the shot and instantiate the projectile with the calculated force
        if (Input.GetMouseButtonUp(0) && isCharging){
            isCharging = false;

            // Calculate the charge duration
            float chargeDuration = Time.time - chargeStartTime;

            // Calculate the force based on how long the button was held
            float force = Mathf.Lerp(minForce, maxForce, chargeDuration / chargeTime);

            // Clamp the force to make sure it doesn't exceed the maximum force
            force = Mathf.Clamp(force, minForce, maxForce);
            
            float initialeffectsPitch = AudioManager.Instance.soundEffects.pitch;
            AudioManager.Instance.soundEffects.pitch = Random.Range(AudioManager.Instance.soundEffects.pitch-0.3f, AudioManager.Instance.soundEffects.pitch + 0.3f);
            AudioManager.Instance.PlaySound(AudioManager.Instance.soundEffects, shootSound);
            AudioManager.Instance.soundEffects.pitch = initialeffectsPitch;
            // Instantiate the projectile at the instantiate area
            GameObject thrownProjectile = Instantiate(projectile, instantiateArea.position, Quaternion.identity);
            
            // Calculate the throw direction based on the instantiate area
            Vector2 throwDirection = instantiateArea.right; // Direction the instantiate area is facing

            // Apply the calculated force to the projectile
            thrownProjectile.GetComponent<Rigidbody2D>().AddForce(throwDirection * force);

            // Reset the slider value and charge level
            if (shootBarSlider != null){
                shootBarSlider.value = 0f;
            }

            if (chargeBar != null){
                chargeBar.color = startColor; // Reset color to red
            }
        }

        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the object to the mouse
        Vector3 direction = mousePosition - parentAxis.position;

        // Calculate the angle in degrees (in the Z axis)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the object (rotation in Z-axis only)
        parentAxis.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    private void ResetCooldown(){
        onCooldown = false;
    }
}
