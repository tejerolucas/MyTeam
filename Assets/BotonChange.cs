using UnityEngine;
using System.Collections;
using MaterialUI;
using Firebase.Database;

public class BotonChange : MonoBehaviour {
	public CanvasGroup canvasgo;
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
	public GameObject popup;

	void Start ()
	{
		canvasgo.alpha = 0;
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
					UserAuth.instance.tipo=tipo;
				}
				iTween.ValueTo(this.gameObject,iTween.Hash("from",0.0f,"to",1.0f,"time",1.0f,"onupdate","UpdateCanvas","onupdatetarget",this.gameObject));
				SetState(registrado);

			}
		});
	}

	public void ToggleButton(){
		registrado = !registrado;
		if (registrado) {
			popup.SetActive (true);
		}
		SetState (registrado);
	}

	public void SetState (bool state)
	{	
		if (!state) {
			boton.text.text = textaceptar;
			boton.SetButtonBackgroundColor (coloraceptar, true);
			boton.iconVectorImageData = iconaceptar;
			reference.Child ("Jugadores").Child(UserAuth.instance.tipo).Child (userid).RemoveValueAsync ();
		} else {
			boton.SetButtonBackgroundColor (colorabandonar,false);
			boton.text.text = textabandonar;
			boton.iconVectorImageData = iconabandonar;

		}
	}

	public void UpdateCanvas(float value){
		canvasgo.alpha = value;
	}
}
