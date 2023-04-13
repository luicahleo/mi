function quitarCombo(obj) {
    $(obj).closest('td').find('select').remove();
    $(obj).closest('td').find('input').attr('type', 'text');
    $(obj).hide();
}

///* When the user clicks on the button, 
//toggle between hiding and showing the dropdown content */
function file_changed(prueba) {

}
function estadoAnterior() {
    var riesgo = "";
    if (localStorage.getItem("riesgo") != null) {
        riesgo = localStorage.getItem("riesgo");
        $('.Contenido_' + riesgo).show();
    }
    if (localStorage.getItem("situacion") != null) {
        var situacion = localStorage.getItem("situacion");
        riesgo = localStorage.getItem("riesgo");
        $('.ContenidoSituacion_' + riesgo + '_' + situacion).show();
    }
    localStorage.clear();
    // $('.ContenidoSituacion_1_1').show();

    window.onload = function () {
        var pos = window.name || 0;
        window.scrollTo(0, pos);
    }
    window.onunload = function () {
        window.name = self.pageYOffset || (document.documentElement.scrollTop + document.body.scrollTop);
    }
}

function ocultar(valor) {
    var idRiesgo = '';
    if (valor.hasAttribute('mdriesgo')) {
        idRiesgo = valor.getAttribute('mdriesgo');
    }
    $('#Subir_' + idRiesgo).hide();
    $('#SubirGrande_' + idRiesgo).hide();
}
$(document).ready(function () {
    $('.loader').hide();
    $("#MenuMedidasPreventivas").css('color', 'black');
    $("#MenuMedidasPreventivas").css('background-color', '#ebf1de');
    $("#MenuMedidasPreventivas").css('font-weight', 'bold');
    //ObtenerMedidasGenerales();
    estadoAnterior();

    $('.file').on('change', function () {
        var fileupload = $(this).closest('input')[0];
        var idRiesgo = '';
        if (fileupload.hasAttribute('mdriesgo')) {
            idRiesgo = fileupload.getAttribute('mdriesgo');
            var boton = $('#Subir_' + idRiesgo).show();
        }
        if (fileupload.hasAttribute('mdSituacion')) {
            idRiesgo = fileupload.getAttribute('mdSituacion');
            var boton = $('#Subir_' + idRiesgo).show();
        }
        if (fileupload.hasAttribute('mdSituacionGrande')) {
            idRiesgo = fileupload.getAttribute('mdSituacionGrande');
            var boton = $('#SubirGrande_' + idRiesgo).show();
        }

    });

    $('.fileGrande').on('change', function () {
        var fileupload = $(this).closest('input')[0];
        var idRiesgo = '';
        if (fileupload.hasAttribute('mdriesgo')) {
            idRiesgo = fileupload.getAttribute('mdriesgo');
            var boton = $('#SubirGrande_' + idRiesgo).show();
        }
        if (fileupload.hasAttribute('mdSituacion')) {
            idRiesgo = fileupload.getAttribute('mdSituacion');
            var boton = $('#Subir_' + idRiesgo).show();
        }

    });

    $('.subirImagen').on('click', function () {
        var boton = $(this).closest('button')[0];
        var idRiesgo = '';
        var idmdSituacion = '';
        var idmdSituacionGrande = '';
        if (boton.hasAttribute('mdriesgo')) {
            idRiesgo = boton.getAttribute('mdriesgo');
        }
        if (boton.hasAttribute('mdSituacion')) {
            idmdSituacion = boton.getAttribute('mdSituacion');
        }
        if (boton.hasAttribute('mdSituacionGrande')) {
            idmdSituacionGrande = boton.getAttribute('mdSituacionGrande');
        }

        // var file_data = $('#file_' + idRiesgo).prop('files')[0];
        var form_data = new FormData();
        var req = new XMLHttpRequest();
        var formData = new FormData();

        if (idRiesgo != '') {
            var photo = document.getElementById('file_' + idRiesgo).files[0];
            form_data.append("photo", photo);
            form_data.append("idRiesgo", idRiesgo);
        }

        //if (idRiesgo != '') {
        //    var photo = document.getElementById('fileGrande_' + idRiesgo).files[0];
        //    form_data.append("photo", photo);
        //    form_data.append("idRiesgo", idRiesgo);
        //}

        if (idmdSituacion != '') {
            var photo = document.getElementById('file_' + idmdSituacion).files[0];
            form_data.append("photo", photo);
            form_data.append("idmdSituacion", idmdSituacion);
        }
        if (idmdSituacionGrande != '') {
            var photo = document.getElementById('fileGrande_' + idmdSituacionGrande).files[0];
            form_data.append("photo", photo);
            form_data.append("idmdSituacionGrande", idmdSituacionGrande);
        }

        var imagen = $.ajax({
            url: 'GuardarImagen', // point to server-side controller method
            data: form_data, // what to expect back from the server
            // what to expect back from the server
            cache: false,
            contentType: false,
            processData: false,
            data: form_data,
            type: 'post',
            success: function (response) {

                if (response != null) {
                    $.each(response, function (i, response) {

                        $('#Subir_' + response.id).hide();
                        $('#IMG_' + response.id).attr("src", response.imagen);
                        Swal.fire({
                            position: 'center',
                            icon: 'success',
                            title: 'Imagen modificada',
                            showConfirmButton: false,
                            timer: 1500
                        })
                        location.reload();

                    });
                }

            },
            error: function (response) {
                alert('error'); // display error response from the server
            }
        });
    });

    $('.subirImagenGrande').on('click', function () {
        var boton = $(this).closest('button')[0];
        var idRiesgo = '';
        var idmdSituacion = '';
        if (boton.hasAttribute('mdriesgo')) {
            idRiesgo = boton.getAttribute('mdriesgo');
        }
        if (boton.hasAttribute('mdSituacion')) {
            idmdSituacion = boton.getAttribute('mdSituacion');
        }

        // var file_data = $('#file_' + idRiesgo).prop('files')[0];
        var form_data = new FormData();
        var req = new XMLHttpRequest();
        var formData = new FormData();

        if (idRiesgo != '') {
            var photo = document.getElementById('fileGrande_' + idRiesgo).files[0];
            form_data.append("photo", photo);
            form_data.append("idRiesgo", idRiesgo);
        }
     
        if (idmdSituacion != '') {
            var photo = document.getElementById('fileGrande_' + idmdSituacion).files[0];
            form_data.append("photo", photo);
            form_data.append("idmdSituacion", idmdSituacion);
        }

        var imagen = $.ajax({
            url: 'GuardarImagenGrande', // point to server-side controller method
            data: form_data, // what to expect back from the server
            // what to expect back from the server
            cache: false,
            contentType: false,
            processData: false,
            data: form_data,
            type: 'post',
            success: function (response) {

                if (response != null) {
                    $.each(response, function (i, response) {

                        $('#Subir_' + response.id).hide();
                        $('#IMG_' + response.id).attr("src", response.imagen);
                        Swal.fire({
                            position: 'center',
                            icon: 'success',
                            title: 'Imagen modificada',
                            showConfirmButton: false,
                            timer: 1500
                        })
                        location.reload();

                    });
                }

            },
            error: function (response) {
                alert('error'); // display error response from the server
            }
        });


    });

    $('.EliminarRiesgoMedida').on('click', function () {
        var fila = $(this).closest('tr');
        var contenido = $(this).closest('a');
        var ident = contenido[0].getAttribute('mdRiesgo');

        Swal.fire({
            title: '¿Esta seguro?',
            text: "Esta medida de riesgo será eliminada!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, eliminar!',
            cancelButtonText: 'Cancelar',

        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire(
                    'Borrado!',
                    'La medida de riesgo fue eliminada.',
                    'success'
                )
                fila.hide();
                fila.remove();
                EliminarRiesgoMedida(ident);
            }
        })
        //var resultado = confirm('Se eliminará la medida. ¿Desea continuar?');
        //if (resultado) {
        //    fila.hide();
        //    EliminarRiesgoMedida(ident);
        //}

    });

    $('.EliminarSituacion').on('click', function () {

        var fila = $(this).closest('tr');
        var contenido = $(this).closest('a');
        var ident = contenido[0].getAttribute('situacion');
        var resultado = confirm('Se eliminará la situación y las medidas asociadas a esta. ¿Desea continuar?');
        if (resultado) {

            if (fila[0].parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.hasAttribute('riesgo')) {
                var ri = fila[0].parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.getAttribute('riesgo');
                $('.ContenidoSituacion_' + ri + '_' + ident).remove();
            }
            fila.hide();
            fila.remove();
            EliminarSituacionRiesgo(ident);
        }

    });

    $('.EliminarMedida').on('click', function () {
        var fila = $(this).closest('tr');
        var contenido = $(this).closest('a');
        var ident = contenido[0].getAttribute('medida');

        Swal.fire({
            title: '¿Esta seguro?',
            text: "Esta medida será eliminada!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, eliminar!',
            cancelButtonText: 'Cancelar',

        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire(
                    'Borrado!',
                    'La medida fue eliminada.',
                    'success'
                )
                fila.hide();
                fila.remove();
                EliminarMedidaSituacion(ident);
            }
        })


        //var resultado = confirm('Se eliminará la medida. ¿Desea continuar?');
        //if (resultado) {
        //    fila.hide();
        //    fila.remove();
        //    EliminarMedidaSituacion(ident);
        //}

    });

    $('.EditarMedida').on('click', function () {
        var fila = $(this).closest('tr');
        var medida = parseInt(this.attributes['medida'].value);
        descModal.setAttribute("medida", medida);

        //descModal.value = fila[0].children[1].value;

        $.ajax({
            url: "obtenerMedidaPorIdAjax", //Your path should be here
            data: { medida: medida },
            type: "post",
            success: function (datos) {
                if (datos != null) {

                    var obj = datos.descripcion;
                    var descModal = document.querySelector("#descModal");
                    descModal.value = obj;

                    $('#mi-modal').modal({ show: true });
                }
            }
        });
    });

    $('.EditarRiesgoMedida').on('click', function () {
        var fila = $(this).closest('tr');
        var medida = parseInt(this.attributes['medida'].value);
        descModalRiesgo.setAttribute("medida", medida);

        //descModal.value = fila[0].children[1].value;

        $.ajax({
            url: "obtenerRiesgoMedidaPorIdAjax", //Your path should be here
            data: { medida: medida },
            type: "post",
            success: function (datos) {
                if (datos != null) {

                    var obj = datos.descripcion;
                    var descModalRiesgo = document.querySelector("#descModalRiesgo");
                    descModalRiesgo.value = obj;

                    $('#mi-modal-riesgo').modal({ show: true });
                   // window.location.reload(true);

                }
            }
        });
    });

    $('.AddMedidaGeneral').on('click', function () {

        var riesgo = "";
        var tablaMedidasGenerales = null;
        var indice = $(this).index;
        var table = $(this)[0].parentElement.parentElement.offsetParent;
        if (table.hasAttribute('riesgo')) {
            riesgo = table.getAttribute('riesgo');
            tablaMedidasGenerales = $(table).find('table[class="MedidasGeneralesRiesgo_' + riesgo + '"]');
        }

        var row = $('.MedidasGeneralesRiesgo_' + riesgo + '')[0].insertRow(-1);
        //var row = $('.RegistroNuevo_' + riesgo + '');

        row.innerHTML = FilaNuevaMedidaGeneral('MedidaGeneral');

        row.color = "#ff0f64";
        $(row).addClass('borde');
        $(row).addClass('FilaNueva');
        $(row).attr("riesgo", riesgo);


    });

    $('.AddVariasMedidasRiesgo').on('click', function () {

        var riesgo = "";
        var tablaMedidasGenerales = null;
        var indice = $(this).index;
        var table = $(this)[0].parentElement.parentElement.offsetParent;
        if (table.hasAttribute('riesgo')) {
            riesgo = table.getAttribute('riesgo');
            tablaMedidasGenerales = $(table).find('table[class="MedidasGeneralesRiesgo_' + riesgo + '"]');
        }

        var row = $('.MedidasGeneralesRiesgo_' + riesgo + '')[0].insertRow(-1);
        //var row = $('.RegistroNuevo_' + riesgo + '');

        row.innerHTML = FilaNuevaMedidaGeneralVarias('MedidaGeneral');

        row.color = "#ff0f64";
        $(row).addClass('borde');
        $(row).addClass('FilaNueva');
        $(row).attr("riesgo", riesgo);


    });
    $('.AddMedidaGeneralImagen').on('click', function () {

        var riesgo = "";
        var tablaMedidasGenerales = null;
        var indice = $(this).index;
        var table = $(this)[0].parentElement.parentElement.offsetParent;
        if (table.hasAttribute('riesgo')) {
            riesgo = table.getAttribute('riesgo');
            tablaMedidasGenerales = $(table).find('table[class="MedidasGeneralesRiesgo_' + riesgo + '"]');
        }

        var row = $('.MedidasGeneralesRiesgo_' + riesgo + '')[0].insertRow(-1);
        //var row = $('.RegistroNuevo_' + riesgo + '');

        row.innerHTML = FilaNuevaMedidaGeneralImagen('MedidaGeneral');

        row.color = "#ff0f64";
        $(row).addClass('borde');
        $(row).addClass('FilaNuevaImagen');
        $(row).attr("riesgo", riesgo);


    });

    $('.AddMedida').on('click', function () {

        var riesgo = "";
        var situacion = "";
        var table = $(this)[0].parentElement.parentElement.parentElement.firstElementChild.children[0];

        if (table.hasAttribute('situacion')) {
            situacion = table.getAttribute('situacion');
        }
        var row = table.insertRow(-1);
        row.innerHTML = FilaNueva('Medida');
        //style = "background-color:#ff0f64":
        row.color = "#ff0f64";
        $(row).attr("Situacion", situacion);

        if (table.parentElement.parentElement.parentElement.parentElement.hasAttribute('riesgo')) {
            riesgo = table.parentElement.parentElement.parentElement.parentElement.getAttribute('riesgo');
            $(row).attr("Riesgo", riesgo);
        }

        $(row).addClass('borde');
        $(row).addClass('FilaNueva');

    });
    $('.SubMedidas').on('click', function () {

        var riesgo = "";
        var situacion = "";
        var table = $(this)[0].parentElement.parentElement.parentElement.firstElementChild.children[0];

        if (table.hasAttribute('situacion')) {
            situacion = table.getAttribute('situacion');
        }
        var row = table.insertRow(-1);
        row.innerHTML = FilaNueva('MedidasCompuestas');
        //style = "background-color:#ff0f64":
        row.color = "#ff0f64";
        $(row).attr("Situacion", situacion);

        if (table.parentElement.parentElement.parentElement.parentElement.hasAttribute('riesgo')) {
            riesgo = table.parentElement.parentElement.parentElement.parentElement.getAttribute('riesgo');
            $(row).attr("Riesgo", riesgo);
        }
        $(row).addClass('borde');
        $(row).addClass('FilaNueva');

    });

    $('.AddVariasMedidas').on('click', function () {

        var riesgo = "";
        var situacion = "";
        var table = $(this)[0].parentElement.parentElement.parentElement.firstElementChild.children[0];

        if (table.hasAttribute('situacion')) {
            situacion = table.getAttribute('situacion');
        }
        var row = table.insertRow(-1);
        row.innerHTML = FilaNueva('VariasMedidas');
        //style = "background-color:#ff0f64":
        row.color = "#ff0f64";
        $(row).attr("Situacion", situacion);

        if (table.parentElement.parentElement.parentElement.parentElement.hasAttribute('riesgo')) {
            riesgo = table.parentElement.parentElement.parentElement.parentElement.getAttribute('riesgo');
            $(row).attr("Riesgo", riesgo);
        }

        $(row).addClass('borde');
        $(row).addClass('FilaNueva');

    });
    $('.AddImagenGrandeMedida').on('click', function () {

        var riesgo = "";
        var situacion = "";
        var table = $(this)[0].parentElement.parentElement.parentElement.firstElementChild.children[0];

        if (table.hasAttribute('situacion')) {
            situacion = table.getAttribute('situacion');
        }
        var row = table.insertRow(-1);
        row.innerHTML = FilaNuevaImagenGrandeMedida();
        //style = "background-color:#ff0f64":
        row.color = "#ff0f64";
        $(row).attr("Situacion", situacion);

        if (table.parentElement.parentElement.parentElement.parentElement.hasAttribute('riesgo')) {
            riesgo = table.parentElement.parentElement.parentElement.parentElement.getAttribute('riesgo');
            $(row).attr("Riesgo", riesgo);
        }

        $(row).addClass('borde');
        $(row).addClass('FilaNuevaImagenMedidaPreventiva');

    });


    $('.AddSituacion').on('click', function () {

        var riesgo = "";

        var table = $(this)[0].parentElement.parentElement.firstElementChild;
        if (table.hasAttribute('riesgo')) {
            riesgo = table.getAttribute('riesgo');
        }
        var row = table.insertRow(-1);
        row.innerHTML = FilaNueva('Situacion');
        //style = "background-color:#ff0f64":
        row.color = "#ff0f64";
        $(row).addClass('borde');
        $(row).addClass('FilaNueva');
        $(row).attr("riesgo", riesgo);
        //  CrearFilaMedidaSituacion();
        // CreaFilaMedidaGeneralCentral(tecnologiafila);
    });

    function FilaNuevaMedidaGeneral() {
        var select = '<select id="apartadosajax" class="form-control" style="width:auto;">';
        $.ajax({
            url: "ObtenerApartados", //Your path should be here
            async: false,
            type: "post",
            success: function (datos) {
                /* var select = '<select id="apartadosajax"><option id="NuevoApartado"></option>';*/
                if (datos != null) {
                    $.each(datos, function (i, datos) {
                        select += '<option value="' + datos.Value + '">' + datos.Text + '</option>';
                    });
                    select += '</select>';
                }
            }
        });
        var fila = '';
        //fila += '';
        //fila += '<td class="text-center"  width="5%"><button id="botonNuevo" onclick="quitarCombo(this)">Seleccione apartado</button>' + select + '</select><input placeholder="apartado" name="apartado" hidden type="hidden" value="" id="" class="form-control"></td>'; //esta linea tiene un evento en el boton, lo he quitado para ponerle solamente un label
        fila += '<td class="text-center"  width="5%"><label id="botonNuevo" >Seleccione apartado</label>' + select + '</select><input placeholder="apartado" name="apartado" hidden type="hidden" value="" id="" class="form-control"></td>';
        fila += '<td class="text-center" ><textarea rows="6" cols="50"  style="resize: none;" placeholder="descripcion" name="descripcion"  type="text" value="" id="" class="form-control"></textarea></td>';
        /*         fila += '<td class="text-center"  ><input placeholder="descripcion" name="descripcion" type="text" value="" id="" class="form-control"></td>';*/
        fila += '<td class="text-center"> <a name="Guardar" class="EnviarMedidaGeneral" id="GuardarNuevo" onclick="GuardarMedidaGeneral()"><i class="icon-checkmark" style="color:green;"  title="Guardar"></i></a></td>';
        fila += '<td class="text-center" > <a name="Cancelar" class="CancelarMedidaGeneral" onclick="Cancelar()" id="CancelarNuevo"><i class="icon-cancel" style="color:red;"  title="Cancelar"></i></a></td>';

        return fila;
    }
    function FilaNuevaMedidaGeneralVarias() {
        var select = '<select id="apartadosajax" class="form-control" style="width:auto;">';
        $.ajax({
            url: "ObtenerApartados", //Your path should be here
            async: false,
            type: "post",
            success: function (datos) {
                /* var select = '<select id="apartadosajax"><option id="NuevoApartado"></option>';*/
                if (datos != null) {
                    $.each(datos, function (i, datos) {
                        select += '<option value="' + datos.Value + '">' + datos.Text + '</option>';
                    });
                    select += '</select>';
                }
            }
        });
        var fila = '';
        //fila += '';
        fila += '<td class="text-center" width="5%"><button id="botonNuevo" onclick="quitarCombo(this)">Nuevo</button>' + select + '</select><input placeholder="apartado" name="apartado" hidden type="hidden" value="" id="" class="form-control"></td>';
        fila += '<td class="text-center" ><textarea rows="6" cols="50" style="resize: none;" placeholder="descripcion" name="descripcion" type="text" value="" id="" class="form-control"></textarea></td>';
        /*         fila += '<td class="text-center"  ><input placeholder="descripcion" name="descripcion" type="text" value="" id="" class="form-control"></td>';*/
        fila += '<td class="text-center"> <a name="Guardar" class="EnviarMedidaGeneral" id="GuardarNuevo" onclick="GuardarMedidaGeneralVarias()"><i class="icon-checkmark" style="color:green;"  title="Guardar"></i></a></td>';
        fila += '<td class="text-center" > <a name="Cancelar" class="CancelarMedidaGeneral" onclick="Cancelar()" id="CancelarNuevo"><i class="icon-cancel" style="color:red;"  title="Cancelar"></i></a></td>';

        return fila;
    }

    function FilaNuevaImagenGrandeMedida() {

        var fila = '';
        fila += '<td class="text-center" width="10%" colspan="3"><input id="file_nuevo" name="fichero" type="file" name="img" multiple="" accept="image/*" mdriesgo="4" class="file" ></td>';
        fila += '<td class="text-center" width="60%" ><textarea rows="6" cols="50" style="resize: none;" placeholder="descripcion" name="descripcion" type="text" value="" id="" class="form-control"></textarea></td>';
        fila += '<td class="text-center" width="5%"> <a name="Guardar" class="EnviarMedidaGeneral" id="GuardarNuevo" onclick="GuardarMedidaPreventivaImagen()"><i class="icon-checkmark" style="color:green;"  title="Guardar"></i></a></td>';
        fila += '<td class="text-center" width="5%"> <a name="Cancelar" class="CancelarMedidaGeneral" onclick="Cancelar()" id="CancelarNuevo"><i class="icon-cancel" style="color:red;"  title="Cancelar"></i></a></td>';

        return fila;
    }
    function FilaNuevaMedidaGeneralImagen() {

        var select = '<select id="apartadosajax" class="form-control" style="width:auto;">';
        $.ajax({
            url: "ObtenerApartados", //Your path should be here
            async: false,
            type: "post",
            success: function (datos) {
                /* var select = '<select id="apartadosajax"><option id="NuevoApartado"></option>';*/
                if (datos != null) {
                    $.each(datos, function (i, datos) {
                        select += '<option value="' + datos.Value + '">' + datos.Text + '</option>';
                    });
                    select += '</select>';
                }
            }
        });

        var fila = '';

        //fila += '<td class="text-center" width="5%"><button id="botonNuevo" onclick="quitarCombo(this)">Nuevo</button>' + select + '</select><input placeholder="apartado" name="apartado" hidden type="hidden" value="" id="" class="form-control"></td>';
        fila += '<td class="text-center" width="5%">' + select + '</select><input placeholder="apartado" name="apartado" hidden type="hidden" value="" id="" class="form-control"></td>';
        // fila += '<td class="text-center"  ><input placeholder="apartado" name="apartado" type="text" value="" id="" class="form-control"></td>';
        fila += '<td class="text-center"  ><input id="file_nuevo" name="fichero" type="file" name="img" multiple="" accept="image/*" mdriesgo="4" class="file" ></td>';

        // fila += '<td class="text-center"  ><input placeholder="descripcion" name="descripcion" type="text" value="" id="" class="form-control"></td>';
        fila += '<td class="text-center" ><textarea rows="6" cols="50" style="resize: none;" placeholder="descripcion" name="descripcion" type="text" value="" id="" class="form-control"></textarea></td>';
        fila += '<td class="text-center"> <a name="Guardar" class="EnviarMedidaGeneral" id="GuardarNuevo" onclick="GuardarMedidaGeneralImagen()"><i class="icon-checkmark" style="color:green;"  title="Guardar"></i></a></td>';
        fila += '<td class="text-center" > <a name="Cancelar" class="CancelarMedidaGeneral" onclick="Cancelar()" id="CancelarNuevo"><i class="icon-cancel" style="color:red;"  title="Cancelar"></i></a></td>';

        return fila;
    }

    function FilaNueva(tipoFila) {

        var colspan = '';
        var evento = '';

        if (tipoFila == "Situacion") {
            colspan = 'colspan="2"';
            evento = "GuardarSituacion()";
        } else if (tipoFila == "Medida") {
            colspan = 'colspan="4"';
            evento = "GuardarMedida()";
        } else if (tipoFila == "MedidaGeneral") {
            colspan = 'colspan="2"';
            evento = "GuardarMedidaGeneral()";
        } else if (tipoFila == "VariasMedidas") {
            colspan = 'colspan="4"';
            evento = "GuardarVariasMedidas()";
        }
        else if (tipoFila == "MedidasCompuestas") {
            colspan = 'colspan="4"';
            evento = "GuardarMedidasCompuestas()";
        }
        var fila = '';

        if (tipoFila != "VariasMedidas" && tipoFila != "MedidasCompuestas") {
            fila += '<td class="text-center" ' + colspan + ' ><input placeholder="descripcion" name="descripcion"  type="text" value="" id="" class="form-control"></td>';
        } else {
            fila += '<td class="text-center" ' + colspan + ' ><textarea rows="6" cols="50"  style="resize: none;" placeholder="descripcion" name="descripcion"  type="text" value="" id="" class="form-control"></textarea></td>';
        }

        fila += '<td class="text-center" width="5%"> <a name="Guardar" class="EnviarMedidaGeneral" id="GuardarNuevo" onclick="' + evento + '"><i class="icon-checkmark" style="color:green;"  title="Guardar"></i></a></td>';
        fila += '<td class="text-center" width="5%"> <a name="Cancelar" class="CancelarMedidaGeneral" onclick="Cancelar()" id="CancelarNuevo"><i class="icon-cancel" style="color:red;"  title="Cancelar"></i></a></td>';

        return fila;
    }
    $('.EnviarMedidaGeneral').on('click', function () {

    });
    $('.CancelarMedidaGeneral').on('click', function () {
        Cancelar();
    });


    $('.titMedidasGenerales').on('click', function () {
        var filatr = $(this).closest('tr');
        //var icono = $(this).find('i[class="icon-arrow-down"]');
        var icono = $(this).find('i');
        if ($(icono)[0].classList.contains('icon-arrow-up')) {
            $(icono).removeClass();
            $(icono).addClass('icon-arrow-down');
            $(".MedidasGeneralesCentro").show();

        } else {
            $(icono).removeClass();
            $(icono).addClass('icon-arrow-up');
            $(".MedidasGeneralesCentro").hide();

        }
        //if ($(".MedidasGeneralesCentro").is(':visible')) {
        //    $(".MedidasGeneralesCentro").hide();
        //    $(icono).hide();
        //} else {
        //    $(".MedidasGeneralesCentro").show();
        //    $(icono).show();
        //}
    });

    $('.desplegar').on('click', function () {
        // var id_des = $(this)[0].id;

        var filatr = $(this).closest('tr');
        var id_des = $(filatr).find('i')[0].id;
        if (id_des.search('_') > 0) {
            var contenidoSituacion = '.ContenidoSituacion_' + id_des;

            if ($(contenidoSituacion).is(':visible')) {
                $(contenidoSituacion).hide();

                $('#' + id_des).removeClass();
                //$(icono).removeClass();
                $('#' + id_des).addClass('icon-arrow-up');
            } else {

                $(contenidoSituacion).show();
                $('#' + id_des).removeClass();
                // $(icono).removeClass();
                $('#' + id_des).addClass('icon-arrow-down');
            }
        } else {
            var contenido = '.Contenido_' + id_des;

            if ($(contenido).is(':visible')) {
                $(contenido).hide();
                $('#' + id_des).removeClass();
                $('#' + id_des).addClass('icon-arrow-up');
            } else {
                $(contenido).show();
                $('#' + id_des).removeClass();
                $('#' + id_des).addClass('icon-arrow-down');
            }
        }


    });
});
function seleccionTecnologia() {
    // ObtenerMedidasGenerales();
}
$('.EliminarGeneral').on('click', function () {

});
$('#medidasGenerales').on('click', 'td', 'a', function () {
    var fila = $(this).closest('tr');
    var td = $(this);
    var icono = td.find('a');
    if (icono.length > 0) {
        if (icono[0].classList.contains('EliminarGeneral')) {
            if ($(fila)[0].hasAttribute('idMedida')) {
                var idMedida = $(fila)[0].getAttribute('idMedida');
                var resultado = confirm('Se eliminará la medida. ¿Desea continuar?');
                if (resultado) {
                    EliminarMedida(idMedida);
                    // ObtenerMedidasGenerales();
                }
            }
        }
    }
});

