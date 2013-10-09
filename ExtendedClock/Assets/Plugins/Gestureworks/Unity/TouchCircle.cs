////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Core
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

/// <summary>
/// Class for touch point display, depends on Prefabs/FingerPoint
/// </summary>

namespace GestureWorksCoreNET.Unity {
	
	public class TouchCircle {
		
		private int Id;
		private float X,Y,Z;
		
		private GUIStyle TextStyle;
		private GameObject Ring;
		private Vector3 ringPosition;
		
		private string LabelText;
	
		public TouchCircle(int id, float x, float y)
		{
			float z = 0;
			Id = id;
			
			TextStyle = new GUIStyle();
			TextStyle.normal.textColor = new Color(0/255.0F, 122/255.0F, 157/255.0F);
			
			Ring = (GameObject) UnityEngine.Object.Instantiate(Resources.Load("Prefabs/FingerPoint"), Vector3.zero, Quaternion.identity);
			ringPosition = new Vector3(x, y, z);
			
			LabelText = "";
			
			Update(x,y);
			
		}
		
		public void Update(float x, float y)
		{
			float z = 0;
			X = x;
			Y = Camera.main.GetScreenHeight()-y;
			Z = z;
			
			ringPosition.Set(X,Y,Z);
			Ring.transform.position = Camera.main.ScreenToViewportPoint(ringPosition);
			
			LabelText = "ID: " + Id + "\n";
			LabelText += "X: " + Math.Ceiling(X) + " | " + "Y: " + Math.Ceiling(Y);
			
			Ring.guiText.text = LabelText;
			
		}
		
		// Call this just before removing
		public void RemoveRing()  
	    {
			UnityEngine.Object.Destroy(Ring); 
	    }
		
		public void Hide(){
			Ring.SetActive(false);
		}
		
		public void Show(){
			Ring.SetActive(true);
		}
		
	}
}
