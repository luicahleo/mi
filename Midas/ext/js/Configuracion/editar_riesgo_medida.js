
var miJason = '';
//var ei = false;



function validaTecnologiaSeleccionada(item) {

    let panelZona = document.getElementById("panelDdlZonas");
    let panelAgrupacion = document.getElementById("panelDdlAgrupacion");
    let guardarUsuario = document.getElementById("panelDdlAgrupacion");
    muestraInputCargar();
    if (item != "0") {

        if (item == "7" || item == "8" || item == "9") {
            panelZona.classList.replace("visibleOff", "visibleOn");
            ObtenerZonas();
            //guardarUsuario.disabled = false;
        } else {
            panelZona.classList.replace("visibleOn", "visibleOff");
            panelAgrupacion.classList.replace("visibleOn", "visibleOff");
        }
        //guardarUsuario.disabled = false;


    } else {
        panelZona.classList.replace("visibleOn", "visibleOff");
        panelAgrupacion.classList.replace("visibleOn", "visibleOff");
        //guardarUsuario.disabled = true;
    }

}
function muestraInputCargar() {
    var seleccionArchivoIC = document.getElementById("seleccionArchivoIC");
    var seleccionArchivoIL = document.getElementById("seleccionArchivoIL");

    seleccionArchivoIC.classList.replace("visibleOff", "visibleOn");
    seleccionArchivoIL.classList.replace("visibleOff", "visibleOn");

}

function seleccionTecnologia() {

    var idTipo = $("#ddlTipo").val();
    validaTecnologiaSeleccionada(idTipo);
}

function seleccionZona() {
    let panelZona = document.getElementById("panelDdlZonas");
    let panelAgrupacion = document.getElementById("panelDdlAgrupacion");
    let guardarUsuario = document.getElementById("panelDdlAgrupacion");
    var idTipo = $("#ddlZonas").val();
    if (idTipo == "5" || idTipo == "33" || idTipo == "35") {
        ObtenerAgrupacion();
        panelAgrupacion.classList.replace("visibleOff", "visibleOn");
    } else {
        panelZona.classList.replace("visibleOn", "visibleOff");
        panelAgrupacion.classList.replace("visibleOn", "visibleOff");
    }
}

function ObtenerZonas() {

    var idTipo = $("#ddlTipo").val();

    $.ajax({
        url: "../ObtenerZonas", //Your path should be here
        data: { idTipo: idTipo },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                $("#ddlZonas").find('option').remove();
                $.each(datos, function (i, datos) {
                    $("#ddlZonas").append('<option value="' + datos.Value + '">' +
                        datos.Text + '</option>');
                });
            } else {
                $("#ddlZonas").find('option').remove();
            }

        }
    });

}

function ObtenerAgrupacion() {
    var idTipo = $("#ddlTipo").val();
    var idZona = $("#ddlZonas").val();
    $.ajax({
        url: "../ObtenerAgrupacion", //Your path should be here
        data: { idTipo: idTipo, idZona: idZona },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                $("#ddlAgrupacion").find('option').remove();
                $.each(datos, function (i, datos) {
                    $("#ddlAgrupacion").append('<option value="' + datos.Value + '">' +
                        datos.Text + '</option>');
                });
            } else {
                $("#ddlAgrupacion").find('option').remove();
            }
        }
    });
}


function archivoIC() {

    const $seleccionArchivoIC = document.querySelector("#seleccionArchivoIC"),
        $imagenPrevisualizacionIC = document.querySelector("#imagenPrevisualizacionIC"),
        guardarUsuario = document.getElementById("GuardarUsuario");

    // Los archivos seleccionados, pueden ser muchos o uno
    const archivos = $seleccionArchivoIC.files;
    // Si no hay archivos salimos de la función y quitamos la imagen
    if (!archivos || !archivos.length) {
        $imagenPrevisualizacionIC.src = "";
        return;
    }

    // Ahora tomamos el primer archivo, el cual vamos a previsualizar
    const primerArchivo = archivos[0];
    // Lo convertimos a un objeto de tipo objectURL
    const objectURL = URL.createObjectURL(primerArchivo);
    // Y a la fuente de la imagen le ponemos el objectURL
    $imagenPrevisualizacionIC.src = objectURL;

    CheckImageSize();
    //ExisteImagenIconoAjax()

}

function CheckImageSize() {
    var image = document.getElementById("seleccionArchivoIC").files[0];
    createReader(image, function (w, h) {
        guardarUsuario = document.getElementById("GuardarUsuario");

        isAltoAncho = (w > 800 || h > 800) ? true : false;

        if (isAltoAncho) {
            alert("La imagen no debe superar 800 pixels de alto y 800 pixels de ancho");
            guardarUsuario.setAttribute('disabled', 'disabled');
        } else {
            guardarUsuario.removeAttribute('disabled');
        }
    });
}

