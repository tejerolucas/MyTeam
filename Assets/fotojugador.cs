using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using Firebase.Database;

public class fotojugador : MonoBehaviour {
	public Image foto;
	public Image anillo;
	private string filename;
	public string nombre;
	public string key;

	public void SetPicture(string url){
		key = url;
		DatabaseReference refJugador = FirebaseDatabase.DefaultInstance.GetReference ("Jugadores");
		refJugador.Child(url).GetValueAsync().ContinueWith (task => {
			if (task.IsCompleted) {
				DataSnapshot snap=task.Result;
				nombre=snap.Child("nombre").Value.ToString();
				string fotostring=snap.Child("foto").Value.ToString();
				filename = fotostring.Replace ("http://images.etermax.com/rrhh/staff/", "");
				filename = filename.Replace (".jpg", "");
				if (Resources.Load ("Fotos/"+filename) != null) {
					foto.sprite = Resources.Load<Sprite> ("Fotos/" + filename);
				}
			}

		});

			
		
	}

	IEnumerator GetPicture (string url2)
	{
		Debug.Log ("GETPICTURE");
		// Start a download of the given URL
		WWW www = new WWW (url2);
				
		// Wait for download to complete
		yield return www;

		// assign texture
		if (www.error == null) {
			Texture2D tex2d = www.texture;
			Sprite sp = Sprite.Create (www.texture, new Rect (0, 0, tex2d.width, tex2d.height), new Vector2 (0.5f, 0.5f));
				byte[] bytes = tex2d.EncodeToJPG();
				File.WriteAllBytes(Application.dataPath + "/../Assets/Resources/Fotos/"+filename+".jpg", bytes);
				foto.sprite=sp;
				}
		}
}
