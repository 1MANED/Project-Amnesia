using UnityEngine;
using System.Collections;

public class Open : MonoBehaviour {
	public bool open;
	public GameObject player;
	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectsWithTag("Player")[0];
	}
	
	// Update is called once per frame
	void Update () 
	{	
		   if(Input.GetKeyDown(KeyCode.E) & 
		   		Vector3.Distance(transform.position, player.transform.position)<2)
			{
			open = !open;
			if(open==true)
			{
				//transform.anim
				transform.animation.Play("Open_Door");
				//a_open.Play();
			}
			else
			{
				transform.animation.CrossFade("Close_Door");
			}
		}
	}		
}