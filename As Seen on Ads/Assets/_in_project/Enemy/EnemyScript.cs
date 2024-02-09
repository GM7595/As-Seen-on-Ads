using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class EnemyScript : MonoBehaviour
{
    [Header("Health Settings")]
    public GameObject hpText;
    public int enemyHP = 1;
    public bool showHP;

    [Header("Damage Settings")]
    public int enemyDamage = 1;

    [Header("Speed Settings")]
    public float playerScanDistance = 10;
    public float maxEnemySpeed = 5;

    public GameObject effect;
    TextMeshProUGUI hpTextComp;
    Transform player;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        hpTextComp = GetComponentInChildren<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        enemyDamage = enemyHP;

        //Show HP
        if(hpTextComp != null)
            hpTextComp.text = showHP ? enemyHP.ToString() : null;

        //Chase Player
        float distance = Mathf.Abs(transform.position.z - player.position.z);
        if (distance < playerScanDistance)
        {
            animator.SetTrigger("run");
            float percentage = (playerScanDistance - distance) / playerScanDistance;
            transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * maxEnemySpeed * percentage);
        }

        //Damage Player
        if (transform.position.z <= 1)
        {
            Debug.Log("[DAMAGE]\n" + enemyDamage + " from " + name);
            Instantiate(effect, transform.position, Quaternion.identity);
            player.SendMessage("TakeDamage", enemyDamage);
            Destroy(this.gameObject);
        }

        //Die
        if (enemyHP <= 0)
        {
            Debug.Log("[KILL]\nKilled: " + name);
            Destroy(gameObject);
        }
    }

    void TakeDamage(int value)
    {
        enemyHP -= value;
    }
}
