﻿using UnityEngine;
using System.Collections;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;
using UnityEngine.UI;
using MaterialUI;

public class ShowPlayers: MonoBehaviour
{
	public string hijo;
	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
	ArrayList players;
	public GameObject playerprefab;
	public GameObject jugadores;
	public GameObject deselect;
	public GameObject Popup;
	public List<eterplayer> eterp = new List<eterplayer> ();
	public List<GameObject> lista = new List<GameObject> ();
	private string projectid = "https://soccerapp-5d7ac.firebaseio.com/";
	public CircularProgressIndicator progressindicator;
	public Animation anim;
	public CanvasGroup canvasgroup;
	public Color playerbackground;


	void Start ()
	{
		if(canvasgroup.alpha>0.5f){
			anim.Play("HideMatch");
		}
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
		foreach (eterplayer etp in eterp) {
			etp.Popup = go;
		}
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
			.GetReference ("Jugadores").Child(hijo).OrderByChild("nombre")
			.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			if (e2.DatabaseError != null) {
				Debug.LogError (e2.DatabaseError.Message);
				return;
			}
			players.Clear ();
			progressindicator.Show(true);
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
						#if !UNITY_EDITOR
						if(childSnapshot.Child("userid").Value.ToString()!=UserAuth.instance.user.UserId){
						#else
						if(true){
						#endif
							if(canvasgroup.alpha<1){
								anim.Play("ShowMatch");
							}
							progressindicator.Hide();
							GameObject etpgo = (GameObject)Instantiate (playerprefab, jugadores.transform);
							etpgo.GetComponent<Image>().color=playerbackground;
							eterplayer etp = etpgo.GetComponent<eterplayer> ();
							etpgo.transform.localScale=Vector3.one;
							etp.Popup = Popup;
							etp.SetData (childSnapshot);
							eterp.Add (etp);
							lista.Add(etpgo);
						}
					}
				}
			}
		};
	}
}
