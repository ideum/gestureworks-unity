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
	
	// Use this for initialization
	void Start () {
		GestureWorksUnity.Instance.Initialize();
		
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
	}
}
