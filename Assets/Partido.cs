using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;

public class Partido : MonoBehaviour {
	public Text jugadorescant;
	public Text vacantescant;
	public Text fechatext;
	public int jugadores;
	public int vacantes;
	public Button boton;


	public Color asistirecolor;
	public Color abandonarcolor;
	public string asistirestring;
	public string abandonarstring;

	// Use this for initialization
	void Start () {
		FirebaseDatabase.DefaultInstance.GetReference ("Evento").ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			if (e2.DatabaseError != null) {
				Debug.LogError (e2.DatabaseError.Message);
				return;
			}
			vacantescant.text=e2.Snapshot.Child("Cantidad").Value.ToString();
			jugadorescant.text=e2.Snapshot.Child("Jugadores").Children.ToString();
			fechatext.text= e2.Snapshot.Child("Fecha").Value.ToString();

		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
