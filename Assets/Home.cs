using UnityEngine;
using System.Collections;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class Home : MonoBehaviour {
	private DatabaseReference reference;
	public GameObject abierto;
	public GameObject cerrado;
	// Use this for initialization
	void Start () {
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl ("https://soccerapp-5d7ac.firebaseio.com/");
		if (app.Options.DatabaseUrl != null){
			app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
		}
		reference = FirebaseDatabase.DefaultInstance.GetReference ("Evento/Estado");
		reference.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			bool isOpen=false;
			Debug.Log("ESTADO EVENTO: "+e2.Snapshot.Value.ToString());
			if(e2.Snapshot.Value.ToString()=="Abierto"){
				isOpen=true;
			}
			abierto.SetActive(isOpen);
			cerrado.SetActive(!isOpen);
		};
	}
}
