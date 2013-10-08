////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Unity
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
	
		private RaycastHit Hit = new RaycastHit();
		
		public bool DetectHitSingle(float screenPointX, float screenPointY, out string objectGestureName)
		{
			Camera camera = GestureWorksUnity.Instance.GameCamera;
			
			Vector3 fingerPosition = new Vector3(screenPointX, screenPointY, 0.0f);
			
			Ray ray = camera.ScreenPointToRay(fingerPosition);
			
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
			
			Camera camera = GestureWorksUnity.Instance.GameCamera;
			
			Vector3 fingerPosition = new Vector3(screenPointX, screenPointY, 0.0f);
			
			Ray ray = camera.ScreenPointToRay(fingerPosition);
			
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