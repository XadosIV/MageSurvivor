using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] Health player;
    [SerializeField] Spawner spawner;
    [SerializeField] Slider hpSlider;
    [SerializeField] GameObject gameOverPanel;

    bool isGameOver;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InitHealthUI();
    }

    void InitHealthUI()
    {
        if (player && hpSlider)
        {
            hpSlider.minValue = 0f;
            hpSlider.maxValue = player.maxHealth;
            hpSlider.value = player.currentHealth;
        }
    }

    void Update()
    {
        if (player == null)
        {
            if (hpSlider) hpSlider.gameObject.SetActive(false);
            return;
        }

        if (hpSlider)
        {
            if (hpSlider.maxValue != player.maxHealth)
                hpSlider.maxValue = player.maxHealth;

            hpSlider.value = player.currentHealth;
        }

        if (!isGameOver && player.currentHealth <= 0)
        {
            isGameOver = true;
            if (spawner) spawner.enabled = false;
            if (gameOverPanel) gameOverPanel.SetActive(true);
        }
    }
}