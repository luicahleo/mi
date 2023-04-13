$('[data-toggle="buttons"] .btn').on('click', function () {
    // toggle style
    $(this).toggleClass('btn-success btn-danger active');

    // toggle checkbox
    var $chk = $(this).find('[type=checkbox]');
    $chk.prop('checked', !$chk.prop('checked'));

    return false;
});

$('form').on('submit', function (e) {
    // watch form values
    $('#formValues').html(($('form').serialize()));
    e.preventDefault();
});

function obtenerModalTecnologias() {
    var idVersion = $("#versionHI").val();
    $.ajax({
        url: "../ObtenerTecnologias",
        type: "post",
        data: { idVersion: idVersion },
        success: function (datos) {
            if (datos != null) {
                $("#combotecnologias").find('option').remove();
                $.each(datos, function (i, datos) {
                    $("#combotecnologias").append('<option value="' + datos.Value + '">' +
                        datos.Text + '</option>');
                });
            } else {
                $("#combotecnologias").find('option').remove();
            }
        }
    });
}

function EliminaArea(tecnologia, area) {
    $('.loader').show();
    $.ajax({
        url: "../EliminaArea", //Your path should be here
        data: { tecnologia: tecnologia, area: area },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                location.reload();
            } else {
            }
        }
    });
}

function CreaSistema(tecnologia, area) {
    var table = document.getElementById("datatableMatrizRiesgos");
    var row = table.insertRow(3);
    row.className = "SistemaNuevo";
    row.innerHTML = SistemaNuevo(tecnologia, area);
    $(row).attr("tecnologia", tecnologia);
    $(row).attr("area", area);
}

function EliminaSistema(tecnologia, area, sistema) {
    $('.loader').show();
    $.ajax({
        url: "../EliminaSistema", //Your path should be here
        data: { tecnologia: tecnologia, area: area, sistema: sistema },
        type: "post",
        success: function (datos) {
            if (datos != null) {

                location.reload();
            } else {
            }
        }
    });
}

function CreaTecnologia() {

    //var table = document.getElementById("datatableMatrizRiesgos");
    //var row = table.insertRow();
    var tecnologia = $("#combotecnologias").val();
    var textoTecnologia = $("#combotecnologias").find("option:selected").text();
    ////row.className = "Tecnologia";
    //row.innerHTML =/*'<td><select id="prueba"></select></td>'+*/
    //    '<td colspan = "32" style = "font-size: 18px; font-weight: bold; color: white;background-color: #ff0f64 ">' + $("#combotecnologias").val() + ' - ' + $("#combotecnologias").find("option:selected").text()+
    //    '</td>'
    //+'<td style = "font-size: 18px; font-weight: bold; color: white;background-color: #ff0f64 " onclick=""><i class="icon-plus"  style="color: white; float: right"></i></td>';
    //$(row).attr("tecnologia", tecnologia);
    //$(row).attr("version", $("#versionHI").val());
    //var posicion = $(row).rowIndex;
    //CreaFila(tecnologia, "", "", "", posicion, $("#versionHI").val(), "AREA");
    $('.loader').show();
    var version = $("#versionHI").val();
    $.ajax({
        url: "../AddTecnologiaVersion", //Your path should be here
        data: { tecnologia: tecnologia, version: version },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                location.reload();
            }

        }
    });

}


function crearNuevaFilaTecnologia() {
    var filatr = $(this).parent.closest('tr');

    var indice = filatr[0].rowIndex + 1;
    var confilas = false;
    var tecnologiafila = "";
    var versionfila = "";
    if (filatr[0].hasAttribute("tecnologia")) {
        tecnologiafila = filatr[0].getAttribute("tecnologia");
    }
    if (filatr[0].hasAttribute("version")) {
        versionfila = filatr[0].getAttribute("version");
    }
    $('#datatableMatrizRiesgos tr').each(function () {
        var tr = $(this);

        if (tr[0].hasAttribute("area")) {
            confilas = true;
        }

    });
    if (!confilas) {
        CreaFila(tecnologiafila, "", "", "", "", indice, versionfila, "AREA");
    }
}
function CreaEquipo() {
    var table = document.getElementById("datatableMatrizRiesgos");
    var row = table.insertRow(3);
    row.className = "AreaNueva";
    row.innerHTML = AreaNueva(tecnologia);
    $(row).attr("tecnologia", tecnologia);
}

function EditaEquipo() {

}


function EliminaNivelCuatro(tecnologia, area, sistema, equipo, nivelcuatro) {
    $('.loader').show();
    $.ajax({
        url: "../EliminaNivelCuatro", //Your path should be here
        data: { tecnologia: tecnologia, area: area, sistema: sistema, equipo: equipo, nivelcuatro: nivelcuatro },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                location.reload();
            } else {
            }
        }
    });
}
function EliminaEquipo(tecnologia, area, sistema, equipo) {
    $('.loader').show();
    $.ajax({
        url: "../EliminaEquipo", //Your path should be here
        data: { tecnologia: tecnologia, area: area, sistema: sistema, equipo: equipo },
        type: "post",
        success: function (datos) {
            if (datos != null) {

                location.reload();
            } else {

            }

        }
    });
}
function CreaArea(tecnologia) {
    var table = document.getElementById("datatableMatrizRiesgos");
    var row = table.insertRow(3);
    row.className = "AreaNueva";
    row.innerHTML = AreaNueva(tecnologia);
    $(row).attr("tecnologia", tecnologia);
    $(row).addClass("AreaNueva2");
}

