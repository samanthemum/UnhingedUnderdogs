using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100.0f;
    Animator animator;
    public AudioClip damageSound;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if(!animator)
        {
            Debug.Log("Could not find animator component");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetHealth()
    {
        return health;
    }

    public void TakeDamage(float attackAmount, Vector3 knockBackDirection)
    {
        health -= attackAmount;
        Debug.Log("Setting damage trigger");
        animator.SetTrigger("damage");
        Debug.Log("Remaining health is " + health);

        if (this.GetComponent<Rigidbody>())
        {
            this.GetComponent<Rigidbody>().AddForce(knockBackDirection * attackAmount * 150.0f);
        }

        if(this.GetComponent<AudioSource>())
        {
            AudioSource source = this.GetComponent<AudioSource>();
            source.clip = damageSound;
            source.Play();
        }
    }

    public void SetHealth(float newHealth)
    {
        this.health = newHealth;
    }
}
