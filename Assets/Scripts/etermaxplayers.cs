﻿using UnityEngine;
using System.Collections;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;
using UnityEngine.UI;

public class etermaxplayers : MonoBehaviour
{
	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
	ArrayList players;
	public GameObject playerprefab;
	public GameObject jugadores;
	public GameObject deselect;
	public GameObject Popup;
	public List<eterplayer> eterp = new List<eterplayer> ();
	public List<GameObject> lista = new List<GameObject> ();
	private string projectid = "https://soccerapp-5d7ac.firebaseio.com/";


	void Start ()
	{
		dependencyStatus = FirebaseApp.CheckDependencies ();
		if (dependencyStatus != DependencyStatus.Available) {
			FirebaseApp.FixDependenciesAsync ().ContinueWith (task => {
				dependencyStatus = FirebaseApp.CheckDependencies ();
				if (dependencyStatus == DependencyStatus.Available) {
					InitializeFirebase ();
				} else {
					Debug.LogError (
						"Could not resolve all Firebase dependencies: " + dependencyStatus);
				}
			});
		} else {
			InitializeFirebase ();
		}
	}

	public void SetPopUp (GameObject go)
	{
		Popup = go;
	}

	void InitializeFirebase ()
	{
		Debug.Log ("Inicio Jugadores");
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl (projectid);
		if (app.Options.DatabaseUrl != null) {
			app.SetEditorDatabaseUrl (app.Options.DatabaseUrl);
		}

		players = new ArrayList ();
		FirebaseDatabase.DefaultInstance
						.GetReference ("Jugadores").OrderByChild("nombre")
						.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			if (e2.DatabaseError != null) {
				Debug.LogError (e2.DatabaseError.Message);
				return;
			}
			players.Clear ();
			if (e2.Snapshot != null && e2.Snapshot.ChildrenCount > 0) {
				foreach(GameObject go in lista){
					eterp.Remove(go.GetComponent<eterplayer> ());
					Destroy(go);
				}
				foreach (var childSnapshot in e2.Snapshot.Children) {
					if (childSnapshot.Child ("nombre") == null
					    || childSnapshot.Child ("nombre").Value == null) {
						Debug.LogError ("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
						break;
					} else {
						Debug.Log("user player2 id: "+childSnapshot.Child("userid").Value.ToString());
						Debug.Log("user player id: "+UserAuth.instance.user.UserId);
						if(childSnapshot.Child("userid").Value.ToString()!=UserAuth.instance.user.UserId){
							GameObject etpgo = (GameObject)Instantiate (playerprefab, jugadores.transform);
							eterplayer etp = etpgo.GetComponent<eterplayer> ();
							etpgo.transform.localScale=Vector3.one;
							etp.puntuador = Popup;
							etp.SetData (childSnapshot);
							eterp.Add (etp);
							lista.Add(etpgo);
						}
					}
				}
			}
		};
	}



	public void Buscar (string searchtext)
	{
		searchtext = searchtext.ToLower ();
		foreach (eterplayer etp in eterp) {
			if (etp.nombre.Contains (searchtext)) {
				etp.gameObject.transform.SetParent (jugadores.transform);
				continue;
			}
			if (etp.posicion.Contains (searchtext)) {
				etp.gameObject.transform.SetParent (jugadores.transform);
				continue;
			}
			etp.gameObject.transform.SetParent (deselect.transform);
		}
	}


}
