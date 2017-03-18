using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class pixeltester : MonoBehaviour {

	void OnGUI () {
		GUILayout.Label ("Screen Resolution: " + Screen.resolutions.ToString(),GUILayout.MinHeight(Screen.height/20),GUILayout.MinWidth(Screen.width));
		GUILayout.Label ("Screen Density: "+Screen.dpi,GUILayout.MinHeight(Screen.height/20),GUILayout.MinWidth(Screen.width));
	}
}
