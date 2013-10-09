using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class TestObject : TouchObject {
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void NDrag(GestureEvent gEvent)
	{
		MoveObjectInCameraPlane(gEvent);
		return;
		float scale = 0.002f;
		float dX = gEvent.Values["drag_dx"] * scale;
		float dY = gEvent.Values["drag_dy"] * Flipped * scale;
		
		transform.position += new Vector3(dX, dY, 0.0f);
	}
	
	public void NRotate(GestureEvent gEvent)
	{
		float dTheta = gEvent.Values["rotate_dtheta"];
	
		if(dTheta == 0.0f)
		{
			return;
		}
		
		transform.Rotate(0, 0, -dTheta);
	}
	
	public void NScale(GestureEvent gEvent)
	{
		float scale = 0.005f;
		if(!Application.isEditor)
		{
			scale = 0.01f;
		}
		
		float x = gEvent.Values["scale_dsx"] * scale;
		float y = gEvent.Values["scale_dsy"] * scale;
		
		if(x == 0.0f && y == 0.0f)
		{
			return;	
		}
		
		transform.localScale += new Vector3(x, y, 0.0f);
	}
}
