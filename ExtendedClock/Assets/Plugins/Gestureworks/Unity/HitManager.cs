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
		private Camera Cam;
		//private int MaskLayer; // excludes hits to only objects here. 
		//private float RayDistance;
		
		public HitManager(Camera asSeenByCamera){
			Hit = new RaycastHit();
			//RayDistance = Mathf.Infinity;
			//MaskLayer = Physics.kDefaultRaycastLayers;
			SetHitCamera(asSeenByCamera);
		}
	
		private void SetHitCamera(Camera asSeenByCamera){
			// TODO: Enable more options for cameras
			Cam = asSeenByCamera; 
			
		}
		
		public bool DetectHit(float screenPointX, float screenPointY, GameObject touchableObject){
			
			Vector3 fingerPosition = new Vector3(screenPointX, screenPointY, 0.0f);
			
			Ray ray = Cam.ScreenPointToRay(fingerPosition);
			//Debug.DrawRay(ray.origin, ray.direction, Color.green, 1.0f);
			
			if(Physics.Raycast(ray, out Hit)){
				if(Hit.transform.gameObject.name == touchableObject.name){
					//Debug.DrawLine(ray.origin, Hit.point, Color.red);
					return true;
				}
			}
			return false;
	
		}
		
		
	}
			
}