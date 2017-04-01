using UnityEngine;
using System.Collections;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;

public class testdata : MonoBehaviour {
	System.DateTime date=new System.DateTime();
	private bool ready;
	// Use this for initialization
	void Start ()
	{
		ready = false;
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl ("https://soccerapp-5d7ac.firebaseio.com/");
		if (app.Options.DatabaseUrl != null) {
		app.SetEditorDatabaseUrl (app.Options.DatabaseUrl);
		}

		date = System.DateTime.Now.AddDays (4);
		FirebaseDatabase.DefaultInstance.GetReference ("Evento").Child ("Timer").SetValueAsync (date.ToShortDateString());
	    
		Debug.Log ("termine");
		ready = true;
	}

	void Update ()
	{
		if (ready) {
			System.TimeSpan ts = date.Subtract (System.DateTime.Now);
			Debug.LogFormat ("{0} Horas,{1} Minuto,{2} Segundos", ts.Hours, ts.Minutes, ts.Seconds);
		}
	}
}
