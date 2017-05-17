using UnityEngine;
using System.Collections;
using Firebase.Database;

public class EventoCerrado : MonoBehaviour {
	public BotonChange botonchange;
	// Use this for initialization
	void Start () {
		bool registrado;

		string userid="";
		#if UNITY_EDITOR
		userid="editor";
		#else
		userid=UserAuth.instance.user.UserId;
		#endif
		FirebaseDatabase.DefaultInstance.GetReference ("Evento/Jugadores").GetValueAsync ().ContinueWith (task => {
			if (task.IsCompleted) {
				DataSnapshot snapshot = task.Result;
				bool registradoh=(bool)snapshot.Child("Hombres").HasChild(userid);
				bool registradou=(bool)snapshot.Child("Unisex").HasChild(userid);

				registrado=registradoh||registradou;
				botonchange.SetState(registrado);
			}
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
