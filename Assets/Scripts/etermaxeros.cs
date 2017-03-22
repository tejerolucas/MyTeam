using UnityEngine;
using System.Collections;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;
using UnityEngine.UI;
using MaterialUI;

public class etermaxeros : MonoBehaviour {
	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
	ArrayList players;
	public GameObject playerprefab;
	public GameObject jugadores;
	public GameObject deselect;
	public GameObject PuntajeGO;
	public List<eterempleado> eterp=new List<eterempleado>();
	private string projectid="https://soccerapp-5d7ac.firebaseio.com/";
	public CircularProgressIndicator progress;

	void Start() {
		progress.Show();
		dependencyStatus = FirebaseApp.CheckDependencies();
		if (dependencyStatus != DependencyStatus.Available) {
			FirebaseApp.FixDependenciesAsync().ContinueWith(task => {
				dependencyStatus = FirebaseApp.CheckDependencies();
				if (dependencyStatus == DependencyStatus.Available) {
					InitializeFirebase();
				} else {
					Debug.LogError(
						"Could not resolve all Firebase dependencies: " + dependencyStatus);
				}
			});
		} else {
			InitializeFirebase();
		}
	}

	void InitializeFirebase() {
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl(projectid);
		if (app.Options.DatabaseUrl != null){
			app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
		}

		players = new ArrayList();	
					
			FirebaseDatabase.DefaultInstance
			.GetReference("Usuarios").OrderByChild("nombre")
			.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			if (e2.DatabaseError != null) {
				Debug.LogError(e2.DatabaseError.Message);
				return;
			}
			players.Clear();
			progress.Hide();
			if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
				foreach (var childSnapshot in e2.Snapshot.Children) {
					if (childSnapshot.Child("nombre") == null
						|| childSnapshot.Child("nombre").Value == null) {
						Debug.LogError("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
						break;
					} else {
						if(!childSnapshot.HasChild("Usado")){
							GameObject etpgo= (GameObject)Instantiate(playerprefab,jugadores.transform) ;
							etpgo.transform.localScale=Vector3.one;
							eterempleado etp=etpgo.GetComponent<eterempleado>();
							etp.puntuador=PuntajeGO;
							etp.SetData(childSnapshot,childSnapshot.Reference,childSnapshot.GetValue(true));
							eterp.Add(etp);
						}

					}
				}
			}
		};
	}



	public void Buscar(string searchtext){
		searchtext=searchtext.ToLower();
		foreach(eterempleado etp in eterp){
			if(etp.nombre.Contains(searchtext)){
				etp.gameObject.transform.SetParent(jugadores.transform);
				continue;
			}
			if(etp.posicion.Contains(searchtext)){
				etp.gameObject.transform.SetParent(jugadores.transform);
				continue;
			}
			etp.gameObject.transform.SetParent(deselect.transform);
		}	
	}


}
