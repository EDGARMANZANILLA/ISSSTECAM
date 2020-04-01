//function pageLoad() {
    
//    ConfigurarGrids();


//}
$(document).ready(function () {
    window.history.forward(1);

    //if (hdfNumEmpleado.value != 0) {
        CargarMenu('31004');
    //}
    //Esto se habilita si usan los grid del asp
    //ConfigurarGrids();
});

/*------------------------------  Menus  ----------------------------------------------------------------------*/
//Menu principal
function CargarMenu(NumeroEmpleado) {
    var parametro = { "NumeroEmpleado": NumeroEmpleado};
    $.ajax({
        type: "POST",
        data: JSON.stringify(parametro),
        dataType: "json",
        async: false,
        url: baseUrl+"Servicios/ServicioSistema.asmx/ObtenerModulosporNumeroEmpleado",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            var b = result.d;
            for (i = 0; i < b.length; i++) {
                var urlimagenmod = b[i].UrlImagen.replace('~', '..');
                var icono="";
                if (ValirdarURL(urlimagenmod))
                {
                    icono = "<img style='margin-right: 5px;' src='" + urlimagenmod + "' />";
                }
                else
                {
                    icono = "<em class='" +urlimagenmod + "'></em>";
                }
                var idmodulo = b[i].Nombre.replace(/\s+/g, '');
             
                if (b[i].labelInfo > 0) {

                    $('#menu').append("<li class=''>" +
                                               "<a href='#" + idmodulo + "' title='" + b[i].NombreModulo + "' data-toggle='collapse'> " +
                                               "<div class='pull-right label label-info'>" + b[i].LabelInfo + "</div>" +
                                               icono +
                       "<span data-localize='sidebar.nav." + idmodulo + "'>" + b[i].Nombre + "</span>" +
                   "</a> <ul id=" + idmodulo + " class='nav sidebar-subnav collapse'><li class='sidebar-subnav-header'>" + b[i].Nombre + "</li></ul></li>");
                }
                else {
                    $('#menu').append("<li class=''>" +
                                               "<a href='#" + idmodulo + "' title='" + b[i].Nombre + "' data-toggle='collapse'> " +
                                               icono +
                       "<span data-localize='sidebar.nav." + idmodulo + "'>" + b[i].Nombre + "</span>" +
                   "</a><ul id=" + idmodulo + " class='nav sidebar-subnav collapse'><li class='sidebar-subnav-header'>" + b[i].Nombre + "</li></ul></li>");
                }
                for (x = 0; x < b[i].Opciones.length; x++) {
                    //var urlopc = b[i].opciones[x].UrlOpcion.replace('~', '..');
                    var urlopc = ResolveUrl(b[i].Opciones[x].UrlOpcion);
                    //var urlimgopc = b[i].opciones[x].UrlImagen.replace('~', '..');  
                    var nomOpc = b[i].Opciones[x].Nombre;

                    $("#" + idmodulo).append("<li class=''><a href='" + urlopc + "' target='_self'>" + "<span>" + nomOpc + "</span></a></li>");
                }
            }
            ////if (hdfIdSSO.value != 0) {
            ////    CargarSistemas(hdfIdSSO.value);
                
            ////}
           
        },
        error: function (error) {
            console.log('error');
        }
    });
}

function ResolveUrl(url) {
    return url.replace("~/", baseUrl);
}