function CheckSituacion(situacionCHK) {
    $('.loader').show();
    var situacion = situacionCHK.id.replace("chk_", "");
    if (situacion != "") {
        if ($(situacionCHK).is(":checked")) {
            var activo = 1;
        } else {
            var activo = 0;
        }
        var situacionvuelta = $(situacionCHK).closest('table').attr('situacion');
        if (situacionvuelta != "" || typeof (situacionvuelta) != "undefined") {
            localStorage.setItem("situacion", situacionvuelta);
        }
        var riesgovuelta = "";
        if ($(situacionCHK)[0].parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.hasAttribute('riesgo')) {
            var riesgovuelta = $(situacionCHK)[0].parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.getAttribute('riesgo');
        }
        if (riesgovuelta != "" || typeof (riesgovuelta) != "undefined") {
            localStorage.setItem("riesgo", riesgovuelta);
        }
        $.ajax({
            url: "checkSituacion", //Your path should be here
            data: { situacion: situacion, activo: activo },
            type: "post",
            success: function (datos) {
                if (datos != null) {
                    //location.reload();
                    window.location.reload(true);
                }
            }

        });
    }
}

function CheckMedida(medidaCHK) {

    var riesgovuelta = "";
    var situacionvuelta = $(medidaCHK).closest('table').attr('situacion');
    if (situacionvuelta != "" || typeof (situacionvuelta) != "undefined") {
        localStorage.setItem("situacion", situacionvuelta);
    }
    if ($(medidaCHK)[0].parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.hasAttribute('riesgo')) {
        var riesgovuelta = $(medidaCHK)[0].parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.parentElement.getAttribute('riesgo');
    }
    if (riesgovuelta != "" || typeof (riesgovuelta) != "undefined") {
        localStorage.setItem("riesgo", riesgovuelta);
    }

    var medida = medidaCHK.id.replace("chk_", "");
    if (medida != "") {
        if ($(medidaCHK).is(":checked")) {
            var activo = 1;
        } else {
            var activo = 0;
        }


        $.ajax({
            url: "checkMedida", //Your path should be here
            data: { medida: medida, activo: activo },
            type: "post",
            success: function (datos) {
                if (datos != null) {

                    location.reload();
                }
            }
        });
    }

}

