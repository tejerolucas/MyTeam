using UnityEngine;
using System.Collections.Generic;

public class FotosJugadores : MonoBehaviour {
	public List<fotojugador> jugadores=new List<fotojugador>();

	public GameObject GetPictureByKey (string key)
	{
		foreach (var item in jugadores) {
			if (item.key == key) {
				return item.gameObject;
			}
		}
		return null;
	}

	public bool ContieneJugador (string key)
	{
		foreach (var item in jugadores) {
			if (item.key == key) {
				return true;
			}
		}
		return false;
	}
}
