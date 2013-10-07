using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GestureWorksCoreNET;
using GestureWorksCoreNET.Unity;

#if UNITY_EDITOR
	using UnityEditor;
#endif

public class GestureWorksUnity 
{	
	private string dllFileName = "GestureworksCore32.dll";
	public string DllFileName
	{
		get { return dllFileName; }
		set { dllFileName = value; }	
	}
	
	private string dllFilePathEditor = @"\Gestureworks\Core\";
	public string DllFilePathEditor
	{
		get { return dllFilePathEditor; }
		set { dllFilePathEditor = value; }
	}
	
	private string gmlFileName = "my_gestures.gml";
	public string GmlFileName
	{
		get { return gmlFileName; }
		set { gmlFileName = value; }
	}
	
	private string gmlFilePathEditor = @"\Gestureworks\Core\";
	public string GmlFilePathEditor
	{
		get { return gmlFilePathEditor; }
		set { gmlFilePathEditor = value; }	
	}
	
	private string dllFilePath;
	private string gmlFilePath;
	
	private static GestureWorksUnity instance = null;
	public static GestureWorksUnity Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GestureWorksUnity();	
			}
			
			return instance;
		}
	}
	
	private bool initialized = false;
	public bool Initialized
	{
		get { return initialized; }	
	}
	
	private string initializationError = "";
	public string InitializationError
	{
		get 
		{
			if(string.IsNullOrEmpty(initializationError))
			{
				return "";
			}
			
			return "GestureWorks initialization error:\n" + initializationError; 
		}	
	}
	
	private bool mouseSimEnabled = false;
	public bool MouseSimEnabled
	{
		get { return mouseSimEnabled; }	
	}
	
	private GestureWorksUnityMouseSim mouseSimulator = new GestureWorksUnityMouseSim();
	
	private bool forceMouseSimEnabled = false;
	public bool ForceMouseSimEnabled
	{
		set { forceMouseSimEnabled = value; }
		get { return forceMouseSimEnabled; }
	}
	
	public bool ShowMousePoints
	{
		get { return mouseSimulator.ShowMousePoints; }
		set { mouseSimulator.ShowMousePoints = value; }
	}
	
	public bool ShowMouseEventInfo
	{
		get { return mouseSimulator.ShowMouseEventInfo; }
		set { mouseSimulator.ShowMousePoints = value; }
	}
	
	private bool showTouchPoints = true;
	public bool ShowTouchPoints
	{
		get 
		{
			if(MouseSimEnabled)
			{
				return false;	
			}
			
			return showTouchPoints; 
		}
		set { showTouchPoints = value; }
	}
	
	private bool showTouchEventInfo = false;
	public bool ShowTouchEventInfo
	{
		get { return showTouchEventInfo; }
		set { showTouchEventInfo = value; }
	}
	
	private string CurrentSceneName
	{
		get
		{
#if UNITY_EDITOR
			string[] scenePathParts = EditorApplication.currentScene.Split(char.Parse("/"));
			if(scenePathParts.Length <= 0)
			{
				Debug.LogWarning("Could not find current scene name");	
				
				return "";
			}
			
			return scenePathParts[scenePathParts.Length - 1];
#else
			return Application.loadedLevelName;
#endif
		}
	}
	
	private string CurrentSceneNameNoExtension
	{
		get
		{
			string name = CurrentSceneName;
			int unityIndex = CurrentSceneName.IndexOf(".unity");
			if(unityIndex >= 0)
			{
				return name.Remove(unityIndex);	
			}
			else
			{
				return name;	
			}
		}
	}
	
	private string EditorWindowName
	{
		get
		{
			return "Unity - " + CurrentSceneName + " - " + CurrentSceneNameNoExtension + " - PC, Mac & Linux Standalone*";	
		}
	}
	
	private string gameWindowName = "";
	
	public string GameWindowName
	{
		get 
		{
			if(string.IsNullOrEmpty(gameWindowName))
			{
				Debug.LogWarning("GameWindow name not properly set");
				
				return CurrentSceneNameNoExtension;
			}
			
			return gameWindowName; 
		}
		set { gameWindowName = value; }
	}
	
	private string windowName = "";
	
	private List<TouchObject> gestureObjects = new List<TouchObject>();
	
	private GestureWorks gestureWorksCore;
	
	private GestureEventArray gestureEvents = null;
	
	private PointEventArray pointEvents = null;
	
	private Dictionary<int, TouchCircle> touchCircles = new Dictionary<int, TouchCircle>();
	
	private HitManager hitManager;
	
	public bool LogInputEnabled { get; set; }
	
	private GestureWorksUnity()
	{
		LogInputEnabled = false;
	}
	
	public void Initialize()
	{
		if(initialized)
		{
			Debug.LogWarning("Initialize being called more than once on GestureWorksUnity");
			
			return;
		}
		
		if(!FindSetupInfo())
		{
			return;
		}
		
		if(!InitializeGestureworks())
		{
			return;	
		}
		
		initialized = true;
		
		RegisterGestureObjects();
	}
	
	private bool FindSetupInfo()
	{	
		if(Application.isEditor)
		{
			mouseSimEnabled = true;
			windowName = EditorWindowName;
			
			Debug.Log("== NOTE: to touch the demo, you must build and run ==");
			
			dllFilePath = Application.dataPath.Replace("/", "\\") + DllFilePathEditor + DllFileName;
			gmlFilePath = Application.dataPath.Replace("/", "\\") + GmlFilePathEditor + GmlFileName;
		} 
		else 
		{
			// Running exe 
			mouseSimEnabled = ForceMouseSimEnabled;
			windowName = GameWindowName;
			dllFilePath = Application.dataPath.Replace("/", "\\") + "\\" + DllFileName;
			gmlFilePath = Application.dataPath.Replace("/", "\\") + "\\" + GmlFileName;
		}
		
		if(!File.Exists(dllFilePath))
		{
			initializationError = "Could not find dll at " + dllFilePath;
			Debug.LogError(initializationError + " Stopping Gestureworks Initialization");
			return false;
		}
		
		if(!File.Exists(gmlFilePath))
		{
			initializationError = "Could not find gml at " + dllFilePath;
			Debug.LogError(initializationError + " Stopping Gestureworks Initialization");
			return false;
		}
		
		Debug.Log("DLL file path: " + dllFilePath);
		Debug.Log("GML file path: " + gmlFilePath);
		
		return true;
	}
	
	private bool InitializeGestureworks()
	{	
		gestureWorksCore = new GestureWorks();
		
		bool dllLoaded = gestureWorksCore.LoadGestureWorksDll(dllFilePath);
		
		if(!dllLoaded)
		{
			initializationError = "Could not load dll " + dllFilePath;
			Debug.LogError(initializationError + " Stopping Gestureworks Initialization");	
			return false;
		}
		
		gestureWorksCore.InitializeGestureWorks(Screen.width, Screen.height);
		
		bool gmlLoaded = gestureWorksCore.LoadGML(gmlFilePath);
		
		if(!gmlLoaded)
		{
			initializationError = "Could not load gml " + gmlFilePath ;
			Debug.LogError(initializationError + " Stopping Gestureworks Initialization");	
			return false;
		}
		
		bool windowLoaded = gestureWorksCore.RegisterWindowForTouchByName(windowName);
		if(!windowLoaded)
		{
			initializationError = "Could not register window for touch " + windowName;
			Debug.LogError(initializationError + " Stopping Gestureworks Initialization");
			return false;
		}
		
		hitManager = new HitManager(Camera.main);
		
		mouseSimulator.Initialize(gestureWorksCore);
		
		Debug.Log("Success initializing Gestureworks");
		
		return true;
	}
	
	public void RegisterGestureObjects()
	{	
		if(!initialized)
		{
			Debug.LogWarning("Trying to register touch objects when GestureWorksUnity has not been initialized");
			return;
		}
		
		int currentTouchObjectCount = gestureObjects.Count;
		
		TouchObject[] touchObjects = GameObject.FindObjectsOfType(typeof(TouchObject)) as TouchObject[];
		foreach(TouchObject obj in touchObjects)
		{
			RegisterGestureObject(obj);
		}
		
		Debug.Log ("RegisterGestureObjects found " + (gestureObjects.Count - currentTouchObjectCount) + " touch objects");
	}
	
	public bool RegisterGestureObject(TouchObject obj)
	{
		if(!initialized)
		{
			Debug.LogWarning("Trying to register touch object when GestureWorksUnity has not been initialized");
			return false;
		}
		
		if(obj == null)
		{
			return false;	
		}
		
		Debug.Log ("Touch object " + obj.gameObject.name + " found.");
		
		gestureWorksCore.RegisterTouchObject(obj.GestureObjectName);
		
		foreach(string gesture in obj.SupportedGestures)
		{
			Debug.Log ("	Object has gesture " + gesture);	
		
			gestureWorksCore.AddGesture(obj.GestureObjectName, gesture);
		}
		
		gestureObjects.Add(obj);
		
		return true;
	}
	
	public void DeregisterAllTouchObjects()
	{
		TouchObject[] touchObjects = GameObject.FindObjectsOfType(typeof(TouchObject)) as TouchObject[];
		foreach(TouchObject obj in touchObjects)
		{
			DeregisterTouchObject(obj);
		}
	}
	
	public void DeregisterTouchObject(TouchObject obj)
	{
		gestureWorksCore.DeregisterTouchObject(obj.GestureObjectName);
		
		foreach(string gesture in obj.SupportedGestures)
		{	
			gestureWorksCore.DisableGesture(obj.GestureObjectName, gesture);
			gestureWorksCore.RemoveGesture(obj.GestureObjectName, gesture);
		}
		
		gestureObjects.Remove(obj);
	}
	
	public void Update()
	{
		if(!initialized)
		{
			return;
		}
		
		gestureWorksCore.ProcessFrame();
		
		pointEvents = gestureWorksCore.ConsumePointEvents();
		
		if(mouseSimEnabled)
		{
			UpdateMouseEvents();	
		}
		
		if(LogInputEnabled)
		{
			LogPoints();	
		}
		
		UpdateTouchPoints();
		
		gestureEvents = gestureWorksCore.ConsumeGestureEvents();
		
		UpdateGestureEvents();
	}
	
	private void UpdateMouseEvents()
	{
		if(!initialized || !mouseSimEnabled)
		{
			return;	
		}
		
		mouseSimulator.Update();
	}
	
	private void LogPoints()
	{
		if(pointEvents == null)
		{
			return;	
		}
		
		foreach (PointEvent pEvent in pointEvents)
		{
			if (pEvent.Status == TouchStatus.TOUCHADDED)
			{
				string output = "TOUCHADDED-----------------------------\r\n";
				output += "Point ID:  " +    		pEvent.PointId.ToString();
				output += "\r\n X: " +           	pEvent.Position.X.ToString();
				output += "\r\n Y: " +            	pEvent.Position.Y.ToString();
				output += "\r\n W: " +           	pEvent.Position.W.ToString();
				output += "\r\n H: " +            	pEvent.Position.H.ToString();
				output += "\r\n Z: " +            	pEvent.Position.Z.ToString();
				output += "\r\n Touch Status: " + 	pEvent.Status.ToString();
				output += "\r\n Timestamp: \r\n" +  pEvent.Timestamp.ToString();
				
				Debug.Log(output);
			}
		}
	}
	
	private void UpdateTouchPoints()
	{
		if(pointEvents == null)
		{
			return;
		}
		
		foreach(PointEvent pEvent in pointEvents)
		{
			switch(pEvent.Status)
			{
				
			case TouchStatus.TOUCHADDED:
			{
				if(ShowTouchPoints &&
					!touchCircles.ContainsKey(pEvent.PointId))
				{
					touchCircles.Add(pEvent.PointId, 
									new TouchCircle(pEvent.PointId,
													ShowTouchEventInfo,
													pEvent.Position.X, pEvent.Position.Y));	
				}
				
				string hitObjectName = "";
				bool hitSomething = hitManager.DetectHitSingle(pEvent.Position.X,
															   Screen.height - pEvent.Position.Y, 
															   out hitObjectName);
				
				
				bool touchPointHitSomething = false;
				if(hitSomething)
				{
					Debug.Log("Hit " + hitObjectName);
					
					foreach(TouchObject obj in gestureObjects)
					{
						if(obj.GestureObjectName == hitObjectName)
						{
							Debug.Log("Adding touch point to " + obj.GestureObjectName);
							
							gestureWorksCore.AddTouchPoint(obj.GestureObjectName, pEvent.PointId);
							
							touchPointHitSomething = true;
							
							break;
						}
					}
				}
					
				if(!touchPointHitSomething)
				{
					gestureWorksCore.AddTouchPoint(Camera.main.name, pEvent.PointId);
				}
			}
				break;
				
			case TouchStatus.TOUCHREMOVED:
			{
				if(ShowTouchPoints && touchCircles.ContainsKey(pEvent.PointId))
				{
					touchCircles[pEvent.PointId].RemoveRing();
					touchCircles.Remove(pEvent.PointId);
				}
			}
				break;
				
			case TouchStatus.TOUCHUPDATE:
			{
				if(ShowTouchPoints && touchCircles.ContainsKey(pEvent.PointId))
				{
					touchCircles[pEvent.PointId].Update(pEvent.Position.X, pEvent.Position.Y);
				}
			}
				break;
				
			default:
				break;
				
			}
		}
	}
	
	private void UpdateGestureEvents()
	{
		if(gestureEvents == null)
		{
			return;
		}
		
		foreach (GestureEvent gEvent in gestureEvents)
		{
			if(LogInputEnabled)
			{
		        string o = "-----------------------------\r\n";
		        o += gEvent.ToString();
		        o += "EventID: "   + gEvent.EventID;
		        o += "\r\n GestureID: " + gEvent.GestureID;
		        o += "\r\n Target: "    + gEvent.Target;
		        o += "\r\n N: "         + gEvent.N.ToString();
		        o += "\r\n X: "         + gEvent.X.ToString();
		        o += "\r\n Y: "         + gEvent.Y.ToString();
		        o += "\r\n Timestamp: " + gEvent.Timestamp.ToString();
		        o += "\r\n Locked Points: " + gEvent.LockedPoints.GetLength(0).ToString();
				o += "\r\n";
						
				Debug.Log(o);
			}
					
			// send gesture events to all subscribers
			foreach(TouchObject obj in gestureObjects)
			{
				//send events only to corresponding registered touch objects
				if(obj.GestureObjectName == gEvent.Target)
				{
					obj.SendMessage(gEvent.GestureID, gEvent);
				}
			}
		}
	}
}