function Eliminar() {
    alert('Eliminar');
}
function Cancelar() {
    $("table tr").each(function () {
        var fila = $(this);
        if (fila[0].classList.contains('FilaNueva')) {
            fila.remove();
        }
        if (fila[0].classList.contains('FilaNuevaImagen')) {
            fila.remove();
        }
        //if (fila[0].classList.contains('ClaseEdicion')) {
        //    fila.show();
        //}

        if (fila[0].classList.contains('FilaNuevaImagenMedidaPreventiva')) {
            fila.remove();
        }
    });
}
function Guardar() {
    $("table tr").each(function () {
        var fila = $(this);
        if (fila[0].classList.contains('FilaNueva')) {

            var descripcionFila = fila.find('input[name="descripcion"]');
            var descripcion = $(descripcionFila).val();
            agregarMedidaGeneral($("#hiTecnologia").val(), descripcion);
        }
    });
    Cancelar();
    //ObtenerMedidasGenerales();
}

function GuardarMedidas(elemento) {

    console.log(elemento);
    let riesgoSituacionTabla = elemento.attributes["riesgoSituacionTabla"];

    let cadena = riesgoSituacionTabla.textContent.split("_");
    let riesgo = cadena[0];
    let situacion = cadena[1];

    let tabla = $("table[situacion = '" + situacion + "']");
    let id = tabla[0].id;

    let ele = document.getElementById(id).getElementsByClassName('ContenidoSituacion_' + riesgo + '_' + situacion);
    let dicMedidas = {};
    for (var i = 0; i < ele.length; i++) {
        let idCheckBox = ele[i].children[0].children[0].id;
        let chk = ele[i].children[0].children[0].checked;
        dicMedidas[idCheckBox] = chk;
    }

    GuardarDicMedidas(dicMedidas, situacion);
}

