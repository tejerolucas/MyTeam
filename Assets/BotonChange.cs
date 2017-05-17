using UnityEngine;
using System.Collections;
using MaterialUI;
using Firebase.Database;

public class BotonChange : MonoBehaviour {
	[Header("Aceptar")]
	public string textaceptar;
	public Color coloraceptar;
	public VectorImageData iconaceptar;
	[Header("Cancelar")]
	public string textabandonar;
	public Color colorabandonar;
	public VectorImageData iconabandonar;
	private MaterialButton boton;
	private string tipo;
	private bool registrado;
	private string userid;
	public DatabaseReference reference;

	void Awake ()
	{
		boton = this.gameObject.GetComponent<MaterialButton> ();
		#if UNITY_EDITOR
		userid="editor";
		#else
		userid=UserAuth.instance.user.UserId;
		#endif
		reference = FirebaseDatabase.DefaultInstance.GetReference ("Evento");
		reference.Child("Jugadores").GetValueAsync ().ContinueWith (task => {
			if (task.IsCompleted) {
				DataSnapshot snapshot = task.Result;
				bool registradoh=(bool)snapshot.Child("Hombres").HasChild(userid);
				bool registradou=(bool)snapshot.Child("Unisex").HasChild(userid);

				registrado=registradoh||registradou;
				if(registrado){
				tipo=registradoh?"Hombres":"Unisex";
				}
				SetState(registrado);
			}
		});
	}

	public void SetState (bool state)
	{	
		if (!state) {
			boton.text.text = textaceptar;
			boton.SetButtonBackgroundColor (coloraceptar, true);
			boton.iconVectorImageData = iconaceptar;
			reference.Child ("Jugadores").Child(tipo).Child (userid).RemoveValueAsync ();
		} else {
			boton.text.text = textabandonar;
			boton.SetButtonBackgroundColor (colorabandonar, true);
			boton.iconVectorImageData = iconabandonar;
		}
	}
}
