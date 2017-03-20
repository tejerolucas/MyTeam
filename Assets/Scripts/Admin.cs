using UnityEngine;
using System.Collections;

public class Admin : MonoBehaviour {
	public etermaxplayers eterplayers;
	public GameObject amonestarpopup;
	public void CrearEvento(){

	}

	public void GenerarEquipo(){

	}

	public void Amonestar ()
	{
		eterplayers.SetPopUp (amonestarpopup);
	}
}