function GuardarDicMedidas(dicMedidas, situacion) {

    $.ajax({
        url: "GuardarDicMedidas", //Your path should be here
        data: { dicMedidas: dicMedidas, situacion: situacion },
        type: "post",
        success: function (datos) {
            if (datos != null) {

                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Medidas seleccionadas guardadas',
                    showConfirmButton: false,
                    timer: 1500
                })
            }
        }
    });
}

function ModificaPadre(elemento) {
    let idTabla = elemento.parentElement.parentElement.parentElement.parentElement.id;
    let tabla = document.getElementById(idTabla);
    let situacion = elemento.parentElement.parentElement.parentElement.parentElement.attributes["situacion"];
    let arrayBoleans = new Array();
    let contSituaciones = 0;

    for (let i = 1; i < tabla.rows.length; i++) {
        if (tabla.rows[i].children[0].children[0].checked) {
            arrayBoleans[i - 1] = true;
        }
    }

    let inputChkSituacion = tabla.children[0].children[0].children[1].children[0];
    if (arrayBoleans.length > 0) {
        inputChkSituacion.checked = true;
        SetCheckSituacion(inputChkSituacion);

    } else {
        inputChkSituacion.checked = false;
        //si no hay medidas seleccionadas tambien tenemos que borrar de la BD la situacion 
        SetCheckSituacion(inputChkSituacion);
    }
}

