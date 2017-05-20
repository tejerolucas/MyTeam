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
	public int cantidad2;
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
			WWW www = new WWW ("https://us-central1-soccerapp-5d7ac.cloudfunctions.net/CreateTesterPlayers?cantidad=" + cantidad.ToString ());
				 ContinuationManager.Add(() => www.isDone, () =>
				 {
				     if (!string.IsNullOrEmpty(www.error)) Debug.Log("WWW failed: " + www.error);
					EditorUtility.DisplayDialog("CREATE PLAYER",www.text.Replace("<br/>",System.Environment.NewLine),"OK");
				 });
		}
		GUILayout.EndHorizontal ();
		GUILayout.Space (6);
		if (GUILayout.Button ("BORRAR")) {
			WWW www = new WWW ("https://us-central1-soccerapp-5d7ac.cloudfunctions.net/ClearTesterUsers");
			ContinuationManager.Add(() => www.isDone, () =>
				 {
				     if (!string.IsNullOrEmpty(www.error)) Debug.Log("WWW failed: " + www.error);
					EditorUtility.DisplayDialog("DELETE ALL PLAYERS",www.text,"OK");
				 });
		}


		GUILayout.Label ("EVENTO");
		GUILayout.Space (6);
		cantidad2 = EditorGUILayout.IntField (cantidad2);
		GUILayout.BeginHorizontal ();

		if (GUILayout.Button ("AGREGAR")) {
			WWW www = new WWW ("https://us-central1-soccerapp-5d7ac.cloudfunctions.net/AddPlayerstoEvent?cantidad=" + cantidad2.ToString ());
			ContinuationManager.Add(() => www.isDone, () =>
				 {
				     if (!string.IsNullOrEmpty(www.error)) Debug.Log("WWW failed: " + www.error);
					EditorUtility.DisplayDialog("ADD PLAYERS TO EVENT",www.text,"OK");
				 });
		}
		if (GUILayout.Button ("BORRAR")) {
			WWW www = new WWW ("https://us-central1-soccerapp-5d7ac.cloudfunctions.net/RemovePlayersfromEvent?cantidad=" + cantidad2.ToString ());
			ContinuationManager.Add(() => www.isDone, () =>
				 {
				     if (!string.IsNullOrEmpty(www.error)) Debug.Log("WWW failed: " + www.error);
					EditorUtility.DisplayDialog("ADD PLAYERS TO EVENT",www.text,"OK");
				 });
		}
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		estado = (ESTADOEVENTO)EditorGUILayout.EnumPopup ("ESTADO:", estado);
		GUILayout.EndHorizontal ();
		if (GUILayout.Button ("CAMBIAR")) {
			CambiarEstadoEvento (estado);
		}
	}


	public void CambiarEstadoEvento (ESTADOEVENTO est)
	{
		string estado = "";
		switch ((int)est) {
		case 0:
		estado="Abierto";
			break;
		case 1:
			estado="Cerrado";
			break;

		case 2:
			estado="Equipos";
			break;
		}
		WWW www = new WWW ("https://us-central1-soccerapp-5d7ac.cloudfunctions.net/ChangeEventState?estado="+estado);
		ContinuationManager.Add(() => www.isDone, () =>
				 {
				     if (!string.IsNullOrEmpty(www.error)) Debug.Log("WWW failed: " + www.error);
					EditorUtility.DisplayDialog("CHANGE EVENT STATE",www.text,"OK");
				 });
	}
}
