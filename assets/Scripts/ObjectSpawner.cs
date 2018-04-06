using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

    public GameObject objectPrefab;
    public Vector3 spawnPos;
    public KeyCode spawnKey;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(spawnKey))
        {
            SpawnObject();
        }
	}

    private void SpawnObject()
    {
        Instantiate(objectPrefab, spawnPos, objectPrefab.transform.rotation);
    }
}
