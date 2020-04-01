var Contexto, mensajes, ultimo = 0, $nodo = null;

function GenerarToken(IdUsuario, Servicio, Url) {
    Contexto = {
        "IdUsuario": IdUsuario,
        "Servicio": Servicio,
        "Url": Url
    };
    $.ajax({
        type: "GET",        
        dataType: "jsonp",
        async: false,
        url: "http://187.157.24.229/Servicio/jsonapi.svc/Token/" + IdUsuario + "/" + Servicio,
        contentType: "application/json; charset=utf-8",
        success: AbrirSistema,
        error: CapturarError
    });
    
}


function AbrirSistema(result) {
    Contexto.Url = Contexto.Url + '?Token=' + result + '&Servicio=' + Contexto.Servicio;
	window.open(Contexto.Url);
}

function CapturarError(error) {
    var Mensaje = jQuery.parseJSON(error.responseText);
    Contexto.Url = 'http://187.157.24.229/inicio.aspx' + '?Servicio=' + Contexto.Servicio + '&Error=' + Mensaje.Message.replace(/ /g, '+');
	window.open(Contexto.Url);
}

