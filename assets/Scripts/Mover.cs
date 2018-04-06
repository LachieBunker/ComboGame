using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public Vector3 moveAmount;
    public Vector3 lowerLimit;
    public Vector3 upperLimit;
    public GameObject objectToDestroy;

	// Use this for initialization
	void Start () {
		if(objectToDestroy == null)
        {
            objectToDestroy = this.gameObject;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(moveAmount.x, moveAmount.y, moveAmount.z);
        if(CheckOutOfBounds())
        {
            Destroy(objectToDestroy);
        }
	}

    public bool CheckOutOfBounds()
    {
        if(CheckAxis(transform.position.x, lowerLimit.x, upperLimit.x))
        {
            return true;
        }
        if(CheckAxis(transform.position.y, lowerLimit.y, upperLimit.y))
        {
            return true;
        }
        if(CheckAxis(transform.position.z, lowerLimit.z,upperLimit.z))
        {
            return true;
        }
        return false;
    }

    public bool CheckAxis(float num, float _lowerLimit, float _upperLimit)
    {
        if(num < _lowerLimit)
        {
            return true;
        }
        else if(num > _upperLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
