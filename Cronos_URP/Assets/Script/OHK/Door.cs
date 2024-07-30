using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
	bool isOpening = false;
	float angle = 0f;
	float speed = -20;
	// Start is called before the first frame update

	public CombatZone _combatZone;

	public bool isLeft;

	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (_combatZone.isClear)
		{
			// 프로토타입용
			{
				//Destroy(this.gameObject);
			}

			isOpening = true;
		}

		if (Mathf.Abs(angle) >= 80f)
		{
			isOpening = false;
		}

		OpenTheDoor(isOpening);

	}
	void OpenTheDoor(bool value)
	{
		if (isOpening)
		{
			if (isLeft)
			{
				angle += Time.deltaTime * speed;
			GetComponent<Transform>().Rotate(new Vector3(0f, 1f, 0f), Time.deltaTime * speed);
			}
			else
			{
				angle -= Time.deltaTime * speed;
			GetComponent<Transform>().Rotate(new Vector3(0f, 1f, 0f), Time.deltaTime * -speed);
			}
		}
	}
}
