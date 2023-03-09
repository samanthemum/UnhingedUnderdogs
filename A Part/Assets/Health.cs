using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100.0f;
    Animator animator;
    public AudioClip damageSound;
    float defense = 0.0f;

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
        health -= (attackAmount - defense);
        Debug.Log("Setting damage trigger");
        if(animator)
        {
            // animator.SetTrigger("damage");
        }

        if(GetComponent<ParticleSystem>())
        {
            GetComponent<ParticleSystem>().Play();
        }
        
        Debug.Log("Remaining health is " + health);

        if (this.GetComponent<Rigidbody>())
        { 
            this.GetComponent<Rigidbody>().AddForce(knockBackDirection.normalized * 30.0f, ForceMode.Impulse);
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

    public void SetDefense(float newDefense)
    {
        this.defense = newDefense;
    }
}
