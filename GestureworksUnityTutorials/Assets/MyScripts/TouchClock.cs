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
	
	protected GameObject AffineTransform;
	protected readonly string AffineTransformName = "GW Transform Context";
	protected Transform OriginalParentTransform;
	
	/// <summary>
	/// Starts the transform context. Call this before any value changes occur 
	/// if you want manipulations to start where the center of the gesture happens. 
	/// Important: you must use AffineTransform in the GestureUpdate as the point 
	/// of manipulation. 
	/// </summary>
	public void StartAffineTransform(Vector3 contextLocation){
		if (AffineTransform == null) {
			AffineTransform = new GameObject();
			AffineTransform.name = AffineTransformName;
			AffineTransform.transform.position = contextLocation;	
		}	
		AffineTransform.transform.LookAt(Vector3.forward);
		OriginalParentTransform = this.transform.parent;
		this.transform.parent = null;
		AffineTransform.transform.parent = OriginalParentTransform;
		this.transform.parent = AffineTransform.transform;

	}
	
	/// <summary>
	/// Call this at the end of the contextual gesture manipulation.
	/// </summary>
	public void EndAffineTransform(){
		
		this.transform.parent = OriginalParentTransform;		
		AffineTransform.transform.parent = null;	
		Destroy(AffineTransform);	
		
	}
	
	void Start(){
		//
	}

	void Update(){
		//
	}
	
	public void NDrag(GestureEvent gEvent){
		
		MoveObjectInCameraPlane(gEvent);

		float cx = Mathf.Clamp(transform.position.x, -2.0f, 2.0f);
		float cy = Mathf.Clamp(transform.position.y, 0.0f, 2.0f);
		float cz = 0.0f;
	
		transform.position = new Vector3(cx, cy, cz);
	}
		
	public void NRotate(GestureEvent gEvent){
			
		float multiplier = 0.75f;
		
		Camera cam = Camera.main;
		
		float dTheta = gEvent.Values["rotate_dtheta"]*multiplier;
		
		float screenX = gEvent.X;
		float screenY = Screen.height - gEvent.Y;
		float screenZ = cam.WorldToScreenPoint(this.transform.position).z;
	
		Vector3 touchLocation = cam.ScreenToWorldPoint(new Vector3(screenX, screenY, screenZ));
		
		StartAffineTransform(touchLocation);
		
			AffineTransform.transform.Rotate(0, 0, dTheta);
		
		EndAffineTransform();
	}
		
	public void NScale(GestureEvent gEvent){
			
		float multiplier = 0.005f;
	
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
		float screenY = Screen.height - gEvent.Y;
		float screenZ = cam.WorldToScreenPoint(this.transform.position).z;
	
		Vector3 touchLocation = cam.ScreenToWorldPoint(new Vector3(screenX, screenY, screenZ));
		
		StartAffineTransform(touchLocation);
		
			AffineTransform.transform.Rotate(Vector3.up * (tiltDX));
			AffineTransform.transform.Rotate(Vector3.right * tiltDY*-1);
		
		EndAffineTransform();
	}

}