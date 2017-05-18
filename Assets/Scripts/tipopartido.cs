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
		UserAuth.instance.tipo = tipostring;
		FirebaseDatabase.DefaultInstance.GetReference ("Evento/Jugadores").Child (tipostring).Child(userid).SetValueAsync ("");
		FirebaseDatabase.DefaultInstance.GetReference ("Jugadores").Child(userid).Child("Evento") .SetValueAsync (tipostring);
		FirebaseDatabase.DefaultInstance.GetReference("Evento/CantidadJugadores").GetValueAsync().ContinueWith(task => {
			if (task.IsCompleted) {
				DataSnapshot snapshot = task.Result;
				if(snapshot.Value==""){
					snapshot.Reference.SetValueAsync(1);
				}else{
					int pre=0;
					if (int.TryParse(snapshot.Value.ToString(), out pre))
					{	
						pre++;
						Debug.Log("AGREGANDO: "+pre.ToString());
						snapshot.Reference.SetValueAsync(pre);
					}
				}
			}
		});
	}
}