function SetCheckSituacion(situacionCHK) {
    var situacion = situacionCHK.id.replace("chk_", "");
    if (situacion != "") {
        if ($(situacionCHK).is(":checked")) {
            var activo = 1;
        } else {
            var activo = 0;
        }

        $.ajax({
            url: "SetCheckSituacion", //Your path should be here
            data: { situacion: situacion, activo: activo },
            type: "post",
            success: function (datos) {
                if (datos != null) {
                    //location.reload();
                }
            }
        });
    }
}


function GuardarMedidasCompuestas() {
    $("table tr").each(function () {
        var situacion = '';
        var riesgo = '';
        var fila = $(this);
        if (fila[0].classList.contains('FilaNueva')) {

            if (fila[0].hasAttribute('situacion')) {
                situacion = fila[0].getAttribute('situacion');
                localStorage.setItem("situacion", situacion);
            }
            if (fila[0].hasAttribute('riesgo')) {
                riesgo = fila[0].getAttribute('riesgo');
                localStorage.setItem("riesgo", riesgo);
            }

            var descripcionFila = fila.find('textarea');
            var descripcion = $(descripcionFila).val();
            agregarMedidasCompuestas(descripcion, situacion);
        }
    });
    Cancelar();
    //ObtenerMedidasGenerales();
}

