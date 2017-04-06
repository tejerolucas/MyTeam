using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;
using MaterialUI;
using System;
using System.Globalization;

public class Partido : MonoBehaviour {
	private string[] m_SmallStringList = new string[] { "Hombres", "Unisex" };
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
	public Text timer;
	public Image header;
	public Image circulo;
	public bool registrado;
	public DateTime CloseDate;
	public DateTime EventDate;
	private bool _isDataLoaded;
	public DatabaseReference reference;
	public LinearProgressIndicator linearprogress;
	private string userid;
	private float prejugadores;

	void Start(){
		#if UNITY_EDITOR
		userid="editor";
		#else
		userid=UserAuth.instance.user.UserId;
		#endif
		circulo.fillAmount = 0;
		prejugadores = 0;
		jugadorestext.text = "0";
		canvasgroup.alpha = 0;
		canvasgroup.blocksRaycasts = false;
		canvasgroup.interactable = false;
		fechatext.text="Buscando Partido";
		linearprogress.Show (true);
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
				if(canvasgroup.alpha>0.5f){
					anim.Play("HideMatch");
				}
				Debug.Log("partido deshabilitado");
				fechatext.text="EVENTO NO DISPONIBLE";
				TweenManager.TweenColor(color => header.color = color, header.color, colorabandonar, 2f);
				linearprogress.Hide();
				return;
			}else{
			Debug.Log("Partido habilitado");
				TweenManager.TweenColor(color => header.color = color, header.color, coloraceptar, 2f);
			linearprogress.Hide();
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


			horatext.text=e2.Snapshot.Child("Hora").Value.ToString();
			nombrecanchatext.text=e2.Snapshot.Child("Lugar").Child("nombre").Value.ToString();
			direccioncanchatext.text=e2.Snapshot.Child("Lugar").Child("direccion").Value.ToString();
			//FirebaseDatabase.DefaultInstance.GetReference("Evento/Fecha").SetValueAsync(System.DateTime.Now.ToString());
			EventDate=DateTime.Parse( e2.Snapshot.Child("Fecha").Value.ToString());
			CloseDate=EventDate.Subtract(new TimeSpan(2,0,0,0));
			string dia=EventDate.ToString("dddd",CultureInfo.CreateSpecificCulture("es-AR")).ToUpperInvariant();
			string num=EventDate.ToString("dd",CultureInfo.CreateSpecificCulture("es-AR"));
			string mes=EventDate.ToString("MMMM",CultureInfo.CreateSpecificCulture("es-AR")).ToUpper();
			fechatext.text= dia+" <size=90> "+num+" </size> "+mes;
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
				anim.Play("ShowMatch");
			}
		};

	}

	void Update(){
		TimeSpan TimeRemain = CloseDate.Subtract (System.DateTime.Now);
		string horas = TimeRemain.Hours > 9 ? TimeRemain.Hours.ToString () : "0" + TimeRemain.Hours.ToString ();
		string minutos = TimeRemain.Minutes > 9 ? TimeRemain.Minutes.ToString () : "0" + TimeRemain.Minutes.ToString ();
		string segundos = TimeRemain.Seconds > 9 ? TimeRemain.Seconds.ToString () : "0" + TimeRemain.Seconds.ToString ();
		timer.text ="Cierra en:\n<size=70>"+ horas + " : " + minutos + " : " + segundos+"</size>";
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

	public void ChooseMatch ()
	{
		DialogManager.ShowRadioList(m_SmallStringList, (int selectedIndex) => {
			ToastManager.Show("Item #" + selectedIndex + " selected: " + m_SmallStringList[selectedIndex]);
		}, "OK", "Big Radio List", MaterialIconHelper.GetRandomIcon(), () => { ToastManager.Show("You clicked the cancel button"); }, "CANCEL");
	}

	public void ChangeState ()
	{	
		Debug.Log ("BOTON");
		//ChooseMatch ();
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