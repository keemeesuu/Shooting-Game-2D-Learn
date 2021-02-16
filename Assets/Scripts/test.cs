using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // transform.position = transform.position + Vector3.up * 10;
        // transform.position = transform.position + Vector3.up * 10;
        // transform.position = transform.position + new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(1, 0, 0) * Time.deltaTime;
    }
}