function ExisteImagenIconoAjax() {

    var iCentro = $("#seleccionArchivoIC").val();

    //var boton = $(this).closest('button')[0];

    var form_data = new FormData();
    //var req = new XMLHttpRequest();
    //var formData = new FormData();
    var photo = document.getElementById('seleccionArchivoIC').files[0];
    var central = $('#hdnIdCentral').val();
    form_data.append("photo", photo);
    form_data.append('hdnIdCentral', central);

    var imagen = $.ajax({
        url: '../ExisteImagenIcono', // point to server-side controller method
        data: form_data,
        cache: false,
        contentType: false,
        processData: false,
        data: form_data,
        type: 'post',
        success: function (response) {

            if (response != null) {
                alert('Imagen icono existente');

                //$.each(response, function (i, response) {

                //    //$('#Subirlogo').hide();
                //    // $('#IMGlogo').attr("src", '../' + response.rutaImagenLogo);
                //    //if (response != null) {

                //    //    alert('Imagen existente');
                //    //}


                //});
            }

        },
        //error: function (response) {
        //    alert('error'); // display error response from the server
        //}
    });




}


function archivoIG() {

    const $seleccionArchivoIG = document.querySelector("#seleccionArchivoIG"),
        $imagenPrevisualizacionIG = document.querySelector("#imagenPrevisualizacionIG"),
        guardarUsuario = document.getElementById("panelDdlAgrupacion");

    // Los archivos seleccionados, pueden ser muchos o uno
    const archivos = $seleccionArchivoIG.files;
    // Si no hay archivos salimos de la función y quitamos la imagen
    if (!archivos || !archivos.length) {
        $imagenPrevisualizacionIG.src = "";
        return;
    }

    // Ahora tomamos el primer archivo, el cual vamos a previsualizar
    const primerArchivo = archivos[0];
    // Lo convertimos a un objeto de tipo objectURL
    const objectURL = URL.createObjectURL(primerArchivo);
    // Y a la fuente de la imagen le ponemos el objectURL
    $imagenPrevisualizacionIG.src = objectURL;

    CheckImageSizeG();
    //ExisteImagenGrandeAjax();
}


function CheckImageSizeG() {
    var image = document.getElementById("seleccionArchivoIG").files[0];
    createReader(image, function (w, h) {
        guardarUsuario = document.getElementById("guardarUsuario");

        isAltoAncho = (w > 800 || h > 800) ? true : false;

        if (isAltoAncho) {
            alert("La imagen del logo no debe superar 800 pixels de alto y 800 pixels de ancho");
            guardarUsuario.setAttribute('disabled', 'disabled');
        } else {
            guardarUsuario.removeAttribute('disabled');
        }
    });
}

function ExisteImagenGrandeAjax() {

    var iCentro = $("#seleccionArchivoIG").val();

    //var boton = $(this).closest('button')[0];

    var form_data = new FormData();
    //var req = new XMLHttpRequest();
    //var formData = new FormData();
    var photo = document.getElementById('seleccionArchivoIG').files[0];
    var central = $('#hdnIdCentral').val();
    form_data.append("photo", photo);
    form_data.append('hdnIdCentral', central);

    var imagen = $.ajax({
        url: '../ExisteImagenGrande', // point to server-side controller method
        data: form_data,
        cache: false,
        contentType: false,
        processData: false,
        data: form_data,
        type: 'post',
        success: function (response) {

            if (response != null) {
                alert('Imagen grande existente');

                //$.each(response, function (i, response) {

                //    //$('#Subirlogo').hide();
                //    // $('#IMGlogo').attr("src", '../' + response.rutaImagenLogo);
                //    //if (response != null) {

                //    //    alert('Imagen existente');
                //    //}


                //});
            }

        },
        //error: function (response) {
        //    alert('error'); // display error response from the server
        //}
    });
}

function createReader(file, whenReady) {
    var reader = new FileReader;
    reader.onload = function (evt) {
        var image = new Image();
        image.onload = function (evt) {
            var width = this.width;
            var height = this.height;
            if (whenReady) whenReady(width, height);
        };
        image.src = evt.target.result;
    };
    reader.readAsDataURL(file);
}


$(function () {
    $("#chkIC").change(function () {

        var hval = $('#hdIdMedida').val(); 

        if ($(this).is(':checked')) {
            $('.loader').show();
            $.ajax({
                url: "../EliminaImagenMedida",
                data: { idMedida: hval, tipoImagen: "icono" },
                type: "post",
                success: function (datos) {
                    if (datos != null) {
                        location.reload();
                    } else {
                    }
                }
            });
        } else {

        }
    });

    $("#chkIG").change(function () {

        var hval = $('#hdIdMedida').val();

        if ($(this).is(':checked')) {
            $('.loader').show();
            $.ajax({
                url: "../EliminaImagenMedida",
                data: { idMedida: hval, tipoImagen: "grande" },
                type: "post",
                success: function (datos) {
                    if (datos != null) {
                        location.reload();
                    } else {
                    }
                }
            });
        } else {

        }
    });
});



//function archivoIC() {

