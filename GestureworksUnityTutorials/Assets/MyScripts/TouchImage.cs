using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class TouchImage : TouchObject {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void NDrag(GestureEvent gEvent){
		
	    float dX = gEvent.Values["drag_dx"];
	    float dY = gEvent.Values["drag_dy"]*Flipped;
	    
		Camera cam = Camera.main;
		
	   	Vector3 previousPosition = cam.WorldToScreenPoint(transform.position);
	   	Vector3 nextPosition = new Vector3(dX, dY, 0.0f);
	   	Vector3 newPosition = previousPosition + nextPosition;
		transform.position = cam.ScreenToWorldPoint(newPosition);
	}
	    
	public void NRotate(GestureEvent gEvent){
	        
	    float dTheta = gEvent.Values["rotate_dtheta"];
		
	    transform.Rotate(0, dTheta, 0);   
	}
	    
	public void NScale(GestureEvent gEvent){
	    
	    const float multiplier = 0.005f;
		const float scaleMin = 0.1f;
		
	    float scaleDX = gEvent.Values["scale_dsx"]*multiplier;
	    float scaleDY = gEvent.Values["scale_dsy"]*multiplier;
		
		Vector3 newScale = transform.localScale + new Vector3(scaleDX, scaleDY, scaleDY);
		newScale.x = Mathf.Max(newScale.x, scaleMin);
		newScale.y = Mathf.Max(newScale.y, scaleMin);
		newScale.z = Mathf.Max(newScale.z, scaleMin);
	    
		transform.localScale = newScale;       
	}
	    
	public void ThreeFingerTilt(GestureEvent gEvent){
	            
	    float tiltDX = gEvent.Values["tilt_dx"];
	    float tiltDY = gEvent.Values["tilt_dy"];

	    transform.Rotate(0,0 , tiltDX * Flipped);
	    transform.Rotate(tiltDY * Flipped,0 , 0);
	}
}
