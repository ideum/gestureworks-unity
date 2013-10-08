////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Unity
//
//  File:    TouchCircle.cs
//  Authors:  Ideum
//
//  NOTICE: Ideum permits you to use, modify, and distribute this file only
//  in accordance with the terms of the license agreement accompanying it.
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class TouchCircle {

	private int eventId = -1;
	
	private bool showEventInfo = false;
	
	private GUIStyle textStyle = null;
	private GameObject ring = null;
	private Vector3 ringPosition;
	
	private bool isMouse = false;

	public TouchCircle(int eventId, bool showEventInfo, 
						float x, float y, float z = 0.0f,
						bool isMouse = false)
	{	
		this.eventId = eventId;
		this.showEventInfo = showEventInfo; 
		this.isMouse = isMouse;
		
		if(showEventInfo)
		{
			textStyle = new GUIStyle();
			textStyle.normal.textColor = new Color(0/255.0f, 122/255.0f, 157/255.0f);
		}
		
		UnityEngine.Object pointPrefab = null;
		
		if(isMouse)
		{
			pointPrefab = Resources.Load("MousePoint");
		}
		else
		{
			pointPrefab = Resources.Load("FingerPoint");	
		}
		
		if(!pointPrefab)
		{
			Debug.LogWarning("Could not load point prefab");
			return;
		}
		
		ring = (GameObject) UnityEngine.Object.Instantiate(pointPrefab, Vector3.zero, Quaternion.identity);
		ringPosition = new Vector3(x, y, z);
		
		Update(x,y,z);		
	}
	
	public void Update(float x, float y, float z = 0.0f)
	{
		if(!ring)
		{
			return;	
		}
		
		if(!isMouse)
		{
			y = Screen.height - y;
		}
		
		ringPosition.Set(x, y, z);
		ring.transform.position = GestureWorksUnity.Instance.GameCamera.ScreenToViewportPoint(ringPosition);
		
		if(showEventInfo)
		{
			string labelText = "Event ID: " + eventId + "\n";
			labelText += "X: " + Math.Ceiling(x) + " | " + "Y: " + Math.Ceiling(y);
			
			ring.guiText.text = labelText;
		}
		else
		{
			ring.guiText.text = "";	
		}
	}
	
	// Call this just before removing
	public void RemoveRing()  
    {
		if(!ring)
		{
			return;	
		}
		
		UnityEngine.Object.Destroy(ring); 
    }
	
	public void Hide()
	{
		if(!ring)
		{
			return;	
		}
		
		ring.SetActive(false);
	}
	
	public void Show()
	{
		if(!ring)
		{
			return;	
		}
		
		ring.SetActive(true);
	}
}
