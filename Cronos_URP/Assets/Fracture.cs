using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fracture : MonoBehaviour, IMessageReceiver
{
	public Collider[] colliders;
	public Damageable Damageable;
	public event Action OnDeath;

	[SerializeField] private Collider myColldier;
	[SerializeField] private Renderer myRenderer;

	private void Awake()
	{
		colliders = GetComponentsInChildren<Collider>();
		foreach (Collider c in colliders)
		{
			if (c == myColldier) continue;
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
			Damageable.JustDead();
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

	public void Death()
	{
		OnDeath?.Invoke();
		//GetComponent<Renderer>().enabled = false;
		myRenderer.enabled = false;
		foreach (Collider c in colliders)
		{
			if (c == myColldier) continue;

			c.gameObject.GetComponent<Renderer>().enabled = true;
			Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
			rb.constraints = (RigidbodyConstraints)0;
		}

		Destroy(gameObject, 3f);
	}

	private void Damaged(Damageable.DamageMessage damageData)
	{
		throw new NotImplementedException();
	}
}
