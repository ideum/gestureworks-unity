using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class RemoveCube : TouchObject {
	
	public GestureWorksScript gestureWorks;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Tap(GestureEvent gEvent) {
		
		TapSphere[] spheres = FindObjectsOfType(typeof(TapSphere)) as TapSphere[];
		
		if(spheres.Length == 0)
		{
			return;
		}
		
		GameObject obj = spheres[0].gameObject;
		
		if(!obj)
		{
			return;
		}
		
		TouchObject touchObj = obj.GetComponent<TouchObject>();
		
		if(touchObj && gestureWorks) {
			
			gestureWorks.DeregisterAndDestroyTouchObject(touchObj);	
		}
	}
}
