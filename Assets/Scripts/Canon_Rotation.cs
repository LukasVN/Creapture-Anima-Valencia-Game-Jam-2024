using UnityEngine;

public class Canon_Rotation : MonoBehaviour
{
    // Assign the target GameObject in the Unity Inspector
    public Transform target;

    void Update()
    {
        if (target.gameObject.activeSelf)
        {
            // Calculate direction vector from this object to the target
            Vector2 direction = target.position - transform.position;

            // Calculate the angle between the current position and the target position
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply the rotation to the Z axis of the sprite
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}
