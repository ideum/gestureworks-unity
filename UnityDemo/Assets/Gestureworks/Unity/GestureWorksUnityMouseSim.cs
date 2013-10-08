////////////////////////////////////////////////////////////////////////////////
//
//  IDEUM
//  Copyright 2011-2013 Ideum
//  All Rights Reserved.
//
//  Gestureworks Unity
//
//  File:    GestureWorksUnityMouseSim.cs
//  Authors:  Ideum
//
//  NOTICE: Ideum permits you to use, modify, and distribute this file only
//  in accordance with the terms of the license agreement accompanying it.
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

public class GestureWorksUnityMouseSim {
	
	private bool initialized = false;
	
	private GestureWorks gestureWorksCore = null;
	
	private bool showMousePoints = true;
	public bool ShowMousePoints
	{
		get { return showMousePoints; }
		set { showMousePoints = value; }
	}
	
	private bool showMouseEventInfo = false;
	public bool ShowMouseEventInfo
	{
		get { return showMouseEventInfo; }
		set { showMouseEventInfo = value; }
	}
	
	private const int LeftMouseBaseId = 9000;
	private const int LeftMouseLimitId = 9999;
	
	private const int RightAMouseBaseId = 10000;
	private const int RightAMouseLimitId = 10999;
	
	private const int RightBMouseBaseId = 11000;
	private const int RightBMouseLimitId = 11999;
	
	private int leftMouseEventId = LeftMouseBaseId;
	private int rightAMouseEventId = RightAMouseBaseId;
	private int rightBMouseEventId = RightBMouseBaseId;
	
	private Vector3 rightMouseBasePosition = Vector3.zero;
	
	private Vector3 prevMousePosition = Vector3.zero;
	
