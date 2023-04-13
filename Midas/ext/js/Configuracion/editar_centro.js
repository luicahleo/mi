
var miJason = '';
//var ei = false;



$(document).ready(function () {
    //$('[type!=\'hidden\'][data-val-required]').after('<span style="color:red; font-size: 20px; vertical-align: middle;">*</span>');
});


function validaTecnologiaSeleccionada(item) {

    let panelZona = document.getElementById("panelDdlZonas");
    let panelAgrupacion = document.getElementById("panelDdlAgrupacion");
    let guardarUsuario = document.getElementById("panelDdlAgrupacion");
    muestraInputCargar();
    if (item != "0") {

        if (item == "7" || item == "8" || item == "9") {
            panelZona.classList.replace("visibleOff", "visibleOn");

            var validator = $("#RequiredFieldValidatorZona");
            ValidatorEnable(validator[0], true);

            if ($("#RequiredFieldValidatorZona").hasClass("ocultar")) {
                $("#RequiredFieldValidatorZona").removeClass("ocultar");
            }
            
            if (item == "7") {
                panelAgrupacion.classList.replace("visibleoff", "visibleOn");
            } else {
                var validator = $("#RequiredFieldValidatorAgrupacion");
                ValidatorEnable(validator[0], false);
                $("#RequiredFieldValidatorAgrupacion").addClass("ocultar");
                panelAgrupacion.classList.replace("visibleOn", "visibleOff");
            }
            ObtenerZonas();
        } else {
            var validator = $("#RequiredFieldValidatorZona");
            ValidatorEnable(validator[0], false);
            $("#RequiredFieldValidatorZona").addClass("ocultar");

            var validator = $("#RequiredFieldValidatorAgrupacion");
            ValidatorEnable(validator[0], false);
            $("#RequiredFieldValidatorAgrupacion").addClass("ocultar");

            panelZona.classList.replace("visibleOn", "visibleOff");
            panelAgrupacion.classList.replace("visibleOn", "visibleOff");
        }


    } else {
        panelZona.classList.replace("visibleOn", "visibleOff");
        panelAgrupacion.classList.replace("visibleOn", "visibleOff");
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
        var validator = $("#RequiredFieldValidatorAgrupacion");
        ValidatorEnable(validator[0], true);
        if ($("#RequiredFieldValidatorAgrupacion").hasClass("ocultar")) {
            $("#RequiredFieldValidatorAgrupacion").removeClass("ocultar");
        }
        ObtenerAgrupacion();
        let tecnologia = document.getElementById("ddlTipo");
        if (idTipo == "5" && tecnologia.value == "9") {
            panelAgrupacion.classList.replace("visibleOn", "visibleOff");
        } else {
            panelAgrupacion.classList.replace("visibleOff", "visibleOn");
        }
    } else if (idTipo == "4" || idTipo == "7") {
        panelZona.classList.replace("visibleOff", "visibleOn");
    } else {
        $("#RequiredFieldValidatorAgrupacion").addClass("ocultar");
        var validator = $("#RequiredFieldValidatorAgrupacion");
        ValidatorEnable(validator[0], false);

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
        guardarUsuario = document.getElementById("panelDdlAgrupacion");

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
    ExisteImagenCentroAjax()

}

function CheckImageSize() {
    var image = document.getElementById("seleccionArchivoIC").files[0];
    createReader(image, function (w, h) {
        guardarUsuario = document.getElementById("panelDdlAgrupacion");

        isAltoAncho = (w > 800 || h > 800) ? true : false;

        if (isAltoAncho) {
            alert("La imagen no debe superar 800 pixels de alto y 800 pixels de ancho");
            guardarUsuario.setAttribute('disabled', 'disabled');
        } else {
            guardarUsuario.removeAttribute('disabled');
        }
    });
}

function ExisteImagenCentroAjax() {

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
        url: '../ExisteImagenCentro', // point to server-side controller method
        data: form_data,
        cache: false,
        contentType: false,
        processData: false,
        data: form_data,
        type: 'post',
        success: function (response) {

            if (response != null) {
                //alert('Imagen centro existente');

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


function archivoIL() {

    const $seleccionArchivoIL = document.querySelector("#seleccionArchivoIL"),
        $imagenPrevisualizacionIL = document.querySelector("#imagenPrevisualizacionIL"),
        guardarUsuario = document.getElementById("panelDdlAgrupacion");

    // Los archivos seleccionados, pueden ser muchos o uno
    const archivos = $seleccionArchivoIL.files;
    // Si no hay archivos salimos de la función y quitamos la imagen
    if (!archivos || !archivos.length) {
        $imagenPrevisualizacionIL.src = "";
        return;
    }

    // Ahora tomamos el primer archivo, el cual vamos a previsualizar
    const primerArchivo = archivos[0];
    // Lo convertimos a un objeto de tipo objectURL
    const objectURL = URL.createObjectURL(primerArchivo);
    // Y a la fuente de la imagen le ponemos el objectURL
    $imagenPrevisualizacionIL.src = objectURL;

    CheckImageSizeL();
    ExisteImagenCentroLogoAjax();
}


function CheckImageSizeL() {
    var image = document.getElementById("seleccionArchivoIL").files[0];
    createReader(image, function (w, h) {
        guardarUsuario = document.getElementById("panelDdlAgrupacion");

        isAltoAncho = (w > 800 || h > 800) ? true : false;

        if (isAltoAncho) {
            alert("La imagen del logo no debe superar 800 pixels de alto y 800 pixels de ancho");
            guardarUsuario.setAttribute('disabled', 'disabled');
        } else {
            guardarUsuario.removeAttribute('disabled');
        }
    });
}

function ExisteImagenCentroLogoAjax() {

    var iCentro = $("#seleccionArchivoIL").val();

    //var boton = $(this).closest('button')[0];

    var form_data = new FormData();
    //var req = new XMLHttpRequest();
    //var formData = new FormData();
    var photo = document.getElementById('seleccionArchivoIL').files[0];
    var central = $('#hdnIdCentral').val();
    form_data.append("photo", photo);
    form_data.append('hdnIdCentral', central);

    var imagen = $.ajax({
        url: '../ExisteImagenLogo', // point to server-side controller method
        data: form_data,
        cache: false,
        contentType: false,
        processData: false,
        data: form_data,
        type: 'post',
        success: function (response) {

            if (response != null) {
                //alert('Imagen logo existente');

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