function CreaFila(tecnologia, area, sistema, equipo, nivelcuatrofila, posicion, version, tipo) {
    if (comprobarEdicion()) {
        var clase = "";
        var color = "white";
        var table = document.getElementById("datatableMatrizRiesgos");
        var row = table.insertRow(posicion);
        row.innerHTML = FilaNueva();
        if (tipo == "AREA") {
            clase = "AreaNueva";
            $(row).attr("version", version);
            // color = "#ff0f64";
        } else if (tipo == "SISTEMA") {
            clase = "SistemaNuevo";
            $(row).attr("area", area);
            //     color = "#ff0f64";
        } else if (tipo == "EQUIPO") {
            clase = "EquipoNuevo";
            $(row).attr("area", area);
            $(row).attr("sistema", sistema);

        } else if (tipo == "NIVELCUATRO") {
            clase = "NivelCuatroNuevo";
            $(row).attr("area", area);
            $(row).attr("sistema", sistema);
            $(row).attr("equipo", equipo);
        }
        //style = "background-color:#ff0f64":
        row.color = "#ff0f64";

        $(row).addClass('borde');
        $(row).attr("tipo", tipo);
        $(row).attr("tecnologia", tecnologia);
        $(row).addClass(clase);

        const input = document.getElementById('nuevaFilaid');
        input.focus();
    } else {
        alert('Ya se está creando otro registro');
    }
}
function FilaNueva() {

    var fila = '';
    for (i = 0; i < 32; i++) {

        if (i == 0) {
            fila += '<td class="text-center" columnaPrincipal><input name="nuevaFila"  type="text" value="" id="nuevaFilaid" class="form-control"></td>';
        } else if (i < 29 && i > 0) {
            fila += '<td class="text-center" id="Riesgo_' + i + '" columnaRiesgo><input class="form-check-input"  checked type="checkbox" value="" id="chk_' + i + '"></td>';
        } else if (i == 30) {
            fila += '<td class="text-center"> <a name= "Guardar" onclick="Guardar()" id="GuardarNuevo"><i class="icon-checkmark" style="color:green; " title="Guardar"></i></a></td>';
        } else if (i == 31) {
            fila += '<td class="text-center"> <a onclick="Cancelar()" id="CancelarNuevo"><i class="icon-cancel" style="color:red;" title="Cancelar"></i></a></td>';

        }

    }
    return fila;
}
function CancelarEdicion() {
    $("table tr").each(function () {
        var fila = $(this);
        if (fila[0].classList.contains('FilaEdicion')) {
            fila.remove();
        }
        if (fila[0].classList.contains('ClaseEdicion')) {
            fila.show();
            fila.removeClass('ClaseEdicion');

        }
    });
}
function iconoNuevoDDL() {
    return hmltext = '<div class="dropdown">< button onclick = "myFunction()" class="dropbtn" > Dropdown</button ><div id="myDropdown" class="dropdown-content"><a href="#home">Home</a><a href="#about">About</a> <a href="#contact">Contact</a> </div> </div >';
}



function Cancelar() {
    $("table tr").each(function () {
        var fila = $(this);
        if (fila[0].classList.contains('AreaNueva') || fila[0].classList.contains('SistemaNuevo') || fila[0].classList.contains('EquipoNuevo') || fila[0].classList.contains('NivelCuatroNuevo')) {
            fila.remove();
        }
        if (fila[0].classList.contains('ClaseEdicion')) {
            fila.show();

        }
    });
}
function comprobarEdicion() {
    if (document.getElementsByClassName('AreaNueva').length > 0 || document.getElementsByClassName('AreaNueva').length > 0 || document.getElementsByClassName('SistemaNuevo').length > 0 || document.getElementsByClassName('FilaEdicion').length > 0) {
        return false;
    } else {
        return true;
    }
}

function Guardar() {
    //$("table tr").each(function () {
    //    var fila = $(this);
    //    if (fila[0].className == "AreaNueva") {

    //        $("tr td").each(function () {
    //            var celda = $(this);
    //            console.log(celda[0].id);
    //        });

    //    }
    //});
}
function agregarArea(matriz, tecnologia, nombre, version) {
    var matrizRiesgos = matriz;

    $('.loader').show();
    $.ajax({
        url: "../GuardarAreaRiesgos", //Your path should be here
        data: { matrizRiesgos: matrizRiesgos, tecnologia: tecnologia, nombre: nombre, nivel: "AREA", version: version },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                location.reload();
            } else {
                CancelarEdicion();
            }
        }
    });
}

function agregarSistema(matriz, tecnologia, area, nombre) {
    var matrizRiesgos = matriz;
    $('.loader').show();
    $.ajax({
        url: "../GuardarSistemaRiesgos", //Your path should be here
        data: { matrizRiesgos: matrizRiesgos, tecnologia: tecnologia, nombre: nombre, area: area, nivel: "SISTEMA" },
        type: "post",
        success: function (datos) {
            if (datos != null) {

                location.reload();
            } else {
            }
        }
    });
}

function agregarNivelCuatro(matriz, tecnologia, area, sistema, equipo, nombre) {
    var matrizRiesgos = matriz;
    $('.loader').show();
    $.ajax({
        url: "../GuardarNivelCuatroRiesgos", //Your path should be here
        data: { matrizRiesgos: matrizRiesgos, tecnologia: tecnologia, nombre: nombre, area: area, sistema: sistema, equipo: equipo, nivel: "NIVELCUATRO" },
        type: "post",
        success: function (datos) {
            if (datos != null) {
                location.reload();
            } else {
            }
        }
    });
}

