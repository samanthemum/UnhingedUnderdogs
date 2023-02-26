using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float attackDamage = 10;
    [SerializeField] float currentCoolDownTime = .0f;
    [SerializeField] float attackCoolDown = .5f;
    [SerializeField] Animator animator;
    GameObject currentAttackTarget = null;
    public AudioClip attackAudioClip;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        // Refresh cooldown
        if(currentCoolDownTime > 0)
        {
            currentCoolDownTime -= Time.deltaTime;
            currentCoolDownTime = currentCoolDownTime < 0 ? 0 : currentCoolDownTime;
        }
    }

    public void setCurrentAttackTarget(GameObject gameObject)
    {
        currentAttackTarget = gameObject;
    }

    public void DoAttack()
    {
        if(currentCoolDownTime != 0.0f)
        {
            Debug.Log("Waiting on cooldown");
            return;
        }

        animator.SetTrigger("attack");
        currentCoolDownTime = attackCoolDown;
        Debug.Log("You attacked");
        if(this.GetComponent<AudioSource>())
        {
            AudioSource source = this.GetComponent<AudioSource>();
            source.clip  = attackAudioClip;
            source.Play();
        }

        if (!currentAttackTarget)
        {
            Debug.Log("No attack object");
            return;
        }

            // check to ensure the enemy is something that can actually be damaged
            Health enemyHealth = currentAttackTarget.GetComponent<Health>();
            if(enemyHealth)
            {
                Debug.Log("Dealing damage");
                Vector3 knockBackDirection = (currentAttackTarget.transform.position - this.transform.position).normalized;
                knockBackDirection.y = 0.0f;
                knockBackDirection = knockBackDirection.normalized;
                enemyHealth.TakeDamage(attackDamage, knockBackDirection);

            } else
            {
                Debug.Log("Current target does not have attack component");
            }
        
    }

    public float GetAttack()
    {
        return attackDamage;
    }

    public void SetAttack(float newAttack)
    {
        attackDamage = newAttack;
    }
}
