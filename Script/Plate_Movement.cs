using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plate_Movement : MonoBehaviour
{
    [SerializeField] float speed;
    Vector3 movementDirection;
    bool alive = true;
    uint score = 0;
    [SerializeField] AudioClip[] characterClips;
    private AudioSource audiosource;

    void Start()
    {
        audiosource = GetComponent<AudioSource>(); 
        
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
            Debug.Log("dead");
            audiosource.clip = characterClips[1]; //changed the audioclip
            audiosource.Play(); //play the audio


        }     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("food"))
        {
            score++;
            Debug.Log("score" + score);
            audiosource.clip = characterClips[0]; 
            audiosource.Play(); 
        }

    }
}