function agregarEquipo(matriz, tecnologia, area, sistema, nombre) {
    var matrizRiesgos = matriz;
    $('.loader').show();
    $.ajax({
        url: "../GuardarEquipoRiesgos", //Your path should be here
        data: { matrizRiesgos: matrizRiesgos, tecnologia: tecnologia, nombre: nombre, area: area, sistema: sistema, nivel: "EQUIPO" },
        type: "post",
        success: function (datos) {
            if (datos != null) {

                location.reload();
            } else {

            }

        }
    });
}
function subidaImagen(obj, id) {
    var fileupload = $(this).closest('input');
    var button = $(obj).closest('td').closest('tr').find('button[class="subirImagen"]');
    //button.attr(prueba, 'prueba');
    button.prop('disabled', false);

}
function Subir(obj) {
    var boton = $(this).closest('button')[0];
    var idNivel = $(obj).attr('identificador');
    var nivel = $(obj).closest('tr').attr('nivel');
    //var file_data = $('#file_' + idRiesgo).prop('files')[0];
    //var form_data = new FormData();
    //var req = new XMLHttpRequest();
    //var formData = new FormData();
    //var photo = document.getElementById('file_' + idRiesgo).files[0];

    var form_data = new FormData();
    var photo = document.getElementById('file_nuevo' + idNivel).files[0];
    form_data.append("photo", photo);
    form_data.append("idNivel", idNivel);
    form_data.append("nivel", nivel);

    var imagen = $.ajax({
        url: '../GuardarImagenNivel',
        data: form_data,
        cache: false,
        contentType: false,
        processData: false,
        data: form_data,
        type: 'post',
        success: function (response) {
            if (response != null) {
                $.each(response, function (i, response) {
                    // $('#Subir_' + response.id).hide();
                    // $('#IMG_' + response.id).attr("src", response.imagen);
                    alert('Imagen modificada');
                    $(obj).attr('disabled', true);

                    var nivelicono = $(obj).closest('tr')[0].getAttribute('nivel');
                    var identificadoricono = $(obj).closest('tr')[0].getAttribute('identificador');


                    CancelarEdicion();
                    location.reload();

                    //if (nivelicono == "EQUIPO") nivelnumero = "3_";
                    //if (nivelicono == "AREA") nivelnumero = "";
                    //if (nivelicono == "SISTEMA") nivelnumero = "2_";
                    //var encontraricono = $($("#btn_sub_" + nivelnumero + identificadoricono)[0]);
                    //$($(encontraricono).find('i')[0]).removeClass('icon-camera2');
                    //$($(encontraricono).find('i')[0]).addClass('icon-camera');
                    //$(encontraricono).find('i')[0].style.color = "green";



                    //getDatos();
                });
            }
        },
        error: function (response) {
            alert('error');
        }
    });
    //button.prop('disabled', true);

}
function EliminarTecnologiaCentroVersion(obj) {
    var tecnologia = $(obj).closest('tr').attr('tecnologia');
    var version = $('#versionHI').val();
    var resultado = confirm('¿Está seguro de que desea eliminar los registros de la tecnología?');
    $('.loader').show();
    if (tecnologia != "" && version != "" && resultado == true) {
        $.ajax({
            url: "../EliminarTecnologiaCentroVersion", //Your path should be here
            data: { tecnologia: tecnologia, version: version },
            type: "post",
            success: function (datos) {
                if (datos != null) {
                    if (datos == false) {
                        location.reload();
                    } else {
                        window.location.href = '../lista_matrices';
                    }
                }
            }
        });
    } else {
        $('.loader').hide();
    }

}

