using UnityEngine;
using System.Collections;
using UnityEditor;

public class Tester : EditorWindow {
	public enum ESTADOEVENTO{
    ABIERTO = 0,
    CERRADO = 1,
    EQUIPOS = 2
	}
	public ESTADOEVENTO estado;
	public int cantidad;
	[MenuItem("Window/Tester")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        Tester window = (Tester)EditorWindow.GetWindow(typeof(Tester));
        window.Show();
    }

    void OnGUI ()
	{
		GUILayout.Label ("JUGADORES");
		GUILayout.Space (6);
		GUILayout.BeginHorizontal ();
		cantidad = EditorGUILayout.IntField (cantidad);
		if (GUILayout.Button ("CREAR")) {
			WWW www = new WWW ("https://us-central1-soccerapp-5d7ac.cloudfunctions.net/CreatePlayers?cantidad=" + cantidad.ToString ());
		}
		GUILayout.EndHorizontal ();
		GUILayout.Space (6);
		if (GUILayout.Button ("BORRAR")) {
			WWW www = new WWW ("https://us-central1-soccerapp-5d7ac.cloudfunctions.net/ClearUsedUsers");
		}


		GUILayout.Label ("EVENTO");
		GUILayout.Space (6);
		GUILayout.BeginHorizontal ();
		estado = (ESTADOEVENTO)EditorGUILayout.EnumPopup ("ESTADO:", estado);
		GUILayout.EndHorizontal ();
		if (GUILayout.Button ("CAMBIAR")) {
			CambiarEstadoEvento (estado);
		}
	}

	public void CambiarEstadoEvento(ESTADOEVENTO est){
		WWW www = new WWW ("https://us-central1-soccerapp-5d7ac.cloudfunctions.net/ChangeEventState?state=");
	}
}
