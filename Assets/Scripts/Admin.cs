using UnityEngine;
using System.Collections;

public class Admin : MonoBehaviour {
	public int cantidad;
	public int cantidad2;
	public string estado;

	public void CrearJugadores(){
		WWW www = new WWW ("https://us-central1-soccerapp-5d7ac.cloudfunctions.net/CreateTesterPlayers?cantidad=" + cantidad.ToString ());
	}

	public void AgregarJugadores(){
		WWW www = new WWW ("https://us-central1-soccerapp-5d7ac.cloudfunctions.net/AddPlayerstoEvent?cantidad=" + cantidad2.ToString ());
	}

	public void BorrarJugadores ()
	{
		WWW www = new WWW ("https://us-central1-soccerapp-5d7ac.cloudfunctions.net/ClearTesterUsers");
	}

	public void ChangeState ()
	{
		WWW www = new WWW ("https://us-central1-soccerapp-5d7ac.cloudfunctions.net/ChangeEventState?estado="+estado);
	}

	public void updatecantidad(string num){
		int numero = 0;
		if(int.TryParse(num,out numero)){
			cantidad = numero;
		}
	}

	public void updatecantidad2(string num){
		int numero = 0;
		if(int.TryParse(num,out numero)){
			cantidad2 = numero;
		}
	}

	public void updateEstado(string num){
		estado = num;
	}
}