//Menu Sistemas
function CargarSistemas(idSSO) {
    $.ajax({
        type: "GET",
        dataType: "jsonp",
        async: false,
        url: "http://187.157.24.229/servicio/jsonapi.svc/Sistema/" + idSSO + "/Herramientas",
        contentType: "application/json; charset=utf-8",
        success: function (sistemas) {
            if (sistemas.length > 0)
            {
                $('#menu').append("<li class='nav-heading'><span data-localize='sidebar.heading.Sistemas'>Aplicaciones</span></li>" +
                    "<li class=''><a class='' href='#apps' title='apps' data-toggle='collapse'><em class='icon-grid'></em><span data-localize='sidebar.nav.element.apps'>Sistemas</span></a>" +
                    "<ul id='apps' class='nav sidebar-subnav collapse' style='margin-left: 30px;'></ul></li>"
                );
                if ($('#menu').find('#apps').length > 0) {
                    for (x = 0; x < sistemas.length; x++) {
                        if (sistemas[x].Activo) {
                            $('#apps').append("<li class=''><a href=\"javascript:GenerarToken(" + idSSO + ",\'" + sistemas[x].NombreServicio + "\',\'" + sistemas[x].Url + "\')\" target='_blank'><em class='fa " + jQuery.parseJSON("{" + sistemas[x].Propiedades + "}").Icono + "'></em><span >" +sistemas[x].NombreSistema+ " </span></a></li>");
                            
                        }
                    }
                }
                var sidebarCollapse = $('.sidebar').find('.collapse');
                sidebarCollapse.on('show.bs.collapse', function (event) {

                    event.stopPropagation();
                    if ($(this).parents('.collapse').length === 0)
                        sidebarCollapse.filter('.in').collapse('hide');
                });
                MarcarActivo();
            }
        },
        error: function (error) {
            console.log('error');
        }
    });
}
//Marcar una opcion como activa
function MarcarActivo() {
    var URLactual = window.location.pathname;
    URLactual = baseUrl.replace("/", "") + URLactual;
    
    var url = URLactual.split("/");
    var urlFormateado = "";
    for (x = 0; x < url.length; x++) {
        if (x != 1)
        {
            if (x == url.length - 1)
                urlFormateado = urlFormateado + url[x] ;
            else
                urlFormateado = urlFormateado + url[x] + "/";
        }           
       
    } 

    $("#menu li a[href='" + urlFormateado + "']").parent().addClass('active');
    $("#menu li a[href='" + urlFormateado + "']").parent().parent().parent().addClass('active');
    $("#menu li a[href='" + urlFormateado + "']").parent().parent().parent().find("ul").addClass('in');

    $("#menu li a[href='" + URLactual + "']").parent().addClass('active');
    $("#menu li a[href='" + URLactual + "']").parent().parent().parent().addClass('active');
    $("#menu li a[href='" + URLactual + "']").parent().parent().parent().find("ul").addClass('in');
}

//Validar url para la imagen del menu princial
function ValirdarURL(s) {
    var regexp = /\.(gif|jpg|jpeg|tiff|png)$/i;  
    return regexp.test(s);
}

/*------------------------------    postbacks  ---------------------------------------------------------------*/
//funicon mara manejar los postbacks del sistema para que bloquee la pantalla

Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

