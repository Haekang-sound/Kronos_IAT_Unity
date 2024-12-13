using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 오브젝트 파괴를 위한 클래스
/// 
/// ohk    v1
/// </summary>
public class Fracture : MonoBehaviour, IMessageReceiver
{
	public Collider[] colliders;
	public Damageable damageable;
	public event Action OnDeath;
	public GameObject timeRing;

	[SerializeField] private Collider _myColldier;
	[SerializeField] private Renderer _myRenderer;
	[SerializeField] private bool isDestroy;

	private int deathCount = 0;
	private bool isBroke;

	private void Awake()
	{
		colliders = GetComponentsInChildren<Collider>();
		foreach (Collider c in colliders)
		{
			if (c == _myColldier) continue;
			c.gameObject.GetComponent<Renderer>().enabled = false;
			Rigidbody rb = c.GetComponent<Rigidbody>();
			rb.constraints = (RigidbodyConstraints)126;
		}
	}

	private void Update()
	{
		if (damageable.currentHitPoints <= 0f)
		{
			damageable.JustDead();
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
		if (deathCount == 0)
		{
			OnDeath?.Invoke();
			deathCount = 1;
		}

		_myRenderer.enabled = false;
		foreach (Collider c in colliders)
		{
			if (c == _myColldier) continue;

			c.gameObject.GetComponent<Renderer>().enabled = true;
			Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
			rb.constraints = (RigidbodyConstraints)0;
		}

		if (isDestroy)
		{
			if (timeRing != null)
				Destroy(timeRing);

			Destroy(gameObject, 3f);
		}
	}

	public void SoundCrack()
	{
		if (!isBroke)
		{
			isBroke = true;
			SoundManager.Instance.PlaySFX("Stone_Crack_1_Sound_SE", transform);
		}
	}

	private void Damaged(Damageable.DamageMessage damageData)
	{
		throw new NotImplementedException();
	}
}
