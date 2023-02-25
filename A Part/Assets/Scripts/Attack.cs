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
            return;
        }

        animator.SetTrigger("attack");
        currentCoolDownTime = attackCoolDown;
        Debug.Log("You attacked");

        if (!currentAttackTarget)
        {
            Debug.Log("No attack object");
            return;
        }

        if(currentCoolDownTime == 0.0f)
        {
            // check to ensure the enemy is something that can actually be damaged
            Health enemyHealth = currentAttackTarget.GetComponent<Health>();
            if(enemyHealth)
            {
                enemyHealth.TakeDamage(attackDamage);
            } 
        } 
        
    }

    public float GetAttack()
    {
        return attackDamage;
    }
}
