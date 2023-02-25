using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovement : MonoBehaviour
{

    // Update is called once per frame
     [SerializeField] float characterSpeed = 1.0f;
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Attack attack;
    Animator animator;
    Health health;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        if(rigidbody == null)
        {
            Debug.Log("Could not set rigid body");
        }

        attack = GetComponent<Attack>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        // check to see if we should die
        if(health.GetHealth() <= 0)
        {
            Debug.Log("You died!");
        }

        float horizontalComponent = Input.GetAxis("Horizontal");
        float verticalComponent = Input.GetAxis("Vertical");
        
        if(rigidbody)
        {
            Vector3 currentPosition = rigidbody.transform.position;
            Vector3 targetPosition = new Vector3(currentPosition.x + horizontalComponent * characterSpeed, currentPosition.y, currentPosition.z + verticalComponent * characterSpeed);
            rigidbody.MovePosition(targetPosition);
        }

        // Do attacks
        if(Input.GetAxis("Attack") > 0)
        {
            attack.DoAttack();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<EnemyMovement>())
        {
            attack.setCurrentAttackTarget(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        attack.setCurrentAttackTarget(null);
    }

}
