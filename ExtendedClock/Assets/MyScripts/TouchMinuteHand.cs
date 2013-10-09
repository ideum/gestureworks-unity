////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Core
//
//  File:    TouchMinuteHand.cs
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

public class TouchMinuteHand : TouchObject {
	
	public Transform HourHand;
	
	float _x0;
	float _y0;
	
	// prev
	float _x1;
	float _y1;
	
	// now
	float _x2;
	float _y2;
	
	bool init_previous;
	
	void Start () {
		_x0 = Camera.main.WorldToScreenPoint(transform.position).x;
		_y0 = Camera.main.WorldToScreenPoint(transform.position).y;
		init_previous = false;
	}
	
	void Update () {
		
	}
		
	public void NDrag(GestureEvent gEvent){
		//transform.renderer.material.color = Color.blue;
		// store initial value if haven't already
		if(init_previous==false){
			_x1 = gEvent.X;
			_y1 = gEvent.Y;
			init_previous = true;
		}
		
		float _dx1 = _x1-_x0;
		float _dy1 = _y1-_y0;
		
		_x2 = gEvent.X;
		_y2 = gEvent.Y;
		
		float _dx2 = _x2-_x0;
		float _dy2 = _y2-_y0;
		
		
		float _theta1 = Mathf.Atan(_dy1/_dx1);
		float _theta2 = Mathf.Atan(_dy2/_dx2);
		
		float _dTheta = _theta1 - _theta2;
		
		// convert delta of theta to delta of degrees
		float _dDegrees = (float) (_dTheta * 57.2957795)*-1;
		
		Debug.Log("_dTheta: "+_dTheta);
		Debug.Log("_dDegrees: "+_dDegrees);

		// and finally rotate clock hand
		if(_dTheta < 0.2 &&_dTheta > -0.2){
			transform.Rotate(0, 0, _dDegrees);
			//Debug.Log("_dTheta: "+_dTheta);
			//Debug.Log("_dDegrees: "+_dDegrees);
		
			// and effect hour hand too
			HourHand.transform.Rotate(0, 0, _dDegrees/12);
		}
		
		// where we were
		_x1 = gEvent.X;
		_y1 = gEvent.Y;

	}

}