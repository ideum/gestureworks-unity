using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class SwitchSceneDrag : TouchObject {
	
	bool loadingLevel = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void NDrag(GestureEvent gEvent)
	{
		if(loadingLevel)
			return;
		
		loadingLevel = true;
		
		Debug.Log("Loading level GestureWorksUnity.Instance.SwitchScenes");
		
		((GestureworksScript)FindObjectOfType(typeof(GestureworksScript))).SwitchScenes("UnityTestSwitchScene");	
	}
}
