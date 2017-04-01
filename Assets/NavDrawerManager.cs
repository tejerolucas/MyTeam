using UnityEngine;
using System.Collections;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;

public class NavDrawerManager : MonoBehaviour {
	public GameObject equipo;
	public GameObject admin;
	private string userid;

	void Awake (){
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl ("https://soccerapp-5d7ac.firebaseio.com/");
				if (app.Options.DatabaseUrl != null){
						app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
				}


		#if UNITY_EDITOR
		userid="editor";
		#else
		userid=UserAuth.instance.user.UserId;
		#endif
		FirebaseDatabase.DefaultInstance.GetReference ("Equipos").ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			equipo.SetActive ((bool)e2.Snapshot.Child ("Habilitado").Value);
		};
		FirebaseDatabase.DefaultInstance.GetReference ("Admins").ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			admin.SetActive (e2.Snapshot.Child(userid).Exists);
		};
	}
}
