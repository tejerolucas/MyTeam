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
		//aumento la cantidad de jugadores
		FirebaseDatabase.DefaultInstance
			.GetReference("Evento/Jugadores/cant")
			.GetValueAsync().ContinueWith(task => {
				if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					if(snapshot.Value==""){
						snapshot.Reference.SetValueAsync("1");
					}else{
						int pre=0;
						if (int.TryParse(snapshot.Value.ToString(), out pre))
						{	
							pre++;
							snapshot.Reference.SetValueAsync(pre);
						}
					}
				}
			});
		//Agrego jugador a la lista que pertenece
		string tipostring=tipo==1?"Hombres":"Unisex";
		FirebaseDatabase.DefaultInstance.GetReference ("Evento/Jugadores").Child (tipostring).Child(userid).SetValueAsync ("");
		Partido.instance.registrado = true;
		Partido.instance.tipo = tipostring;
		Partido.instance.SetState (true);
	}
}
