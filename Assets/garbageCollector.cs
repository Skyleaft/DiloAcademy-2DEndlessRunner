using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class garbageCollector : MonoBehaviour
{
    public Transform CamPos;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = CamPos.position;
    }

    //trigger untuk menghapus energi boost dan bomb
    void OnTriggerEnter2D(Collider2D colider)
    {
        if (colider.gameObject.tag == "Enemy")
        {
            Destroy(colider.gameObject);
        }

    }
}
