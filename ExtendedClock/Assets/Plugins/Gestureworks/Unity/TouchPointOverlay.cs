using UnityEngine;
using System;
using System.Collections.Generic;
using GestureWorksCoreNET;

namespace GestureWorksCoreNET.Unity {

	public class TouchPointOverlay {
		
		private Dictionary<int, TouchCircle> TouchCircles;
		//private bool PointsVisible;
		
		public TouchPointOverlay(){
			
			TouchCircles = new Dictionary<int, TouchCircle>();
			//PointsVisible = false;
	
		}
		
		public void Update(PointEventArray pEvents, bool ShowGui){
			if(pEvents!=null){
	
				foreach(PointEvent pEvent in pEvents){
					if(pEvent.Status == TouchStatus.TOUCHADDED && ShowGui){
						if(!TouchCircles.ContainsKey(pEvent.PointId)){
							TouchCircles.Add(pEvent.PointId, new TouchCircle(pEvent.PointId, pEvent.Position.X, pEvent.Position.Y));
						}
					}
					if(pEvent.Status == TouchStatus.TOUCHREMOVED){
						if(TouchCircles.ContainsKey(pEvent.PointId)){
							TouchCircles[pEvent.PointId].RemoveRing();
							TouchCircles.Remove(pEvent.PointId);
						}
						
					}
					if(pEvent.Status == TouchStatus.TOUCHUPDATE){
						if(TouchCircles.ContainsKey(pEvent.PointId)){
							TouchCircles[pEvent.PointId].Update(pEvent.Position.X, pEvent.Position.Y);
						}
					}
				}
			}
	
		}
		
		public void HideAll(){
			//if(PointsVisible==true){
				foreach(KeyValuePair<int, TouchCircle> kvp in TouchCircles){
					kvp.Value.Hide();
				}
				//PointsVisible = false;
			//}
		}
		
		public void ShowAll(){
			//if(PointsVisible==false){
				foreach(KeyValuePair<int, TouchCircle> kvp in TouchCircles){
					kvp.Value.Show();
				}
				//PointsVisible = true;
			//}
		}
		
	}
	
}