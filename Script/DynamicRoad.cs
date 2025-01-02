using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicRoad : MonoBehaviour
{
    [SerializeField] GameObject[] obstacles; // Obstacles to avoid
    [SerializeField] GameObject[] enemies;   // Enemies to fight or evade
    [SerializeField] GameObject[] food;      // Food to collect for score
    [SerializeField] Transform[] points;     // Spawn points for items

    private List<GameObject> protectedObjects = new List<GameObject>(); // List of objects to protect

    GameObject[] createdObstacles;
    GameObject[] createdEnemies;
    GameObject[] createdFood;

    private void Start()
    {
        createdObstacles = new GameObject[points.Length];
        createdEnemies = new GameObject[points.Length];
        createdFood = new GameObject[points.Length];

        RefreshItems();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("TheCamera"))
        {
            // Move the parent block forward when triggered
            transform.parent.transform.position += new Vector3(0, 0, 15);

            // Clear and refresh items
            /*ClearItems(createdObstacles);
            ClearItems(createdEnemies);
            ClearItems(createdFood);*/

            RefreshItems();
            Debug.Log("!!!block moved!!!");
        }
    }

    public void AddProtectedObject(GameObject obj)
    {
        if (!protectedObjects.Contains(obj))
        {
            protectedObjects.Add(obj);
        }
    }

    public void RemoveProtectedObject(GameObject obj)
    {
        if (protectedObjects.Contains(obj))
        {
            protectedObjects.Remove(obj);
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
                // Log the object details for debugging
                Debug.Log($"Checking object: {items[i].name}");

                if (protectedObjects.Contains(items[i]))
                {
                    Debug.Log($"Skipping protected object: {items[i].name}");
                    continue;
                }

                if (items[i].transform.parent != null && items[i].transform.parent.CompareTag("Player"))
                {
                    Debug.Log($"Skipping object with Player as parent: {items[i].name}");
                    continue;
                }

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
