using UnityEngine;
using UnityEngine.SceneManagement;
public class PlatformLogic : MonoBehaviour
{
    private float MoveSpeed = 3f;
    [SerializeField] private float Pos2Destroy_Y = 6f;
    private float SpeedIncreaseRate = 0.075f;
    private float MaxMoveSpeed = 5f;

    private float elapsedTime = 0f;

    [SerializeField]
    private bool Left_Platform, Right_Platform,
                            Breakable_Platform,
                            Spike_Platform,
                            Normal_Platform;
    [SerializeField]
    private GameObject Left_Platform_GO, Right_Platform_GO,
                                         Breakable_Platform_GO,
                                         Spike_Platform_GO,
                                         Normal_Platform_GO;

    [SerializeField] private GameObject Coin_Prefab;

    void OnEnable()
    {
        Normal_Platform_GO.SetActive(Normal_Platform);
        Spike_Platform_GO.SetActive(Spike_Platform);
        Breakable_Platform_GO.SetActive(Breakable_Platform);
        Left_Platform_GO.SetActive(Left_Platform);
        Right_Platform_GO.SetActive(Right_Platform);

        SpawnCoin();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (MoveSpeed < MaxMoveSpeed)
        {
            MoveSpeed += SpeedIncreaseRate * Time.deltaTime;
        }

        MoveUp();
    }

    private void MoveUp()
    {
        Vector2 temp = this.gameObject.transform.position;
        temp.y += MoveSpeed * Time.deltaTime;
        this.gameObject.transform.position = temp;

        if (temp.y >= Pos2Destroy_Y)
        {
            Destroy(this.gameObject);
        }
    }

    void BreakableDeactivate()
    {
        Invoke("DeactivateGameObject", 0.35f);
    }

    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag == "Player")
        {
            if (Breakable_Platform)
            {
                BreakableDeactivate();
            }
        }
    }

    void OnCollisionStay2D(Collision2D target)
    {
        if (target.gameObject.tag == "Player")
        {
            PlayerController player = target.gameObject.GetComponent<PlayerController>();
            if (player != null && player.IsGrounded == true)
            {
                if (Left_Platform)
                {
                    player.PlatformMove(-2.5f);
                }

                if (Right_Platform)
                {
                    player.PlatformMove(2.5f);
                }
            }
        }
    }

    private void SpawnCoin()
    {
        if (Coin_Prefab != null && SceneManager.GetActiveScene().name == "Gameplay")
        {
            if (Spike_Platform)
            {
                Vector3 coinSpawn = new Vector3(0, 1, 0);
                if (Random.Range(0, 2) == 1)
                {
                    coinSpawn = new Vector3(1, 1, 0);
                }
                else
                {
                    coinSpawn = new Vector3(-1, 1, 0);
                }
                Vector3 coinPosition = transform.position + coinSpawn;
                GameObject coin = Instantiate(Coin_Prefab, coinPosition, Quaternion.identity);
                coin.transform.parent = transform;
            }
            else if(!Spike_Platform)
            {
                Vector3 coinPosition = transform.position + new Vector3(0, 1, 0);
                GameObject coin = Instantiate(Coin_Prefab, coinPosition, Quaternion.identity);
                coin.transform.parent = transform;
            }

        }
    }
}