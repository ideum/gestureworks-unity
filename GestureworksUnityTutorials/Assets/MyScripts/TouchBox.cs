using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class TouchBox : TouchObject {
	
	public void NDrag(GestureEvent gEvent){
		MoveObjectInCameraPlane(gEvent);	
	}
}
