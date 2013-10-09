////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Core
//
//  File:    Main.cs
//  Authors:  Ideum
//
//  NOTICE: Ideum permits you to use, modify, and distribute this file only
//  in accordance with the terms of the license agreement accompanying it.
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class Main : MonoBehaviour {
	
	public Transform OriginalClockPosition;
	public Transform OriginalCameraPosition;
	public GameObject ClockObject;
	
	public float TimeToReset = 3.0f;

	// Use this for initialization
	void Start () {
	}
	
	private void ResetScene(){
		
		if(OriginalClockPosition == null || OriginalCameraPosition == null ||
			ClockObject == null)
		{
			return;	
		}
		
		ClockObject.transform.position =  new Vector3(0, 0, 0);
		ClockObject.transform.localScale = new Vector3(1, 1, 1);
		ClockObject.transform.rotation = Quaternion.identity;
		
		Camera.main.transform.position =  OriginalCameraPosition.position;
		GestureWorksUnity.Instance.ResetTimeSinceLastEvent();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(GestureWorksUnity.Instance.TimeSinceLastEvent >= TimeToReset) {
			ResetScene();
			Debug.Log("Resetting scene");
		}
	}
}
