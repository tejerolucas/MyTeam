using UnityEngine;
using System.Collections.Generic;

public class togglelist : MonoBehaviour {
		public puntajemanager puntman;
		public int cant;
		public List<toggleicon> toggles=new List<toggleicon>(); 
		public bool ready;
	
		void Start(){
				ready=false;
				cant=0;
		}


		public void SetInt (int num)
	{
		num--;
		for (int i = 0; i < toggles.Count; i++) {
				if(i<=num){
					toggles[i].ToggleOn();	
				}else{
					toggles[i].ToggleOff();
				}
			}
	}

		public void TurnOn (toggleicon go) {
			int num=toggles.IndexOf(go);
			for (int i = 0; i < toggles.Count; i++) {
				if(i<=num){
					toggles[i].ToggleOn();	
				}else{
					toggles[i].ToggleOff();
				}
			}
				cant=num+1;
				ready=true;
				puntman.updatepuntaje();

		}
	
		public void TurnOff (toggleicon go) {
				int num=toggles.IndexOf(go);
				for (int i = 0; i < toggles.Count; i++) {
						if(i<=num){
								toggles[i].ToggleOn();	
						}else{
								toggles[i].ToggleOff();
						}
				}
				cant=num+1;
				ready=true;
				puntman.updatepuntaje();
			
	}

		public void Reset(){
		Debug.Log("RESET");
				cant=0;
				ready=false;
				for (int i = 0; i < toggles.Count; i++) {
						toggles[i].ToggleOff();
				}
		}
}


