const functions = require('firebase-functions');

const admin =require("firebase-admin");
admin.initializeApp(functions.config().firebase);


exports.newPlayer = functions.database.ref("Jugadores/{newplayer}/nombre").onWrite(event =>{

	if (event.data.previous.exists()) {
        return;
      }
      // Exit when the data is deleted.
      if (!event.data.exists()) {
        return;
      }
	const request =event.data.val();
	var payload={
		notification: {
    		title: "Nuevo Jugador Creado",
    		body: event.data.val()+" creo su usuario"
  		}
	};
	var registrationToken="fMBwu_nJF7o:APA91bFmYW6fTPzvDFBP0_h-o1BBN0inIsKdQFtZ7cB0KVn5PvPFTlTaJjtnS_fWtOFdzV-cRog4tzHw-bf6MM4YU2auZ9gLW_rcu0_QgtQ6iBZugkBVev9kMfUyqmLk_BazeuBR0n9Z";


	admin.messaging().sendToDevice(registrationToken,payload)

});

