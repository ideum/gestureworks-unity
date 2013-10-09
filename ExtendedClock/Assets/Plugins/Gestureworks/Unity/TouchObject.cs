////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Core
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
		
		//TODO: 
		//public bool AffectsChildren = true;
		//public bool AffineTransformationsEnabled = true;
		public string[] SupportedGestures;
		
		protected GameObject AffineTransform;
		protected readonly string AffineTransformName = "GW Transform Context";
		protected Transform OriginalParentTransform;
		
		protected float Flipped = -1.0f; // flip value multiplier
		
		void Start()
		{
			//
		}
		
		void Update(){
			//
		}
		
		public string GetObjectName(){
			return this.gameObject.name; // get name of object this is attached to
		}
		
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
		
		//TODO: Default gesture response handlers for automatic mapping. 
		
	}
}