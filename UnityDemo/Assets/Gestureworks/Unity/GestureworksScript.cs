using UnityEngine;
using System.Collections;

public class GestureworksScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GestureWorksUnity.Instance.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		GestureWorksUnity.Instance.Update();
	}
	
	void OnApplicationQuit() {
		GestureWorksUnity.Instance.DeregisterAllTouchObjects();
	}
}
