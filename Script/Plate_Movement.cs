using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate_Movement : MonoBehaviour
{
    [SerializeField] float speed;
    Vector3 movementDirection;
    bool alive = true;
    int score = 0;
    [SerializeField] AudioClip[] characterClips; // Array of audio clips
    private AudioSource audioSourceEffects; // Audio source for effects
    private AudioSource audioSourceBackground; // Audio source for background music

    private List<GameObject> stack = new List<GameObject>(); // Stack for food objects
    private HashSet<Collider> processedEnemies = new HashSet<Collider>();

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSourceEffects = audioSources[0]; // First AudioSource for effects
        audioSourceBackground = audioSources[1]; // Second AudioSource for background music

        PlayBackgroundMusic(); // Start background music
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

    private void PlayBackgroundMusic()
    {
        audioSourceBackground.clip = characterClips[3]; // Use the background music clip
        audioSourceBackground.loop = true; // Enable looping
        audioSourceBackground.Play(); // Play the music
    }

    private void StopBackgroundMusic()
    {
        if (audioSourceBackground.isPlaying)
        {
            audioSourceBackground.Stop(); // Stop the background music
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with: " + other.gameObject.name); // Log the name of the object triggered

        if (other.transform.CompareTag("food"))
        {
            score++;
            Debug.Log("score: " + score);
            audioSourceEffects.clip = characterClips[0]; // Play food pickup sound
            audioSourceEffects.loop = false; // Disable looping for food sound
            audioSourceEffects.Play();

            // تنظیم اندازه غذا
            Vector3 targetScale = new Vector3(1f, 1f, 1f);
            other.gameObject.transform.localScale = targetScale;

            // Reset position and rotation
            other.gameObject.transform.SetParent(null);
            other.gameObject.transform.localPosition = Vector3.zero;
            other.gameObject.transform.rotation = Quaternion.identity;

            // Add to stack
            stack.Add(other.gameObject);
            UpdateStackPositions(); // Recalculate positions for all stack items

            // Attach the item to the player
            other.gameObject.transform.SetParent(transform);
        }
        else if (other.transform.CompareTag("enemy"))
        {
            if (!processedEnemies.Contains(other))
            {
                processedEnemies.Add(other); // Mark this enemy as processed

                score--;
                Debug.Log("score: " + score);
                audioSourceEffects.clip = characterClips[1]; // Play enemy sound
                audioSourceEffects.loop = false; // Disable looping for enemy sound
                audioSourceEffects.Play();

                if (stack.Count > 0)
                {
                    GameObject topFood = stack[stack.Count - 1];
                    stack.RemoveAt(stack.Count - 1);

                    Destroy(topFood);

                    // Recalculate positions for all remaining items in the stack
                    UpdateStackPositions();
                }

                if (score < 0)
                {
                    alive = false;
                    StopBackgroundMusic(); // Stop the background music
                    Debug.Log("dead");
                    audioSourceEffects.clip = characterClips[2]; // Play death sound
                    audioSourceEffects.loop = false; // Disable looping for death sound
                    audioSourceEffects.Play();
                }
            }
        }
        else if (other.transform.CompareTag("obstacle"))
        {
            Debug.Log("Obstacle hit!");
            alive = false;
            StopBackgroundMusic(); // Stop the background music
            audioSourceEffects.clip = characterClips[2]; // Play death sound
            audioSourceEffects.loop = false; // Disable looping for death sound
            audioSourceEffects.Play();

            Debug.Log("Player is dead.");
        }
    }

    // Helper method to update the positions of all items in the stack
    private void UpdateStackPositions()
    {
        for (int i = 0; i < stack.Count; i++)
        {
            if (stack[i] != null)
            {
                Vector3 newPosition = transform.position + new Vector3(0, 0.05f * (i + 1), 0);
                stack[i].transform.position = newPosition;
            }
        }
    }

    public List<GameObject> GetStack()
    {
        return stack;
    }
}