$(document).ready(function () {
    $("#MenuMatrizRiesgos").css('color', 'black');
    $("#MenuMatrizRiesgos").css('background-color', '#ebf1de');
    $("#MenuMatrizRiesgos").css('font-weight', 'bold');
    $(document).on("keypress", '#editarNombre', function (event) {
        var key = event.which;
        if (key === 13) {
            event.preventDefault();
            var arrayRiesgos = new Array();
            var filatr = $("#editarNombre").closest('tr');
            var tecnologiafila = "";
            var areafila = "";
            var sistemafila = "";
            var nivelfila = "";
            var equipofila = "";
            var versionfila = "";
            var nivelcuatrofila = "";

            if (filatr[0].hasAttribute("version")) {
                versionfila = filatr[0].getAttribute("version");
            }

            if (filatr[0].hasAttribute("tecnologia")) {
                tecnologiafila = filatr[0].getAttribute("tecnologia");
            }
            if (filatr[0].hasAttribute("area")) {
                areafila = filatr[0].getAttribute("area");
            }
            if (filatr[0].hasAttribute("sistema")) {
                sistemafila = filatr[0].getAttribute("sistema");
            }
            if (filatr[0].hasAttribute("equipo")) {
                equipofila = filatr[0].getAttribute("equipo");
            }
            if (filatr[0].hasAttribute("nivelcuatro")) {
                nivelcuatrofila = filatr[0].getAttribute("nivelcuatro");
            }
            if (filatr[0].hasAttribute("nivel")) {
                nivelfila = filatr[0].getAttribute("nivel");
            }
            if (filatr[0].hasAttribute("identificador")) {
                identificador = filatr[0].getAttribute("identificador");
            }
            $(filatr).find('td').each(function () {
                var arrayChecks = new Array();
                var td = $(this).closest('td');
                var valor = td.find('input[type="text"]').val();
                if (td[0].hasAttribute('columnaPrincipal')) {
                    nombreNuevo = valor;
                }
                if (td[0].hasAttribute('columnaRiesgo')) {
                    var riesgo = parseInt(td[0].id.replace('Riesgo_', ''));
                    var chk = $(this).find('input[type="checkbox"]').prop("checked");
                    arrayChecks.push(riesgo, chk);
                    arrayRiesgos.push(arrayChecks);
                }
            });
            var tipo_guardar = filatr[0].classList;
            if (tipo_guardar.contains("AreaNueva")) {
                agregarArea(arrayRiesgos, tecnologiafila, nombreNuevo, versionfila);
            } else if (tipo_guardar.contains("SistemaNuevo")) {
                localStorage.setItem("areaSeleccionada", areafila);
                localStorage.setItem("nivel", 1);
                agregarSistema(arrayRiesgos, tecnologiafila, areafila, nombreNuevo);
            } else if (tipo_guardar.contains("EquipoNuevo")) {
                localStorage.setItem("areaSeleccionada", areafila);
                localStorage.setItem("sistemaSeleccionado", sistemafila);
                localStorage.setItem("nivel", 2);
                agregarEquipo(arrayRiesgos, tecnologiafila, areafila, sistemafila, nombreNuevo);
            } else if (tipo_guardar.contains("NivelCuatroNuevo")) {
                localStorage.setItem("areaSeleccionada", areafila);
                localStorage.setItem("sistemaSeleccionado", sistemafila);
                localStorage.setItem("equipoSeleccionado", equipofila);
                localStorage.setItem("nivel", 3);
                agregarNivelCuatro(arrayRiesgos, tecnologiafila, areafila, sistemafila, equipofila, nombreNuevo);
            } else if (tipo_guardar.contains("FilaEdicion")) {
                if (nivelfila != "") {
                    EditarRegi(arrayRiesgos, identificador, nombreNuevo, tecnologiafila, versionfila, nivelfila);
                }
            }
        }
    });
    $('#file_nuevo').change(function () {
        var fileupload = $(this).closest('input')[0];
        //  var idRiesgo = '';
        if (fileupload.hasAttribute('mdriesgo')) {
            idRiesgo = fileupload.getAttribute('mdriesgo');
        }
        //var boton = $('#Subir_' + idRiesgo).show();
    });



    $('.subirImagen').on('click', function () {
        var boton = $(this).closest('button')[0];
        var idRiesgo = '';
        if (boton.hasAttribute('mdriesgo')) {
            idRiesgo = boton.getAttribute('mdriesgo');
        }
        var form_data = new FormData();
        var photo = document.getElementById('file_' + idRiesgo).files[0];
        form_data.append("photo", photo);
        form_data.append("idRiesgo", idRiesgo);

        var imagen = $.ajax({
            url: 'GuardarImagenNivel',
            data: form_data,
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
                        alert('Imagen modificada');
                    });
                }
            },
            error: function (response) {
                alert('error');
            }
        });
    });
    $('.icon-camera').click(function () {
        var identificadorArea = $(this).attr('identificador');
        var nivel = $(this).closest('tr')[0].className;
        var datoimagen = "";
        $.ajax({
            url: "../ObtenerImagenNivel",
            type: "post",
            data: { identificadorArea: identificadorArea, nivel: nivel },
            success: function (datos) {
                if (datos != null) {
                    datoimagen = datos;
                    if (datoimagen == "") {
                        $('#dialog').empty();
                        $('#dialog').append('<div style="text-align: center;" class="center">No contiene imágenes.<div>').append($(this).html());
                        alert("No contiene imágenes");
                    } else {
                        $('#dialog').empty();
                        $('#dialog').append('<div style="text-align: center;" class="center"><img style="width: 50%; height: 50%;" method="post" enctype="multipart/form-data" src="../' + datos + '"/><div>').append($(this).html());

                        $('#dialog').dialog({
                            modal: true,
                            resizable: true,
                            width: "800",
                            height: "600",
                            position: 'center',

                            buttons: {
                                "Cerrar": function () {
                                    $(this).dialog('close');
                                    $('#dialog').empty();
                                }
                            }
                        });
                    }
                }

            }
        });
    });

    $('.Tecnologia').click(function () {
        var filatr = $(this).closest('tr');

        var indice = filatr[0].rowIndex + 1;
        var confilas = false;
        var tecnologiafila = "";
        var versionfila = "";
        if (filatr[0].hasAttribute("tecnologia")) {
            tecnologiafila = filatr[0].getAttribute("tecnologia");
        }
        if (filatr[0].hasAttribute("version")) {
            versionfila = filatr[0].getAttribute("version");
        }
        $('#datatableMatrizRiesgos tr').each(function () {
            var tr = $(this);

            if (tr[0].hasAttribute("area")) {
                confilas = true;
            }
        });
        if (!confilas) {
            CreaFila(tecnologiafila, "", "", "", "", indice, versionfila, "AREA");
        }
    });

    getDatos();
    obtenerModalTecnologias();


    function getDatos() {
        if (localStorage.getItem("areaSeleccionada") != null) {
            var areasel = localStorage.getItem("areaSeleccionada");
            var nivels = localStorage.getItem("nivel");
            var equiposel = "";
            if (localStorage.getItem("equipoSeleccionado") != null) {
                equiposel = localStorage.getItem("equipoSeleccionado");
            }
            var sistemasel = localStorage.getItem("sistemaSeleccionado");
            localStorage.clear();


            $('#datatableMatrizRiesgos').find('tr').each(function () {
                var tr = $(this);
                if (nivels == 1) {
                    if (tr[0].hasAttribute("area") && (tr[0].classList.contains('SISTEMA') || tr[0].classList.contains('AREA'))) {
                        if (tr[0].getAttribute("area") == areasel) {
                            $(tr).show();
                        }
                    }
                } else if (nivels == 2) {
                    if (tr[0].hasAttribute("area") && (tr[0].classList.contains('SISTEMA') || tr[0].classList.contains('AREA'))) {
                        if (tr[0].getAttribute("area") == areasel) {
                            $(tr).show();
                        }
                    }
                    if (tr[0].hasAttribute("area") && (tr[0].classList.contains('SISTEMA') || tr[0].classList.contains('AREA') || tr[0].classList.contains('EQUIPO'))) {

                        if (tr[0].getAttribute("area") == areasel && tr[0].getAttribute("sistema") == sistemasel) {
                            $(tr).show();
                        }
                    }
                } else if (nivels == 3) {
                    if (tr[0].hasAttribute("area") && (tr[0].classList.contains('SISTEMA') || tr[0].classList.contains('AREA'))) {
                        if (tr[0].getAttribute("area") == areasel) {
                            $(tr).show();
                        }
                    }
                    if (tr[0].hasAttribute("area") && (tr[0].classList.contains('SISTEMA') || tr[0].classList.contains('AREA') || tr[0].classList.contains('EQUIPO'))) {

                        if (tr[0].getAttribute("area") == areasel && tr[0].getAttribute("sistema") == sistemasel) {
                            $(tr).show();
                        }
                    }
                    if (tr[0].getAttribute("area") == areasel && tr[0].getAttribute("equipo") == equiposel) {
                        $(tr).show();
                    }
                }
            });
            $('.loader').hide();
        }


        var arrayActivos = new Array();
        var version = $('#versionHI').val();
        $('.loader').show();
        //  $(this).html('<div class="loading"><img src="images/loader.gif" alt="loading" /><br/>Un momento, por favor...</div>');

        $.ajax({
            url: "../ObtenerActivos",
            type: "post",
            data: { version: version },
            success: function (datos) {
                if (datos != null) {
                    $.each(datos, function (i, datos) {
                        arrayActivos.push(datos);
                    });
                    $('.loader').show();
                    informarChecks(arrayActivos);

                } else {
                    if (tr[0].hasAttribute("area")) {
                        areafila = parseInt(tr[0].getAttribute("area"));
                    }
                }
                /*  setTimeout(function () { $('.loader').hide(); }, 1000);*/

                //  $(this).fadeIn(1000).html(datos);
            }
        });

    }
    function informarChecks(arrayActivos) {

        $('#datatableMatrizRiesgos').find('tr').each(function () {
            var tr = $(this);
            var tecnologiafila = "";
            var areafila = "";
            var sistemafila = "";
            var equipofila = "";
            var nivelcuatrofila = "";

            if (tr[0].hasAttribute("tecnologia")) {
                tecnologiafila = parseInt(tr[0].getAttribute("tecnologia"));
            }
            if (tr[0].hasAttribute("area")) {
                areafila = parseInt(tr[0].getAttribute("area"));
            }
            if (tr[0].hasAttribute("sistema")) {
                sistemafila = parseInt(tr[0].getAttribute("sistema"));
            }
            if (tr[0].hasAttribute("equipo")) {
                equipofila = parseInt(tr[0].getAttribute("equipo"));
            }
            if (tr[0].hasAttribute("nivelcuatro")) {
                nivelcuatrofila = parseInt(tr[0].getAttribute("nivelcuatro"));
            }
            var filaObjeto = {
                tecnologia: tecnologiafila,
                area: areafila,
                sistema: sistemafila,
                equipo: equipofila,
                nivelcuatro: nivelcuatrofila,
                riesgo: ""
            };
            $(tr).find('td').each(function () {
                var chkreset = $(this).find('input[type="checkbox"]');
                chkreset.prop('checked', false);
            });

            $(tr).find('td').each(function () {
                var indice = $(this).index();
                var riesgo = 0;
                if (indice > 0 && indice < 29) {
                    riesgo = parseInt($('#datatableMatrizRiesgos').find('th')[indice].innerText);
                }

                var chk = $(this).find('input[type="checkbox"]');
                if (typeof (chk[0]) != "undefined") {
                    $.each(arrayActivos, function (i, arrayActivos) {
                        if (filaObjeto.tecnologia == arrayActivos.id_tecnologia) {

                            if (tr[0].classList.contains("AREA")) {
                                if (filaObjeto.area == arrayActivos.id_areanivel1 && arrayActivos.id_riesgo == riesgo && arrayActivos.activo == true) {
                                    chk.prop('checked', 'true');
                                }
                            }
                            if (tr[0].classList.contains("SISTEMA")) {
                                if (filaObjeto.sistema == arrayActivos.id_areanivel2 && arrayActivos.id_riesgo == riesgo && arrayActivos.activo == true) {
                                    chk.prop('checked', 'true');
                                }
                            }
                            // filaObjeto.area == arrayActivos.id_areanivel1 &&

                            if (tr[0].classList.contains("EQUIPO")) {
                                if (filaObjeto.equipo == arrayActivos.id_areanivel3 && arrayActivos.id_riesgo == riesgo && arrayActivos.activo == true) {
                                    chk.prop('checked', 'true');
                                }
                                // && filaObjeto.sistema == arrayActivos.id_areanivel2 && filaObjeto.area == arrayActivos.id_areanivel1 &&
                            }
                            if (tr[0].classList.contains("NIVELCUATRO")) {
                                if (filaObjeto.nivelcuatro == arrayActivos.id_areanivel4 && arrayActivos.id_riesgo == riesgo && arrayActivos.activo == true) {
                                    chk.prop('checked', 'true');
                                }
                                // && filaObjeto.sistema == arrayActivos.id_areanivel2 && filaObjeto.area == arrayActivos.id_areanivel1 &&
                            }
                        }
                    });

                }


            });
        });
        $('.loader').hide();
    }
    function recorrerTdsArray(row) {
        var arrayRiesgos = new Array();
        $(row).find('td').each(function () {
            var arrayChecks = new Array();
            var td = $(this).closest('td');
            if ($(td).find('input[type="checkbox"]').length > 0) {
                var riesgo = $(td).index() - 1;
                var chk = $(this).find('input[type="checkbox"]').prop("checked");
                arrayChecks.push(riesgo, chk);
                arrayRiesgos.push(arrayChecks);
            }

        });
        return arrayRiesgos;
    }

    function actualizarNombre(i, nombre, tipo) {
        var atributo = tipo.toLowerCase();
        if (tipo == "AREA") {

        } else if (tipo == "SISTEMA") {

        } else if (tipo == "EQUIPO") {

        }
        $("table tr").each(function () {
            var fila = $(this);
            if (fila[0].classList.contains(tipo)) {
                if (fila[0].hasAttribute(atributo)) {
                    if (fila[0].getAttribute(atributo) == i) {
                        fila.find("td[columnaprincipal]")[0].innerText = nombre;
                        if (fila[0].children[30].childNodes[1].hasAttribute('conhijos')) {
                                fila.find("td[columnaprincipal]").append('<i class="icon-arrow-up" style="color: #ff0f64; height: 5px; float: right; font-size: 10px;" hidden></i><i class= "icon-arrow-down" style = "color: #ff0f64; height: 5px; float: right; font-size: 10px;" ></i>');
                        }
                    }
                }
            }

        });
    }

    function EditarRegi(matrizRiesgos, i, nombre, tecnologia, version, tipo) {
        var identificador = i;
        $('.loader').show();
        $.ajax({
            url: "../ActualizarRegistro", //Your path should be here
            data: { matrizRiesgos: matrizRiesgos, id: identificador, nombre: nombre, tecnologia: tecnologia, version: version, nivel: tipo },
            type: "post",
            success: function (datos) {
                if (datos != null) {
                    CancelarEdicion();
                    getDatos();
                    // location.reload();
                    if (datos != "") {
                        actualizarNombre(i, nombre, tipo);
                    }

                } else {
                }
            }
        });
    }

    $('#datatableMatrizRiesgos').on('click', 'td', 'a', function () {
        var filatr = $(this).closest('tr');
        var iconoPulsado = $(this).find('a[name="Guardar"]');
        var icono = $(this).find('a');

        var tecnologiafila = "";
        var areafila = "";
        var sistemafila = "";
        var nivelfila = "";
        var equipofila = "";
        var versionfila = "";
        var posicion = filatr[0].rowIndex + 1;
        if (filatr[0].hasAttribute("version")) {
            versionfila = filatr[0].getAttribute("version");
        }

        if (filatr[0].hasAttribute("tecnologia")) {
            tecnologiafila = filatr[0].getAttribute("tecnologia");
        }
        if (filatr[0].hasAttribute("area")) {
            areafila = filatr[0].getAttribute("area");
        }
        if (filatr[0].hasAttribute("sistema")) {
            sistemafila = filatr[0].getAttribute("sistema");
        }
        if (filatr[0].hasAttribute("equipo")) {
            equipofila = filatr[0].getAttribute("equipo");
        }
        if (filatr[0].hasAttribute("nivel")) {
            nivelfila = filatr[0].getAttribute("nivel");
        }
        if (filatr[0].hasAttribute("identificador")) {
            identificador = filatr[0].getAttribute("identificador");
        }



        //ESTO SE PUEDE SIMPLIFICAR CAMBIANDO LOS NAME DE LOS ICONOS Y HACIENDO UNA SOLA LLAMADA A CREAFILA CON UNA VARIABLE DE TIPO = AL NAME

        //if (typeof (icono[0]) != 'undefined') {
        //    var tipo_nuevo = icono[0].name;
        //    if (icono[0].name == "Nueva_Area") {
        //        CreasFila(tecnologiafila, areafila, sistemafila, equipofila, posicion, "AREA");
        //    }
        //    if (icono[0].name == "Nuevo_Sistema") {
        //        CresaFila(tecnologiafila, areafila, sistemafila, equipofila, posicion, "SISTEMA");
        //    }
        //    if (icono[0].name == "Nuevo_Equipo") {
        //        CreasFila(tecnologiafila, areafila, sistemafila, equipofila, posicion, "EQUIPO");
        //    }
        //}

        var arrayRiesgos = new Array();
        var nombreNuevo = "";
        if (typeof (iconoPulsado[0]) != "undefined") {
            if (iconoPulsado[0].name == "Guardar") {
                $(filatr).find('td').each(function () {
                    var arrayChecks = new Array();
                    var td = $(this).closest('td');
                    var valor = td.find('input[type="text"]').val();
                    if (td[0].hasAttribute('columnaPrincipal')) {
                        nombreNuevo = valor;
                    }
                    if (td[0].hasAttribute('columnaRiesgo')) {
                        var riesgo = parseInt(td[0].id.replace('Riesgo_', ''));
                        var chk = $(this).find('input[type="checkbox"]').prop("checked");
                        arrayChecks.push(riesgo, chk);
                        arrayRiesgos.push(arrayChecks);
                    }
                });
                var tipo_guardar = filatr[0].classList;
                if (tipo_guardar.contains("AreaNueva")) {
                    agregarArea(arrayRiesgos, tecnologiafila, nombreNuevo, versionfila);
                } else if (tipo_guardar.contains("SistemaNuevo")) {
                    localStorage.setItem("areaSeleccionada", areafila);
                    localStorage.setItem("nivel", 1);
                    agregarSistema(arrayRiesgos, tecnologiafila, areafila, nombreNuevo);
                } else if (tipo_guardar.contains("EquipoNuevo")) {
                    localStorage.setItem("areaSeleccionada", areafila);
                    localStorage.setItem("sistemaSeleccionado", sistemafila);
                    localStorage.setItem("nivel", 2);
                    agregarEquipo(arrayRiesgos, tecnologiafila, areafila, sistemafila, nombreNuevo);
                } else if (tipo_guardar.contains("NivelCuatroNuevo")) {
                    localStorage.setItem("areaSeleccionada", areafila);
                    localStorage.setItem("sistemaSeleccionado", sistemafila);
                    localStorage.setItem("equipoSeleccionado", equipofila);
                    localStorage.setItem("nivel", 3);
                    agregarNivelCuatro(arrayRiesgos, tecnologiafila, areafila, sistemafila, equipofila, nombreNuevo);
                }
                else if (tipo_guardar.contains("FilaEdicion")) {
                    if (nivelfila != "") {
                        EditarRegi(arrayRiesgos, identificador, nombreNuevo, tecnologiafila, versionfila, nivelfila);
                    }
                }
            }
        }
    });


    $('.EditarRegistro').on('click', function () {
        var textofinal = "";
        var conHijos = "";
        if ($(this)[0].hasAttribute('conHijos')) {
            conHijos = $(this)[0].getAttribute('conHijos');
        }
        if (comprobarEdicion()) {
            var row = $(this).closest('tr');
            var indice = $(row).index();
            var posicion = row[0].rowIndex + 1;
            var clase = row[0].className;
            var versionfila = "";


            if (clase == "AREA") {
                var idReg = row[0].getAttribute('area');

            } else if (clase == "SISTEMA") {
                var idReg = row[0].getAttribute('sistema');
            } else if (clase == "EQUIPO") {
                var idReg = row[0].getAttribute('equipo');
            } else if (clase == "NIVELCUATRO") {
                var idReg = row[0].getAttribute('nivelcuatro');
            }
            var arrayRiesgos = recorrerTdsArray(row);

            var table = document.getElementById("datatableMatrizRiesgos");
            //var filanueva= row.clone().appendTo(table);
            var filanueva = table.insertRow(posicion);
            filanueva.innerHTML = row[0].innerHTML;

            $(filanueva).addClass('FilaEdicion');
            row.addClass('ClaseEdicion');
            row.hide();
            $(filanueva).attr('nivel', clase);
            $(filanueva).attr('identificador', idReg);
            $(filanueva).attr('tecnologia', row[0].getAttribute('tecnologia'));

            if (row[0].hasAttribute("version")) {
                $(filanueva).attr('version', row[0].getAttribute('version'));
            }

            var contador = 0;
            $(filanueva).find('td').each(function () {
                if ($(this)[0].hasAttribute('ColumnaPrincipal')) {
                    var texto = $(this)[0].innerText;
                    textofinal = texto;
                    $(this).empty().append('<input name="nuevaFila"  type="text" value="' + texto + '" id="editarNombre" class="form-control"></td>');
                }
                if ($(this).index() > 0 && $(this).index() < 29) {
                    $(this)[0].id = 'Riesgo_' + $(this).index();

                    if (conHijos == "") {
                        if (arrayRiesgos[$(this).index() - 1][1]) {

                            $(this).empty().append('<input type="checkbox" checked>');
                        } else {
                            $(this).empty().append('<input type="checkbox">');
                        }
                    } else {
                        if (arrayRiesgos[$(this).index() - 1][1]) {

                            $(this).empty().append('<input type="checkbox" checked disabled>');
                        } else {
                            $(this).empty().append('<input type="checkbox" disabled>');
                        }
                    }

                    $(this).attr('columnaRiesgo', '');
                }
                if ($(this).index() == 29) {
                    $(this).empty().append('<a name="Guardar" class="ActualizarRegistro"><i class="icon-checkmark" style="color:green; " title="Guardar"></i></a>');
                }
                if ($(this).index() == 30) {
                    $(this).empty().append('<a onclick="CancelarEdicion()" id="CancelarNuevo"><i class="icon-cancel" style="color:red; " title="Cancelar"></i></a>');
                }

                //if ($(this).index() == 31) {
                //    if (clase == "AREA") {
                //        $(this).empty().append('<button id="file_' + idReg + '+" identificador="' + idReg + '" disabled onclick="Subir(this)" class="subirImagen">Subir</button>');
                //    } else {
                //        $(this).empty();
                //    }
                //}
                //if ($(this).index() == 32) {
                //    if (clase == "AREA") {
                //        $(this).empty().append('<input id="file_nuevo' + idReg + '" name="fichero" type="file" name="img" multiple="" accept="image/*" onchange="subidaImagen(this,' + idReg + ')" class="file" >');
                //    } else {
                //        $(this).empty();
                //    }
                //}
                if ($(this).index() == 31) {
                    $(this).empty().append('<button id="file_' + idReg + '+" identificador="' + idReg + '" disabled onclick="Subir(this)" class="subirImagen">Subir</button>');
                }
                if ($(this).index() == 32) {
                    $(this).empty().append('<input id="file_nuevo' + idReg + '" name="fichero" type="file" name="img" multiple="" accept="image/*" onchange="subidaImagen(this,' + idReg + ')" class="file" >');
                }

                //if ($(this).index() > 30) {
                //    
                //}


            });
            $(filanueva).addClass('borde');
            const input = document.getElementById('editarNombre');
            input.focus();
            input.setSelectionRange(textofinal.length, textofinal.length);
        }
    });

    $('.Nueva_Area').on('click', function () {
        var filatr = $(this).closest('tr');
        var tecnologiafila = "";
        var areafila = "";
        var sistemafila = "";
        var equipofila = "";
        var versionfila = "";
        var posicion = filatr[0].rowIndex + 1;
        if (filatr[0].hasAttribute("tecnologia")) {
            tecnologiafila = filatr[0].getAttribute("tecnologia");
        }
        if (filatr[0].hasAttribute("area")) {
            areafila = filatr[0].getAttribute("area");
        }
        if (filatr[0].hasAttribute("version")) {
            versionfila = filatr[0].getAttribute("version");
        }
        CreaFila(tecnologiafila, areafila, sistemafila, equipofila, "", posicion, versionfila, "AREA");
    });
    $('.Nuevo_Sistema').on('click', function () {
        var filatr = $(this).closest('tr');
        var tecnologiafila = "";
        var areafila = "";
        var sistemafila = "";
        var equipofila = "";
        var versionfila = "";
        var posicion = filatr[0].rowIndex + 1;

        if (filatr[0].hasAttribute("tecnologia")) {
            tecnologiafila = filatr[0].getAttribute("tecnologia");
        }
        if (filatr[0].hasAttribute("area")) {
            areafila = filatr[0].getAttribute("area");
        }
        if (filatr[0].hasAttribute("sistema")) {
            sistemafila = filatr[0].getAttribute("sistema");
        }

        CreaFila(tecnologiafila, areafila, sistemafila, equipofila, "", posicion, versionfila, "SISTEMA");
    });


    $('.Nuevo_NIVELCUATRO').on('click', function () {
        var filatr = $(this).closest('tr');
        var tecnologiafila = "";
        var areafila = "";
        var sistemafila = "";
        var equipofila = "";
        var nivelcuatrofila = "";
        var posicion = filatr[0].rowIndex + 1;
        var versionfila = "";

        if (filatr[0].hasAttribute("tecnologia")) {
            tecnologiafila = filatr[0].getAttribute("tecnologia");
        }
        if (filatr[0].hasAttribute("area")) {
            areafila = filatr[0].getAttribute("area");
        }
        if (filatr[0].hasAttribute("sistema")) {
            sistemafila = filatr[0].getAttribute("sistema");
        }
        if (filatr[0].hasAttribute("equipo")) {
            equipofila = filatr[0].getAttribute("equipo");
        }
        if (filatr[0].hasAttribute("nivelcuatro")) {
            nivelcuatrofila = filatr[0].getAttribute("nivelcuatro");
        }
        CreaFila(tecnologiafila, areafila, sistemafila, equipofila, nivelcuatrofila, posicion, versionfila, "NIVELCUATRO");



    });

    $('.Nuevo_Equipo').on('click', function () {
        var filatr = $(this).closest('tr');
        var tecnologiafila = "";
        var areafila = "";
        var sistemafila = "";
        var equipofila = "";
        var posicion = filatr[0].rowIndex + 1;
        var versionfila = "";

        if (filatr[0].hasAttribute("tecnologia")) {
            tecnologiafila = filatr[0].getAttribute("tecnologia");
        }
        if (filatr[0].hasAttribute("area")) {
            areafila = filatr[0].getAttribute("area");
        }
        if (filatr[0].hasAttribute("sistema")) {
            sistemafila = filatr[0].getAttribute("sistema");
        }
        if (filatr[0].hasAttribute("equipo")) {
            equipofila = filatr[0].getAttribute("equipo");
        }

        CreaFila(tecnologiafila, areafila, sistemafila, equipofila, "", posicion, versionfila, "EQUIPO");
    });

    $('.Eliminar').on('click', function () {
        var fila = $(this).closest('tr');
        var tipoRegistro = fila[0].className;
        var tecnologia = fila[0].getAttribute('tecnologia');
        var textoHijos = ' y se eliminarán los niveles inferiores asociados al nivel seleccionado';

        if (tipoRegistro == "NIVELCUATRO") {
            textoHijos = "";
        }
        var resultado = confirm('Se eliminará el nivel' + textoHijos + '. ¿Desea continuar?');

        if (resultado) {
            if (tipoRegistro == "AREA") {
                var area = fila[0].getAttribute('area');
                EliminaArea(tecnologia, area);
            } else if (tipoRegistro == "SISTEMA") {
                var area = fila[0].getAttribute('area');
                var sistema = fila[0].getAttribute('sistema');
                localStorage.setItem("areaSeleccionada", area);
                localStorage.setItem("nivel", 1);

                EliminaSistema(tecnologia, area, sistema);

            } else if (tipoRegistro == "EQUIPO") {
                var area = fila[0].getAttribute('area');
                var sistema = fila[0].getAttribute('sistema');
                var equipo = fila[0].getAttribute('equipo');
                localStorage.setItem("areaSeleccionada", area);
                localStorage.setItem("sistemaSeleccionado", sistema);
                localStorage.setItem("nivel", 2);
                EliminaEquipo(tecnologia, area, sistema, equipo);
            } else if (tipoRegistro == "NIVELCUATRO") {
                var area = fila[0].getAttribute('area');
                var sistema = fila[0].getAttribute('sistema');
                var equipo = fila[0].getAttribute('equipo');
                var nivelcuatro = fila[0].getAttribute('nivelcuatro');
                localStorage.setItem("areaSeleccionada", area);
                localStorage.setItem("sistemaSeleccionado", sistema);
                localStorage.setItem("equipoSeleccionado", equipo);
                localStorage.setItem("nivel", 3);
                EliminaNivelCuatro(tecnologia, area, sistema, equipo, nivelcuatro);
            }
        }
    });

    $('#datatableMatrizRiesgos').on('click', 'td', function () {
        var tr = $(this).closest('tr');
        var td = $(this).closest('td');
        var table = $("#datatableMatrizRiesgos");
        //var row = table.row(tr);
        var row = table[0].rows[parseInt(tr[0].className.trim())];

        var claseclick = tr[0].className;
        var valor = td.find('input[type="text"]');


        if (valor != "nuevaFila" && valor != "EditaFila") {


            var estadoIcono;
            if (!(this).closest('tr').classList.contains("Tecnologia")) {


                if (td[0].hasAttribute("columnaPrincipal")) {
                    if (typeof ($(this).closest('td')[0]) != "undefined") {

                        if (typeof ($(this).closest('td')[0].childNodes[1]) != "undefined") {
                            estadoIcono = $(this).find('i').first().is(':hidden');

                            if (estadoIcono == false) {
                                // $(this).closest('td')[0].childNodes[1].hidden = true;
                                // $(this).closest('td').find('i[class="icon-arrow-down"]')[0].hidden = false;
                                $(this).children('i').first().hide();
                                $(this).children('i').last().show();
                                // $(this).closest('td').find('i[class="icon-arrow-up"]')[0].hidden = true;
                            } else {
                                // $(this).closest('td')[0].childNodes[1].hidden = false;
                                //$(this).closest('td').find('i[class="icon-arrow-down"]')[0].hidden = true;
                                //$(this).closest('td').find('i[class="icon-arrow-up"]')[0].hidden = false;
                                $(this).children('i').last().hide();
                                $(this).children('i').first().show();
                            }
                        }
                    }
                }
            }
            //else {
            //    if ($(this).closest('tr').find('i').is(':hidden')) {
            //        $(this).closest('tr').find('i').show();
            //    } else {
            //        $(this).closest('tr').find('i').hide();
            //    }

            //}

            var nombrefila = tr[0].id;
            var tipofila = tr[0].id;

            var tecnologia_fila = tr[0].getAttribute("tecnologia");
            var area_fila = tr[0].getAttribute("area");
            var sistema_fila = tr[0].getAttribute("sistema");
            var equipo_fila = tr[0].getAttribute("equipo");
            if (td[0].hasAttribute("columnaPrincipal")) {
                $("table tr").each(function () {

                    var fila = $(this);
                    if (fila[0].hasAttribute("tecnologia")) {
                        if (claseclick == "Tecnologia") {
                            if (fila[0].getAttribute("tecnologia") == tecnologia_fila) {
                                if (fila.is(':hidden')) {
                                    if (fila[0].className != claseclick && fila[0].className == "AREA") {
                                        $(this).show();
                                    }
                                } else {
                                    if (fila[0].className != claseclick) {
                                        $(this).hide();

                                        //$(this).closest('td').find('i[class="icon-arrow-down"]')[0].hidden = false;
                                    }
                                }
                            }
                        }
                    }

                    if (fila[0].hasAttribute("area")) {
                        if (claseclick == "AREA" && fila[0].getAttribute("area") == area_fila && fila[0].getAttribute("tecnologia") == tecnologia_fila) {
                            if (fila.is(':hidden')) {
                                if (fila[0].className == "SISTEMA" && fila[0].className != claseclick) {
                                    $(this).show();
                                }
                            } else {
                                if (fila[0].className != claseclick) {
                                    $(this).hide();
                                }
                            }
                        }

                        if (claseclick == "SISTEMA" && fila[0].getAttribute("sistema") == sistema_fila && fila[0].getAttribute("tecnologia") == tecnologia_fila) {
                            if (fila.is(':hidden')) {
                                if (fila[0].className == "EQUIPO" && fila[0].className != claseclick) {
                                    $(this).show();
                                }
                            } else {
                                if (fila[0].className != claseclick) {
                                    $(this).hide();
                                }
                            }
                        }
                        if (claseclick == "EQUIPO" && fila[0].getAttribute("equipo") == equipo_fila && fila[0].getAttribute("tecnologia") == tecnologia_fila) {
                            if (fila.is(':hidden')) {
                                if (fila[0].className != claseclick && fila[0].className == "NIVELCUATRO") {
                                    $(this).show();
                                }
                            } else {
                                if (fila[0].className != claseclick) {
                                    $(this).hide();
                                }
                            }
                        }

                        if ($(fila).is(":hidden")) {
                            $(fila).children('td').first().children('i').first().hide();
                            $(fila).children('td').first().children('i').last().show();
                        }
                    }

                });
            }

        }

    });


});
