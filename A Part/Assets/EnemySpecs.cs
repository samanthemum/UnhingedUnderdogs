using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecs : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite sprite;
    public float speed;
    public float attack;
    public float health;
    public RuntimeAnimatorController animator;
    public AudioClip attackedSound;
    public AudioClip deadSound;



    public void SetStats(Sprite newSprite, float newSpeed, float newAttack, float newHealth, RuntimeAnimatorController newAnimator, AudioClip attackedSound, AudioClip deathSonud)
    {
        this.GetComponent<SpriteRenderer>().sprite = newSprite;
        this.GetComponent<Attack>().SetAttack(newAttack);
        this.GetComponent<Health>().SetHealth(newHealth);
        this.GetComponent<EnemyMovement>().SetSpeed(newSpeed);
        this.GetComponent<Attack>().attackAudioClip = attackedSound;
        this.GetComponent<EnemyMovement>().deathSound = deathSonud;

        // don't know if this will work, will worry about it later
        this.GetComponent<Animator>().runtimeAnimatorController = newAnimator;
    }
}
