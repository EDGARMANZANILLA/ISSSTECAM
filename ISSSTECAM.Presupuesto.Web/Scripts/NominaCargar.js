//Javascript completamente realizado por Kevin A. Valle Landa



 function mostrarModal(){
        document.getElementById('Modal').style.display = 'inline';
        document.getElementById('Grafos').style.display = 'inline';
}
//funcion para ocultar el modal y mostrar los botones de guardar tabla en PDF y guardar el archivo en la BD
function ocultarModal() {
    //mostrar botones de guardadado
    //document.getElementById('ConvertPDF').style.display = 'inline';
    //document.getElementById('guardar').style.display = 'inline';

   // document.getElementById('Formulario').style.display = 'inline';
    document.getElementById('MenuFecha').style.display = 'inline-block';
    document.getElementById('MenuFecha2').style.display = 'inline-block';
    document.getElementById('MenuFecha3').style.display = 'inline-block';

    //document.getElementById('MenuFecha3').style.display = 'block';
    document.getElementById('Modal').style.display = 'none';
    document.getElementById('btnGuarda').style.display = 'inline';
    document.getElementById('nomina').style.display = "inline";
    document.getElementById('Deshacer').style.display = "inline";
    //getAño('#AutoSelec');
    $.unblockUI();
}
function ocultarModalReporte() {
    document.getElementById('Modal2').style.display = 'none';
    //$.unblockUI();
}

//funcion para ocultar loa botones y las graficas, además de borrar la tabla generada 
function Delete() {
    document.getElementById('Modal').style.display = 'none';
    document.getElementById('CuerpoTabla').innerHTML = '';
    document.getElementById('Grafos').style.display = 'none';
}
function Comienzo() {
    document.getElementById('Enviar').style.display = 'inline';
    //document.getElementById('Formulario1').style.display = 'inline';
    //document.getElementById('Formulario2').style.display = 'inline';
    //document.getElementById('FormularioNomina1').style.display = 'inline';
    //document.getElementById('FormularioNomina2').style.display = 'inline';
    //document.getElementById('CargarAño').style.display = 'none';
    //document.getElementById('CargarAño').style.display = 'none';
    document.getElementById('nominaReporte').style.display = 'inline-block';
    //document.getElementById('nominaReporte2').style.display = 'inline-block';
    document.getElementById('MenuFecha4').style.display = 'inline-block';
    document.getElementById('MenuFecha5').style.display = 'inline-block';
    document.getElementById('MenuFecha6').style.display = 'inline-block';
    document.getElementById('MenuFecha7').style.display = 'inline-block';
    document.getElementById('MenuFecha8').style.display = 'inline-block';
    document.getElementById('MenuFecha9').style.display = 'inline-block';
    ObtenAño();

    
}

function HideModalReport() {
    document.getElementById('Modal').style.display = 'none';
    document.getElementById('ConvertPDF').style.display = 'none';
    $.unblockUI();
}
//Funcion para guardar la tabla como un archivo pdf
function CrearPDF() {
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

function crearTablaConceptos(ArrayConceptos) {
    //dibujar tabla
    var ArrayTabla = null;
    if (ArrayTabla == null) {
        ArrayTabla = ArrayConceptos;
        const CUERPOTABLA = document.querySelector('#CuerpoTabla');
        //recorrer todos los datos 
        ArrayTabla.forEach(Dato => {
            //crear un <tr>
            const TR = document.createElement("tr");
            // Creamos el <td> de Nómina y lo adjuntamos a tr
            let tdConcepto = document.createElement("td");
            tdConcepto.textContent = Dato.Concepto; //el textContent del td es el concepto
            TR.appendChild(tdConcepto);
            // el td de nómina
            let tdTotal = document.createElement("td");
            tdTotal.textContent = "$  "+Dato.Total;
            TR.appendChild(tdTotal);

            CUERPOTABLA.appendChild(TR);
            //y el ciclo se repite hasta q se termine de recorrer el arreglo
        })
        //
        const SUMATOTAL = document.querySelector('#PieDeTabla')
        var SumatoriaTotal = 0;
        var Totales = "Suma de Totales";
        const TR2 = document.createElement("tr");
        let TdTotales = document.createElement("td");
        TdTotales.textContent = Totales;
        TR2.appendChild(TdTotales);
        let TdSuma = document.createElement("td");
        ArrayTabla.forEach(Dato => {
            SumatoriaTotal = SumatoriaTotal + Dato.Total;
        })
        TdSuma.textContent = "$  "+SumatoriaTotal;
        TR2.appendChild(TdSuma);
        SUMATOTAL.appendChild(TR2);
    }
}