function GuardarVariasMedidas() {
    $("table tr").each(function () {
        var situacion = '';
        var riesgo = '';
        var fila = $(this);
        if (fila[0].classList.contains('FilaNueva')) {

            if (fila[0].hasAttribute('situacion')) {
                situacion = fila[0].getAttribute('situacion');
                localStorage.setItem("situacion", situacion);
            }
            if (fila[0].hasAttribute('riesgo')) {
                riesgo = fila[0].getAttribute('riesgo');
                localStorage.setItem("riesgo", riesgo);
            }

            var descripcionFila = fila.find('textarea');
            var descripcion = $(descripcionFila).val();
            agregarVariasMedidas(descripcion, situacion);
        }
    });
    Cancelar();
    // ObtenerMedidasGenerales();
}

function GuardarMedida() {
    $("table tr").each(function () {
        var situacion = '';
        var riesgo = '';
        var fila = $(this);
        if (fila[0].classList.contains('FilaNueva')) {

            if (fila[0].hasAttribute('situacion')) {
                situacion = fila[0].getAttribute('situacion');
                localStorage.setItem("situacion", situacion);
            }
            if (fila[0].hasAttribute('riesgo')) {
                riesgo = fila[0].getAttribute('riesgo');
                localStorage.setItem("riesgo", riesgo);
            }


            var descripcionFila = fila.find('input[name="descripcion"]');
            var descripcion = $(descripcionFila).val();
            agregarMedidaSituacion(descripcion, situacion);
        }
    });
    Cancelar();
    //  ObtenerMedidasGenerales();
}

