using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	public bool open;
	public GameObject player;
	public float distance_to_door;
	public int num_door;
	public Collision collision;
	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}
	public void collision_detect(Collision collision){
		ContactPoint contact = collision.contacts[0];
		Quaternion.FromToRotation(Vector3.up, contact.normal);
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
				switch(num_door){
				case 1:
					transform.animation.Play("Open_Door"); 
					break;
				case 2:
					transform.animation.Play("Open_Door_2");
					break;
				default:transform.animation.Play("Open_Door");break;
				}

			}
			else
			{
				switch(num_door){
				case 1:
					transform.animation.Play("Close_Door"); 
					break;
				case 2:
					transform.animation.Play("Close_Door_2");
					break;
				default:transform.animation.Play("Close_Door");break;
				}
			}
		}
	}		
}