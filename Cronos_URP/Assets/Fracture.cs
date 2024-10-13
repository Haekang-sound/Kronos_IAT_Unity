using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour, IMessageReceiver
{
	public Collider[] colliders;
	public Damageable Damageable;
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
	private void Update()
	{
        if (Damageable.currentHitPoints <= 0f)
		{
			Death();
		}
	}

	public void OnReceiveMessage(MessageType type, object sender, object msg)
	{
		switch (type)
		{
			case MessageType.DAMAGED:
				{
					Damageable.DamageMessage damageData = (Damageable.DamageMessage)msg;

					Damaged(damageData);
				}
				break;
			case MessageType.DEAD:
				{
					Damageable.DamageMessage damageData = (Damageable.DamageMessage)msg;
					Death(/*damageData*/);
				}
				break;
			case MessageType.RESPAWN:
				{

				}
				break;
		}
	}

	private void Death()
	{
		GetComponent<Renderer>().enabled = false;
		foreach (Collider c in colliders)
		{
			if (c.name == "Fracture_box_Test" || c.name == "Fracture_tower") continue;

			c.gameObject.GetComponent<Renderer>().enabled = true;
			Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
			rb.constraints = (RigidbodyConstraints)0;
		}
	}

	private void Damaged(Damageable.DamageMessage damageData)
	{
		throw new NotImplementedException();
	}
}
