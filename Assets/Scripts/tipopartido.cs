using UnityEngine;
using System.Collections;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class tipopartido : GenericPopUp {
	private string userid;

	public void SeleccionarPartido (int tipo)
	{
		#if UNITY_EDITOR
		userid="editor";
		#else
		userid=UserAuth.instance.user.UserId;
		#endif
		//Agrego jugador a la lista que pertenece
		string tipostring=tipo==1?"Hombres":"Unisex";
		FirebaseDatabase.DefaultInstance.GetReference ("Evento/Jugadores").Child (tipostring).Child(userid).SetValueAsync ("");
		Partido.instance.registrado = true;
		Partido.instance.tipo = tipostring;
		Partido.instance.botonchange.SetState (true);
	}
}
