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
	
	private GestureWorks Core;

	private string windowName;
	
	private string dllFilePath;
	private string gmlFilePath;
	
	private string dllFileName;
	private string gmlFileName;

	private bool DllLoaded; 
	private bool GmlLoaded;
	private bool WindowLoaded;
	private PointEventArray pEvents;
	
	private TouchPointOverlay _touchPointOverlay;
	private GestureEventArray gEvents;
	private List<TouchObject> GestureObjects;
	private HitManager HitManager;
	
	private bool ShowGui = false;
	private bool HelpVisible = false;
	private string HelpText = "";
	private GUIStyle BoxStyle;
	
	public Transform OriginalClockPosition;
	public Transform OriginalCameraPosition;
	public GameObject ClockObject;
	
	float TimeElapsedSinceNothing = 0.0f;
	public float TimeToReset = 6000.0f;

	// Use this for initialization
	void Start () {

		dllFileName = "GestureworksCore32.dll";
		gmlFileName = "my_gestures.gml";
		
		if(Application.isEditor==true){
			windowName = "Unity - MyScene.unity - unity-gestureworks-clock - PC, Mac & Linux Standalone*";
			dllFilePath = Application.dataPath.Replace("/", "\\")+"\\Plugins\\Gestureworks\\Core\\"+dllFileName;
			gmlFilePath = Application.dataPath.Replace("/", "\\")+"\\MyScripts\\"+gmlFileName;
		} else {
			// Running exe 
			windowName = "InteractiveClock";
			dllFilePath = Application.dataPath.Replace("/", "\\")+"\\"+dllFileName;
			gmlFilePath = Application.dataPath.Replace("/", "\\")+"\\"+gmlFileName;
		}
		
		Core = new GestureWorks();
		DllLoaded = Core.LoadGestureWorksDll(dllFilePath);
		Debug.Log("DllLoaded: "+DllLoaded);
		
		Core.InitializeGestureWorks(Screen.width, Screen.height);
		GmlLoaded = Core.LoadGML(gmlFilePath);
		WindowLoaded = Core.RegisterWindowForTouchByName(windowName);
		
		_touchPointOverlay = new TouchPointOverlay();

		Debug.Log("Interactive Clock");
		Debug.Log("Is DLL Loaded: " + DllLoaded);
		Debug.Log("lIs GML Loaded: " + GmlLoaded.ToString());
		Debug.Log("Is Window Loaded: " + WindowLoaded.ToString());
		
		GestureObjects = new List<TouchObject>();
		HitManager = new HitManager(Camera.main);
		InitializeGestureObjects();
		
		HelpText = "Clock:\nDrag with one finger \nScale/rotate with two fingers\nTilt with 3 fingers\nAdjust minute hand with one finger\n\n";
		HelpText += "Camera:\nOutside of clock pan with one finger\nScale with two fingers";
		
		BoxStyle = new GUIStyle();
		BoxStyle.alignment = TextAnchor.UpperLeft;
		BoxStyle.fontStyle = FontStyle.Normal;

	}
	
	void OnGUI(){
		/*
		if(ShowGui==true && pEvents!=null) { 
			_touchPointOverlay.ShowAll();
		} else {
			_touchPointOverlay.HideAll();
		}
		
		if (GUI.Button (new Rect (Screen.width-360,20, 100, 40), (ShowGui ? "Hide" : "Show" )+" Touch")) {
			if(ShowGui==true) {
				ShowGui = false; 
			} else {
				ShowGui = true;
			}
			ResetTimeElapsedSinceNothing();
		}

		if (GUI.Button (new Rect (Screen.width-240,20, 100, 40), (HelpVisible ? "Hide" : "Show" )+" Help")) {
			if(HelpVisible==true) {
				HelpVisible = false; 
			} else {
				HelpVisible = true;
			}
			ResetTimeElapsedSinceNothing();
		}
		
		if (GUI.Button (new Rect (Screen.width-120,20, 100, 40), "Reset Scene")) {
			ResetScene();
		}
		
		if(HelpVisible){
			GUI.Box(new Rect(Screen.width-360,90, 340, 340),HelpText,BoxStyle);
			
		}
		*/
	}
	
	private void ResetTimeElapsedSinceNothing(){
		TimeElapsedSinceNothing = 1.0f;
	}
	
	private void IncrementTimeElapsedSinceNothing(){
		TimeElapsedSinceNothing = TimeElapsedSinceNothing + ((1/Application.targetFrameRate)*-1);
		if(TimeElapsedSinceNothing>TimeToReset){
			ResetScene();
			Debug.Log("Reset scene automatically");
		}
		//Debug.Log("TimeElapsedSinceNothing: "+TimeElapsedSinceNothing.ToString());
	}
	
	private void ResetScene(){
		ClockObject.transform.position =  new Vector3(0, 0, 0);
		ClockObject.transform.localScale = new Vector3(1, 1, 1);
		ClockObject.transform.rotation = Quaternion.identity;
		
		Camera.main.transform.position =  OriginalCameraPosition.position;
		//Camera.main.transform.rotation =  OriginalCameraPosition.rotation;
		//Camera.main.transform.LookAt(OriginalClockPosition);
		ResetTimeElapsedSinceNothing();
	}
	
	private void InitializeGestureObjects(){
		GameObject[] objects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject obj in objects) {
        	TouchObject script = obj.GetComponent<TouchObject>();
			if(script!=null){
				Debug.Log("I found "+script.GetObjectName()+" a TouchObject.");
				// register object with core
				Core.RegisterTouchObject(script.GetObjectName());
				// and register gestures
				foreach(string gesture in script.SupportedGestures){
					Core.AddGesture(script.GetObjectName(), gesture);
				}
				// keep references to TouchObjects in scene
				GestureObjects.Add(script);
			}
        }
	}

	
	// Update is called once per frame
	void Update () {

		Core.ProcessFrame();
        pEvents = Core.ConsumePointEvents();
		
		if (Input.GetKey ("escape")) {
        	Application.Quit();
		}
		
		if (Input.GetKeyDown(KeyCode.G)){
			if(ShowGui){ ShowGui = false; } else { ShowGui = true; }
		}
		
		if(pEvents!=null && gEvents!=null){
			if(pEvents.Count>0 && gEvents.Count>0){
				ResetTimeElapsedSinceNothing();
			} else {
				IncrementTimeElapsedSinceNothing();
			}
		}

		if(pEvents!=null){
			
			_touchPointOverlay.Update(pEvents, ShowGui);

			foreach(PointEvent pEvent in pEvents){
				if(pEvent.Status == TouchStatus.TOUCHADDED){
					
					bool touchPointHitSomething = false;
					foreach(TouchObject obj in GestureObjects){
						if(HitManager.DetectHit(pEvent.Position.X, Camera.main.GetScreenHeight()-pEvent.Position.Y, obj.gameObject)){
							Core.AddTouchPoint(obj.GetObjectName(), pEvent.PointId);
							
							touchPointHitSomething = true;
							//Debug.Log("I hit "+obj.GetObjectName()+" pEvent.PointId"+pEvent.PointId);
						} 
					}
					if(touchPointHitSomething==false){
						Core.AddTouchPoint(Camera.main.name, pEvent.PointId);
					}
				}
			}
		}
		
		gEvents = Core.ConsumeGestureEvents();
				
		if(gEvents!=null){
			
			foreach (GestureEvent gEvent in gEvents){
					
				// send gesture events to all subscribers
				foreach(TouchObject obj in GestureObjects){
					//send events only to corresponding registered touch objects
					//Debug.Log(gEvent.GestureID+" gesture and for "+gEvent.Target);
					if(obj.name==gEvent.Target){
						//Debug.Log(gEvent.GestureID+" gesture found for "+gEvent.Target);
						obj.SendMessage(gEvent.GestureID, gEvent);
					}
				}
			}
		} else {
			//Debug.Log("gEvents null");
		}
	}
}
