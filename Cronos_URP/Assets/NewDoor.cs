using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoor : MonoBehaviour
{
	[SerializeField] private Transform left;
	[SerializeField] private Transform right;

	public bool isOpen;

	private float degree;
	
	[SerializeField] private float speed = 1f;
	[SerializeField] private float time = 1f;

	private void Update()
	{
		if (isOpen)
		{
			Debug.Log(degree);
			if (degree < time)
			{
				degree += Time.deltaTime * speed;
				left.Rotate(new Vector3(0, 1, 0), -degree);
				right.Rotate(new Vector3(0, 1, 0), degree);
			}

			
		}
	}

	public void OpenDoor()
	{
		isOpen = true; 
	}

}
