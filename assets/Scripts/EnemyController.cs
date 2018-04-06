using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseCharacterController {

    public ActionState currentState;
    public string currentAction;
    public float detectionRange;
    public float detectionAngle;
    public GameObject currentTargetObject;
    public Vector3 currentTargetPosition;
    public float scoutTimer;
    public float scoutDuration;
    public bool _debug;

	// Use this for initialization
	void Start () {
        //get attacks
        attacks = gameObject.GetComponents<CharacterAttack>();
        //debug log all brain decisions
    }
	
	// Update is called once per frame
	void Update ()
    {
        //CheckSight(true);

        /*if (currentTargetObject != null)
        {
            float angle = Vector3.Angle(transform.forward, transform.position-currentTargetObject.transform.position);
            Debug.Log("Angle: " + angle);
        }*/

        if(_debug)
        {
            //Debug.Log("current target pos is: " + currentTargetPosition);
        }

        if (!busy)
        {
            List<GameObject> surroundingTargets;
            GameObject new_target = CheckSight(out surroundingTargets);
            if(_debug)
            {
                Debug.Log("Can see: " + surroundingTargets.Count + " other targets");
                Debug.Log("CheckSight target: " + new_target);
            }
            if(currentTargetObject != null && new_target != null && new_target.GetComponent<BaseCharacterController>().GetPriorityLevel(transform.position) > currentTargetObject.GetComponent<BaseCharacterController>().GetPriorityLevel(transform.position))
            {
                if(_debug)
                {
                    Debug.Log("New target higher priority than currentTarget. New target: " + new_target);
                }
                currentTargetObject = new_target;
                int abilityNum = GetAttack(currentTargetObject, surroundingTargets);//CheckAbilitiesInRange(Vector3.Distance(gameObject.transform.position, currentTargetObject.transform.position));
                if (Vector3.Distance(transform.position, currentTargetObject.transform.position) < 1.3f)
                {
                    SetState(ActionState.Moving, 1);
                    currentTargetPosition = new Vector3(0, 0, 0);// GetMovePositionFromTarget(currentTargetObject);
                }
                else if (abilityNum >= 0 && Vector3.Distance(transform.position, currentTargetPosition) < 0.1f)
                {
                    //Debug.Log("Have and can see target. Using ability: " + abilityNum);
                    //int attackIndex = getAttackByName(currentAction);
                    float angle = Vector3.Angle(transform.forward, transform.position - currentTargetObject.transform.position);
                    Debug.DrawRay(transform.position, Quaternion.AngleAxis(10 + attacks[abilityNum].angle, Vector3.up) * transform.forward, Color.cyan, 1);
                    Debug.DrawRay(transform.position, Quaternion.AngleAxis(-10 - attacks[abilityNum].angle, Vector3.up) * transform.forward, Color.cyan, 1);
                    if (angle < (170 - attacks[abilityNum].angle))
                    {
                        Debug.Log("Angle: " + angle);
                        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, currentTargetObject.transform.position - transform.position, turningSpeed, 0.0F));
                    }
                    SetState(ActionState.Attacking);
                    Attack(abilityNum);
                }
                else //No available abilities
                {
                    //Debug.Log("Have and can see target. Moving towards target");
                    SetState(ActionState.Moving, 1);
                    currentTargetPosition = GetMovePositionFromTarget(currentTargetObject);
                    //transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, moveSpeed);
                }
            }
            else if(currentTargetObject != null && Physics.Raycast(transform.position, currentTargetObject.transform.position - transform.position, detectionRange))//Already have a target and can see target
            {
                if(_debug)
                {
                    Debug.Log("Have current target, and can see current target. Current target: " + currentTargetObject);
                }
                int abilityNum = GetAttack(currentTargetObject, surroundingTargets);//CheckAbilitiesInRange(Vector3.Distance(gameObject.transform.position, currentTargetObject.transform.position));
                if (Vector3.Distance(transform.position, currentTargetObject.transform.position) < 1.3f)
                {
                    SetState(ActionState.Moving, 1);
                    currentTargetPosition = new Vector3(0, 0, 0);// GetMovePositionFromTarget(currentTargetObject);
                }
                else if (abilityNum >= 0 && Vector3.Distance(transform.position, currentTargetPosition) < 0.1f)
                {
                    //Debug.Log("Have and can see target. Using ability: " + abilityNum);
                    float angle = Vector3.Angle(transform.forward, transform.position - currentTargetObject.transform.position);
                    Debug.DrawRay(transform.position, Quaternion.AngleAxis(10 + attacks[abilityNum].angle, Vector3.up) * transform.forward, Color.cyan, 1);
                    Debug.DrawRay(transform.position, Quaternion.AngleAxis(-10 - attacks[abilityNum].angle, Vector3.up) * transform.forward, Color.cyan, 1);
                    if (angle < (170 - attacks[abilityNum].angle))
                    {
                        Debug.Log("Angle: " + angle);
                        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, currentTargetObject.transform.position - transform.position, turningSpeed, 0.0F));
                    }
                    SetState(ActionState.Attacking);
                    Attack(abilityNum);
                }
                else //No available abilities
                {
                    //Debug.Log("Have and can see target. Moving towards target");
                    SetState(ActionState.Moving, 1);
                    currentTargetPosition = GetMovePositionFromTarget(currentTargetObject);
                    //transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, moveSpeed);
                }
            }
            else if(currentTargetObject == null && new_target != null)//Don't have current target, but can see new target
            {
                if(_debug)
                {
                    Debug.Log("Have no target, and can see new target. New target: " + new_target);
                }
                currentTargetObject = new_target;
                int abilityNum = GetAttack(currentTargetObject, surroundingTargets);//CheckAbilitiesInRange(Vector3.Distance(gameObject.transform.position, currentTargetObject.transform.position));
                if (Vector3.Distance(transform.position, currentTargetObject.transform.position) < 1.3f)
                {
                    SetState(ActionState.Moving, 1);
                    currentTargetPosition = new Vector3(0, 0, 0);// GetMovePositionFromTarget(currentTargetObject);
                }
                else if (abilityNum >= 0 && Vector3.Distance(transform.position, currentTargetPosition) < 0.1f)
                {
                    //Debug.Log("Have new target. Using ability: " + abilityNum);
                    float angle = Vector3.Angle(transform.forward, transform.position - currentTargetObject.transform.position);
                    Debug.DrawRay(transform.position, Quaternion.AngleAxis(10 + attacks[abilityNum].angle, Vector3.up) * transform.forward, Color.cyan, 1);
                    Debug.DrawRay(transform.position, Quaternion.AngleAxis(-10 - attacks[abilityNum].angle, Vector3.up) * transform.forward, Color.cyan, 1);
                    if (angle < (170 - attacks[abilityNum].angle))
                    {
                        Debug.Log("Angle: " + angle);
                        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, currentTargetObject.transform.position - transform.position, turningSpeed, 0.0F));
                    }
                    SetState(ActionState.Attacking);
                    Attack(abilityNum);
                }
                else //No available abilities
                {
                    //Debug.Log("Have new target. Moving towards target");
                    SetState(ActionState.Moving, 1);
                    currentTargetPosition = GetMovePositionFromTarget(currentTargetObject);
                    //transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, moveSpeed);
                }
            }
            else if(new_target == null && CheckTargetOnNearbyAllies(transform.tag))//can't see new target, but nearby allies have target
            {
                currentTargetObject = GetTargetOnNearbyAllies(transform.tag);
                if(_debug)
                {
                    Debug.Log("Have no target, can't see new target. Ally has target. New target: " + currentTargetObject);
                }
                Debug.Log("new target: " + currentTargetObject);
                currentTargetPosition = GetMovePositionFromTarget(currentTargetObject);
                
                //get position to move to
                //set action to move and move towards that position
                SetState(ActionState.Moving, 1);
            }
            else if(new_target == null && currentTargetObject != null)//Can't see new target, but know last target
            {
                if(_debug)
                {
                    Debug.Log("Have old target, and can't see new. Moving towards target");
                }
                currentTargetObject = null;//Remove current target, because out of sight
                SetState(ActionState.Moving, 1);//Move to last known position of target
                //transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, moveSpeed);
            }
            else if(new_target == null && currentTargetObject == null)//Don't have target, can't see new target
            {
                if(_debug)
                {
                    Debug.Log("Don't have target, and can't see new one");
                }
                SetState(ActionState.Scouting, 1.5f);
                scoutTimer = Time.time + scoutDuration;
            }
            else//Error
            {
                if(_debug)
                {
                    Debug.Log("Error! currentTarget: " + currentTargetObject + ". new_target: " + new_target);
                }
                SetState(ActionState.Scouting, 1.5f);
                scoutTimer = Time.time + scoutDuration;
            }
        }
        else//if busy
        {
            switch(currentState)
            {
                case ActionState.Attacking:
                    /*if(currentTargetObject != null)
                    {
                        int attackIndex = getAttackByName(currentAction);
                        float angle = Vector3.Angle(transform.forward, transform.position - currentTargetObject.transform.position);
                        if(attackIndex >= 0)
                        {
                            Debug.DrawRay(transform.position, Quaternion.AngleAxis(10 + attacks[attackIndex].angle, Vector3.up) * transform.forward, Color.cyan, 1);
                            Debug.DrawRay(transform.position, Quaternion.AngleAxis(-10 - attacks[attackIndex].angle, Vector3.up) * transform.forward, Color.cyan, 1);
                            if (angle < (170-attacks[attackIndex].angle))
                            {
                                Debug.Log("Angle: " + angle);
                                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, currentTargetObject.transform.position - transform.position, turningSpeed, 0.0F));
                            }
                        }
                        
                    }*/
                    
                    break;
                case ActionState.Idle:
                    
                    break;
                case ActionState.Moving:
                    if(_debug)
                    {
                        //Debug.Log("Moving towards: " + currentTargetPosition);
                    }
                    if(currentTargetObject != null)
                    {
                        RaycastHit hit;
                        //Debug.DrawRay(transform.position, currentTargetObject.transform.position - transform.position, Color.green);
                        //Debug.DrawLine(transform.position, currentTargetPosition, Color.magenta);
                        //Debug.Log(hit.transform.gameObject);
                        //Debug.Log("Checking can see target object");
                        if (Physics.Raycast(transform.position, currentTargetObject.transform.position - transform.position, out hit, detectionRange))
                        {
                            //Debug.Log("Can see target: " + hit.transform.gameObject);
                            currentTargetPosition = GetMovePositionFromTarget(currentTargetObject);
                        }
                    }
                    
                    if(Vector3.Distance(transform.position, currentTargetPosition) > 0.1f)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, moveSpeed);
                        //Face targetPos
                        Vector3 targetDir = currentTargetPosition - transform.position;
                        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, turningSpeed, 0.0F);
                        if(_debug)
                        {
                            Debug.Log("Pos is: " + transform.position + ". target pos is: " + currentTargetPosition);
                            Debug.Log("targetDir: " + targetDir + ". newDir: " + newDir);
                        }
                        Debug.DrawRay(transform.position, newDir, Color.magenta);
                        if(_debug)
                        {
                            //Debug.Log("rot before turning: " + transform.rotation);
                        }
                        transform.rotation = Quaternion.LookRotation(newDir);
                        if (_debug)
                        {
                            Debug.Log("rot after turning: " + transform.rotation);
                        }
                    }
                    else
                    {
                        //Face target
                        Vector3 targetDir = currentTargetObject.transform.position - transform.position;
                        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, turningSpeed, 0.0F);
                        Debug.DrawRay(transform.position, newDir, Color.magenta);
                        transform.rotation = Quaternion.LookRotation(newDir);
                    }
                    

                    

                    break;
                case ActionState.Scouting:
                    transform.Rotate(0, turningSpeed, 0);
                    if(scoutTimer <= Time.time)
                    {
                        GameObject new_target = CheckSight();
                        if(new_target != null)
                        {
                            busy = false;
                            currentTargetObject = new_target;
                        }
                        else
                        {
                            scoutTimer = Time.time + scoutDuration;
                        }
                    }
                    break;
            }
        }
	}

    public GameObject CheckSight(bool drawRay = false)//Change raycast to be relative to rotation (add bracket, btw)
    {//add changes to other CheckSight()
        RaycastHit[] hits;
        GameObject currentTarget = null;
        int currentTargetPriority = 0;
        if (drawRay)
        {
            Debug.DrawRay(transform.position, transform.forward, Color.blue);
            //Debug.DrawRay(transform.localPosition - transform.right, transform.forward, Color.magenta);
            //Debug.DrawRay(transform.localPosition + transform.right, transform.forward, Color.magenta);
        }
        hits = Physics.BoxCastAll(((transform.position + (transform.forward* 10))), new Vector3(detectionRange / 2, detectionRange / 2, detectionRange / 2), transform.forward, transform.rotation, detectionRange);// (transform.position, transform.forward, out hit_Middle, detectionRange))
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.tag == "Respawn" && drawRay)
                {
                    hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                if (enemyTags.Contains(hit.transform.tag))
                {
                    RaycastHit recentHit;
                    if (Physics.Raycast(transform.position, hit.transform.position - transform.position, out recentHit))
                    {
                        if (recentHit.transform.gameObject == hit.transform.gameObject)
                        {
                            //Debug.Log("Recent hit: " + recentHit.transform.gameObject + " is same as hit: " + hit.transform.gameObject);
                            if (hit.transform.gameObject.GetComponent<BaseCharacterController>().GetPriorityLevel(transform.position) > currentTargetPriority)
                            {
                                currentTarget = hit.transform.gameObject;
                                currentTargetPriority = hit.transform.gameObject.GetComponent<BaseCharacterController>().GetPriorityLevel(transform.position);
                            }
                        }
                        else
                        {
                            //Debug.Log("Recent hit: " + recentHit.transform.gameObject + " is different from hit: " + hit.transform.gameObject);
                        }

                    }
                }
            }
            
        }
        return currentTarget;
    }

    public GameObject CheckSight(out List<GameObject> possibleTargets, bool drawRay = false)//Change raycast to be relative to rotation (add bracket, btw)
    {
        //RaycastHit hit_Middle;
        RaycastHit[] hits;
        GameObject currentTarget = null;
        int currentTargetPriority = 0;
        possibleTargets = new List<GameObject>();
        //RaycastHit hit_Left;
        //RaycastHit hit_Right;
        if(drawRay)
        {
            Debug.DrawRay(transform.position, transform.forward, Color.blue);
            Debug.DrawRay( transform.localPosition - transform.right, transform.forward, Color.magenta);
            Debug.DrawRay(transform.localPosition + transform.right, transform.forward, Color.magenta);
        }
        hits = Physics.BoxCastAll(((transform.position + (transform.forward * 10))), new Vector3(detectionRange / 2, detectionRange / 2, detectionRange / 2), transform.forward, transform.rotation, detectionRange);// (transform.position, transform.forward, out hit_Middle, detectionRange))
        if (hits.Length > 0)
        {
            foreach(RaycastHit hit in hits)
            {
                if (hit.transform.tag == "Respawn" && drawRay)
                {
                    hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                if (enemyTags.Contains(hit.transform.tag))
                {
                    RaycastHit recentHit;
                    if(Physics.Raycast(transform.position, hit.transform.position - transform.position, out recentHit))
                    {
                        if(recentHit.transform.gameObject == hit.transform.gameObject)
                        {
                            //Debug.Log("Recent hit: " + recentHit.transform.gameObject + " is same as hit: " + hit.transform.gameObject);
                            possibleTargets.Add(hit.transform.gameObject);
                            if (hit.transform.gameObject.GetComponent<BaseCharacterController>().GetPriorityLevel(transform.position) > currentTargetPriority)
                            {
                                currentTarget = hit.transform.gameObject;
                                currentTargetPriority = hit.transform.gameObject.GetComponent<BaseCharacterController>().GetPriorityLevel(transform.position);
                            }
                        }
                        else
                        {
                            //Debug.Log("Recent hit: " + recentHit.transform.gameObject + " is different from hit: " + hit.transform.gameObject);
                        }
                        
                    }
                }
            }
            
            //return hit.transform.gameObject;
        }
        if(drawRay)
        {
            foreach (GameObject _target in possibleTargets)
            {
                Debug.DrawLine(transform.position, _target.transform.position, Color.green, 1);
            }
            if(currentTarget != null)
            {
                Debug.DrawLine(transform.position, currentTarget.transform.position, Color.magenta, 1);
            }
        }
        return currentTarget;
    }

    public bool CheckPosHasAlliesNearby(Vector3 posToCheck, string allyTag = "null", float radius = 1.0f, bool debug = false)//Overloading:list of tags|return num instead of bool|bool for checking against "environmental" tags
    {
        if (allyTag == "null")
        {
            allyTag = transform.tag;
        }
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(posToCheck, radius, Vector3.forward, 0.0f);
        if(hits.Length > 0)
        {
            foreach(RaycastHit hit in hits)
            {
                if(hit.transform.tag == "Respawn" && (_debug || debug))
                {
                    hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                if(hit.transform.gameObject != gameObject)
                {
                    if(hit.transform.tag == allyTag)
                    {
                        return true;
                    }
                }
            }
        }
        else
        {
            return false;
        }

        return false;
    }

    public bool CheckTargetOnNearbyAllies(string tag = "null")
    {
        //sphere cast nearby/ get nearby with matching tags/ return true if any nearby allies have a target
        if(tag == "null")
        {
            tag = transform.tag;
        }
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 5, Vector3.forward, 0.0f);
        if(hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if(hit.transform.tag == "Respawn" && _debug)
                {
                    hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                if(hit.transform.gameObject != gameObject)
                {
                    if (hit.transform.tag == tag)
                    {
                        EnemyController eScript = hit.transform.gameObject.GetComponent<EnemyController>();
                        if (eScript != null)
                        {
                            if (eScript.currentTargetObject != null)
                            {
                                //Debug.Log("Ally: " + hit.transform.gameObject + " has target: " + eScript.currentTargetObject);
                                return true;
                            }
                        }
                    }
                }
                
            }
        }
        else
        {
            Debug.Log("No allies in range");
        }

        return false;
    }

    public bool CheckTargetOnNearbyAllies(string tag, out int numAlliesWTargets)
    {
        int alliesWTargets = 0;
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 5, Vector3.forward, 0.0f);
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject != gameObject)
                {
                    if (hit.transform.tag == tag)
                    {
                        EnemyController eScript = hit.transform.gameObject.GetComponent<EnemyController>();
                        if (eScript != null)
                        {
                            if (eScript.currentTargetObject != null)
                            {
                                alliesWTargets++;
                            }
                        }
                    }
                }
                
            }
        }

        numAlliesWTargets = alliesWTargets;

        if(alliesWTargets > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckTargetOnNearbyAllies(List<string> allyTags)
    {
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 5, Vector3.forward, 0.0f);
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if(hit.transform.gameObject != gameObject)
                {
                    if (allyTags.Contains(hit.transform.tag))
                    {
                        EnemyController eScript = hit.transform.gameObject.GetComponent<EnemyController>();
                        if (eScript != null)
                        {
                            if (eScript.currentTargetObject != null)
                            {
                                return true;
                            }
                        }
                    }
                }
                
            }
        }

        return false;
    }

    public bool CheckTargetOnNearbyAllies(List<string> allyTags, out int numAlliesWTargets)
    {
        int alliesWTargets = 0;
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 5, Vector3.forward, 0.0f);
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if(hit.transform.gameObject != gameObject)
                {
                    if (allyTags.Contains(hit.transform.tag))
                    {
                        EnemyController eScript = hit.transform.gameObject.GetComponent<EnemyController>();
                        if (eScript != null)
                        {
                            if (eScript.currentTargetObject != null)
                            {
                                alliesWTargets++;
                            }
                        }
                    }
                }
                
            }
        }

        numAlliesWTargets = alliesWTargets;

        if (alliesWTargets > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameObject GetTargetOnNearbyAllies(string tag = "null")
    {
        //sphere cast nearby/ get nearby with matching tags/ return true if any nearby allies have a target
        if (tag == "null")
        {
            tag = transform.tag;
        }
        RaycastHit[] hits;
        GameObject tempTarget = null;
        int tempTargetPriority = 0;
        hits = Physics.SphereCastAll(transform.position, 5, Vector3.forward, 0.0f);
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if(hit.transform.gameObject != gameObject)
                {
                    if (hit.transform.tag == tag)
                    {
                        EnemyController eScript = hit.transform.gameObject.GetComponent<EnemyController>();
                        if (eScript != null)
                        {
                            if (eScript.currentTargetObject != null)
                            {
                                if (eScript.currentTargetObject.GetComponent<BaseCharacterController>() != null)
                                {
                                    if(eScript.currentTargetObject.GetComponent<BaseCharacterController>().GetPriorityLevel(transform.position) > tempTargetPriority)
                                    {
                                        //Debug.Log("New target: " + eScript.currentTargetObject + " has priority of:" + eScript.currentTargetObject.GetComponent<BaseCharacterController>().GetPriorityLevel(transform.position));
                                        tempTargetPriority = eScript.currentTargetObject.GetComponent<BaseCharacterController>().GetPriorityLevel(transform.position);
                                        tempTarget = eScript.currentTargetObject;
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
        }

        return tempTarget;
    }

    public Vector3 GetMovePositionFromTarget(GameObject _target)
    {
        Vector3 tempReturn = transform.position;
        List<Vector3> possiblePositions = new List<Vector3>();
        //get rotation angle of self relative to target
        float angle = Vector3.Angle(_target.transform.forward, _target.transform.forward);// _target.transform.position - gameObject.transform.position);
        /*if(_target.transform.position.x < transform.position.x)
        {
            angle *= -1;
        }*/
        //find positions "forward" and x degrees around target
        Quaternion rot;
        Vector3 moveDir;
        Vector3 movePos;
        for (int i = 0; i < 6; i++)//play with angle and number of points.
        {
            
            if(angle > 180)
            {
                angle -= 360;
            }
            rot = Quaternion.AngleAxis(angle, Vector3.up);
            moveDir = rot * _target.transform.forward;
            movePos = (moveDir * 1.5f) + _target.transform.position;
            //check if enemies in range
            if (!CheckPosHasAlliesNearby(movePos, transform.tag, 0.7f))
            {
                possiblePositions.Add(movePos);
            }
            //if free, add pos to possiblePositions
            angle += 60;
        }

        //Check each possiblePos to find closest one
        if(possiblePositions.Count > 0)
        {
            tempReturn = possiblePositions[0];
            foreach (Vector3 pos in possiblePositions)
            {
                Debug.DrawLine(_target.transform.position, pos, Color.green);
                if (Vector3.Distance(transform.position, pos) < Vector3.Distance(transform.position, tempReturn))
                {
                    tempReturn = pos;
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, _target.transform.position) > 5)
            {
                tempReturn = transform.position;
            }
            else
            {
                angle = Vector3.Angle(_target.transform.forward, _target.transform.position - gameObject.transform.position);
                if (_target.transform.position.x < transform.position.x)
                {
                    //angle *= -1;
                }
                rot = Quaternion.AngleAxis(angle, Vector3.up);
                moveDir = rot * _target.transform.forward;
                movePos = (moveDir * 5.0f) + _target.transform.position;
                tempReturn = movePos;
            }
            /*
            angle += 90;
            for (int i = 0; i < 6; i++)//play with angle and number of points.
            {

                if (angle > 180)
                {
                    angle -= 360;
                }
                rot = Quaternion.AngleAxis(angle, Vector3.up);
                moveDir = rot * _target.transform.forward;
                movePos = (moveDir * 2.6f) + _target.transform.position;
                //check if enemies in range
                if (!CheckPosHasAlliesNearby(movePos, transform.tag, 0.8f))
                {
                    possiblePositions.Add(movePos);
                }
                //if free, add pos to possiblePositions
                angle += 60;
            }
            if (possiblePositions.Count > 0)
            {
                tempReturn = possiblePositions[0];
                foreach (Vector3 pos in possiblePositions)
                {
                    Debug.DrawLine(_target.transform.position, pos, Color.green);
                    if (Vector3.Distance(transform.position, pos) < Vector3.Distance(transform.position, tempReturn))
                    {
                        tempReturn = pos;
                    }
                }
            }
            else
            {
                if(Vector3.Distance(transform.position, _target.transform.position) > 5)
                {
                    tempReturn = new Vector3(0, 0, 0);
                }
                else
                {
                    angle = Vector3.Angle(_target.transform.forward, _target.transform.position - gameObject.transform.position);
                    if (_target.transform.position.x < transform.position.x)
                    {
                        angle *= -1;
                    }
                    rot = Quaternion.AngleAxis(angle, Vector3.up);
                    moveDir = rot * _target.transform.forward;
                    movePos = (moveDir * 5.0f) + _target.transform.position;
                    tempReturn = new Vector3(0, 0, 0);
                }
            }*/
            //check (8/12) spaces instead of 6|check again further from player
        }

        //Check if tempReturn is closer to target than currentTargetPos
        /*if (Vector3.Distance(currentTargetPosition, _target.transform.position) < 1.6f)//currentTargetPosition in range of target
        {
            if (Vector3.Distance(tempReturn, _target.transform.position) + 0.2f < Vector3.Distance(currentTargetPosition, _target.transform.position))//tempReturn closer to target than currentTargetPosition
            {
                return tempReturn;
            }
            else
            {
                //return currentTargetPosition;
            }
        }
        else
        {
            
        }*/

        //Instantiate(new GameObject("temp"), tempReturn, Quaternion.identity);
        //Debug.Log("Returning new pos at: " + tempReturn);
        Debug.DrawLine(_target.transform.position, tempReturn, Color.yellow, 2.0f);
        if(_debug)
        {
            Debug.Log("Returning new target move pos: " + tempReturn);
        }
        return tempReturn;
    }

    public int GetAttack(GameObject target, List<GameObject> nearbyTargets)
    {
        int currentAttackNum = -1;
        int currentAttackDamage = 0;
        for (int i = 0; i < attacks.Length; i++)
        {
            if (attacks[i].CanAttack())
            {
                int damage = 0;
                int hits = attacks[i].hitLimit;
                float targetDistance = Vector3.Distance(transform.position, target.transform.position);
                if (attacks[i].range >= targetDistance)
                {
                    if (hits > 0)
                    {
                        hits--;
                        damage += attacks[i].damage;
                    }
                    foreach (GameObject _target in nearbyTargets)
                    {
                        if (_target != target && Vector3.Distance(transform.position, _target.transform.position) <= attacks[i].range)
                        {
                            if (hits > 0)
                            {
                                hits--;
                                damage += attacks[i].damage;
                            }
                        }
                    }
                }
                if (damage > currentAttackDamage)
                {
                    if (currentAttackNum != -1 && _debug)
                    {
                        Debug.Log("Attack: " + attacks[i].attackName + ": " + damage + " damage is better than " + attacks[currentAttackNum].attackName + ": " + currentAttackDamage + " damage");
                    }
                    //Debug.Log("Attack: " + attacks[i].attackName + ": " + damage + " damage is better");


                    currentAttackDamage = damage;
                    currentAttackNum = i;
                }
                else
                {
                    if (currentAttackNum != -1 && _debug)
                    {
                        Debug.Log("Attack: " + attacks[i].attackName + ": " + damage + " damage isn't better than " + attacks[currentAttackNum].attackName + ": " + currentAttackDamage + " damage");
                    }
                }
            }
            else
            {
                if(_debug)
                {
                    Debug.Log("Attack: " + attacks[i].attackName + " on cooldown");
                }
            }
            
        }
        return currentAttackNum;
    }

    public int getAttackByName(string _name)
    {
        for(int i = 0; i < attacks.Length; i++)
        {
            if(attacks[i].attackName == _name)
            {
                return i;
            }
        }
        Debug.Log("No attack with name " + _name);
        return -1;
    }

    public int CheckAbilitiesInRange(float targetDistance)
    {
        for(int i = 0; i < attacks.Length; i++)
        {
            if(attacks[i].range >= targetDistance)
            {
                if(attacks[i].CanAttack())
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public void Attack(int attackNum)
    {
        if(_debug)
        {
            Debug.Log("Attacking");
        }
        busy = true;
        StartCoroutine(DelayDisableBusy(2));
        SetAction(attacks[attackNum].attackName);
        attacks[attackNum].Attack(enemyTags);
    }

    public void SetState(ActionState newState)
    {
        currentState = newState;
    }

    public void SetState(ActionState newState, float busyDuration)
    {
        currentState = newState;
        busy = true;
        StartCoroutine(DelayDisableBusy(busyDuration));
    }

    public void SetAction(string _action)
    {
        currentAction = _action;
    }

    public override void CharacterDead()
    {
        Debug.Log("Enemy Killed");
        Destroy(gameObject);
    }
}

public enum ActionState { Idle, Scouting, Moving, Attacking}
