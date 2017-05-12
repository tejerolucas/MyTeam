using UnityEngine;
using System.Collections;
using MaterialUI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class Canchas : MonoBehaviour {
	public cancha[] canchas;
	public TabView tabview;
	[Header("Hombres")]
	public string textohombres;
	public Color colorhombres;
	public VectorImageData iconohombres;
	[Header("Unisex")]
	public string textounisex;
	public Color colorunisex;
	public VectorImageData iconounisex;
	[Space(10)]
	public LinearProgressIndicator linearprogress;
	// Use this for initialization
	void Start () {
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl ("https://soccerapp-5d7ac.firebaseio.com/");
		if (app.Options.DatabaseUrl != null){
				app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
		}
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference ("Evento");

		reference.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			if (e2.DatabaseError != null) {
				Debug.LogError (e2.DatabaseError.Message);
				return;
			}

			if (e2.Snapshot.Child ("Estado").Value.ToString () == "Cerrado") {
				CargarCanchas();
			} else {
				

			}
		};
	}

	void CargarCanchas ()
	{
		int cant = 0;
		FirebaseDatabase.DefaultInstance.GetReference ("Evento/Canchas").GetValueAsync ().ContinueWith (task => {
			if(task.IsCompleted){
				
				cant=(int)task.Result.ChildrenCount;
				for (int i = 0; i < cant; i++) {
					bool isMenMatch=task.Result.Child(i.ToString()).Value.ToString()=="H"?true:false;
					canchas[i].tipo.text=isMenMatch?textohombres:textounisex;
					Color col=isMenMatch?colorhombres:colorunisex;
					canchas[i].SetFiledColor(col);
				}
				tabview.gameObject.SetActive(true);
			}
		});

		linearprogress.Hide ();
	}
}