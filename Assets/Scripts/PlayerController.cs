using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private int health = 3;
    private float friction = 1f;
    private float shotCooldown = 0.5f;
    private float shotDamage = 1f;
    private bool reloading = false;
    private float recoilStrength = 5f;
    private Rigidbody2D rb;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private float bulletSpeed = 10f;
    private bool piercing = false;
    private bool shotgun = false;
    private AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip playerHitSound;
    public AudioClip powerUpSound;
    public string SelectedItem = "";
    public Rect boardBounds;



    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        audioSource = GetComponent<AudioSource>();
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
            audioSource.volume = shotDamage;
            audioSource.PlayOneShot(shootSound);
            StartCoroutine(Reload());
            ApplyRecoil();
            FireBullet(0);
            if (shotgun == true){
                FireBullet(-45);
                FireBullet(45);  
            }          
        }
    }

    void FireBullet(float angleOffset)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null){
            Vector2 direction = (firePoint.position - transform.position).normalized;
            Vector2 rotatedDirection = Quaternion.Euler(0, 0, angleOffset) * direction;
            rb.velocity = rotatedDirection * bulletSpeed;
            
        }
        bullet.GetComponent<BulletController>().SetPiercing(piercing);
        bullet.GetComponent<BulletController>().SetDamage(shotDamage);
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

    public void SetPiercing(bool value)
    {
        piercing = value;
    }

    public void GetSniper()
    {
        audioSource.PlayOneShot(powerUpSound);
        shotCooldown *= 2;
        shotDamage *= 2;
        recoilStrength = System.Math.Min(recoilStrength * 1.5f, 8);
        bulletSpeed *= 2;
    }

    public void GetShotgun()
    {
        audioSource.PlayOneShot(powerUpSound);
        shotgun = true;
        shotCooldown *= 1.5f;
        recoilStrength = System.Math.Min(recoilStrength * 1.5f, 8);
    }
    public void GetSMG()
    {
        audioSource.PlayOneShot(powerUpSound);
        shotCooldown *= 0.2f;
        shotDamage *= 0.2f;
        recoilStrength *= 0.2f;
    }

    public string GetItem()
    {
        return SelectedItem;
    }
    public void ResetItem()
    {
        SelectedItem = "";
    }

    public void FellOffBoard()
    {
        transform.position = Vector2.zero;
        PlayerHit();
    }

    public void PlayerHit(){
        health -= 1;
        audioSource.PlayOneShot(playerHitSound);
        Debug.Log(health);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Shop")
        {
            SelectedItem = collision.gameObject.name;
        }

    }
}
