using UnityEngine;
using System.Collections;
using MaterialUI;

public class toggleicon : MonoBehaviour {
		public togglelist tlist;
	private bool state;
		public VectorImage icon;
		public VectorImageData Onicon;
		public VectorImageData Officon;

		public Color OnColor;
		public Color OffColor;
	
		void Awake(){
				icon.SetImage(Officon);	
				state=false;
		}

	public void Toggle (){
		if(state){
			tlist.TurnOff(this);	
		}else{
			tlist.TurnOn(this);		
		}
		state=!state;
	}

		public void ToggleOn(){
				icon.SetImage(Onicon);	
				icon.color=OnColor;	
				state=true;
		}
		public void ToggleOff(){
				icon.SetImage(Officon);	
				icon.color=OffColor;
				state=false;
		}
}
