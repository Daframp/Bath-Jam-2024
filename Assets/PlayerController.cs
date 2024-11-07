using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int health = 3;
    private float friction = 1f;
    private float shotCooldown = 0.5f;
    private bool reloading = false;
    private float recoilStrength = 5f;
    private Rigidbody2D rb;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed;
    private bool piercing = false;


    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        ChangeDrag();
    }


    void Update()
    {   
        //FixedUpdate misses clicks
        if (Input.GetMouseButton(0)){
            Shoot();
        }
    }

    void FixedUpdate()
    {
        //rotation
		Vector3 mousePos = Input.mousePosition;

		Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
		mousePos.x = mousePos.x - objectPos.x;
		mousePos.y = mousePos.y - objectPos.y;

		float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }


    void Shoot()
    {
        if (!reloading){
            StartCoroutine(Reload());
            ApplyRecoil();

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null){
                Vector2 direction = (firePoint.position - transform.position).normalized;
                rb.velocity = direction * bulletSpeed;
            }
            bullet.SetPiercing(piercing);
        }
    }

    void ApplyRecoil()
    {
         // Get the player's current rotation in degrees (Z axis)
        float playerRotationZ = transform.eulerAngles.z;

        // Convert the rotation to the opposite direction (180 degrees away) + 90 because its just wrong for some reason
        float oppositeDirectionZ = playerRotationZ + 180f +90f;

        // Convert this opposite direction to a unit vector (x, y)
        Vector2 oppositeDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * oppositeDirectionZ), Mathf.Sin(Mathf.Deg2Rad * oppositeDirectionZ));

        // Normalize it to ensure it's always a unit vector
        oppositeDirection.Normalize();

        // Apply the opposite momentum (force) to the player (assuming you have a Rigidbody2D component)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(oppositeDirection * recoilStrength, ForceMode2D.Impulse);
        }
    }
    IEnumerator Reload()
    {
        reloading = true;

        yield return new WaitForSeconds(shotCooldown);

        reloading = false;
    }

    public void SetFriction(float value)
    {
        friction = value;
        ChangeDrag();
    }

    public float GetFriction()
    {
        return friction;
    }

    public void SetHealth(int value)
    {
        health = value;
    }

    public float GetHealth()
    {
        return health;
    }

    private void ChangeDrag()
    {
        rb.drag = friction;
    }

    private void SetPiercing(bool value)
    {
        piercing = value
    }
}
