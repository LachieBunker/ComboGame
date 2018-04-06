using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseCharacterController {

    public KeyCode[] moveKeys;//[Left, Right, Up, Down]
    public KeyCode jumpKey;
    public KeyCode action1Key;
    public KeyCode action2Key;
    public KeyCode action3Key;
    public KeyCode action4Key;

    public bool attacking;
    public bool lockMovement;

    // Use this for initialization
    void Start ()
    {
        attacking = false;
        attacks = gameObject.GetComponents<CharacterAttack>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!busy)
        {
            //Movement
            if (Input.GetKey(moveKeys[0]))
            {
                transform.Rotate(0, -turningSpeed, 0);
            }
            if (Input.GetKey(moveKeys[1]))
            {
                transform.Rotate(0, turningSpeed, 0);
            }
            if (Input.GetKey(moveKeys[2]))
            {
                //transform.Translate(Vector3.forward * moveSpeed);
                float moveDist = 0;
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.forward, out hit, 3))
                {
                    float dist = Vector3.Distance(transform.position, hit.point);
                    if(dist < 1.25)
                    {
                        Debug.Log("close to: " + hit.transform.gameObject + ". " + dist + " away");
                        moveDist = dist/4;
                    }
                    else
                    {
                        moveDist = 1;
                    }
                }
                else
                {
                    moveDist = 1;
                }
                transform.position = Vector3.Lerp(transform.position, (transform.position + (transform.forward.normalized * moveDist)), moveSpeed);
            }
            if (Input.GetKey(moveKeys[3]))
            {
                float moveDist = 0;
                RaycastHit hit;
                if(Physics.Raycast(transform.position, -transform.forward, out hit, 3))
                {
                    float dist = Vector3.Distance(transform.position, hit.transform.position);
                    if(dist < 2)
                    {
                        Debug.Log("close to: " + hit.transform.gameObject + ". " + dist + " away");
                        moveDist = dist/4;
                    }
                    else
                    {
                        moveDist = 1;
                    }
                }
                else
                {
                    moveDist = 1;
                }
                transform.position = Vector3.Lerp(transform.position, (transform.position + (-transform.forward.normalized * moveDist)), moveSpeed);
            }
            if (Input.GetKey(jumpKey))
            {

            }

            if(!attacking)
            {
                if (Input.GetKeyDown(action1Key))
                {
                    if (attacks[0].CanAttack())
                    {
                        Attack(0);
                    }
                }
                if (Input.GetKeyDown(action2Key))
                {
                    if (attacks[1].CanAttack())
                    {
                        Attack(1);
                    }
                }
                if (Input.GetKeyDown(action3Key))
                {
                    if (attacks[2].CanAttack())
                    {
                        Attack(2);
                    }
                }
                if (Input.GetKeyDown(action4Key))
                {
                    if (attacks[3].CanAttack())
                    {
                        Attack(3);
                    }
                }
            }
            
        }
        
    }

    public void Attack(int attackNum)
    {
        if(lockMovement)
        {
            busy = true;
            StartCoroutine(DelayDisableBusy(attacks[attackNum].duration));
        }
        else
        {
            attacking = true;
            StartCoroutine(DelayDisableAttacking(attacks[attackNum].duration));
        }
        attacks[attackNum].Attack(enemyTags);
    }

    public IEnumerator DelayDisableAttacking(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        attacking = false;
    }

    public override void CharacterDead()
    {
        Debug.Log("Player Killed");
    }

}
