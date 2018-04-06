using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAttack : MonoBehaviour {

    public string attackName;
    public AttackTypes attackType;
    public int damage;
    public int charge;
    public bool isFinisher;
    public float range;
    public float angle;
    public int hitLimit;
    public float coolDownTimer;
    public float coolDownDuration;
    public GameObject attackObject;

    public Animation anim;
    public float duration;
    public float colliderDelay;

    public int ui_displayState;
    public RawImage ui_attackIcon;
    public Vector3 ui_attackIconPos;
    public RawImage ui_attackImage;
    public RawImage ui_attackIconCover;

	// Use this for initialization
	void Start ()
    {
        AnimationClip[] clips = attackObject.GetComponentInChildren<Animator>().runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if(clip.name == attackName + "AttackAnimation")
            {
                duration = clip.length / 2;
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(ui_displayState >= 2)
        {
            if(!CanAttack())
            {
                ui_attackIconCover.transform.position = new Vector3(ui_attackIconPos.x, ui_attackIconPos.y + ((coolDownTimer - Time.time) * (50/coolDownDuration)));
                ui_attackIconCover.transform.localScale = new Vector3(1, ((coolDownTimer - Time.time) * 1/coolDownDuration), 1);
            }
            else if(CanAttack() && ui_attackIconCover.IsActive())
            {
                ui_attackIconCover.gameObject.SetActive(false);
            }
        }
	}

    public bool CanAttack()
    {
        if(coolDownTimer <= Time.time)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Attack(List<string> enemyTags)
    {
        coolDownTimer = Time.time + coolDownDuration;
        if(ui_displayState >= 2)
        {
            ui_attackIconCover.gameObject.SetActive(true);
        }
        switch(attackType)
        {
            case AttackTypes.Melee:
                attackObject.GetComponent<AttackObject>().Attack_Melee(damage, charge, isFinisher, enemyTags, hitLimit, attackName, duration, colliderDelay);
                break;
            case AttackTypes.Projectile:
                GameObject attack = (GameObject)Instantiate(attackObject, transform.position, transform.localRotation);
                attack.GetComponent<AttackObject>().Attack_Projectile(damage, charge, isFinisher, enemyTags, hitLimit);
                break;
        }
    }
}

public enum AttackTypes { Melee, Projectile }
