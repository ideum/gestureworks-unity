////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Core
//
//  File:    HitManager.cs
//  Authors:  Ideum
//
//  NOTICE: Ideum permits you to use, modify, and distribute this file only
//  in accordance with the terms of the license agreement accompanying it.
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;

namespace GestureWorksCoreNET.Unity {
	
	public class HitManager {
	
		private RaycastHit Hit;
		private static Camera Cam;
		
		public HitManager(Camera asSeenByCamera){
			Hit = new RaycastHit();
			SetHitCamera(asSeenByCamera);
		}
	
		private void SetHitCamera(Camera asSeenByCamera){
			// TODO: Enable more options for cameras
			Cam = asSeenByCamera; 
			
		}
		
		public bool DetectHitSingle(float screenPointX, float screenPointY, out string objectGestureName)
		{
			Vector3 fingerPosition = new Vector3(screenPointX, screenPointY, 0.0f);
			
			Ray ray = Cam.ScreenPointToRay(fingerPosition);
			
			if(Physics.Raycast(ray, out Hit))
			{	
				objectGestureName = Hit.transform.gameObject.name;
				
				TouchObject touchObject = Hit.transform.gameObject.GetComponent<TouchObject>();
				
				if(touchObject)
				{
					objectGestureName = touchObject.GestureObjectName;
					
					return true;
				}
			}
			
			objectGestureName = "";
			
			return false;
		}
		
		public static bool DetectHit(float screenPointX, float screenPointY, GameObject touchableObject, out RaycastHit hit){
			
			Vector3 fingerPosition = new Vector3(screenPointX, screenPointY, 0.0f);
			
			Ray ray = Cam.ScreenPointToRay(fingerPosition);
			
			if(Physics.Raycast(ray, out hit)){
				
				if(hit.transform.gameObject.name == touchableObject.name){
					Debug.DrawLine(ray.origin, hit.point, Color.red);
					return true;
				}
			}
			return false;
	
		}
	}
			
}