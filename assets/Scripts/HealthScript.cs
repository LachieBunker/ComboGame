using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour {

    public int currentHealth;
    public int maxHealth;
    public int chargeLevel;

    public int ui_displayState;
    Camera cam;
    public GameObject ui_healthCanvas;
    public Slider ui_healthSlider;
    public Text ui_chargeNum;

    public GameObject ui_damageNotificationPrefab;

	// Use this for initialization
	void Start ()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        UpdateUI();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 charPos = cam.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y +1, transform.position.z));
        if(charPos.x > 0 && charPos.x < 1 && charPos.y > 0 && charPos.y < 1)
        {
            if(ui_displayState >= 1)
            {
                //Debug.Log(charPos);
                //Debug.Log(ui_healthCanvas.GetComponent<RectTransform>().position);
                ui_healthCanvas.GetComponent<RectTransform>().position = new Vector3(charPos.x * Screen.width, charPos.y * Screen.height, charPos.z);
                
            }
        }
    }
    public void UpdateUI()
    {
        ui_healthSlider.maxValue = maxHealth;
        ui_healthSlider.value = currentHealth;
        ui_chargeNum.text = chargeLevel.ToString();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateUI();
        if(ui_displayState >= 2)
        {
            Vector3 charPos = cam.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z));
            if (charPos.x > 0 && charPos.x < 1 && charPos.y > 0 && charPos.y < 1)
            {
                GameObject damagePrefab = Instantiate(ui_damageNotificationPrefab);
                damagePrefab.transform.GetChild(0).GetComponent<RectTransform>().position = new Vector3(charPos.x * Screen.width, charPos.y * Screen.height, charPos.z);
                damagePrefab.GetComponentInChildren<Text>().text = damageAmount.ToString();
                damagePrefab.GetComponentInChildren<Text>().color = Color.red;
            }
        }
        CheckHealth();
    }

    public void TakeDamage(int damageAmount, int chargeAmount)
    {
        currentHealth -= damageAmount;
        chargeLevel += chargeAmount;
        UpdateUI();
        if (ui_displayState >= 2)
        {
            Vector3 charPos = cam.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z));
            if (charPos.x > 0 && charPos.x < 1 && charPos.y > 0 && charPos.y < 1)
            {
                GameObject damagePrefab = Instantiate(ui_damageNotificationPrefab);
                damagePrefab.transform.GetChild(0).GetComponent<RectTransform>().position = new Vector3(charPos.x * Screen.width, charPos.y * Screen.height, charPos.z);
                damagePrefab.GetComponentInChildren<Text>().text = damageAmount.ToString();
                damagePrefab.GetComponentInChildren<Text>().color = Color.red;
            }
        }
        CheckHealth();
    }

    public void TakeFinisher(int baseDamage, int chargeDamage)
    {
        int bonusDamage = chargeLevel * chargeDamage;
        int totalDamage = baseDamage + bonusDamage;
        currentHealth -= totalDamage;
        RemoveCharges();
        UpdateUI();
        if (ui_displayState >= 2)
        {
            Vector3 charPos = cam.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z));
            if (charPos.x > 0 && charPos.x < 1 && charPos.y > 0 && charPos.y < 1)
            {
                GameObject damagePrefab = Instantiate(ui_damageNotificationPrefab);
                damagePrefab.transform.GetChild(0).GetComponent<RectTransform>().position = new Vector3((charPos.x -0.01f) * Screen.width, charPos.y * Screen.height, charPos.z);
                damagePrefab.GetComponentInChildren<Text>().text = baseDamage.ToString();
                damagePrefab.GetComponentInChildren<Text>().color = Color.red;
                damagePrefab = Instantiate(ui_damageNotificationPrefab);
                damagePrefab.transform.GetChild(0).GetComponent<RectTransform>().position = new Vector3((charPos.x + 0.01f) * Screen.width, charPos.y * Screen.height, charPos.z);
                damagePrefab.GetComponentInChildren<Text>().text = bonusDamage.ToString();
                damagePrefab.GetComponentInChildren<Text>().color = Color.yellow;
            }
        }
        CheckHealth();
    }

    public void CheckHealth()
    {
        if(currentHealth <= 0)
        {
            CharacterDead();
        }
    }

    public void RemoveCharges()
    {
        if (ui_displayState >= 3)
        {
            Vector3 charPos = cam.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z));
            if (charPos.x > 0 && charPos.x < 1 && charPos.y > 0 && charPos.y < 1)
            {
                GameObject damagePrefab = Instantiate(ui_damageNotificationPrefab);
                damagePrefab.transform.GetChild(0).GetComponent<RectTransform>().position = new Vector3(charPos.x * Screen.width, (charPos.y - 0.035f) * Screen.height, charPos.z);
                damagePrefab.GetComponentInChildren<Text>().text = chargeLevel.ToString();
                damagePrefab.GetComponentInChildren<Text>().color = Color.green;
            }
        }
        chargeLevel = 0;
        //remove/update ui/particles
    }

    public void CharacterDead()
    {
        gameObject.GetComponent<BaseCharacterController>().CharacterDead();
    }

}
