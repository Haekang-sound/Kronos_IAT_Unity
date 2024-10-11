using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour
{
	public Collider[] colliders;

	private void Awake()
	{
		colliders = GetComponentsInChildren<Collider>();
		foreach (Collider c in colliders) 
		{
			if (c.name == "Fracture_box_Test" || c.name == "Fracture_tower") continue;

			c.gameObject.GetComponent<Renderer>().enabled = false;
			Rigidbody rb = c.GetComponent<Rigidbody>();
			rb.constraints = (RigidbodyConstraints)126;
		}
	}

	void Start()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		 if(other.tag == "Player")
		{
			GetComponent<Renderer>().enabled = false;
			foreach(Collider c in colliders)
			{
				if (c.name == "Fracture_box_Test" || c.name == "Fracture_tower") continue;

				c.gameObject.GetComponent<Renderer>().enabled = true;
				Rigidbody rb = c.gameObject.GetComponent <Rigidbody>();
				rb.constraints = (RigidbodyConstraints)0;
			}
		}
	}
}
