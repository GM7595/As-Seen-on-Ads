using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroyer : MonoBehaviour
{

    public float time = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("destroy", time);   
    }

    // Update is called once per frame
    void destroy()
    {
        Destroy(this.gameObject);
    }
}
