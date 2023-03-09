using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovement : MonoBehaviour
{

    // Update is called once per frame
     [SerializeField] float characterSpeed = 1.0f;
    [SerializeField] Rigidbody rigidbody;
    [SerializeField] Attack attack;
    [SerializeField] float maxVelocity = 10.0f;
    [SerializeField] float projectileSpeed = 2.0f;
    Animator animator;
    Health health;
    GameManager manager;

    // Audio clips
    public AudioClip wasDamagedNoise;
    public AudioClip didAttackNoise;

    // Audio source
    [SerializeField] AudioSource sourceFootsteps;
    [SerializeField] AudioSource swap;

    // Character alts
    [SerializeField] GameObject alternateBuilds;
    MainCharacterBuild[] builds;
    int currentBuildIndex = 0;
    string currentBuild;

    // Last forward vector
    Vector3 lastForward;
    [SerializeField] GameObject projectilePrefab;

    // Cooldown controls
    bool canPaladinAttack = true;



    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        if(rigidbody == null)
        {
            Debug.Log("Could not set rigid body");
        }

        manager = FindObjectOfType<GameManager>();
        if(!manager)
        {
            Debug.Log("Couldn't grab manager");
        }

        attack = GetComponent<Attack>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();

        attack.attackAudioClip = didAttackNoise;
        health.damageSound = wasDamagedNoise;

        // setup alternate builds
        builds = alternateBuilds.GetComponentsInChildren<MainCharacterBuild>();
        if(builds.Length > 0)
        {
            SetBuild(builds[0]);
            currentBuildIndex = 0;
        }
    }

    private void SetBuild(MainCharacterBuild newBuild)
    {
        // Setup battle stats
        attack.SetAttack(newBuild.attack);
        characterSpeed = newBuild.speed;
        health.SetDefense(newBuild.defense);
        maxVelocity = newBuild.maxSpeed;

        // Set visuals
        animator.runtimeAnimatorController = newBuild.animator;
        GetComponent<SpriteRenderer>().sprite = newBuild.sprite;

        // Set sounds
        wasDamagedNoise = newBuild.hitClip;
        didAttackNoise = newBuild.attackClip;
        attack.attackAudioClip = didAttackNoise;
        health.damageSound = wasDamagedNoise;

        // Set admin stuff
        currentBuild = newBuild.title;
    }

    void Update()
    {

        float horizontalComponent = Input.GetAxis("Horizontal");
        float verticalComponent = Input.GetAxis("Vertical");
        
        if(rigidbody)
        {
            //Vector3 currentPosition = rigidbody.transform.position;
            //Vector3 targetPosition = new Vector3(currentPosition.x + horizontalComponent * characterSpeed, currentPosition.y, currentPosition.z + verticalComponent * characterSpeed);
            // GetComponent<Rigidbody>().velocity = characterSpeed * (new Vector3(horizontalComponent, GetComponent<Rigidbody>().velocity.y, verticalComponent));
            if(rigidbody.velocity.magnitude < maxVelocity)
            {
                GetComponent<Rigidbody>().AddForce(characterSpeed * (new Vector3(horizontalComponent, GetComponent<Rigidbody>().velocity.y, verticalComponent)));
            } else
            {
                rigidbody.velocity = rigidbody.velocity.normalized * maxVelocity;
            }

            if(rigidbody.velocity.magnitude > .0f && health.GetHealth() > 0)
            {
                animator.SetBool("IsWalking", true);
                Debug.Log("And we're walkign!");
                Debug.Log("Current animation state is " + animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"));
                if(horizontalComponent < 0)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                } 
                else
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }

                if(sourceFootsteps.isPlaying == false)
                {
                    sourceFootsteps.Play();
                    sourceFootsteps.loop = true;
                }
            } 
            else
            {
                animator.SetBool("IsWalking", false);
                sourceFootsteps.Stop();
            }

        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
        {
           
        }

        // Do attacks
        if (Input.GetButtonDown("Attack"))
        {
            if (currentBuild != "cleric"  && canPaladinAttack)
            {
                canPaladinAttack = false;
                attack.DoAttack();
                StartCoroutine(CoolDownPaladinAttack());
             }

            else if (currentBuild == "cleric")
            {
                Debug.Log("Spawn projectile");
                EnemyMovement target = FindObjectOfType<EnemyMovement>();
                if(target)
                {
                    Vector3 projectileDirection = (target.transform.position - this.transform.position).normalized;
                    GameObject projectile = Instantiate(projectilePrefab, this.transform.position + .1f * projectileDirection, this.transform.rotation) as GameObject;
                    projectile.GetComponent<Rigidbody>().AddForce(projectileDirection * projectileSpeed);
                    projectile.GetComponent<TalismanController>().mainCharacterPosition = this.transform;
                    projectile.GetComponent<Attack>().SetAttack(attack.GetAttack());
                }
            }
        }

        // Do Swaps
        if(Input.GetButtonDown("Swap"))
        {
            currentBuildIndex++;
            currentBuildIndex %= builds.Length;
            SetBuild(builds[currentBuildIndex]);
            swap.Play();
        }

        // Do Dodges
        if(Input.GetButtonDown("Dodge"))
        {
            Physics.IgnoreLayerCollision(3, 6, true);
            animator.SetTrigger("dodge");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // check to see if it was an enemy
        if(collision.gameObject.GetComponent<EnemyMovement>())
        {
            attack.setCurrentAttackTarget(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        attack.setCurrentAttackTarget(null);
    }

    private void ResetCollisionPhysics()
    {
        Physics.IgnoreLayerCollision(3, 6, false);
    }

    private IEnumerator CoolDownPaladinAttack()
    {
        yield return new WaitForSeconds(.5f);
        canPaladinAttack = true;
    }

}
