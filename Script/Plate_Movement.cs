using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate_Movement : MonoBehaviour
{
    [SerializeField] float speed;
    Vector3 movementDirection;
    bool alive = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (alive) {
            movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 1);
            transform.Translate(movementDirection * Time.deltaTime * speed);
            if (transform.position.x > 0.44f)
            {
                transform.position = new Vector3(0.44f, transform.position.y, transform.position.z);
            }
            if (transform.position.x < -0.44f)
            {
                transform.position = new Vector3(-0.44f, transform.position.y, transform.position.z);
                //test
            }
        }
        
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("obstacle"))
        {
            alive = false;  
        }
    }
}
