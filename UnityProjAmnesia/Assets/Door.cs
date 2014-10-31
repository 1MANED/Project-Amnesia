using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	public bool open;
	public GameObject player;
	public float distance_to_door;
	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void Update () 
	{	
		if(Input.GetKeyDown(KeyCode.E) & 
		   Vector3.Distance(transform.position, player.transform.position)<distance_to_door)
		{
			open = !open;
			if(open==true)
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