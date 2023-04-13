
var miJason = '';
//var ei = false;

$(document).ready(function () {
    $('.single-checkbox').on('change', function () {
        if ($(this).is(':checked')) {
            $('.single-checkbox').not(this).prop('checked', false);


        }
    });
});




function validaItemSeleccionado(item) {

    let areaDescripcion = document.getElementById("ctl00_MainContent_areaDescripcion");
    let guardarUsuario = document.getElementById("ctl00_MainContent_GuardarUsuario");
    if (item.value != "0") {
        areaDescripcion.classList.replace("visibleOff", "visibleOn");
        guardarUsuario.disabled = false;
    } else {
        areaDescripcion.classList.replace("visibleOn", "visibleOff");
        alert("debe seleccionar un item para mostrar mas opciones");
    }

}

function muestraAmbosChk(opcion) {

    let areaAmbos = document.getElementById("ctl00_MainContent_areaAmbos");
    let chkIcono = document.getElementById("ctl00_MainContent_divIcono");
    let chkIG = document.getElementById("ctl00_MainContent_divIG");
    let seleccionArchivoIcono = document.getElementById("seleccionArchivoIcono");
    //let chkCambiarIcono = document.getElementById("ctl00_MainContent_chkCambiarIcono");


    opcion.checked ? areaAmbos.classList.replace("visibleOff", "visibleOn") : areaAmbos.classList.replace("visibleOn", "visibleOff");
    opcion.checked ? chkIcono.classList.replace("visibleOff", "visibleOn") : chkIcono.classList.replace("visibleOn", "visibleOff");
    opcion.checked ? chkIG.classList.replace("visibleOff", "visibleOn") : chkIG.classList.replace("visibleOn", "visibleOff");

    if (opcion.checked) {
        seleccionArchivoIcono.classList.replace("visibleOff", "visibleOn");
        seleccionArchivoIcono.setAttribute('required','required');
    }
    else
    {
        seleccionArchivoIcono.classList.replace("visibleOn", "visibleOff");
        seleccionArchivoIcono.removeAttribute('required');
    }


    //opcion.checked ? chkCambiarIcono.classList.replace("visibleOff", "visibleOn") : chkCambiarIcono.classList.replace("visibleOn", "visibleOff");

}

function muestraIconoCargar() {

    let checkIcono = document.getElementById("ctl00_MainContent_agregarIcono");
    let checkIG = document.getElementById("ctl00_MainContent_agregarIG");
    if (checkIcono.checked == true) {
        checkIG.checked = false;

        let areaIcono = document.getElementById("ctl00_MainContent_areaIcono");
        areaIcono.classList.replace("visibleOff", "visibleOn");
        let areaIG = document.getElementById("ctl00_MainContent_areaIG");
        areaIG.classList.replace("visibleOn", "visibleOff");

    }
    else {
        let areaIcono = document.getElementById("ctl00_MainContent_areaIcono");
        areaIcono.classList.replace("visibleOn", "visibleOff");
    }
}
//function muestraCargar(opcion) {

//    let chkCambiarIcono = document.getElementById("ctl00_MainContent_chkCambiarIcono");
//    let seleccionArchivoIcono = document.getElementById("seleccionArchivoIcono");

//    if (chkCambiarIcono.checked) {
//        seleccionArchivoIcono.classList.replace("visibleOff", "visibleOn");
//    } else {
//        seleccionArchivoIcono.classList.replace("visibleOn", "visibleOff");
//    }

//}

function muestraIGCargar() {

    let checkIcono = document.getElementById("ctl00_MainContent_agregarIcono");
    let checkIG = document.getElementById("ctl00_MainContent_agregarIG");
    if (checkIG.checked == true) {
        checkIcono.checked = false;

        let areaIG = document.getElementById("ctl00_MainContent_areaIG");
        areaIG.classList.replace("visibleOff", "visibleOn");
        let areaIcono = document.getElementById("ctl00_MainContent_areaIcono");
        areaIcono.classList.replace("visibleOn", "visibleOff");
    }
    else {
        let areaIG = document.getElementById("ctl00_MainContent_areaIG");
        areaIG.classList.replace("visibleOn", "visibleOff");
    }


}

function archivoIcono() {

    let ei = false;

    let = btnGuardar = document.getElementById("ctl00_MainContent_GuardarUsuario");

    let nombreArchivo = document.getElementById("seleccionArchivoIcono").files[0].name;
    let ruta = "../Content/images/medidas/medidasgenerales/" + nombreArchivo;
    //existeImagen(nombreArchivo);
    //ei = existeImagen(nombreArchivo);

    for (let i = 0; i < miJason.length; i++) {
        if (ruta == miJason[i].rutaImagen) {
            ei = true;
        }
    }

    if (!ei) {

        const $seleccionArchivoIcono = document.querySelector("#seleccionArchivoIcono"),
            $imagenPrevisualizacionIcono = document.querySelector("#imagenPrevisualizacionIcono");

        // Los archivos seleccionados, pueden ser muchos o uno
        const archivos = $seleccionArchivoIcono.files;
        // Si no hay archivos salimos de la función y quitamos la imagen
        if (!archivos || !archivos.length) {
            $imagenPrevisualizacionIcono.src = "";
            return;
        }

        // Ahora tomamos el primer archivo, el cual vamos a previsualizar
        const primerArchivo = archivos[0];
        // Lo convertimos a un objeto de tipo objectURL
        const objectURL = URL.createObjectURL(primerArchivo);
        // Y a la fuente de la imagen le ponemos el objectURL
        $imagenPrevisualizacionIcono.src = objectURL;

        btnGuardar.disabled = false;
    }
    else {
        btnGuardar.disabled = true;
        alert("imagen existente");
    }

}

function archivoGrande() {

    const $seleccionArchivoGrande = document.querySelector("#seleccionArchivoGrande"),
        $imagenPrevisualizacionGrande = document.querySelector("#imagenPrevisualizacionGrande");

    // Los archivos seleccionados, pueden ser muchos o uno
    const archivos = $seleccionArchivoGrande.files;
    // Si no hay archivos salimos de la función y quitamos la imagen
    if (!archivos || !archivos.length) {
        $imagenPrevisualizacionGrande.src = "";
        return;
    }

    // Ahora tomamos el primer archivo, el cual vamos a previsualizar
    const primerArchivo = archivos[0];
    // Lo convertimos a un objeto de tipo objectURL
    const objectURL = URL.createObjectURL(primerArchivo);
    // Y a la fuente de la imagen le ponemos el objectURL
    $imagenPrevisualizacionGrande.src = objectURL;

}


function existeImagen(nombreArchivo) {

    fetch("../../Json/ObtenerListaImagenes")
        .then(res => res.ok ? Promise.resolve(res) : Promise.resolve(res))
        .then(res => res.json())
        .then(res => {
            miJason = res

            //let ruta = "../Content/images/medidas/medidasgenerales/" + nombreArchivo;
            //for (let i = 0; i < res.length; i++) {
            //    if (ruta == res[i].rutaImagen) {
            //        ei = true;
            //    } else {
            //        ei = false;
            //    }
            //}
        })

    //return ei;
    //return setTimeout(busca(nombreArchivo), 1000);
}

function busca(nombreArchivo) {

    let ei = false;
    for (let i = 0; i < miJason.length; i++) {
        if (ruta == miJason[i].rutaImagen) {
            ei = true;
        }
    }

    return ei;

}

function seleccionarImagen(chk_imagen) {
    if (chk_imagen.checked) {
        let valorCodificacion = chk_imagen.getAttribute('data-codificacion');
        $('#nombreIcono').val(valorCodificacion);



    }
}