using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;

public class Perfil : MonoBehaviour {
	public Text nombre;
	public Text puesto;
	public Text amonestaciones;
	public Image foto;
	// Use this for initialization
	void Start(){
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl ("https://soccerapp-5d7ac.firebaseio.com/");
		if (app.Options.DatabaseUrl != null) {
		app.SetEditorDatabaseUrl (app.Options.DatabaseUrl);
		}
	}

	void OnEnable ()
	{
		if (UserAuth.instance.user != null)
		{
			FirebaseDatabase.DefaultInstance.GetReference("Jugadores/"+UserAuth.instance.user.UserId).ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
				amonestaciones.text =e2.Snapshot.Child("amonestaciones").Value.ToString();
				nombre.text=e2.Snapshot.Child("nombre").Value.ToString();
				puesto.text=e2.Snapshot.Child("puesto").Value.ToString();
				if (Resources.Load ("Fotos/"+e2.Snapshot.Child("filename").Value.ToString()) != null) {
					foto.sprite = Resources.Load<Sprite> ("Fotos/" + e2.Snapshot.Child("filename").Value.ToString());
				}
	    };
		}

	}

	public void LogOut ()
	{
		UserAuth.instance.SignOut ();
		Application.Quit ();
	}
}
