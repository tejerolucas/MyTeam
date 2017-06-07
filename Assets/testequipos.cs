using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class testequipos : MonoBehaviour {
	public List<Jug> jugadores=new List<Jug>();
	public float resultado;
	public float promedio;
	// Use this for initialization
	void Start () {
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl ("https://soccerapp-5d7ac.firebaseio.com/");
		if (app.Options.DatabaseUrl != null) {
			app.SetEditorDatabaseUrl (app.Options.DatabaseUrl);
		}
		jugadores.Clear ();
		FirebaseDatabase.DefaultInstance.GetReference ("TestEquipos/Jugadores").GetValueAsync ().ContinueWith (task => {
			if (task.IsCompleted) {
				DataSnapshot snapshot = task.Result;
				foreach (var item in snapshot.Children) {
					int val=0;
					int.TryParse(item.Value.ToString(),out val);
					Jug ju=new Jug(item.Key.ToString(),val);
					jugadores.Add(ju);
				}
				SumarPuntos();
				ArmarEquipos();
			}

		});


	}
	public void ArmarEquipos ()
	{
		List<Jug> equipoa = new List<Jug>();
		List<Jug> equipob = new List<Jug>();
		List<Jug> equipoc = new List<Jug>();
		List<Jug> equipod = new List<Jug>();

		float equipoatotal = 0;
		float equipobtotal = 0;
		float equipoctotal = 0;
		float equipodtotal = 0;

		List<Jug> jugadorestemp=new List<Jug>();
		foreach (var item in jugadores) {
			jugadorestemp.Add (item);
		}

		int num=Random.Range(0,jugadorestemp.Count);

	}

	public void SumarPuntos(){
		resultado = 0.0f;
		foreach (var item in jugadores) {
			resultado += item.valoracion;
		}
		promedio = resultado / 4;
		Debug.Log ("Total: " + resultado.ToString ());
		Debug.Log ("Promedio: " + promedio.ToString ());
	}
}






public class Jug{
	public string name;
	public float valoracion;
	public Jug (string nom, float val)
	{
		name=nom;
		valoracion=val;
	}

}