function GuardarMedidaPreventivaImagen() {
    $("table tr").each(function () {
        var situacion = '';
        var fila = $(this);
        if (fila[0].classList.contains('FilaNuevaImagenMedidaPreventiva')) {

            if (fila[0].hasAttribute('riesgo')) {
                riesgo = fila[0].getAttribute('riesgo');
            }
            var photo = document.getElementById('file_nuevo').files[0];

            if (fila[0].parentElement.parentElement.hasAttribute("situacion")) {
                situacion = fila[0].parentElement.parentElement.getAttribute("situacion");
            }

            var descripcionFila = fila.find('textarea');
            var descripcion = $(descripcionFila).val();
            //var descripcionFila = fila.find('input[name="descripcion"]');
            //var descripcion = $(descripcionFila).val();

            var ficheroFila = fila.find('input[name="fichero"]');
            var fichero = $(ficheroFila).val();

            var tecnologia = "";


            agregarMedidaPreventivaRiesgoImagen(riesgo, situacion, fichero, descripcion);
        }
    });
    Cancelar();
}
function GuardarMedidaGeneralImagen() {
    $("table tr").each(function () {
        var situacion = '';
        var fila = $(this);
        if (fila[0].classList.contains('FilaNuevaImagen')) {

            if (fila[0].hasAttribute('riesgo')) {
                riesgo = fila[0].getAttribute('riesgo');
            }
            var photo = document.getElementById('file_nuevo').files[0];


            if (document.getElementById('apartadosajax') != null) {
                if ($('#apartadosajax').val() != '0') {
                    var apartado = $('#apartadosajax :selected').text();
                }
            } else {

                var apartadoFila = fila.find('input[name="apartado"]');
                var apartado = $(apartadoFila).val();


            }

            var descripcionFila = fila.find('textarea');
            var descripcion = $(descripcionFila).val();
            //var descripcionFila = fila.find('input[name="descripcion"]');
            //var descripcion = $(descripcionFila).val();

            var ficheroFila = fila.find('input[name="fichero"]');
            var fichero = $(ficheroFila).val();

            var tecnologia = "";


            agregarMedidaGeneralRiesgoImagen(riesgo, apartado, fichero, descripcion);
        }
    });
    Cancelar();
}
function GuardarMedidaGeneral() {
    $("table tr").each(function () {
        var situacion = '';
        var fila = $(this);
        if (fila[0].classList.contains('FilaNueva')) {
            if (fila[0].hasAttribute('riesgo')) {
                riesgo = fila[0].getAttribute('riesgo');
            }
            var descripcionFila = fila.find('textarea');
            var descripcion = $(descripcionFila).val();
            if (document.getElementById('apartadosajax') != null) {
                if ($('#apartadosajax').val() != '0') {
                    var apartado = $('#apartadosajax :selected').text();
                }
            } else {

                var apartadoFila = fila.find('input[name="apartado"]');
                var apartado = $(apartadoFila).val();
            }
            var tecnologia = $('#hiTecnologia').val();
            agregarMedidaGeneralRiesgo(descripcion, tecnologia, riesgo, apartado);
        }
    });
    Cancelar();
}
function GuardarMedidaGeneralVarias() {
    $("table tr").each(function () {
        var situacion = '';
        var fila = $(this);
        if (fila[0].classList.contains('FilaNueva')) {
            if (fila[0].hasAttribute('riesgo')) {
                riesgo = fila[0].getAttribute('riesgo');
            }
            var descripcionFila = fila.find('textarea');
            var descripcion = $(descripcionFila).val();
            if (document.getElementById('apartadosajax') != null) {
                if ($('#apartadosajax').val() != '0') {
                    var apartado = $('#apartadosajax :selected').text();
                }
            } else {

                var apartadoFila = fila.find('input[name="apartado"]');
                var apartado = $(apartadoFila).val();
            }
            var tecnologia = $('#hiTecnologia').val();
            agregarMedidaGeneralRiesgoVarias(descripcion, tecnologia, riesgo, apartado);
        }
    });
    Cancelar();
}
function GuardarSituacion() {
    $("table tr").each(function () {
        var riesgo = '';
        var fila = $(this);
        if (fila[0].classList.contains('FilaNueva')) {

            if (fila[0].hasAttribute('riesgo')) {
                riesgo = fila[0].getAttribute('riesgo');
                localStorage.setItem("riesgo", riesgo);
            }

            var descripcionFila = fila.find('input[name="descripcion"]');
            var descripcion = $(descripcionFila).val();
            agregarSituacion(descripcion, riesgo);
        }
    });
    Cancelar();
    // ObtenerMedidasGenerales();
}


function agregarMedidasCompuestas(descripcion, situacion) {

    $.ajax({
        url: "GuardarMedidasCompuestas", //Your path should be here
        data: { situacion: situacion, descripcion: descripcion },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Nueva medida añadida',
                    showConfirmButton: false,
                    timer: 1500
                })

                location.reload();
            }
        }
    });
}
function agregarVariasMedidas(descripcion, situacion) {

    $.ajax({
        url: "GuardarVariasMedidas", //Your path should be here
        data: { situacion: situacion, descripcion: descripcion },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                location.reload();
            }
        }
    });
}
function agregarMedidaSituacion(descripcion, situacion) {



    $.ajax({
        url: "GuardarMedidaSituacion", //Your path should be here
        data: { situacion: situacion, descripcion: descripcion },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                location.reload();
            }
        }
    });
}
function agregarSituacion(descripcion, riesgo) {

    $.ajax({
        url: "GuardarSituacion", //Your path should be here
        data: { riesgo: riesgo, descripcion: descripcion },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                location.reload();
            }
        }
    });
}


function agregarMedidaPreventivaRiesgoImagen(riesgo, situacion, fichero, descripcion) {
    var form_data = new FormData();
    var photo = document.getElementById('file_nuevo').files[0];
    form_data.append("photo", photo);
    form_data.append("idRiesgo", riesgo);
    form_data.append("situacion", situacion);
    form_data.append("descripcion", descripcion);
    var request = new XMLHttpRequest();

    localStorage.setItem("situacion", situacion);
    localStorage.setItem("riesgo", riesgo);

    $.ajax({
        url: "GuardarMedidaPreventivaRiesgoImagenGrande", //Your path should be here
        cache: false,
        contentType: false,
        processData: false,
        data: form_data,
        type: "post",
        success: function (datos) {
            if (datos != null) {
                location.reload();
            }
        }
    });
}

