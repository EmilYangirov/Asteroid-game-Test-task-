using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxPlayerHealth = 5;
    [SerializeField] private Transform healthBar;
    [SerializeField] private GameObject healthPrefab;
    public static event Action OnEndGame;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxPlayerHealth;

        if (healthBar == null || healthPrefab == null)
            return;

        for (int i = 0; i < maxPlayerHealth; i++)
        {
            GameObject newHeart = Instantiate(healthPrefab, Vector3.zero, Quaternion.identity);
            newHeart.transform.parent = healthBar;
        }
            
    }
    public void OnPlayerDie()
    {
        currentHealth--;
        Debug.Log(currentHealth);
        ChangeHealthBar();

        if (currentHealth <= 0)
            OnGameEnd();
    }

    private void OnGameEnd()
    {
        OnEndGame?.Invoke();
    }

    private void ChangeHealthBar()
    {
        for(int i = 0; i < maxPlayerHealth; i++)
        {
            if (i < currentHealth)
                healthBar.GetChild(i).gameObject.SetActive(true);
            else
                healthBar.GetChild(i).gameObject.SetActive(false);
        }
    }
}