	private bool DragRightMouse
	{
		get
		{
			return Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftAlt);
		}
	}
	
	private const float RightMinDistance = 32.0f;
	
	private bool mouseCirclesEnabled = true;
	public bool MouseCirclesEnabled
	{
		get { return mouseCirclesEnabled; }
		set { mouseCirclesEnabled = value; }	
	}
	
	private Dictionary<int, TouchCircle> mouseCircles = new Dictionary<int, TouchCircle>();
	
	private List<int> touchDownEvents = new List<int>();
	
	private Camera GameCamera
	{
		get
		{
			return GestureWorksUnity.Instance.GameCamera;	
		}
	}
	
	public GestureWorksUnityMouseSim()
	{
		
	}
	
	public void Initialize(GestureWorks gestureWorksCore)
	{
		this.gestureWorksCore = gestureWorksCore;
		
		initialized = true;
	}
	
	public void ClearMousePoints()
	{
		foreach(TouchCircle circle in mouseCircles.Values)
		{
			circle.RemoveRing();	
		}
		
		mouseCircles.Clear();
		
		// For all remaining touch down events GestureWorks
		// needs a touch up
		foreach(int touchDown in touchDownEvents)
		{
			TouchEvent rem_mouseEvent = new TouchEvent();
			rem_mouseEvent.TouchEventID = touchDown;
			rem_mouseEvent.Status = TouchStatus.TOUCHREMOVED;
			gestureWorksCore.AddEvent(rem_mouseEvent);
		}
	}
	
	public void Update()
	{
		if(!initialized)
		{
			return;	
		}
		
		// Mouse DOWN LEFT
		if(Input.GetMouseButtonDown(0)) 
		{
			Vector2 mousePos = GameCamera.ScreenToViewportPoint(Input.mousePosition);
			TouchEvent new_mouseEvent = new TouchEvent();
			new_mouseEvent.TouchEventID = leftMouseEventId;
			new_mouseEvent.Status = TouchStatus.TOUCHADDED;
			SetTouchEventPosition(mousePos, ref new_mouseEvent);
			
			gestureWorksCore.AddEvent(new_mouseEvent);
			
			touchDownEvents.Add(leftMouseEventId);
			
			if(MouseCirclesEnabled)
			{
				TouchCircle mouseCircle = new TouchCircle(leftMouseEventId,
														  ShowMouseEventInfo,
														  Input.mousePosition.x,
														  Input.mousePosition.y,
														  Input.mousePosition.z,
														  true);
				
				mouseCircles.Add(leftMouseEventId, mouseCircle);
			}
		}
		
		// Mouse DRAG LEFT
		if(Input.GetMouseButton(0)) 
		{
			Vector2 mousePos = GameCamera.ScreenToViewportPoint(Input.mousePosition);
			TouchEvent updt_mouseEvent = new TouchEvent();
			updt_mouseEvent.TouchEventID = leftMouseEventId;
			updt_mouseEvent.Status = TouchStatus.TOUCHUPDATE;
			SetTouchEventPosition(mousePos, ref updt_mouseEvent);
			
			gestureWorksCore.AddEvent(updt_mouseEvent);
			
			if(MouseCirclesEnabled)
			{
				if(mouseCircles.ContainsKey(leftMouseEventId))
				{
					mouseCircles[leftMouseEventId].Update(Input.mousePosition.x,
													  Input.mousePosition.y,
													  Input.mousePosition.z);	
				}
			}
		}
		
		// Mouse UP LEFT
		if(Input.GetMouseButtonUp(0)) 
		{
			Vector2 mousePos = GameCamera.ScreenToViewportPoint(Input.mousePosition);
			TouchEvent rem_mouseEvent = new TouchEvent();
			rem_mouseEvent.TouchEventID = leftMouseEventId;
			rem_mouseEvent.Status = TouchStatus.TOUCHREMOVED;
			SetTouchEventPosition(mousePos, ref rem_mouseEvent);
			
			gestureWorksCore.AddEvent(rem_mouseEvent);
			
			if(MouseCirclesEnabled)
			{
				if(mouseCircles.ContainsKey(leftMouseEventId))
				{
					mouseCircles[leftMouseEventId].RemoveRing();
					mouseCircles.Remove(leftMouseEventId);
				}
			}
			
			touchDownEvents.Remove(leftMouseEventId);
			
			leftMouseEventId++;
			
			if(leftMouseEventId > LeftMouseLimitId)
				leftMouseEventId = LeftMouseBaseId;
		}
		
		// Mouse DOWN RIGHT
		if(Input.GetMouseButtonDown(1))
		{
			rightMouseBasePosition = Input.mousePosition;
			prevMousePosition = Input.mousePosition;
			Vector3 pointA, pointB;
			FindRightMousePositions(Input.mousePosition, out pointA, out pointB);
			
			Vector2 mousePosA = GameCamera.ScreenToViewportPoint(pointA);
			TouchEvent new_mouseEventA = new TouchEvent();
			new_mouseEventA.TouchEventID = rightAMouseEventId;
			new_mouseEventA.Status = TouchStatus.TOUCHADDED;
			SetTouchEventPosition(mousePosA, ref new_mouseEventA);
			gestureWorksCore.AddEvent(new_mouseEventA);
			
			Vector2 mousePosB = GameCamera.ScreenToViewportPoint(pointB);
			TouchEvent new_mouseEventB = new TouchEvent();
			new_mouseEventB.TouchEventID = rightBMouseEventId;
			new_mouseEventB.Status = TouchStatus.TOUCHADDED;
			SetTouchEventPosition(mousePosB, ref new_mouseEventB);
			gestureWorksCore.AddEvent(new_mouseEventB);
			
			touchDownEvents.Add(rightAMouseEventId);
			touchDownEvents.Add(rightBMouseEventId);
			
			if(MouseCirclesEnabled)
			{
				TouchCircle mouseCircleA = new TouchCircle(rightAMouseEventId,
															ShowMouseEventInfo,
														  	pointA.x,
														  	pointA.y,
														  	pointA.z,
															true);
				
				mouseCircles.Add(rightAMouseEventId, mouseCircleA);
				
				TouchCircle mouseCircleB = new TouchCircle(rightBMouseEventId,
															ShowMouseEventInfo,
														  	pointB.x,
														  	pointB.y,
														  	pointB.z,
															true);
				
				mouseCircles.Add(rightBMouseEventId, mouseCircleB);
			}
		}
		
		// Mouse DRAG RIGHT
		if(Input.GetMouseButton(1))
		{	
			if(DragRightMouse)
			{	
				Vector3 delta = Input.mousePosition - prevMousePosition;
				
				rightMouseBasePosition += delta;
			}
			
			prevMousePosition = Input.mousePosition;
			
			Vector3 pointA, pointB;
			FindRightMousePositions(Input.mousePosition, out pointA, out pointB);
			
			Vector2 mousePosA = GameCamera.ScreenToViewportPoint(pointA);
			TouchEvent updt_mouseEventA = new TouchEvent();
			updt_mouseEventA.TouchEventID = rightAMouseEventId;
			updt_mouseEventA.Status = TouchStatus.TOUCHUPDATE;
			SetTouchEventPosition(mousePosA, ref updt_mouseEventA);
			
			gestureWorksCore.AddEvent(updt_mouseEventA);
			
			Vector2 mousePosB = GameCamera.ScreenToViewportPoint(pointB);
			TouchEvent updt_mouseEventB = new TouchEvent();
			updt_mouseEventB.TouchEventID = rightBMouseEventId;
			updt_mouseEventB.Status = TouchStatus.TOUCHUPDATE;
			SetTouchEventPosition(mousePosB, ref updt_mouseEventB);
			
			gestureWorksCore.AddEvent(updt_mouseEventB);
			
			if(MouseCirclesEnabled)
			{
				if(mouseCircles.ContainsKey(rightAMouseEventId))
				{
					mouseCircles[rightAMouseEventId].Update(pointA.x,
													  		pointA.y,
													  		pointA.z);	
				}
				
				if(mouseCircles.ContainsKey(rightBMouseEventId))
				{
					mouseCircles[rightBMouseEventId].Update(pointB.x,
													  		pointB.y,
													  		pointB.z);	
				}
			}
		}
		
		// Mouse UP RIGHT
		if(Input.GetMouseButtonUp(1))
		{
			Vector3 pointA, pointB;
			FindRightMousePositions(Input.mousePosition, out pointA, out pointB);
			
			Vector2 mousePosA = GameCamera.ScreenToViewportPoint(pointA);
			TouchEvent rem_mouseEventA = new TouchEvent();
			rem_mouseEventA.TouchEventID = rightAMouseEventId;
			rem_mouseEventA.Status = TouchStatus.TOUCHREMOVED;
			SetTouchEventPosition(mousePosA, ref rem_mouseEventA);
			
			gestureWorksCore.AddEvent(rem_mouseEventA);
			
			Vector2 mousePosB = GameCamera.ScreenToViewportPoint(pointB);
			TouchEvent rem_mouseEventB = new TouchEvent();
			rem_mouseEventB.TouchEventID = rightBMouseEventId;
			rem_mouseEventB.Status = TouchStatus.TOUCHREMOVED;
			SetTouchEventPosition(mousePosB, ref rem_mouseEventB);
			
			gestureWorksCore.AddEvent(rem_mouseEventB);
			
			if(MouseCirclesEnabled)
			{
				if(mouseCircles.ContainsKey(rightAMouseEventId))
				{
					mouseCircles[rightAMouseEventId].RemoveRing();
					mouseCircles.Remove(rightAMouseEventId);
				}
				
				if(mouseCircles.ContainsKey(rightBMouseEventId))
				{
					mouseCircles[rightBMouseEventId].RemoveRing();
					mouseCircles.Remove(rightBMouseEventId);
				}
			}
			
			touchDownEvents.Remove(rightAMouseEventId);
			touchDownEvents.Remove(rightBMouseEventId);
		
			rightAMouseEventId++;
			if(rightAMouseEventId > RightAMouseLimitId)
				rightAMouseEventId = RightAMouseBaseId;
			
			rightBMouseEventId++;
			if(rightBMouseEventId > RightBMouseLimitId)
				rightBMouseEventId = RightBMouseLimitId;
		}
	}
	
	private static void SetTouchEventPosition(Vector2 mousePosition, ref TouchEvent touchEvent)
	{
		touchEvent.X = mousePosition.x;
		touchEvent.Y = (1.0f - mousePosition.y);
		touchEvent.Z = 0.0f;
		touchEvent.W = 1.0f;
		touchEvent.H = 1.0f;
		touchEvent.R = 0.0f;	
	}
	
	private void FindRightMousePositions(Vector3 mousePosition, out Vector3 pointA, out Vector3 pointB)
	{
		if(mousePosition == rightMouseBasePosition)
		{
			pointA = new Vector3(mousePosition.x + RightMinDistance, mousePosition.y, 0.0f);
			pointB = new Vector3(mousePosition.x - RightMinDistance, mousePosition.y, 0.0f);
			return;
		}
		
		// Currently point A matches new mouse
		pointA = new Vector3(mousePosition.x, mousePosition.y, 0.0f);
		
		Vector3 directionToMouseBase = rightMouseBasePosition - mousePosition;
		float distance = directionToMouseBase.magnitude;
		distance = Mathf.Max(distance, RightMinDistance);
		
		directionToMouseBase.Normalize();
		
		Vector3 newPosition = rightMouseBasePosition + directionToMouseBase * distance;
	
		pointB = new Vector3(newPosition.x, newPosition.y, 0.0f);
	}
}
