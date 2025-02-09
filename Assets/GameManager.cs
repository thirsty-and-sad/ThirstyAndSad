using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string currentEnemyTag;
    public int playerSanity = 100;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
