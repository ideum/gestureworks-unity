////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  GestureWorks Unity
//
//  File:    GestureWorksScript.cs
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

public class GestureWorksScript : MonoBehaviour {
	
	/// <summary>
	/// Show touch points dots onscreen.
	/// </summary>
	public bool ShowTouchPoints = true;
	
	/// <summary>
	/// If showing touch points, also show touch points event info.
	/// </summary>
	public bool ShowTouchPointsEventInfo = true;
	
	/// <summary>
	/// Show mouse points onscreen.
	/// </summary>
	public bool ShowMousePoints = true;
	
	/// <summary>
	/// If showing mouse points, also show mouse points event info.
	/// </summary>
	public bool ShowMousePointsEventInfo = true;
	
	/// <summary>
	/// The force mouse sim enabled at runtime (by default mouse sim is disabled
	/// except in the Unity editor.
	/// </summary>
	public bool ForceMouseSimEnabled = false;
	
	/// <summary>
	/// The mouse two point sim start distance.
	/// </summary>
	public float MouseTwoPointSimStartDistance = 32.0f;
	
	/// <summary>
	/// Have pressing the escape key exit the application.
	/// </summary>
	public bool EscapeKeyExitApplication = false;
	
	/// <summary>
	/// Log details of GestureWorks initialization.
	/// </summary>
	public bool LogInitializationDetails = true;
	
	/// <summary>
	/// Log details of GestureWorks input.
	/// </summary>
	public bool LogInput = false;
	
	/// <summary>
	/// Time in seconds since last touch event
	/// </summary>
	public float TimeSinceLastEvent
	{
		get
		{
			return GestureWorksUnity.Instance.TimeSinceLastEvent;
		}
	}
	
	private const float ThreadPollUpdateDelayInterval = 0.1f;
	
	// Use this for initialization
	void Start () {
		
		GestureWorksUnity.Instance.EscapeKeyExitApplication = EscapeKeyExitApplication;
		GestureWorksUnity.Instance.LogInitialization = LogInitializationDetails;
		GestureWorksUnity.Instance.LogInputEnabled = LogInput;
		GestureWorksUnity.Instance.ForceMouseSimEnabled = ForceMouseSimEnabled;
		GestureWorksUnity.Instance.MouseTwoPointSimStartDistance = MouseTwoPointSimStartDistance;
		
		if(!GestureWorksUnity.Instance.Initialized) {
			GestureWorksUnity.Instance.Initialize();
		}
		else {
			GestureWorksUnity.Instance.RegisterTouchObjects();
		}
		
		GestureWorksUnity.Instance.ShowTouchPoints = ShowTouchPoints;
		GestureWorksUnity.Instance.ShowTouchEventInfo = ShowTouchPointsEventInfo;
		GestureWorksUnity.Instance.ShowMousePoints = ShowMousePoints;
		GestureWorksUnity.Instance.ShowMouseEventInfo = ShowMousePointsEventInfo;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(GestureWorksUnity.Instance.MouseSimEnabled) {
			GestureWorksUnity.Instance.MouseTwoPointSimStartDistance = MouseTwoPointSimStartDistance;	
		}
		
		GestureWorksUnity.Instance.Update(Time.deltaTime);
	}
	
	void OnApplicationQuit() {
		
		GestureWorksUnity.Instance.PauseGestureProcessing = true;
		
		GestureWorksUnity.Instance.DeregisterAllTouchObjects();
		
		GestureWorksUnity.Instance.ClearTouchPoints();
	}
	
	public void SwitchScenes(string newSceneName) {
		
		StartCoroutine(UnloadScene(true, newSceneName));
	}
	
	public void SwitchScenes(int index) {
		
		StartCoroutine(UnloadScene(true, "", index));
	}
	
	public IEnumerator UnloadScene(bool switchToNewScene = false, 
									string newSceneName = "", 
									int newSceneIndex = -1) {
		
		GestureWorksUnity.Instance.Loaded = false;
		GestureWorksUnity.Instance.ProcessingGestures = false;
		if(GestureWorksUnity.Instance.ProcessingGestures)
		{
			while(GestureWorksUnity.Instance.ProcessingGestures)
			{
				yield return new WaitForSeconds(ThreadPollUpdateDelayInterval);
			}	
		}
		else
		{
			yield return new WaitForSeconds(0.0f);	
		}
		
		GestureWorksUnity.Instance.DeregisterAllTouchObjects();
		
		GestureWorksUnity.Instance.ClearTouchPoints();
		
		if(switchToNewScene)
		{
			if(newSceneIndex != -1)
			{
				Application.LoadLevel(newSceneIndex);	
			}
			else if(!string.IsNullOrEmpty(newSceneName))
			{
				Application.LoadLevel(newSceneName);	
			}
		}
	}
	
	public void RegisterTouchObject(TouchObject obj) {
		
		StartCoroutine(RegisterTouchObjectRoutine(obj));	
	}
	
	public IEnumerator RegisterTouchObjectRoutine(TouchObject obj) {
		
		GestureWorksUnity.Instance.PauseGestureProcessing = true;
		
		if(GestureWorksUnity.Instance.ProcessingGestures)
		{
			while(GestureWorksUnity.Instance.ProcessingGestures)
			{
				yield return new WaitForSeconds(ThreadPollUpdateDelayInterval);
			}	
		}
		else
		{
			yield return new WaitForSeconds(0.0f);	
		}
		
		Debug.Log ("Registering touch object " + obj.GestureObjectName);
		
		GestureWorksUnity.Instance.RegisterTouchObject(obj);
		
		GestureWorksUnity.Instance.PauseGestureProcessing = false;
	}
	
	public void DeregisterTouchObject(TouchObject obj) {
		
		if(!obj)
		{
			return;
		}
		
		StartCoroutine(DeregisterTouchObjectRoutine(obj, false));
	}
	
	public void DeregisterAndDestroyTouchObject(TouchObject obj) {
		
		if(!obj)
		{
			return;
		}
		
		StartCoroutine(DeregisterTouchObjectRoutine(obj, true));
	}
	
	public IEnumerator DeregisterTouchObjectRoutine(TouchObject obj, bool destroyWhenComplete) {
		
		GestureWorksUnity.Instance.PauseGestureProcessing = true;
		
		if(GestureWorksUnity.Instance.ProcessingGestures)
		{
			while(GestureWorksUnity.Instance.ProcessingGestures)
			{
				yield return new WaitForSeconds(ThreadPollUpdateDelayInterval);
			}	
		}
		else
		{
			yield return new WaitForSeconds(0.0f);	
		}
		
		GestureWorksUnity.Instance.DeregisterTouchObject(obj);
		
		GestureWorksUnity.Instance.PauseGestureProcessing = false;
		
		if(destroyWhenComplete)
		{
			Destroy(obj.gameObject);	
		}
	}
}
