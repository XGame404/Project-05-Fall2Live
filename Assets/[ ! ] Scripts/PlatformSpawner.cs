using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Normal_Platform_Prefab;
    [SerializeField] private GameObject Spike_Platform_Prefab;
    [SerializeField] private GameObject[] Moving_Platform_Prefabs;
    [SerializeField] private GameObject Breakable_Platform_Prefabs;

    [SerializeField] private float Spawn_Timer = 1.2f; // Initial spawn timer
    [SerializeField] private float SpawnTimerDecreaseRate = 0.01f; // How fast the spawn timer decreases
    [SerializeField] private float MinSpawnTimer = 0.6f; // Minimum spawn timer (cap)

    private float Current_Spawn_Timer;
    private int Platform_Spawn_Count;

    [SerializeField] private float Min_X = -2f, Max_X = 2f;

    private float elapsedTime = 0f;

    void Start()
    {
        Current_Spawn_Timer = Spawn_Timer;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Gradually decrease the spawn timer over time, but don't go below the minimum
        if (Spawn_Timer > MinSpawnTimer)
        {
            Spawn_Timer -= SpawnTimerDecreaseRate * Time.deltaTime;
        }

        SpawnPlatforms();
    }

    void SpawnPlatforms()
    {
        Current_Spawn_Timer += Time.deltaTime;

        if (Current_Spawn_Timer >= Spawn_Timer)
        {
            Platform_Spawn_Count++;

            Current_Spawn_Timer = 0f;

            Vector3 temp = transform.position;
            temp.x = Random.Range(Min_X, Max_X);

            GameObject NewPlatform = null;

            // Adjust probabilities to make harder platforms more frequent
            if (Platform_Spawn_Count < 2)
            {
                // Early platforms are mostly normal, but occasionally harder
                if (Random.Range(0, 4) == 0) // 25% chance for a harder platform early
                {
                    NewPlatform = GetRandomHardPlatform(temp);
                }
                else
                {
                    NewPlatform = Instantiate(Normal_Platform_Prefab, temp, Quaternion.identity);
                }
            }
            else if (Platform_Spawn_Count == 2)
            {
                // 50% chance for a harder platform
                if (Random.Range(0, 2) > 0)
                {
                    NewPlatform = Instantiate(Normal_Platform_Prefab, temp, Quaternion.identity);
                }
                else
                {
                    NewPlatform = GetRandomHardPlatform(temp);
                }
            }
            else if (Platform_Spawn_Count == 3)
            {
                // 75% chance for a harder platform
                if (Random.Range(0, 4) == 0) // 25% chance for normal
                {
                    NewPlatform = Instantiate(Normal_Platform_Prefab, temp, Quaternion.identity);
                }
                else
                {
                    NewPlatform = GetRandomHardPlatform(temp);
                }
            }
            else if (Platform_Spawn_Count == 4)
            {
                // Always spawn a harder platform
                NewPlatform = GetRandomHardPlatform(temp);
                Platform_Spawn_Count = 0; // Reset the count
            }

            if (NewPlatform)
                NewPlatform.transform.parent = transform;

            Current_Spawn_Timer = 0f;
        }
    }

    // Helper method to get a random hard platform
    private GameObject GetRandomHardPlatform(Vector3 position)
    {
        int randomType = Random.Range(0, 3); // 0: Moving, 1: Spike, 2: Breakable
        switch (randomType)
        {
            case 0:
                return Instantiate(Moving_Platform_Prefabs[Random.Range(0, Moving_Platform_Prefabs.Length)], position, Quaternion.identity);
            case 1:
                return Instantiate(Spike_Platform_Prefab, position, Quaternion.identity);
            case 2:
                return Instantiate(Breakable_Platform_Prefabs, position, Quaternion.identity);
            default:
                return Instantiate(Normal_Platform_Prefab, position, Quaternion.identity); // Fallback
        }
    }
}