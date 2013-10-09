////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Core
//
//  File:    TouchCamera.cs
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

public class TouchCamera : TouchObject {
	
	public void NDrag(GestureEvent gEvent){
		
		float multiplier = 0.001f;
			
		Camera cam = Camera.main;

		float dX = gEvent.Values["drag_dx"]*multiplier;
		float dY = gEvent.Values["drag_dy"]*multiplier;
		
		Vector3 previousPosition = cam.WorldToScreenPoint(transform.position);
		Vector3 nextPosition = new Vector3(dX, dY, 0.0f);
			
		Vector3 newPosition = previousPosition + nextPosition;
		
		float cx = Mathf.Clamp(cam.ScreenToWorldPoint(newPosition).x+dX, -2f, 2f);
		float cy = Mathf.Clamp(cam.ScreenToWorldPoint(newPosition).y+dY, 0f, 2f);
	
		transform.position = new Vector3(cx,cy,transform.position.z);

	}
		
	public void NScale(GestureEvent gEvent){

		float multiplier = 0.5f;
	
		float scaleDX = gEvent.Values["scale_dsx"]*multiplier;
		float scaleDY = gEvent.Values["scale_dsy"]*multiplier;

		float cz = Mathf.Clamp(transform.position.z+(scaleDX+scaleDY)*Flipped, 0.1f, 4.5f);

		transform.position = new Vector3(transform.position.x, transform.position.y, cz);
	
		
	}

}
