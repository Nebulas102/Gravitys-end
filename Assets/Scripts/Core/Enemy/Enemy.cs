using System.Collections;
using System.Collections.Generic;
using UI.Damage;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public string name;
    [SerializeField]
    public int startDamage;
    [SerializeField]
    public int endDamage;
    [SerializeField]
    public float health;
    [SerializeField]
    public GameObject damageDisplay;

    private float currentHealth;
    private Canvas canvas;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();

        currentHealth = health;
    }

    private void Update()
    {
        //Test taking damage
        // if (Input.GetKeyDown(KeyCode.G))
        // {
        //     TakeDamage(0f);
        // }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float modifier)
    {
        int damage = Random.Range(startDamage, endDamage);
        damage -= (Mathf.RoundToInt(modifier / 100)) * damage;
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        if (damageDisplay != null)
        {
            damageDisplay.GetComponent<DamageDisplay>().Show(damage.ToString(), damageDisplay, canvas);
        }

        currentHealth -= damage;

        Debug.Log(currentHealth);
    }
}
