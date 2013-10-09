using UnityEngine;
using System.Collections;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class SwitchScenes : TouchObject {
	
	public GestureWorksScript gestureWorks;
	
	private bool switchedScenes = false;
	
	public void NDrag(GestureEvent gEvent)
	{
		Switch();
	}
	
	public void Tap(GestureEvent gEvent)
	{		
		Switch();
	}
		
	private void Switch()
	{
		if(switchedScenes || gestureWorks == null)
		{
			return;	
		}
		
		switchedScenes = true;
		
		gestureWorks.SwitchScenes("MySceneB");
	}
}
