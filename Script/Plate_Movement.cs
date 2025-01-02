using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plate_Movement : MonoBehaviour
{
    [SerializeField] float speed;
    Vector3 movementDirection;
    bool alive = true;
    int score = 0;
    [SerializeField] AudioClip[] characterClips;
    private AudioSource audiosource;

    private List<GameObject> stack = new List<GameObject>(); // Stack for food objects
    private DynamicRoad dynamicRoadScript; // Reference to DynamicRoad script

    void Start()
    {
        audiosource = GetComponent<AudioSource>();

        // Find the DynamicRoad script in the scene
        dynamicRoadScript = FindObjectOfType<DynamicRoad>();
    }

    void Update()
    {
        if (alive)
        {
            movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 1);
            transform.Translate(movementDirection * Time.deltaTime * speed);

            if (transform.position.x > 0.44f)
            {
                transform.position = new Vector3(0.44f, transform.position.y, transform.position.z);
            }
            if (transform.position.x < -0.44f)
            {
                transform.position = new Vector3(-0.44f, transform.position.y, transform.position.z);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("obstacle"))
        {
            alive = false;
            Debug.Log("dead");
            audiosource.clip = characterClips[1];
            audiosource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("food"))
        {
            score++;
            Debug.Log("score: " + score);
            audiosource.clip = characterClips[0];
            audiosource.Play();

            // Add food object to the stack
            stack.Add(other.gameObject);

            // Notify DynamicRoad to protect this object
            if (dynamicRoadScript != null)
            {
                dynamicRoadScript.AddProtectedObject(other.gameObject);
            }

            // Position the new food on top of the stack
            Vector3 newPosition = transform.position + new Vector3(0, 0.1f * stack.Count, 0);
            other.gameObject.transform.position = newPosition;
            other.gameObject.transform.SetParent(transform); // Make the food a child of the plate
        }
        else if (other.transform.CompareTag("enemy"))
        {
            score--;
            Debug.Log("score: " + score);
            audiosource.clip = characterClips[1];
            audiosource.Play();

            // Remove the top food object from the stack if available
            if (stack.Count > 0)
            {
                GameObject topFood = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);

                // Notify DynamicRoad to unprotect this object
                if (dynamicRoadScript != null)
                {
                    dynamicRoadScript.RemoveProtectedObject(topFood);
                }

                Destroy(topFood);

                // Reposition the remaining objects in the stack
                for (int i = 0; i < stack.Count; i++)
                {
                    if (stack[i] != null) // Ensure the object is not destroyed
                    {
                        Vector3 newPosition = transform.position + new Vector3(0, 0.05f * (i + 1), 0);
                        stack[i].transform.position = newPosition;
                    }
                }
            }

            if (score < 0)
            {
                alive = false;
                Debug.Log("dead");
                audiosource.clip = characterClips[1];
                audiosource.Play();
            }
        }
    }
}