//    const $seleccionArchivoIC = document.querySelector("#seleccionArchivoIC"),
//        $imagenPrevisualizacionIC = document.querySelector("#imagenPrevisualizacionIC"),

//    // Los archivos seleccionados, pueden ser muchos o uno

//    const archivos = $seleccionArchivoIC.files;
//    // Si no hay archivos salimos de la función y quitamos la imagen
//    if (!archivos || !archivos.length) {
//        $imagenPrevisualizacionIC.src = "";
//        return;
//    }

//    // Ahora tomamos el primer archivo, el cual vamos a previsualizar
//    const primerArchivo = archivos[0];
//    // Lo convertimos a un objeto de tipo objectURL
//    const objectURL = URL.createObjectURL(primerArchivo);
//    // Y a la fuente de la imagen le ponemos el objectURL
//    $imagenPrevisualizacionIC.src = objectURL;

//    CheckImageSize();
//    ExisteImagenIconoAjax()

//}

//function CheckImageSize() {
//    var image = document.getElementById("seleccionArchivoIC").files[0];
//    var guardar = document.getElementById("GuardarUsuario");
//    createReader(image, function (w, h) {
//        //guardarUsuario = document.getElementById("panelDdlAgrupacion");

//        isAltoAncho = (w > 800 || h > 800) ? true : false;

//        if (isAltoAncho) {
//            alert("La imagen no debe superar 800 pixels de alto y 800 pixels de ancho");
//            guardar.setAttribute('disabled', 'disabled');
//        } else {
//            guardar.removeAttribute('disabled');
//        }
//    });
//}

//function ExisteImagenIconoAjax() {

//    var form_data = new FormData();
//    //var req = new XMLHttpRequest();
//    //var formData = new FormData();
//    var photo = document.getElementById('seleccionArchivoIC').files[0];
//    var central = $('#hdnIdCentral').val();
//    form_data.append("photo", photo);
//    form_data.append('hdnIdCentral', central);

//    var imagen = $.ajax({
//        url: '../ExisteImagenIcono', // point to server-side controller method
//        data: form_data,
//        cache: false,
//        contentType: false,
//        processData: false,
//        data: form_data,
//        type: 'post',
//        success: function (response) {

//            if (response != null) {
//                alert('Imagen icono existente');
//            }
//        },
//    });
//}

//function archivoIG() {

//    const $seleccionArchivoIL = document.querySelector("#seleccionArchivoIG"),
//        $imagenPrevisualizacionIG = document.querySelector("#imagenPrevisualizacionIG"),

//    // Los archivos seleccionados, pueden ser muchos o uno
//    const archivos = $seleccionArchivoIG.files;
//    // Si no hay archivos salimos de la función y quitamos la imagen
//    if (!archivos || !archivos.length) {
//        $imagenPrevisualizacionIG.src = "";
//        return;
//    }

//    // Ahora tomamos el primer archivo, el cual vamos a previsualizar
//    const primerArchivo = archivos[0];
//    // Lo convertimos a un objeto de tipo objectURL
//    const objectURL = URL.createObjectURL(primerArchivo);
//    // Y a la fuente de la imagen le ponemos el objectURL
//    $imagenPrevisualizacionIG.src = objectURL;

//    CheckImageSizeG();
//    ExisteImagenGrandeAjax();
//}


//function CheckImageSizeG() {
//    var image = document.getElementById("seleccionArchivoIG").files[0];
//    var guardar = document.getElementById("GuardarUsuario");

//    createReader(image, function (w, h) {

//        isAltoAncho = (w > 800 || h > 800) ? true : false;

//        if (isAltoAncho) {
//            alert("La imagen del logo no debe superar 800 pixels de alto y 800 pixels de ancho");
//            guardar.setAttribute('disabled', 'disabled');
//        } else {
//            guardar.removeAttribute('disabled');
//        }
//    });
//}

//function ExisteImagenGrandeAjax() {

//    //var boton = $(this).closest('button')[0];

//    var form_data = new FormData();
//    //var req = new XMLHttpRequest();
//    //var formData = new FormData();
//    var photo = document.getElementById('seleccionArchivoIG').files[0];
//    var central = $('#hdnIdCentral').val();
//    form_data.append("photo", photo);
//    form_data.append('hdnIdCentral', central);

//    var imagen = $.ajax({
//        url: '../ExisteImagenGrande', // point to server-side controller method
//        data: form_data,
//        cache: false,
//        contentType: false,
//        processData: false,
//        data: form_data,
//        type: 'post',
//        success: function (response) {

//            if (response != null) {
//                alert('Imagen Grande existente');
//            }
//        },
//    });
//}

//function createReader(file, whenReady) {
//    var reader = new FileReader;
//    reader.onload = function (evt) {
//        var image = new Image();
//        image.onload = function (evt) {
//            var width = this.width;
//            var height = this.height;
//            if (whenReady) whenReady(width, height);
//        };
//        image.src = evt.target.result;
//    };
//    reader.readAsDataURL(file);
//}