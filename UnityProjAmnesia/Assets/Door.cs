using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	public bool Open;
	public GameObject Player;
	public float Distance_To_Door;
	// Use this for initialization
	void Start () 
	{
		Player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void Update () 
	{	
		if(Input.GetKeyDown(KeyCode.E) & 
		   Vector3.Distance(transform.position, player.transform.position)<Distance_To_Door)
		{
			Open = !Open;
			if(Open==true)
			{
				transform.animation.Play("Open_Door");
			}
			else
			{
				transform.animation.CrossFade("Close_Door");
			}
		}
	}		
}