using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour {

    public AttackTypes attackType;
    public int damage;
    public int charge;
    public bool finisher;
    public List<string> enemyTags;
    public int hitLimit;

    public float duration;
    public float colliderDelay;

    public List<GameObject> enemiesHit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetProperties(int _damage, int _charge, bool _finisher, List<string> _enemyTags, int _hitLimit)
    {
        damage = _damage;
        charge = _charge;
        finisher = _finisher;
        enemyTags = _enemyTags;
        hitLimit = _hitLimit;
    }

    public void Attack_Melee(int _damage, int _charge, bool _finisher, List<string> _enemyTags, int _hitLimit, string animName, float _duration, float _colDelay)
    {
        attackType = AttackTypes.Melee;
        damage = _damage;
        charge = _charge;
        finisher = _finisher;
        enemyTags = _enemyTags;
        hitLimit = _hitLimit;
        duration = _duration;
        colliderDelay = _colDelay;

        enemiesHit = new List<GameObject>();

        //gameObject.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(DelayEnableCollider(colliderDelay));
        StartCoroutine(DelayDisableCollider(duration));

        gameObject.GetComponent<Animator>().Play(animName + "AttackAnimation");
    }

    public void Attack_Projectile(int _damage, int _charge, bool _finisher, List<string> _enemyTags, int _hitLimit = 0, float lifeTime = 5)
    {
        attackType = AttackTypes.Projectile;
        damage = _damage;
        charge = _charge;
        finisher = _finisher;
        enemyTags = _enemyTags;
        hitLimit = _hitLimit;
        Destroy(gameObject, lifeTime);

        enemiesHit = new List<GameObject>();
    }

    public IEnumerator DelayEnableCollider(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
    }

    public IEnumerator DelayDisableCollider(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
    }

    public void DamageInteractedObject(GameObject interactedObject)
    {
        HealthScript hScript = interactedObject.GetComponent<HealthScript>();
        if(hScript != null)
        {
            if(finisher)
            {
                hScript.TakeFinisher(damage, charge);
            }
            else
            {
                hScript.TakeDamage(damage, charge);
            }
        }
    }

    private void OnTriggerEnter(Collider other)//OnTriggerStay instead?
    {
        //Debug.Log("hit: " + other.gameObject);
        if(hitLimit >= 1)
        {
            hitLimit--;
            if(enemyTags.Contains(other.gameObject.tag))
            {
                if(!enemiesHit.Contains(other.gameObject))
                {
                    enemiesHit.Add(other.gameObject);
                    DamageInteractedObject(other.gameObject);
                }
            }
        }
        else if(hitLimit == 0)
        {
            hitLimit--;
            
            if (enemyTags.Contains(other.gameObject.tag))
            {
                if (!enemiesHit.Contains(other.gameObject))
                {
                    enemiesHit.Add(other.gameObject);
                    DamageInteractedObject(other.gameObject);
                }
            }
            if (attackType == AttackTypes.Projectile)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            //Debug.Log("No hits available (Melee hit unsuccessful)");
        }
    }
}
