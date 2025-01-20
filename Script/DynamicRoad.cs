using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class DynamicRoad : MonoBehaviour
{
    [SerializeField] GameObject[] obstacles; // Obstacles to avoid
    [SerializeField] GameObject[] enemies;   // Enemies to fight or evade
    [SerializeField] GameObject[] food;      // Food to collect for score
    [SerializeField] Transform[] points;     // Spawn points for items

    GameObject[] createdObstacles;
    GameObject[] createdEnemies;
    GameObject[] createdFood;

    private List<GameObject> stack; // Reference to stack from Plate_Movement

    private void Start()
    {
        createdObstacles = new GameObject[points.Length];
        createdEnemies = new GameObject[points.Length];
        createdFood = new GameObject[points.Length];

        // Find the Plate_Movement script and get its stack
        Plate_Movement plateMovement = FindObjectOfType<Plate_Movement>();
        if (plateMovement != null)
        {
            stack = plateMovement.GetStack();
        }

        RefreshItems();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("TheCamera"))
        {
            // Move the parent block forward when triggered
            transform.parent.transform.position += new Vector3(0, 0, 15);

            RefreshItems();
            Debug.Log("!!!block moved!!!");
        }
    }

    private void RefreshItems()
    {
        ClearItems(createdObstacles);
        ClearItems(createdEnemies);
        ClearItems(createdFood);

        int arrayIndex = 0;

        foreach (Transform point in points)
        {
            float randomValue = Random.value;

            if (randomValue < 0.15f) // 15% chance for obstacle
            {
                createdObstacles[arrayIndex] = SpawnRandomItem(obstacles, point);
            }
            else if (randomValue < 0.30f) // 15% chance for enemy
            {
                createdEnemies[arrayIndex] = SpawnRandomItem(enemies, point);
            }
            else // 70% chance for food
            {
                createdFood[arrayIndex] = SpawnRandomItem(food, point);
            }
            arrayIndex++;
        }

    }

    private void ClearItems(GameObject[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                //Debug.Log($"Checking object: {items[i].name}");

                if (stack.Contains(items[i]))
                {
                    Debug.Log($"Skipping stack object: {items[i].name}");
                    continue;
                }

                /*if (items[i].transform.parent != null && items[i].transform.parent.CompareTag("Player"))
                {
                    Debug.Log($"Skipping object with Player as parent: {items[i].name}");
                    continue;
                }*/

                Debug.Log($"Destroying object: {items[i].name}");
                Destroy(items[i]);
                items[i] = null; // Clear the reference
            }
        }
    }
    private GameObject SpawnRandomItem(GameObject[] itemArray, Transform spawnPoint)
    {
        if (itemArray.Length == 0) return null;

        int randomIndex = Random.Range(0, itemArray.Length);
        return Instantiate(itemArray[randomIndex], spawnPoint.position, Quaternion.identity);
    }
}
