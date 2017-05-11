using UnityEngine;
using System.Collections;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;
using UnityEngine.UI;
using MaterialUI;

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
	public CircularProgressIndicator progressindicator;
	public Animation anim;
	public CanvasGroup canvasgroup;
	public Color playerbackground;
	public DatabaseReference refJugadores;

	void Start ()
	{
		if (canvasgroup.alpha > 0.5f) {
			anim.Play ("HideMatch");
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

	void JugadorCreado (object sender, ChildChangedEventArgs e)
	{

		GameObject etpgo = (GameObject)Instantiate (playerprefab, jugadores.transform);
			etpgo.GetComponent<Image> ().color = playerbackground;
			eterplayer etp = etpgo.GetComponent<eterplayer> ();
			etpgo.transform.localScale = Vector3.one;
			etp.Popup = Popup;
			etp.SetData (e.Snapshot);
			eterp.Add (etp);
			lista.Add (etpgo);

		if (canvasgroup.alpha < 1) {
			anim.Play ("ShowMatch");
		}
		progressindicator.Hide ();
	}

	void JugadorBorrado (object sender, ChildChangedEventArgs e)
	{
		GameObject borrar = null;;
			foreach (var item in lista) {
				if (item.GetComponent<eterplayer> ().nombre == e.Snapshot.Child ("nombre").Value.ToString ().ToLower()) {
					borrar=item;
				}
			}
		if(borrar!=null){
			lista.Remove(borrar);
			Destroy(borrar);
		}
		if(lista.Count<=0){
			canvasgroup.alpha=0;
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

		//ref.ChildAdded += HandleChildAdded;

		players = new ArrayList ();
		refJugadores = FirebaseDatabase.DefaultInstance.GetReference ("Jugadores");
		refJugadores.ChildAdded += JugadorCreado;
		refJugadores.ChildRemoved += JugadorBorrado;

		refJugadores.GetValueAsync ().ContinueWith (task => {
			if (task.IsFaulted) {
				Debug.LogError ("ERROR");
			} else if (task.IsCompleted) {
				DataSnapshot snapshot = task.Result;

				players.Clear ();
				progressindicator.Show (true);

				if (snapshot != null) {
					Debug.Log ("CHILDRENS: " + snapshot.ChildrenCount.ToString ());
					if (snapshot.ChildrenCount > 0) {
						Debug.Log ("CHILDRENS: " + snapshot.ChildrenCount.ToString ());
						foreach (GameObject go in lista) {
							eterp.Remove (go.GetComponent<eterplayer> ());
							Destroy (go);
						}


						foreach (var childSnapshot in snapshot.Children) {
							if (childSnapshot.Child ("nombre") == null
							   || childSnapshot.Child ("nombre").Value == null) {
								Debug.LogError ("Bad data in sample.  Did you forget to call SetEditorDatabaseUrl with your project id?");
								break;
							} else {
								#if !UNITY_EDITOR
							if(childSnapshot.Child("userid").Value.ToString()!=UserAuth.instance.user.UserId){
								#else
								if (true) {
									#endif
									if (canvasgroup.alpha < 1) {
										anim.Play ("ShowMatch");
									}
									progressindicator.Hide ();
									GameObject etpgo = (GameObject)Instantiate (playerprefab, jugadores.transform);
									etpgo.GetComponent<Image> ().color = playerbackground;
									eterplayer etp = etpgo.GetComponent<eterplayer> ();
									etpgo.transform.localScale = Vector3.one;
									etp.Popup = Popup;
									etp.SetData (childSnapshot);
									eterp.Add (etp);
									lista.Add (etpgo);
								}
							}
						}
					}else{
						progressindicator.Hide ();
					}
				}else{
					progressindicator.Hide ();
				}
			} 
		});


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
