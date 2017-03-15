using UnityEngine;
using System.Collections;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;

public class testdata : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl ("https://soccerapp-5d7ac.firebaseio.com/");
		if (app.Options.DatabaseUrl != null) {
		app.SetEditorDatabaseUrl (app.Options.DatabaseUrl);
		}

			FirebaseDatabase.DefaultInstance.GetReference("Jugadores/ShWNahpi3bMdotWdtWRgTHRnFt12/amonestaciones").ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
	      Debug.Log(e2.Snapshot.Value);
	    };

	}
}
