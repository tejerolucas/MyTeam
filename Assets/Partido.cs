using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;
using MaterialUI;

public class Partido : MonoBehaviour {
	//datos
	[Header("Boton")]
	public MaterialButton boton;
	[Header("Aceptar")]
	public string textaceptar;
	public Color coloraceptar;
	public VectorImageData iconaceptar;
	[Header("Cancelar")]
	public string textabandonar;
	public Color colorabandonar;
	public VectorImageData iconabandonar;
	[Space(10)]
	public int vacantes;
	public int jugadores;
	//objetos
	public Animation anim;
	public CanvasGroup canvasgroup;
	public Text nombrecanchatext;
	public Text direccioncanchatext;
	public Text horatext;
	public Text fechatext;
	public Text vacantestext;
	public Text jugadorestext;
	public Image circulo;
	public bool registrado;

	private bool _isDataLoaded;
	public DatabaseReference reference;
	private string userid;
	private float prejugadores;

	void Start(){
		#if UNITY_EDITOR
		userid="asdqweasdqwe"+Random.Range(0,100).ToString();
		#else
		userid=UserAuth.instance.user.UserId;
		#endif
		circulo.fillAmount = 0;
		prejugadores = 0;
		jugadorestext.text = "0";
		canvasgroup.alpha = 0;
		canvasgroup.blocksRaycasts = false;
		canvasgroup.interactable = false;
		FirebaseApp app = FirebaseApp.DefaultInstance;
		app.SetEditorDatabaseUrl ("https://soccerapp-5d7ac.firebaseio.com/");
				if (app.Options.DatabaseUrl != null){
						app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
				}
			reference = FirebaseDatabase.DefaultInstance.GetReference ("Evento");
			reference.ValueChanged += (object sender2, ValueChangedEventArgs e2) => {
			if (e2.DatabaseError != null) {
				Debug.LogError (e2.DatabaseError.Message);
				return;
			}
			if(!(bool)e2.Snapshot.Child("Habilitado").Value){
				return;
			}
			if(e2.Snapshot.HasChild("Jugadores")){
				int.TryParse(e2.Snapshot.Child("Jugadores").ChildrenCount.ToString(),out jugadores);
				registrado=(bool)e2.Snapshot.Child("Jugadores").HasChild(userid);
			}else{
				jugadores=0;
				registrado=false;
			}
			int.TryParse(e2.Snapshot.Child("Vacantes").Value.ToString(),out vacantes);
			vacantestext.text=vacantes.ToString()+" Vacantes";
			fechatext.text= e2.Snapshot.Child("Fecha").Value.ToString();

			horatext.text=e2.Snapshot.Child("Hora").Value.ToString();
			nombrecanchatext.text=e2.Snapshot.Child("Cancha").Child("nombre").Value.ToString();
			direccioncanchatext.text=e2.Snapshot.Child("Cancha").Child("direccion").Value.ToString();
			iTween.ValueTo(this.gameObject,iTween.Hash(
			"from",prejugadores,
			"to",jugadores*1.0f,
			"time",0.5f,
			"onupdate","UpdateCircle",
			"onupdatetarget",this.gameObject,
			"oncomplete","CompleteCircle",
			"oncompletetarget",this.gameObject,
			"easetype",iTween.EaseType.easeInOutSine
			));


			if(canvasgroup.alpha<1){
				anim.Play();

			}
		};

	}
	public void CompleteCircle ()
	{
		prejugadores = jugadores*1.0f;
	}

	public void UpdateCircle (float value)
	{
		circulo.fillAmount = Mathf.Min (1.0f, (value*1.0f)/(vacantes*1.0f));
		jugadorestext.text = ((int)value).ToString ();
	}

	public void ChangeState ()
	{	
		

		if (registrado) {
			boton.text.text = textaceptar;
			boton.SetButtonBackgroundColor (coloraceptar, true);
			boton.iconVectorImageData = iconaceptar;
			reference.Child ("Jugadores").Child (userid).RemoveValueAsync ();
		} else {
			boton.text.text = textabandonar;
			boton.SetButtonBackgroundColor (colorabandonar, true);
			boton.iconVectorImageData = iconabandonar;
			reference.Child ("Jugadores").Child (userid).SetValueAsync (jugadores);
		}
		registrado = !registrado;
	}
}
