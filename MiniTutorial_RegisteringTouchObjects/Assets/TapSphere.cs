using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class TapSphere : TouchObject {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Tap(GestureEvent gEvent) {
		
		renderer.material.color = new Color(Random.Range(0.0f, 0.9f), Random.Range(0.0f, 0.9f), Random.Range(0.0f, 0.9f));	
	}
}
