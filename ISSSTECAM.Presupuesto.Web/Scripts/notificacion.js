$(document).ready(function () {
    var conexion = $.hubConnection('http://187.157.24.221/appCuenta');
    conexion.qs = { 'IdUsuario': hdfIdSSO.value, 'u': hdfNumEmpleado.value +'|'+ hdfNombreCompleto.value, 's': 'Consejeria', 'g': '0' };
    hub = conexion.createHubProxy('notificacionHub');
    hub.on('EscribirNotificacion', escribirNotificacion);
    //hub.on('MasNotificaciones', masNotificaciones);
    hub.on('EscribirAviso', escribirAviso);
    hub.on('ActualizarEstatusNotificacion', actualizarEstatusNotificacion)
    conexion.start();
    actualizarEstatus();
    $('#mostrarnotis').click(function () {
        window.open("notificacion.aspx?id=0&idu=" + hdfIdSSO.value, "_parent")
    });
});

function escribirNotificacion(notificaciones){
    var notiMostrados = 0;
    var nuevosnotys = 0;
    $(notificaciones).each(function (i, notificacion) {
        if (notificacion.IdEstatusNotificacion == 1) {
            notificacion.IdEstatusNotificacion = 2;
            if (notiMostrados < 3)
                MostrarNotificacion(notificacion);
            notiMostrados++;
        }
        //Crear la notificacion
        var $a = $('<a/>', {
            'class': 'list-group-item',
            href: '#',
            'data-idNoti': notificacion.IdMensaje,
            'data-estatus': notificacion.IdEstatusNotificacion
        });


        $div = $('<div/>', { 'class': 'media-box' });
        //Agregar icono de app
        var cssleido = '';
        if (notificacion.IdEstatusNotificacion < statusNoti.read)
            cssleido = 'text-unread';
        var $icono = $('<div class="pull-left"><em class="fa fa-comments fa-2x ' + cssleido + '"></em></div>');
        $div.append($icono);
        //agregar Texto
        var $cuerpo = $('<div class="media-box-body clearfix"><p class="m0">' + notificacion.Asunto + '</p><p class="m0 text-muted"><small>' + notificacion.Cuerpo + '</small></p></div>');
        $div.append($cuerpo).appendTo($a);
        $a.click(function () {
            window.open("notificacion.aspx?id=" + notificacion.IdMensaje + "&idu=" + hdfIdSSO.value, "_parent");
        });
        if (notificaciones.length > 1)
            //Cuando inicia los notis vienen ordenados en base a la fecha se agrega uno de tras del otro
            $('.nuevos-notis').append($a);
        else
            //En caso de que solo sea uno se pone al principio
            $('.nuevos-notis').prepend($a);
        //Si tiene un estatus igual a 2 se cuenta como no revisado
        if (notificacion.IdEstatusNotificacion == 2)
            nuevosnotys++;
    });
    if (notificaciones.length) {
        var total = $('#total-notis').html();
        total = parseInt(total) + nuevosnotys;        
        $('#total-notis').html(total);
        if (total > 0)
            $('#total-notis').removeClass('n-total');
    }
}
function MostrarNotificacion (notificacion) {
    noty({
        text: notificacion.Cuerpo,
        type: 'info',
        layout: 'bottomRight',
        dismissQueue: true,
        modal: false,
        closeWith: ['click'],
        timeout: 4000
    });
}

function masNotificaciones(notificaciones){
    $(notificaciones).each(function (i, notificacion) {

    });
    if (notificaciones.length > 0)
        ultimo = notificaciones[notificaciones.length - 1].IdMensaje;
    else
    {
       //remover el boton mas notificaciones
    }
}

function escribirAviso(aviso) {
}
function actualizarEstatusNotificacion(categoria, tipo, idNoti, estatus) {
    $children = $('.nuevos-notis [data-idnoti="' + idNoti + '"]');
    $children.each(function (i, li) {
        var $li = $(li);
        $li.attr({ 'data-estatus': estatus });
        if (estatus == 3) {
            //Descontar del total
            var total = $('#total-notis').html();
            $('#total-notis').html(parseInt(total) - 1);
        }
        if (estatus == 4) {
            //marcar como leido
        }
    });

}

