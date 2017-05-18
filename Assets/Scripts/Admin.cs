using UnityEngine;
using System.Collections;

public class Admin : MonoBehaviour {
	public int cantidad;
	public int cantidad2;
	public string estado;

	public void CrearJugadores(){
		string url = "https://us-central1-soccerapp-5d7ac.cloudfunctions.net/CreateTesterPlayers?cantidad=" + cantidad.ToString ();
		Debug.Log (url);
		WWW www = new WWW (url);
	}

	public void AgregarJugadores(){
		string url = "https://us-central1-soccerapp-5d7ac.cloudfunctions.net/AddPlayerstoEvent?cantidad=" + cantidad2.ToString ();
		Debug.Log (url);
		WWW www = new WWW (url);
	}

	public void BorrarJugadores ()
	{
		string url = "https://us-central1-soccerapp-5d7ac.cloudfunctions.net/ClearTesterUsers";
		Debug.Log (url);
		WWW www = new WWW (url);
	}

	public void ChangeState ()
	{
		string url = "https://us-central1-soccerapp-5d7ac.cloudfunctions.net/ChangeEventState?estado=" + estado;
		Debug.Log (url);
		WWW www = new WWW (url);
	}

	public void updatecantidad(string num){
		int numero = 0;
		if(int.TryParse(num,out numero)){
			Debug.Log (numero);
			cantidad = numero;
		}
	}

	public void updatecantidad2(string num){
		int numero = 0;
		if(int.TryParse(num,out numero)){
			Debug.Log (numero);
			cantidad2 = numero;
		}
	}

	public void updateEstado(string num){
		estado = num;
	}
}
