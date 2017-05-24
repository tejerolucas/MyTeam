using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;

public class puntajemanager : GenericPopUp {
		public GameObject button;
		public togglelist estrellas;
		public togglelist corazones;

	void Start () {
		button.SetActive(false);
	}
	
	public void updatepuntaje () {
		if(estrellas.ready&&corazones.ready){
			button.SetActive(true);
		}
	}

	void OnEnable(){
		Reset ();
	}

	public void SendValoration(){
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference ("Jugadores");
		reference=reference.Child (_ownuserid);
		reference=reference.Child ("valoraciones");
		reference=reference.Child (_userid);
		reference.Child ("nombre").SetValueAsync(_name.text);
		reference.Child ("puesto").SetValueAsync(_position.text);
		reference.Child ("estrellas").SetValueAsync(estrellas.cant);
		reference.Child ("corazones").SetValueAsync(corazones.cant);
		FirebaseDatabase.DefaultInstance.GetReference ("Jugadores").Child(_userid).Child("valoracionesrecibidas").Child (_ownuserid).SetValueAsync(estrellas.cant).ContinueWith(task2=>{
			if(task2.IsCompleted){
				float estrellasfinal = 0.0f;
				DatabaseReference valoracionesref = FirebaseDatabase.DefaultInstance.GetReference ("Jugadores").Child(_userid).Child("valoracionesrecibidas");
				valoracionesref.GetValueAsync ().ContinueWith (task => {
					if (task.IsCompleted) {
						Debug.Log("SDA");
						DataSnapshot snapshot = task.Result;
						foreach (var item in snapshot.Children) {
							int num=0;
							int.TryParse(item.Value.ToString(),out num);
							estrellasfinal+=num*1.0f;
						}
						Debug.Log("ANTES: "+estrellasfinal.ToString());
						float divisor=snapshot.ChildrenCount*1.0f;
						Debug.Log("Divisor: "+divisor.ToString());
						estrellasfinal=estrellasfinal/divisor;
						FirebaseDatabase.DefaultInstance.GetReference ("Jugadores").Child (_userid).Child ("Estrella").SetValueAsync (estrellasfinal);
					}
				});
			}
		});

		this.gameObject.SetActive (false);
	}


	public void GetValoration(int corazonesint,int estrellasint){
		corazones.SetInt (corazonesint);
		estrellas.SetInt (estrellasint);
	}
	
	public void Reset(){
				button.SetActive(false);
				//corazones.Reset();
				//estrellas.Reset();
	}
}
