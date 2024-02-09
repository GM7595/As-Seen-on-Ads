using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float slowdownFactor = 0.5f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 100f;
    public float maxTurnAngle = 30f;
    public float snapBackSpeed = 10f;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform bulletSpawner;
    public Transform bulletDump;
    public bool canFireOutOfRange;
    public float fireRate = 10f;
    Coroutine shootCoroutine;
    bool canShoot;
    bool couldShoot; //to scan for changes in canShoot

    [Header("Health Settings")]
    public int startingHP = 100;
    public static int playerHP = 100;

    Rigidbody rb;
    TextMeshProUGUI hpTextComp;


    void Start()
    {
        playerHP = startingHP;
        canShoot = true;
        rb = GetComponent<Rigidbody>();
        hpTextComp = GetComponentInChildren<TextMeshProUGUI>();

        //Hide the mouse and lock it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void FixedUpdate()
    {
        fireRate = playerHP;
        ShowHP();
        Movement();
        MovementBasedRotation();

        //Shoot or No Shoot
        if (canShoot != couldShoot)
        {
            if (canShoot)
            {
                shootCoroutine = StartCoroutine(Shoot());
            }
            if (!canShoot)
            {
                StopCoroutine(shootCoroutine);
            }
            couldShoot = canShoot;
        }

        //Escape the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorVisibility();
        }

        //Raycast
        Ray ray = new Ray(transform.position, transform.forward);
        float margin = 0.1f;
        if (Physics.Raycast(ray, out RaycastHit col, transform.localScale.z + margin))
        {
            PlayerCollision(col);
        }

        //Death
        if (playerHP <= 0)
        {
            SceneManager.LoadScene("Game Over");
        }
    }

    void ToggleCursorVisibility()
    {
        // Toggle cursor visibility and lock state
        Cursor.visible = !Cursor.visible;

        if (Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void ShowHP()   
    {
        //Show HP
        hpTextComp.text = "HP: " + playerHP.ToString();
    }

    void Movement()
    {
        // Movement (Clamped)
        float slowdown = 1f - Mathf.Abs(Input.GetAxis("Vertical")) * slowdownFactor;
        float xMovement = Input.GetAxis("Horizontal") * moveSpeed * slowdown; 
        float maxDisplacement = 1 * 5 - transform.localScale.x; //Clamp
        float newXPosition = Mathf.Clamp(transform.position.x + xMovement * Time.deltaTime * moveSpeed, -maxDisplacement, maxDisplacement); //Assign next step
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z); //Apply next step
    }

    void MovementBasedRotation()
    {
        // Rotation (Clamped)
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        if (Mathf.Abs(rotation) > 0.1f)
        {
            transform.Rotate(Vector3.up * rotation);

            float yRotation = transform.rotation.eulerAngles.y;

            //Clamp the angle
            if (yRotation <= maxTurnAngle + 1 || yRotation >= 360 - maxTurnAngle)
            {
                // Keep rotating if within turn angle limits
            }
            else
            {
                // Clamp rotation if exceeding turn angle limits
                if (yRotation < 180)
                    transform.rotation = Quaternion.Euler(0, maxTurnAngle, 0);
                else
                    transform.rotation = Quaternion.Euler(0, 360 - maxTurnAngle, 0);
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, snapBackSpeed * 0.1f);
        }
    }

    void PlayerCollision(RaycastHit col)
    {

        GateScript gateScript = col.transform.GetComponentInParent<GateScript>();
        if (gateScript != null)
        {
            playerHP = Mathf.RoundToInt(gateScript.DoMath());
            gateScript.Destroy();
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            if(canFireOutOfRange)
                Instantiate(bulletPrefab, bulletSpawner.position, transform.rotation, bulletDump);
            else
                Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity, bulletDump);
            yield return new WaitForSecondsRealtime(1/fireRate);
        }
    }

    void TakeDamage(int value)
    {
        playerHP -= value;
    }
}
