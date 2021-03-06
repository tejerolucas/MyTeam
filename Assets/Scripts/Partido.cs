﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;
using MaterialUI;
using System;
using System.Globalization;

public class Partido : MonoBehaviour {
	public static Partido instance;
	private string[] m_SmallStringList = new string[] { "Hombres", "Unisex" };
	//datos
	public BotonChange botonchange;
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
	public string tipo;
	public string userid;
	private float prejugadores;
	public GameObject popup;
	public GameObject	fotosjugadores;
	public GameObject fotojugador;

	void Start ()
	{
		if (instance == null) {
			instance = this;
		}
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
			linearprogress.Hide();
			if(e2.Snapshot.HasChild("CantidadJugadores")){
				int.TryParse(e2.Snapshot.Child("CantidadJugadores").Value.ToString(),out jugadores);
				bool registradoh=(bool)e2.Snapshot.Child("Jugadores").Child("Hombres").HasChild(userid);
				bool registradou=(bool)e2.Snapshot.Child("Jugadores").Child("Unisex").HasChild(userid);
				registrado=registradoh||registradou;
				if(registrado){
				tipo=registradoh?"Hombres":"Unisex";
				}
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
			CloseDate=EventDate.Subtract(new TimeSpan(0,0,0,0));
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
		DatabaseReference referenceJugadores = FirebaseDatabase.DefaultInstance.GetReference ("Evento/Jugadores/Hombres");

		referenceJugadores.ChildAdded += AgregarHombre;
		referenceJugadores.ChildRemoved += BorrarHombre;

		DatabaseReference refJugadoresUnisex = FirebaseDatabase.DefaultInstance.GetReference ("Evento/Jugadores/Unisex");
		refJugadoresUnisex.ChildAdded += AgregarUnisex;
		refJugadoresUnisex.ChildRemoved += BorrarUnisex;

	}

	public void AgregarUnisex (object sender, ChildChangedEventArgs e)
	{
		AgregarJugador (e.Snapshot.Key, MaterialColor.pink500);
	}

	public void AgregarHombre (object sender, ChildChangedEventArgs e)
	{
		AgregarJugador (e.Snapshot.Key, MaterialColor.indigo500);
	}

	public void BorrarUnisex (object sender, ChildChangedEventArgs e)
	{
		Debug.Log ("Borro Unisex");
		BorrarJugador (e.Snapshot.Key);
	}

	public void BorrarHombre (object sender, ChildChangedEventArgs e)
	{
		Debug.Log ("Borro Hombre");
		BorrarJugador (e.Snapshot.Key);
	}

	public void AgregarJugador (string key,Color anillo)
	{
		FotosJugadores fjugadoreslista = fotosjugadores.GetComponent<FotosJugadores> ();
		if(!fjugadoreslista.ContieneJugador(key)){
			GameObject foto =Instantiate(fotojugador)as GameObject;
			fotojugador fjugador=foto.GetComponent<fotojugador>();
			fjugador.anillo.color=anillo;
			fjugador.SetPicture(key);
			foto.transform.SetParent(fotosjugadores.transform);
			fjugadoreslista.jugadores.Add (fjugador);
		}
	}

	public void BorrarJugador (string key)
	{
		Debug.Log ("BORRANDO JUGADOR: "+key);

		GameObject go=fotosjugadores.GetComponent<FotosJugadores> ().GetPictureByKey (key);
		fotosjugadores.GetComponent<FotosJugadores> ().jugadores.Remove (go.GetComponent<fotojugador>());
		Destroy (go);
	}

	void Update ()
	{
		TimeSpan TimeRemain = CloseDate.Subtract (System.DateTime.Now);
		if (TimeRemain.Seconds > 0) {
			int horasint = TimeRemain.Hours + (TimeRemain.Days * 24);
			string horas = horasint > 9 ? horasint.ToString () : "0" + horasint.ToString ();
			string minutos = TimeRemain.Minutes > 9 ? TimeRemain.Minutes.ToString () : "0" + TimeRemain.Minutes.ToString ();
			string segundos = TimeRemain.Seconds > 9 ? TimeRemain.Seconds.ToString () : "0" + TimeRemain.Seconds.ToString ();
			timer.text = "Cierra en:\n<size=70>" + horas + " : " + minutos + " : " + segundos + "</size>";
		} else {
			timer.text = "<size=70>CERRADO</size>";
		}
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
			botonchange.SetState (false);
		} else {
			popup.SetActive (true);
		}
		registrado = !registrado;
	}

}