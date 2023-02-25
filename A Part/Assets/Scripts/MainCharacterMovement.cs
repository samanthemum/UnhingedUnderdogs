using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovement : MonoBehaviour
{

    // Update is called once per frame
     [SerializeField] float characterSpeed = 1.0f;
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Attack attack;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        if(rigidbody == null)
        {
            Debug.Log("Could not set rigid body");
        }

        attack = GetComponent<Attack>();
    }

    void Update()
    {
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
}