function actualizarEstatus() {
    $('#notis').click(function () {
        //Actualizar los estatus
        $children = $('.nuevos-notis [data-estatus="2"]');
        $children.each(function (i, li) {
            var $li = $(li);
            if ($li.data().estatus == 2) {
                hub.invoke('ActualizarEstatusNotificacion', "Notificaciones", 'unread', $li.data().idnoti, 3);
                $li.attr({ 'data-estatus': 3 });
            }
        });
        //Actualizar el contador
        $('#total-notis').html('0');
    });
}

function obtenerNotificacion() {
    var parametros = window.location.search.replace('?', '');
    parametros = parametros.split('&');
    var id = parametros[0].split('=')[1];
    var idu = parametros[1].split('=')[1];
    $.getJSON('http://187.157.24.229/servicio/NotificacionAPI.svc/notificacion/' + idu + '/' + id + '?callback=?', null, function (notificaciones) {
        $(notificaciones).each(function (i, notificacion) {
            var $propiedades = $.parseJSON(notificacion.PropiedadesApp); var estilo = 'panel ', texto;
            
            var noty = { id: notificacion.IdMensaje, status: notificacion.IdEstatusNotificacion, user: notificacion.Destinatario};
            var stringNoty = JSON.stringify(noty);
            
            if (notificacion.IdEstatusNotificacion < statusNoti.read) 
                estilo = estilo + 'n-unread';
            else 
                estilo = estilo + 'n-read';
            $divPanel = $('<div/>', { 'class': estilo, 'id': 'notification' + noty.id });
            $cuerpo = $('<div class="table-responsive n-table">' +
               '<table class="table">' +
                   '<tbody>' +
                       '<tr>' +
                           '<td class="b0 wd-xxs text-center">' +
                               '<div class="n-icono"><i class="fa ' + $propiedades.Icono + '"></div>' +
                           '</td>' +
                           '<td class="b0">' +
                               '<p class="h4 mt0">' +
                                   '<span class="label bg-gray-lighter  mr text-sm visible-lg-inline n-status-text"></span>' +
                                   '<a href="#" class="align-middle text-bold link-unstyled">' + notificacion.Asunto + '</a>' +
                               '</p>' +
                               '<small class="text-muted">' + notificacion.Cuerpo +
                                    '<br>' +
                               'Recibido: <span href="#" class="text-bold text-muted">' + notificacion.FechaCorta + '</span>' +
                           '</small>' +
                       '</td>' +
                       '<td class="n-options b0"><div class="btn-group">' +
                            '<button data-toggle="dropdown" class="btn btn-default" aria-expanded="false">...'+
                            '</button>'+
                            '<ul role="menu" class="dropdown-menu animated fadeIn">'+
                            '<li><a class="n-mark-read" data-noty=' + stringNoty + '></a></li>'+
                            '</ul>'+
                            '</div></td>' +
                   '</tr>' +
               '</tbody>' +
           '</table>' +
       '</div>');
            $divPanel.append($cuerpo).appendTo('#notis-container')
        });
        $('.n-options').on('click', '.n-mark-read', function () {
            var $this = $(this);
            var $noty = $('#notification' + $this.data().noty.id);
            if ($this.data().noty.status == statusNoti.read) {
                $this.data().noty.status = 3;
                $noty.removeClass('n-read').addClass('n-unread');
            }
            else {
                $this.data().noty.status = 4;
                $noty.removeClass('n-unread').addClass('n-read');
            }
            var stringNoty = JSON.stringify($this.data().noty);
            
            hub.invoke('ActualizarEstatusNotificacion', "Notificaciones", 'unread', $this.data().noty.id, $this.data().noty.status);
            $this.attr({ 'data-noty': stringNoty });
        });
        if (id > 0) {
            $noty = $('#notification' + id);
            $('body').animate({ scrollTop: $noty.position().top }, 500);
            $noty.addClass('animated flipInX n-selected').attr({ 'data-delay': 1000 });
            $noty.removeClass('n-unread').addClass('n-read');
        }
    });
    
    //$('.item-options').on('click', '.not-recieved', function () { console.log('no recibir'); });
}
var statusNoti = { send: 1, received: 2, unread: 3, read: 4 };

function testAnimation(element) {
    
        element
          .addClass('anim-running');

        setTimeout(function () {
            element
              .addClass('anim-done')
              .animo({ animation: 'shake', duration: 0.7 });
        }, 5000);

    
}
