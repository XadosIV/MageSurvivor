using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Health target;
    [SerializeField] Slider slider;
    [SerializeField] float visibleDuration = 1.5f;
    [SerializeField] bool faceCamera = true;

    int lastHealth;
    float hideTimer;

    void Awake()
    {
        if (!slider) slider = GetComponentInChildren<Slider>(true);
        InitIfTargetReady();
    }

    public void SetTarget(Health h)
    {
        target = h;
        InitIfTargetReady();
        if (slider) slider.gameObject.SetActive(false);
    }

    void InitIfTargetReady()
    {
        if (!target || !slider) return;

        slider.minValue = 0;
        slider.maxValue = target.maxHealth;
        slider.value = target.currentHealth;
        lastHealth = target.currentHealth;
    }

    void Update()
    {
        if (!target || !slider) return;

        // Maj du max si nécessaire (sans forcer l’affichage)
        if (Mathf.Abs(slider.maxValue - target.maxHealth) > 0.001f)
        {
            slider.maxValue = target.maxHealth;
        }

        // Détection de dégâts: affichage + reset timer
        if (target.currentHealth < lastHealth)
        {
            slider.value = target.currentHealth;
            slider.gameObject.SetActive(true);
            hideTimer = visibleDuration;
        }
        else
        {
            // Met à jour la valeur sans forcer l’affichage (ex: soins)
            slider.value = target.currentHealth;
        }

        lastHealth = target.currentHealth;

        // Auto-hide
        if (slider.gameObject.activeSelf && visibleDuration > 0f)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f)
            {
                slider.gameObject.SetActive(false);
            }
        }
    }

    void LateUpdate()
    {
        if (!faceCamera) return;
        var cam = Camera.main;
        if (!cam) return;

        Vector3 dir = transform.position - cam.transform.position;
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(dir);
    }
}