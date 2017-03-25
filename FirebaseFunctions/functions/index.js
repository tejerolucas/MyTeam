const functions = require('firebase-functions');

const admin =require("firebase-admin");
admin.initializeApp(functions.config().firebase);

//Nuevo Jugador
exports.newPlayer = functions.database.ref("Jugadores/{newplayer}/nombre").onWrite(event =>{
	if (event.data.previous.exists()) {
        return;
      }
      if (!event.data.exists()) {
                return;
      }
	const request =event.data.val();
	var     payload={
		notification: {
    		   title: "Nuevo Jugador Creado",
    		body: event.data.val()+" creo su usuario"
  		}
	};
	var registrationToken="fMBwu_nJF7o:APA91bFmYW6fTPzvDFBP0_h-o1BBN0inIsKdQFtZ7cB0KVn5PvPFTlTaJjtnS_fWtOFdzV-cRog4tzHw-bf6MM4YU2auZ9gLW_rcu0_QgtQ6iBZugkBVev9kMfUyqmLk_BazeuBR0n9Z";
	admin.messaging().sendToDevice(registrationToken,payload)
});

//HTTP Request Agrega 10 jugadores
exports.AddPlayersEvent = functions.https.onRequest((req, res) => {
  for (var i = 10 - 1; i >= 0; i--) {
    admin.database().ref('/Evento/Jugadores').push({i: "original"});
  }
  res.send("Agregue 10 Jugadores al Evento!");
});


exports.ChangeEventState = functions.https.onRequest((req, res) => {
  const original = req.query.estado;
  if(original=="true"){
    admin.database().ref("/Evento/Habilitado").set(true);
    res.send("Evento Activo");
  }else{
    admin.database().ref("/Evento/Habilitado").set(false);
    res.send("Evento Inactivo");
  }
  
});