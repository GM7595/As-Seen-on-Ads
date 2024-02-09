using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageScript : MonoBehaviour
{
    public float stageSpeed = 1.0f;
    public float goalZ = 105;

    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.z < -goalZ)
        {
            SceneManager.LoadScene("Clear");
        }

        transform.Translate(-Vector3.forward * Time.deltaTime * stageSpeed);
        foreach (Transform child in transform)
        {
            foreach (Transform grandChild in child)
            {
                foreach(Transform gg in grandChild)
                if (gg.position.z < player.position.z - 5 && !gg.CompareTag("DoNotDestroy") && !grandChild.CompareTag("DoNotDestroy") && !child.CompareTag("DoNotDestroy"))
                {
                    Debug.Log("[auto]\n" + child.name + "/" + grandChild.name + "/" + gg.name + " is out of scope. Destroying.");
                    Destroy(grandChild.gameObject);
                }
            }
        }
    }
}
