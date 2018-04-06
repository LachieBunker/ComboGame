using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterController : MonoBehaviour {

    public bool busy;
    public float moveSpeed;
    public float turningSpeed;
    public List<string> enemyTags;
    public CharacterAttack[] attacks;
    public int threatLevel = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void CharacterDead()
    {

    }

    public IEnumerator DelayDisableBusy(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Debug.Log("Busy now false");
        busy = false;
    }

    public int GetPriorityLevel(Vector3 characterPos)
    {
        HealthScript hScript = gameObject.GetComponent<HealthScript>();
        //Health. Low health > high health
        int p_Health = hScript.maxHealth - hScript.currentHealth;

        //Distance. Closer > further
        float p_Distance = Vector3.Distance(characterPos, transform.position);

        //Charges. Charges > none
        int p_Charges = hScript.chargeLevel;

        //Attacks.
        int p_Attacks = attacks.Length;

        float basePriority = (p_Health) + (p_Distance/5) + (p_Charges/2) + (p_Attacks);

        int totalPriority = (int)(basePriority * threatLevel);

        return totalPriority;
    }
}
