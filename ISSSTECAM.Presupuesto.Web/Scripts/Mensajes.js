/*-----------------------------------------------------------------------------------------------------*/
/*Funciones Globales para los Mensajes*/
//Rojo
function MensajeError(Texto) {
    noty({
        text: "<span style='font-size:13px; font-weight: bold;'>" + "  " + Texto + "  " + "</span>",
        type: 'error',
        layout: 'topCenter',
        dismissQueue: false,        
        modal: true
    });
}
//Verde
function MensajeExito(Texto) {
    noty({
        text: "<span style='font-size:13px; font-weight: bold;'>" + "  " + Texto + "  " + "</span>",
        type: 'success',
        layout: 'topCenter',
        dismissQueue: false,
        modal: true
    });
}
//Azul
function MensajeInformacion(Texto) {
    noty({
        text: "<span style='font-size:13px; font-weight: bold;'>" + "  " + Texto + "  " + "</span>",
        type: 'information',
        layout: 'topCenter',
        dismissQueue: false,
        modal: true
    });
}
//Blanco
function MensajeAlerta(Texto)
{
    noty({
        text: "<span style='font-size:13px; font-weight: bold;'>" + "  " + Texto + "  " + "</span>",
        type: 'alert',
        layout: 'topCenter',
        dismissQueue: false,
        modal: true
    });
}
//Amarillo
function MensajeAdvertencia(Texto)
{
    noty({
        text: "<span style='font-size:13px; font-weight: bold;'>" + "  " + Texto + "  " + "</span>",
        type: 'warning',
        layout: 'topCenter',
        dismissQueue: false,
        modal: true
    });
}
//usar este metodo para detener el envio del servidor si se usa el de abajo el plugin del noti enviara al servidor aunque
//tenga el return false
function Confirmacion(Cadena, s) {

    MensajeConfirmar(Cadena, s);
    return false;
}
function MensajeConfirmar(Texto, IdBtn) {
    noty({
        text: "<span style='font-size:15px; font-weight: bold;'>" + "  " + Texto + "  " + "</span>",
        type: 'Confirm',
        layout: 'center',
        dismissQueue: false,
        modal: true,        
        buttons: [{
            addClass: 'btn btn-success',
            text: "<span style='font-size:14px; font-weight: bold;'>" + 'Si' + "</span>",
            onClick: function ($noty) {                
                
                $noty.close();
                var name = IdBtn.replace(/_/g, "$");
                __doPostBack(name, "");                
            }
        },
        {
            addClass: 'btn  btn-danger',
            text: "<span style='font-size:14px; font-weight: bold;'>" + 'No' + "</span>",
            onClick: function ($noty) { 
                $noty.close();               
            }
        }]
    });
    return false;
}
function MensajeConfirmarDVE(Texto, sender, event) {
    noty({
        text: "<span style='font-size:13px; font-weight: bold;'>" + "  " + Texto + "  " + "</span>",
        type: 'Confirm',
        layout: 'center',
        dismissQueue: false,
        modal: true,
        modal: true,
        buttons: [{
            addClass: 'btn btn-primary',
            text: "<span style='font-size:14px; font-weight: bold;'>" + 'Si' + "</span>",
            onClick: function ($noty) {
                $noty.close();
                ejecutar = true;
                sender.DoClick();
            }
        },
        {
            addClass: 'btn btn-danger',
            text: "<span style='font-size:14px; font-weight: bold;'>" + 'No' + "</span>",
            onClick: function ($noty) {
                $noty.close();
                ejecutar = false;
                sender.DoClick();
            }
        }]
    });
}


//MensajeCargando
function MensajeCargando() {
    $.blockUI({
        message: "<h3>Espere un momento</h3>" +
            "<div class='sk-three-bounce'><div class='sk-child sk-bounce1'></div><div class='sk-child sk-bounce2'></div><div class='sk-child sk-bounce3'></div></div>",
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        }
    });


}
function OcultarMensajeCargando() {
    $.unblockUI();
}