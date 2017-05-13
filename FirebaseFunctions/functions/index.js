const functions = require('firebase-functions');

const admin =require("firebase-admin");
admin.initializeApp(functions.config().firebase);

//Envia push notification a un usuario especifico (string id,string mensaje)
exports.SendMessage = functions.https.onRequest((req, res) => {
  const id = req.query.id;
  const mensaje=req.query.mensaje;
  const ref= admin.database().ref('Jugadores/'+id);

  ref.once("value").then(function(snapshot) {
    var data = snapshot.val();
    if(data==null){
      res.send("No se encuentra jugador");
    }else{
      var payload={
        notification: {
          title: "Mensaje",
          body: mensaje,
          sound: "default"
        }
      };
      var registrationToken=data.token;
      admin.messaging().sendToDevice(registrationToken,payload)
      res.send("Mensaje enviado a "+data.nombre);
    }
  });
});


//Agrega jugadores al evento (int cantidad)
exports.AddPlayerstoEvent=functions.https.onRequest((req,res)=>{
    var cantidad=req.query.cantidad;
    const cantidadconst=cantidad;
    const refEventoJugadores= admin.database().ref('Evento/Jugadores');
    const refJugadores= admin.database().ref('Jugadores');
    var cantidadusados=0;

    refJugadores.once("value").then(function(snapshot){
        snapshot.forEach(function(childSnapshot) {
          if(childSnapshot.hasChild("Evento")){
            cantidadusados++;
          }
        });

        if(cantidad<=(snapshot.numChildren()-cantidadusados)){
            snapshot.forEach(function(childSnapshot) {
              if(!childSnapshot.hasChild("Evento")){
                if(cantidad>0){
                  cantidad--;
                  childSnapshot.child("Evento").ref.set(true);
                  refEventoJugadores.child("Hombres").child(childSnapshot.key).set("");
                }
              }
            });          
          res.send(cantidadconst.toString()+" jugadores agregados al evento");
        }else{
          res.send("Cantidad superior a cantidad de jugadores");
        }
    });
});

//Crea Jugadores Random(int cantidad)
exports.CreatePlayers=functions.https.onRequest((req,res)=>{
  const cantidad=req.query.cantidad;
  var lista="";
  const refUsuarios= admin.database().ref('Usuarios/');
  const refJugadores= admin.database().ref();
    refUsuarios.once("value").then(function(snapshot) {
      var cantidadlibres=0;
      snapshot.forEach(function(childSnapshot) {
            if(!childSnapshot.hasChild("Usado")){
              cantidadlibres++;
            }
          });
      if(cantidadlibres>=cantidad){
        for (var i = cantidad - 1; i >= 0; i--) {
          var num=Math.floor(Math.random() * (snapshot.numChildren() - 0) + 0);

          while(snapshot.child(num.toString()).hasChild("Usado")){
              num=Math.floor(Math.random() * (snapshot.numChildren() - 0) + 0);
          }
          lista+=snapshot.child(num.toString()).child("nombre").val();
          lista+="\n";
          snapshot.child(num.toString()).child("Usado").ref.set("true");
          var newPlayer=refJugadores.child("Jugadores").push();
          var filename=snapshot.child(num.toString()).child("foto").val().replace("http://images.etermax.com/rrhh/staff/","").replace(".jpg","");
          var email=snapshot.child(num.toString()).child("nombre").val().replace(" ",".").toLowerCase();
          newPlayer.set({
            "nombre":snapshot.child(num.toString()).child("nombre").val(),
            "puesto":snapshot.child(num.toString()).child("puesto").val(),
            "amonestaciones":0,
            "foto":snapshot.child(num.toString()).child("foto").val(),
            "filename":filename,
            "userid":newPlayer.ref.key,
            "token":"fakeuser",
            "email":email+"@etermax.com"
          });
      }
    }else{
      res.send("No hay cantidad libres de usuarios: "+cantidadlibres.toString());
    }
      res.send(lista);
    });
});

//borra todos los jugadores y saca el estado "Usado" de los usuarios ()
exports.ClearUsedUsers=functions.https.onRequest((req,res)=>{

  const refjugadores= admin.database().ref('Jugadores');

  refjugadores.once("value").then(function(snapshot) {
    snapshot.forEach(function(childSnapshot) {
       if(childSnapshot.child("token").val()=="fakeuser") {
           childSnapshot.ref.remove().then(function() {
              console.log("Remove succeeded.")
            }).catch(function(error) {
              console.log("Remove failed: " + error.message)
            });
        
      }
    });
  });

  var num=0;
var query = admin.database().ref("Usuarios").orderByKey();
query.once("value").then(function(snapshot) {
    snapshot.forEach(function(childSnapshot) {
      if(childSnapshot.child("Usado").val()!=null) {
        childSnapshot.child("Usado").ref.remove().then(function() {
            console.log("Remove succeeded.")
        }).catch(function(error) {
            console.log("Remove failed: " + error.message)
        });
      }
  });
});

  res.send("Done");
});

//crea un usuario (string email,string password)
exports.CreateNewUser=functions.https.onRequest((req,res)=>{
var lista="Done";
const email=req.query.email;
const password=req.query.password;
admin.auth().createUser({
    email: email,
    emailVerified: false,
    password: password
  })
  res.send(lista);
});




//borra un usuario especifico (string email)
exports.DeleteUser=functions.https.onRequest((req,res)=>{
const email=req.query.email;
admin.auth().getUserByEmail(email)
  .then(function(userRecord) {
  	admin.auth().deleteUser(userRecord.uid)
  .then(function() {
    console.log("Successfully deleted user");
    res.send("Successfully deleted user");
  })
  .catch(function(error) {
    console.log("Error deleting user:", error);
    res.send(error.toString());
  });
  })
  .catch(function(error) {
  	res.send("ERROR FETCHING");
    console.log("Error fetching user data:", error);
  }); 
});