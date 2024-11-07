using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float health = 3f;
    private float friction = 0.5f;
    private float shotCooldown = 0.5f;
    private bool reloading = false;
    private float recoilStrength = 5f;


    // Start is called before the first frame update
    void Start()
    {
        return;
    }


    void Update()
    {   
        //FixedUpdate misses clicks
        if (Input.GetMouseButton(0)){
            Debug.Log("click");
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
            Debug.Log(reloading);
            StartCoroutine(Reload());
            Debug.Log("shoot");
            ApplyRecoil();
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
}