function beginRequest(sender, args) {
    
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
//manejador al finalizar un postback, si ocurre un error cacharlo y mostrarlo 
function EndRequestHandler(sender, args) {
    //si existe un error
    if (args.get_error() != undefined && args.get_error().httpStatusCode == 500) {
        noty({
            text: args.get_error().message.replace('Sys.WebForms.PageRequestManagerServerErrorException:', ""),
            type: 'error',
            layout: 'topCenter',
            dismissQueue: false,
            modal: true,
            closeWith: ['button']
        });
        args.set_errorHandled(true);
    }
    $.unblockUI();
}
/*------------------------------  Configuracion de los grids del gridview asp ----------------------------------------------------------------------*/
function ConfigurarGrids() {
//    //Configurar GridView
//    $(".dataTable").each(function () {
//        var jTbl = $(this);
//        //crear las etiquetas head y foot
//        if (jTbl.find("tbody>tr>th").length > 0) {
//            jTbl.find("tbody").before("<thead><tr></tr></thead>");
//            jTbl.find("thead:first tr").append(jTbl.find("th"));
//            jTbl.find("tbody tr:first").remove();
//            jTbl.find("tbody").after("<tfoot></tfoot>");
//            jTbl.find("thead:first tr").clone().appendTo(jTbl.find("tfoot"));

//            //colocar los inputs de busqueda
//            $("#" + $(this).attr('id') + ' tfoot th').each(function () {
//                var title = $(this).text();
//                if (title != " ")
                    
//                    $(this).html('<input type="text" style="width:100%" class="form-control input-sm datatable_input_col_search" />');
//            });
//            //aplicar plugin para el responsive
//            //$(document).trigger("enhance.tablesaw");

//            //aplicar plugin 
//            var table = $("#" + $(this).attr('id')).DataTable({
//                "dom": '<"top">rt<"bottom"lip><"clear">'
//            });
//            table.order([0, 'desc']).draw();
//            //configurar la busqueda
//            table.columns().every(function () {
//                var that = this;
//                $('input', this.footer()).on('keyup change', function () {
//                    if (that.search() !== this.value) {
//                        that
//                            .search(this.value)
//                            .draw();
//                    }
//                });
//            });
//            // colocar la busqueda en el head
//            var r = $("#" + $(this).attr('id') + ' tfoot tr');
//            r.addClass("search");
//            $("#" + $(this).attr('id') + ' thead').append(r);
//            //ocultar el campo de busqueda por default
//            $(".dataTables_filter").hide();
//        }
//    });
}
/*------------------------------  Botones  ----------------------------------------------------------------------*/

$(function () {
    
    //*************************  botones  *************************
    //agregar    
    $(".btn.agregar").each(function (index) {
        $(this).addClass("btn-success");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-plus'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-plus'></i></br>Agregar"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-plus'></i>Agregar"); }
    });
    //nuevo  
    $(".btn.nuevo").each(function (index) {
        $(this).addClass("btn-success");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-file-text'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-file-text'></i></br>Nuevo"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-file-text'></i>Nuevo"); }
    });
    //aceptar
    $(".btn.aceptar").each(function (index) {
        $(this).addClass("btn-success");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-check'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-check'></i></br>Aceptar"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-check'></i>Aceptar"); }
    });
    //fa-file-text

    //guardar
    $(".btn.guardar").each(function (index) {
        $(this).addClass("btn-success");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-save'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-save'></i></br>Guardar"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-save'></i>Guardar"); }
    });


    //imprimir
    $(".btn.imprimir").each(function (index) {
        $(this).addClass("btn-info");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa icon-printer'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa icon-printer'></i></br>Imprimir"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa icon-printer'></i>Imprimir"); }
    });
    //descargar 
    $(".btn.descargar").each(function (index) {
        $(this).addClass("btn-info");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-download'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-download'></i></br>Descargar"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-download'></i>Descargar"); }
    });
    //detalles    
    $(".btn.detalles").each(function (index) {
        $(this).addClass("btn-info");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-list'></i"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-list'></i></br>Detalles"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-list'></i>Detalles"); }
    });
    //editar
    $(".btn.editar").each(function (index) {
        $(this).addClass("btn-warning");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-pencil'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-pencil'></i></br>Editar"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-pencil'></i>Editar"); }
    });

    //subirarchivo  
    $(".btn.subir").each(function (index) {
        $(this).addClass("btn-warning");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-upload'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-upload'></i></br>Subir"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-upload'></i>Subir"); }
    });


    //Cancelar
    $(".btn.cancelar").each(function (index) {
        $(this).addClass("btn-danger");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-ban'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-ban'></i></br>Cancelar"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-ban'></i>Cancelar"); }
    });

    //Eliminar
    $(".btn.eliminar").each(function (index) {
        $(this).addClass("btn-danger");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-trash'></i>"); }
        if ($(this).hasClass('ti_acciones')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-trash'></i></br>Eliminar"); }
        if ($(this).hasClass('ti_formularios')) { $(this).append("<i class='icon fa fa-trash'></i>Eliminar"); }

    });

    //calendario
    $(".btn.calendario").each(function (index) {
        $(this).addClass("btn-default");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-calendar'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-calendar'></i></br>Calendario"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-calendar'></i>Calendario"); }
    });

    //regresar
    $(".btn.regresar").each(function (index) {
        $(this).addClass("btn-default");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-reply'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-reply'></i></br>Regresar"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-reply'></i>Regresar"); }
    });

    //Enviar
    
    $(".btn.enviar").each(function (index) {
        $(this).addClass("btn-success");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-send'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-send'></i></br>Enviar"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-send'></i>Enviar"); }
    });
    //Seleccionar
    $(".btn.seleccionar").each(function (index) {
        $(this).addClass("btn-default");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-check-square-o'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-check-square-o'></i></br>Seleccionar"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-check-square-o'></i>Seleccionar"); }
    });
    //Deseleccionar     
    $(".btn.deseleccionar").each(function (index) {
        $(this).addClass("btn-default");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-square-o'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-square-o'></i></br>Deseleccionar"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-square-o'></i>Deseleccionar"); }
    });
    //Actualizar
    $(".btn.actualizar").each(function (index) {
        $(this).addClass("btn-default");
        $(this).attr("data-toggle", "tooltip");
        if ($(this).hasClass('i')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-refresh'></i>"); }
        if ($(this).hasClass('ti_accion')) { $(this).addClass("btn-icon"); $(this).append("<i class='icon fa fa-refresh'></i></br>Actualizar"); }
        if ($(this).hasClass('ti_formulario')) { $(this).append("<i class='icon fa fa-refresh'></i>Actualizar") }
    });


})
