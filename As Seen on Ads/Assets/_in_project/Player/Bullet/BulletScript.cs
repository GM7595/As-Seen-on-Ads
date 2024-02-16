using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float destroyTimer = 1.0f;
    public float speed = 10f;
    public int bulletDamage = 10;
    public GameObject defaultEffect;
    public GameObject enemyEffect;
    public GameObject itemEffect;
    public GameObject gateEffect;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Movement
        transform.position += Time.deltaTime * speed * transform.forward;

        //Collision
        Ray ray = new Ray(transform.position, transform.forward);
        float range = 0.5f;
        if (Physics.Raycast(ray, out RaycastHit col, range))
        {
            BulletRaycast(col);
        }
    }

    private void BulletRaycast(RaycastHit col)
    {
        //Debug.Log(col.transform.name);
        GateScript gateScript = col.transform.GetComponentInParent<GateScript>();



        //Enemy
        if (col.transform.tag == "Enemy")
        {
            Instantiate(enemyEffect, transform.position, Quaternion.identity);
            //Debug.Log("Damaged: " + col.transform.gameObject.name + " by " + bulletDamage);
            col.transform.gameObject.SendMessage("TakeDamage", bulletDamage);
        }
        else if (col.transform.tag == "Bullet")
        {
            Instantiate(itemEffect, transform.position, Quaternion.identity);
        }
        else if (gateScript != null)
        {
            Instantiate(gateEffect, transform.position, Quaternion.identity);
        }
        else Instantiate(defaultEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
