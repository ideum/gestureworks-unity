////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  GestureWorks Unity
//
//  File:    TouchObject.cs
//  Authors:  Ideum
//
//  NOTICE: Ideum permits you to use, modify, and distribute this file only
//  in accordance with the terms of the license agreement accompanying it.
//
////////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Inherit this class and define your own gesture reponse handlers
/// </summary>

using UnityEngine;
using System.Collections;

namespace GestureWorksCoreNET.Unity {

	public abstract class TouchObject : MonoBehaviour {
		
		public string[] SupportedGestures;
		
		protected float Flipped = -1.0f; // flip value multiplier
		
		private static int gestureIdCounter = 0;
		private string gestureObjectName = "";
		
		public string GestureObjectName {
			get { 
				
				if(string.IsNullOrEmpty(gestureObjectName)) {
					gestureObjectName = this.gameObject.name + "_" + gestureIdCounter++;	
				}
				
				return gestureObjectName; 
			}
		}
		
		void Start(){
			
		}
		
		void Update(){

		}
		
		public void MoveObjectInCameraPlane(GestureEvent gEvent)
		{
			Camera cam = GestureWorksUnity.Instance.GameCamera;
			
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
			
			Plane plane = new Plane(planes[5].normal, gameObject.transform.position);
			
			float dX = gEvent.Values["drag_dx"];
		    float dY = gEvent.Values["drag_dy"] * Flipped;
			
			if(dX == 0.0f && dY == 0.0f)
			{
				return;
			}
			
			if(gEvent.X == 0.0f && gEvent.Y == 0.0f)
			{
				return;	
			}
		        
		    float screenX = gEvent.X;
		    float screenY = Screen.height - gEvent.Y;
			
			Ray dragRay = cam.ScreenPointToRay(new Vector3(screenX, screenY, 0.0f));
			
			float dragEnter;
			if(!plane.Raycast(dragRay, out dragEnter))
			{
				return;	
			}
			
			Vector3 newPosition = dragRay.origin + dragRay.direction * dragEnter;
			
			Ray prevRay = cam.ScreenPointToRay(new Vector3(screenX - dX, screenY - dY, 0.0f));
			
			float prevEnter;
			if(!plane.Raycast(prevRay, out prevEnter))
			{
				return;	
			}
			
			Vector3 prevPosition = prevRay.origin + prevRay.direction * prevEnter;
			
			gameObject.transform.Translate(newPosition - prevPosition);
		}
		
	}
}