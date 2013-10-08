////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Unity
//
//  File:    GestureworksScript.cs
//  Authors:  Ideum
//
//  NOTICE: Ideum permits you to use, modify, and distribute this file only
//  in accordance with the terms of the license agreement accompanying it.
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class GestureworksScript : MonoBehaviour {
	
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
	/// Log details of GestureWorks initialization.
	/// </summary>
	public bool LogInitializationDetails = true;
	
	/// <summary>
	/// Log details of GestureWorks input.
	/// </summary>
	public bool LogInput = false;
	
	// Use this for initialization
	void Start () {
		
		GestureWorksUnity.Instance.LogInitialization = LogInitializationDetails;
		GestureWorksUnity.Instance.LogInputEnabled = LogInput;
		GestureWorksUnity.Instance.ForceMouseSimEnabled = ForceMouseSimEnabled;
		
		if(!GestureWorksUnity.Instance.Initialized) {
			GestureWorksUnity.Instance.Initialize();
		}
		else {
			GestureWorksUnity.Instance.RegisterGestureObjects();
		}
		
		GestureWorksUnity.Instance.ShowTouchPoints = ShowTouchPoints;
		GestureWorksUnity.Instance.ShowTouchEventInfo = ShowTouchPointsEventInfo;
		GestureWorksUnity.Instance.ShowMousePoints = ShowMousePoints;
		GestureWorksUnity.Instance.ShowMouseEventInfo = ShowMousePointsEventInfo;
	}
	
	// Update is called once per frame
	void Update () {
		GestureWorksUnity.Instance.Update();
	}
	
	void OnApplicationQuit() {
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
				yield return new WaitForSeconds(0.1f);
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
}