function agregarMedidaGeneralRiesgoImagen(riesgo, apartado, fichero, descripcion) {
    var form_data = new FormData();
    var photo = document.getElementById('file_nuevo').files[0];
    form_data.append("photo", photo);
    form_data.append("idRiesgo", riesgo);
    form_data.append("idapartado", apartado);
    form_data.append("descripcion", descripcion);
    var request = new XMLHttpRequest();

    localStorage.setItem("riesgo", riesgo);

    $.ajax({
        url: "GuardarMedidaGeneralRiesgoImagen", //Your path should be here
        cache: false,
        contentType: false,
        processData: false,
        data: form_data,
        type: "post",
        success: function (datos) {
            if (datos != null) {
                location.reload();
            }
        }
    });
}
function agregarMedidaGeneralRiesgo(descripcion, tecnologia, riesgo, apartado) {

    localStorage.setItem("riesgo", riesgo);

    if (typeof apartado !== 'undefined') {
        $.ajax({
            url: "GuardarMedidaGeneralRiesgo", //Your path should be here
            data: { descripcion: descripcion, tecnologia: tecnologia, riesgo: riesgo, apartado: apartado },
            type: "post",
            success: function (datos) {
                if (datos != null) {
                    MostrarMensaje("Medida de riesgo guardada", 'success', 1500);
                    location.reload();
                }
            }
        });
    } else {
        MostrarMensaje("Debe seleccionar una apartado", 'error', 1500);
    }
}

function agregarMedidaGeneralRiesgoVarias(descripcion, tecnologia, riesgo, apartado) {

    localStorage.setItem("riesgo", riesgo);

    $.ajax({
        url: "GuardarMedidaGeneralRiesgoVarias", //Your path should be here
        data: { descripcion: descripcion, tecnologia: tecnologia, riesgo: riesgo, apartado: apartado },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                location.reload();
            }
        }
    });
}
function agregarMedidaGeneral(tecnologia, descripcion) {

    $.ajax({
        url: "GuardarMedidaGeneral", //Your path should be here
        data: { tecnologia: tecnologia, descripcion: descripcion },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                alert('Medida Guardada');
            }
        }
    });
}

function EliminarMedidaSituacion(idMedida) {

    $.ajax({
        url: "EliminarMedidaSituacion", //Your path should be here
        data: { idMedida: idMedida },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                if (datos == true) {
                    //alert('Medida Eliminada');

                } else {
                    alert('Esta medida no se puede eliminar');

                }
            }
        }
    });

}
function EliminarSituacionRiesgo(idSituacion) {


    $.ajax({
        url: "EliminarSituacion", //Your path should be here
        data: { idSituacion: idSituacion },
        type: "post",
        success: function (datos) {
            if (datos != null) {

                alert('Situación Eliminada');
            }
        }
    });

}

function EliminarMedida(idMedida) {


    $.ajax({
        url: "EliminarMedidaGeneral", //Your path should be here
        data: { idMedida: idMedida },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                alert('Medida Eliminada');
            }
        }
    });

}
function EliminarRiesgoMedida(idMedida) {
    $.ajax({
        url: "EliminarRiesgoMedida", //Your path should be here
        data: { idMedida: idMedida },
        type: "post",
        success: function (datos) {
            if (datos != null) {
            } else {
                MostrarMensaje("La medida de riesgo no se pudo eliminar!", info, 2000);
            }
        }
    });

}

function guardarMedidaAjax() {

    let descModal = document.querySelector("#descModal");
    let textoDesc = descModal.value;
    let medida = parseInt(descModal.getAttribute('medida'));

    $.ajax({
        url: "guardarMedidaAjax", //Your path should be here
        data: { textoDesc: textoDesc, medida: medida },
        type: "post",
        success: function (datos) {
            if (datos != null) {

                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Medida editada',
                    showConfirmButton: false,
                    timer: 1500
                })
                $('#mi-modal').modal('hide');
                location.reload();
            }
        }
    });
}
function guardarRiesgoMedidaAjax() {

    let descModalRiesgo = document.querySelector("#descModalRiesgo");
    let textoDesc = descModalRiesgo.value;
    let medida = parseInt(descModalRiesgo.getAttribute('medida'));

    $.ajax({
        url: "guardarRiesgoMedidaAjax", //Your path should be here
        data: { textoDesc: textoDesc, medida: medida },
        type: "post",
        success: function (datos) {
            if (datos != null) {

                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Riesgo Medida editada',
                    showConfirmButton: false,
                    timer: 1500
                })
                $('#mi-modal-riesgo').modal('hide');
                location.reload();
               // window.location.reload(true);

            }
        }
    });
}

        //function ObtenerMedidasGenerales() {

        //    //var idTipo = $("#ddlTecnologias").val();
        //    var idTipo = $('#hiTecnologia').val();

        //    $.ajax({
        //        url: "ObtenerMedidasGenerales", //Your path should be here
        //        data: {idTipo: idTipo },
        //        type: "post",
        //        success: function (datos) {
        //            if (datos != null) {
        //                $("#medidasGenerales").empty();
        //                $.each(datos, function (i, datos) {
        //                    $('#medidasGenerales').append('<tr class="MedidasGeneralesCentro" idMedida=' + datos.id + '><td colspan="6">' /*+ datos.codigo + '. '*/ +
        //                        datos.descripcion + '</td></tr>');
        //                    //$('#medidasGenerales').append('<tr class="MedidasGeneralesCentro" idMedida=' + datos.id + '><td colspan="6">' + datos.id_apartado + '</td><td colspan="6">' + datos.rutaImagen + '</td><td colspan="6">' /*+ datos.codigo + '. '*/ +
        //                    //    datos.descripcion + '</td></tr>');
        //                    //<a class="EliminarGeneral"><i class="icon-remove" style="color:red; "></i></a></td><td width="5%" class="text-center"> <a class="EditarGeneral"><i class="icon-pencil" style="color:gray; "></i></a>
        //                });
        //            } else {
        //                $("#medidasGenerales").empty();
        //            }

        //        }
        //    });

        //}



