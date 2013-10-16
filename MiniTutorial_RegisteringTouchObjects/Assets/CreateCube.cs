using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class CreateCube : TouchObject {
	
	public GameObject testPrefab;
	
	public GestureWorksScript gestureWorks;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Tap(GestureEvent gEvent) {

		Object[] spheres = FindObjectsOfType(typeof(TapSphere));
		
		Object obj = Instantiate(testPrefab, 
								 transform.position + new Vector3((spheres.Length + 1) * 1.5f, 0.0f, 0.0f),
								 Quaternion.identity);
		
		TouchObject touchObj = ((GameObject)obj).GetComponent<TouchObject>();
		
		if(touchObj && gestureWorks) {
			
			gestureWorks.RegisterTouchObject(touchObj);	
		}
	}
}
