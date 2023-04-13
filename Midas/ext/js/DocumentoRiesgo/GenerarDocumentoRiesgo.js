
$(document).ready(function () {
    $("#MenuDocumentos").css('color', 'black');
    $("#MenuDocumentos").css('background-color', '#ebf1de');
    $("#MenuDocumentos").css('font-weight', 'bold');

    /*$("#botonBorrador").hide();*/
    $('.loader').hide();
    if ($("#textooculto").val() != "") {
        var textofinal = $("#textooculto").val().replaceAll("-salto-", "\n");
        $('#texto_descripcion').val(textofinal);
    }
});

//function verComentarios() {

//    if ($("#filaComentarios").is(':hidden')) {
//        $("#filaComentarios").show();
//    } else {
//        $("#filaComentarios").hide();
//    }

//}


function activarboton() {
    if ($("#texto_descripcion").val() == "") {
        $("#botonBorrador").hide();
    } else {
        $("#botonBorrador").show();
    }

}

function MostrarMensajeDoc(mensaje, tipo, tiempoDuracion) {
    Swal.fire({
        position: 'center',
        icon: tipo,
        title: mensaje,
        showConfirmButton: false,
        timer: tiempoDuracion
    })
}

function generarBorrador() {

    var texto = '';
    texto = $("#texto_descripcion").val();

    if (texto.trim().length === 0) {
        Swal.fire({
            position: 'center',
            icon: 'info',
            title: 'La descripcion no puede estar vacia',
            showConfirmButton: false,
            timer: 2000
        })
    } else {

        var textoformateado = texto.replaceAll('\n', '-salto-');

        Swal.fire({
            title: 'Generar documento borrador',
            text: "Se añadirá la modificación al borrador. ¿Desea continuar?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, generar borrador!',
            cancelButtonText: 'Cancelar',

        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire(
                    'Borrador generado!'
                )
                window.open('/evr/DocumentoRiesgos/GenerarDocumentoBorrador/?descrDoc=' + textoformateado + '');
                // window.location.href = '/evr/DocumentoRiesgos/GenerarDocumentoBorrador/?descrDoc=' + textoformateado + '';
                //setTimeout(function () {
                //$('.loader').hide();
            }
        })
    }
}
function generarDefinitivo() {

    var texto = '';
    texto = $("#texto_descripcion").val();

    if (texto.trim().length === 0) {
        Swal.fire({
            position: 'center',
            icon: 'info',
            title: 'La descripcion no puede estar vacia',
            showConfirmButton: false,
            timer: 2000
        })
    } else {

        Swal.fire({
            title: '¿Generar documento definitivo?',
            text: "Va a generar una nueva versión del documento de riesgos, cualquier modificación posterior generará una nueva revisión, por lo que recuerde visualizar previamente el borrador antes de generar la versión definitiva.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, generar definitiva!',
            cancelButtonText: 'Cancelar',

        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire(
                    'Documento definitivo generado!',
                )
                var texto = '';

                texto = $("#texto_descripcion").val();
                var textoformateado = texto.replaceAll('\n', '-salto-');

                window.location.href = '/evr/DocumentoRiesgos/GenerarDocumentoDefinitivo/?descrDoc=' + textoformateado + '';
                console.log(window.location.href);

            }
        })
    }
    //var input;
    //input = confirm('Va a generar una nueva versión del documento de riesgos, cualquier modificación posterior generará una nueva revisión, por lo que recuerde visualizar previamente el borrador antes de generar la versión definitiva');
    //var texto = '';

    //texto = $("#texto_descripcion").val();
    //var textoformateado = texto.replaceAll('\n', '-salto-');

    //if (input === false) {
    //    return; //break out of the function early
    //} else {
    //    window.location.href = '/evr/DocumentoRiesgos/GenerarDocumentoDefinitivo/?descrDoc=' + textoformateado + '';
    //    console.log(window.location.href);

    //}


    //var input;
    //input = confirm('Va a generar una nueva versión del documento de riesgos. ¿Desea continuar?:');
    //var texto = '';
    //texto = $("#texto_descripcion").val();
    //if (input === false) {
    //    return; //break out of the function early
    //} else {
    //    window.location.href = '/evr/DocumentoRiesgos/GenerarDocumentoDefinitivo/?descrDoc=' + texto +'';
    //}
}


        //function CrearMatriz() {
        //    var array = new Array();

        //    $('#prueba').on {
        //        var ent = $(this)[0].id;

        //        var $chk = $(this).find('[type=checkbox]');

        //        if ($(this).prop("checked")) {
        //            array.push(ent);
        //        }

        //    });


        //    if (array.length > 0) {

        //        $.ajax({
        //            url: "CrearMatrizDesdeAnterior",
        //            data: {id: idVersion },
        //            type: "post",
        //            success: function (datos) {
        //                if (datos != null) {

        //                }


        //            }
        //        });

        //    }
        //}

