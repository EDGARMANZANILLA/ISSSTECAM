//Javascript completamente realizado por Kevin A. Valle Landa



function Quincena() {
    var texto;
    var indice = document.formul.quince.selectedIndex;
    var indice2 = document.formul.mes.selectedIndex;



    // var indice3 = document.formul.año.selectedIndex;
    //var valor = document.formul.quince.options[indice].value;
    //texto = "valor de la opcion escogida: " + valor;
    var quincenaEscogida = document.formul.quince.options[indice].text;
    texto = "Quincena escogida: " + quincenaEscogida;
    var mesEscogido = document.formul.mes.options[indice2].text;
    texto += " ,Mes escogido: " + mesEscogido;
    var añoEscogido = parseFloat(document.getElementsByName('año')[0].value);
    if (isNaN(añoEscogido)) {
        alert("El valor ingresado no es número válido");
        return;
    } else {
        texto += " del año " + añoEscogido;
    }

    //document.write(Año.getFullYear());
    //document.getElementById('AutoSelec').write(Año.getFullYear());
    //var mesEscogido = document.getElementById('AutoSelec').month;
    //texto += " ,Mes escogido: " + mesEscogido;
    alert(texto);
    
    document.getElementById('Formulario').style.display = 'none';
    document.getElementById('ConvertPDF').style.display = 'inline';
    document.getElementById('guardar').style.display = 'inline';


   /* int quincena = $('#txtNombre').val();
    string mes =
        int ano = $('#txtApellido').val();
        */

   // var dataPost = { Nombre: indice, Apellido: 'msg' };
    var actionData = {quincena: indice , mes : indice2 , ano:  indice  };
   // var actionData = "{'quincena': '" + indice + "','mes': '" + indice2 + "', 'ano': '" + indice + "' }";

    $.ajax(
        {
            url: '@Url.Action("FechaG", "Nomina")',
            data: actionData,
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            success: function (msg) { alert(msg); },
            error: function (result) {
                //alert("ERROR " + result.status + ' ' + result.statusText);
            }
        });

}

//<script type=” text /javascript”>
//var d = new Date()	
//document.write(d.getDate());
//</script >


//getFullYear()	
function getFullYear() {
    var dateControl = document.querySelector('input[type="date"]');
    dateControl.value = '2020';
}
 function ShowModal(){
        document.getElementById('Modal').style.display = 'inline';
        document.getElementById('Grafos').style.display = 'inline';
}
//funcion para ocultar el modal y mostrar los botones de guardar tabla en PDF y guardar el archivo en la BD
function HideModal() {
    //mostrar botones de guardadado
    //document.getElementById('ConvertPDF').style.display = 'inline';
    //document.getElementById('guardar').style.display = 'inline';
    document.getElementById('Formulario').style.display = 'inline';
    document.getElementById('Modal').style.display = 'none';
    //getAño('#AutoSelec');
    $.unblockUI();
}

//funcion para ocultar loa botones y las graficas, además de borrar la tabla generada 
function Delete() {
    document.getElementById('Modal').style.display = 'none';
    document.getElementById('CuerpoTabla').innerHTML = '';
    document.getElementById('Grafos').style.display = 'none';
}
//Funcion para guardar la tabla como un archivo pdf
function CreatePDF() {
    doc = new jsPDF('l', 'pt', [700, 450]);
    var nombre = $('#cell').html();
    //var specialElementHandlers = {function (element, render) {
    var specialElementHandlers = {
        function(element, render) {
            return true;
        }
    };
    doc.setFontSize(30);
    doc.fromHTML(nombre, 85, 20, { 'width': 10, 'height': 10, 'elementHandlers': specialElementHandlers });

    doc.save('test.pdf');
}