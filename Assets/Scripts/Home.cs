using UnityEngine;
using System.Collections;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using MaterialUI;

public class Home : MonoBehaviour {
	private DatabaseReference reference;
	public GameObject[] estados;
	public CircularProgressIndicator progressindicator;
	// Use this for initialization
	void Start () {
		foreach (var item in estados) {
				item.SetActive(false);
			}
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl ("https://soccerapp-5d7ac.firebaseio.com/");
		if (app.Options.DatabaseUrl != null){
			app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
		}
		reference = FirebaseDatabase.DefaultInstance.GetReference ("Evento/Estado");
		reference.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			foreach (var item in estados) {
				item.SetActive(false);
			}
			progressindicator.Show ();
			Debug.Log("ESTADO EVENTO: "+e2.Snapshot.Value.ToString());

			switch(e2.Snapshot.Value.ToString()){
				case "Abierto":
				estados[0].SetActive(true);
				break;
			case "Cerrado":
				estados[1].SetActive(true);
				break;
			case "Equipos":
				estados[2].SetActive(true);
				break;
			}


			progressindicator.Hide();
		};
	}
}
