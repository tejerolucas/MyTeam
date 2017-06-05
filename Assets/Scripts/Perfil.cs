using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;
using MaterialUI;
using System;
using System.Globalization;

public class Perfil : MonoBehaviour {
	public Text nombre;
	public Text puesto;
	public Text amonestaciones;
	public Text valoraciones;
	public Image foto;
	private string userid;
	public Image mask;
	// Use this for initialization
	void Start ()
	{
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl ("https://soccerapp-5d7ac.firebaseio.com/");
		if (app.Options.DatabaseUrl != null) {
			app.SetEditorDatabaseUrl (app.Options.DatabaseUrl);
		}

		nombre.text = UserAuth.instance._username;
		puesto.text = UserAuth.instance._userposition;
		if (Resources.Load ("Fotos/" + UserAuth.instance._userfilename) != null) {
			foto.sprite = Resources.Load<Sprite> ("Fotos/" + UserAuth.instance._userfilename);
		}
	}

	void OnEnable ()
	{
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl ("https://soccerapp-5d7ac.firebaseio.com/");
		if (app.Options.DatabaseUrl != null) {
			app.SetEditorDatabaseUrl (app.Options.DatabaseUrl);
		}


		#if UNITY_EDITOR
		userid="editor";
		#else
		userid=UserAuth.instance.user.UserId;
		#endif

			FirebaseDatabase.DefaultInstance.GetReference("Jugadores/"+userid).ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
				amonestaciones.text =e2.Snapshot.Child("amonestaciones").Value.ToString();
				nombre.text=e2.Snapshot.Child("nombre").Value.ToString();
				puesto.text=e2.Snapshot.Child("puesto").Value.ToString();
				if (Resources.Load ("Fotos/"+e2.Snapshot.Child("filename").Value.ToString()) != null) {
					foto.sprite = Resources.Load<Sprite> ("Fotos/" + e2.Snapshot.Child("filename").Value.ToString());
				}
				valoraciones.text=e2.Snapshot.Child("Estrella").Value.ToString()+"\n"+e2.Snapshot.Child("valoracionesrecibidas").ChildrenCount.ToString();
			Debug.Log(float.Parse(e2.Snapshot.Child("Estrella").Value.ToString(),CultureInfo.InvariantCulture.NumberFormat));
			mask.fillAmount=float.Parse(e2.Snapshot.Child("Estrella").Value.ToString())/5.0f;

	   	 	};

	}

	public void LogOut ()
	{
		UserAuth.instance.SignOut ();
		Application.Quit ();
	}
}
