////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Core
//
//  File:    TouchClock.cs
//  Authors:  Ideum
//
//  NOTICE: Ideum permits you to use, modify, and distribute this file only
//  in accordance with the terms of the license agreement accompanying it.
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class TouchClock : TouchObject {

	
	void Start(){
		//
	}

	void Update(){
		//
	}
	
	public void NDrag(GestureEvent gEvent){
		
		float multiplier = 0.0008f;
			
		Camera cam = Camera.main;

		float dX = gEvent.Values["drag_dx"]*multiplier*Flipped;
		float dY = gEvent.Values["drag_dy"]*multiplier*Flipped;

		Vector3 previousPosition = cam.WorldToScreenPoint(transform.position);
		Vector3 nextPosition = new Vector3(dX, dY, 0.0f);
				
		Vector3 newPosition = previousPosition + nextPosition;

		float cx = Mathf.Clamp(cam.ScreenToWorldPoint(newPosition).x+dX, -2f, 2f);
		float cy = Mathf.Clamp(cam.ScreenToWorldPoint(newPosition).y+dY, 0f, 2f);
		float cz = 0.0f;
	
		transform.position = new Vector3(cx, cy, cz);
		
	}
		
	public void NRotate(GestureEvent gEvent){

			
		float multiplier = 0.75f;
		
		Camera cam = Camera.main;
		
		float dTheta = gEvent.Values["rotate_dtheta"]*multiplier;
		
		float screenX = gEvent.X;
		float screenY = cam.GetScreenHeight() - gEvent.Y;
		float screenZ = cam.WorldToScreenPoint(this.transform.position).z;
	
		Vector3 touchLocation = cam.ScreenToWorldPoint(new Vector3(screenX, screenY, screenZ));
		
		StartAffineTransform(touchLocation);
		
			AffineTransform.transform.Rotate(0, 0, dTheta);
		
		EndAffineTransform();
			
	}
		
	public void NScale(GestureEvent gEvent){
			
		float multiplier = 0.5f;
	
		float scaleDX = gEvent.Values["scale_dsx"]*multiplier;
		float scaleDY = gEvent.Values["scale_dsy"]*multiplier;

		float cx = Mathf.Clamp(transform.localScale.x+scaleDX, 0.5f, 2f);
		float cy = Mathf.Clamp(transform.localScale.y+scaleDY, 0.5f, 2f);
		float cz = Mathf.Clamp(transform.localScale.z+scaleDY, 0.5f, 2f);
	
		transform.localScale = new Vector3(cx, cy, cz);
			
	}
		
	public void ThreeFingerTilt(GestureEvent gEvent){

		float multiplier = 1.0f;
			
		Camera cam = Camera.main;
		
		float tiltDX = gEvent.Values["tilt_dx"]*multiplier;
		float tiltDY = gEvent.Values["tilt_dy"]*multiplier;
		
		float screenX = gEvent.X;
		float screenY = cam.GetScreenHeight() - gEvent.Y;
		float screenZ = cam.WorldToScreenPoint(this.transform.position).z;
	
		Vector3 touchLocation = cam.ScreenToWorldPoint(new Vector3(screenX, screenY, screenZ));
		
		StartAffineTransform(touchLocation);
		
			AffineTransform.transform.Rotate(Vector3.up * (tiltDX));
			AffineTransform.transform.Rotate(Vector3.right * tiltDY*-1);
		
		EndAffineTransform();
	}

}