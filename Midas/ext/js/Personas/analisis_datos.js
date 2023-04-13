
var filtros = {
    actividad: [],
    centroTrabajo: [],
};

var perfilRiesgo = [];
var resultado = [];
var resultadoListaFinal = [];
var arrayDeObjetosFiltrados = [];

var arrayObjetosAgrupadosPerfilRiesgo = [];
var arrayObjetosAgrupadosOcupacion = [];
var arrayObjetosAgrupadosOcupacion = {};
var arrayObjetosAgrupadosUnidadOrganzativa = [];
var arrayObjetosAgrupadosPosicion = [];
var listaNumeroEmpleadoSeleccionados = [];
var listaFinalPorUsuario = [];
var miArrayChk = [];


$(document).ready(() => {

    $('#DataTables_Table_0_filter').hide(); //oculatos el buscador del dataTable
    $("#perfilRiesgo").hide(); //ocultamos el select de perfil riesgo
    var hdActividad = $('hdActividad');
    $('#container').hide();
    $('#divButtonGuardar').hide();
    //$("#<%=hdnDSS.ClientID%>").val(salida);

    //#region Select multiples

    $('#actividad').multiselect({
        nonSelectedText: 'Ninguna Actividad Seleccionada',
        includeSelectAllOption: true,
        selectAllText: ' Seleccionar/Deseleccionar Todo',
        maxHeight: 200,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        filterPlaceholder: 'Buscar en la lista Actividad',
        nSelectedText: 'seleccionados',

        onChange: function (option, checked, select) {
            let seleccionados = $('#actividad').multiselect('select', []);

            filtros.actividad = seleccionados[0].selectedOptions;
        }
    });

    $('#centroTrabajo').multiselect({
        nonSelectedText: 'Ninguna Centro de Trabajo Seleccionado',
        includeSelectAllOption: true,
        selectAllText: ' Seleccionar/Deseleccionar Todo',
        maxHeight: 200,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true,
        filterPlaceholder: 'Buscar en la lista Centro de Trabajo',
        nSelectedText: 'seleccionados',

        onChange: function (option, checked, select) {
            let seleccionados = $('#centroTrabajo').multiselect('select', []);

            filtros.centroTrabajo = seleccionados[0].selectedOptions;
        }
    });

    function contarOcupacionesPorPerfil(resultados, perfil) {
        // Filtrar los objetos en "resultado" que tengan el perfil especificado
        const resultadosFiltrados = $.grep(resultados, function (resultado) {
            return resultado.Perfil_de_riesgo === perfil;
        });

        // Crear un objeto que contendrá la cuenta de cada "Ocupacion"
        const cuentaOcupaciones = {};

        // Recorrer los objetos filtrados y contar la cantidad de cada "Ocupacion"
        $.each(resultadosFiltrados, function (index, resultado) {
            const ocupacion = resultado.Ocupacion;
            if (cuentaOcupaciones[ocupacion]) {
                cuentaOcupaciones[ocupacion]++;
            } else {
                cuentaOcupaciones[ocupacion] = 1;
            }
        });

        // Devolver el objeto que contiene la cuenta de cada "Ocupacion"
        return cuentaOcupaciones;
    }

    function agruparPorOcupacion(resultados, perfil) {
        // Filtrar los resultados por perfil
        const resultadosFiltrados = resultados.filter(result => result.Perfil_de_riesgo === perfil);

        // Crear un objeto vacío para almacenar los resultados agrupados
        const resultadosAgrupados = {};

        // Iterar por cada resultado filtrado y agruparlo por Ocupacion
        resultadosFiltrados.forEach(resultado => {
            if (resultado.Ocupacion in resultadosAgrupados) {
                resultadosAgrupados[resultado.Ocupacion].push(resultado);
            } else {
                resultadosAgrupados[resultado.Ocupacion] = [resultado];
            }
        });

        // Convertir el objeto a un array de objetos
        const arrayResultadosAgrupados = Object.keys(resultadosAgrupados).map(key => ({ Ocupacion: key, Resultados: resultadosAgrupados[key] }));

        // Retornar el array de resultados agrupados
        return arrayResultadosAgrupados;
    }

    //#endregion Select multiples

    const getDatosFiltrados = async () => {

        try {
            $('#aspnetForm').on('submit', (e) => {
                e.preventDefault();
            });

            const miarrayActividades = Array.from(filtros.actividad);
            const miarrayCentroTrabajo = Array.from(filtros.centroTrabajo);

            let misActividades = [];
            let misCentrosTrabajo = [];

            for (let i = 0; i < miarrayActividades.length; i++) {
                misActividades.push(filtros.actividad[i].value.toString());
            }

            for (let i = 0; i < miarrayCentroTrabajo.length; i++) {
                misCentrosTrabajo.push(filtros.centroTrabajo[i].value.toString());
            }

            var objeto = {
                actividad: misActividades,
                centroTrabajo: misCentrosTrabajo,
            };

            const filtrosJson = JSON.stringify(objeto);

            getListaFinalPorUsuario();

            $.ajax({
                url: "FiltrosDatos", //Your path should be here
                data: { filtrosJson: filtrosJson },
                type: "post",
                success: function (datos) {

                    if (datos != null) {

                        resultado = JSON.parse(datos);

                        if (resultado.length === 0) {

                            Swal.fire({
                                position: 'center',
                                icon: 'info',
                                title: 'No existe resultados para el analisis',
                                showConfirmButton: false,
                                timer: 2000
                            });

                            $("#container").empty();
                            $("#divButtonGuardar").hide();


                        } else {
                            Swal.fire({
                                position: 'center',
                                icon: 'success',
                                title: 'Obteniendo resultados',
                                showConfirmButton: false,
                                timer: 1000
                            });

                            var arrayObjetos = [];

                            var resultadosParaSelectorPerfilRiesgo = {};

                            for (var i = 0; i < resultado.length; i++) {
                                var campoPerfilRiesgo = resultado[i].Perfil_de_riesgo;
                                if (!resultadosParaSelectorPerfilRiesgo[campoPerfilRiesgo]) {
                                    resultadosParaSelectorPerfilRiesgo[campoPerfilRiesgo] = [];
                                }
                                resultadosParaSelectorPerfilRiesgo[campoPerfilRiesgo].push(resultado[i]);
                            }

                            //convertirmos el objeto en array, para el dataProvider

                            arrayDeObjetosFiltrados = Object.entries(resultadosParaSelectorPerfilRiesgo).map(([id, data]) => ({ id, data }));

                            //console.log(arrayDeObjetosFiltrados);

                            for (var i = 0; i < arrayDeObjetosFiltrados.length; i++) {
                                var objeto = {
                                    label: arrayDeObjetosFiltrados[i].id,
                                    title: arrayDeObjetosFiltrados[i].id,
                                    value: arrayDeObjetosFiltrados[i].id
                                }
                                arrayObjetos.push(objeto);
                            }

                            const labelArray = arrayObjetos.map(obj => obj.label);

                            //console.log(labelArray);

                            const agrupado = agruparEmpleadosPorPerfilDeRiesgo(resultado);
                            //console.log(agrupado);

                            $("#container").empty();

                            for (let perfil of labelArray) {
                                contenedorPerfilRiesgo(perfil);
                                //console.log(perfil);
                            }

                            $("#container").show();
                            $("#divButtonGuardar").show();
                        }
                    }
                }
            }
            );

        } catch (error) {
            console.log(error);
        }
    };

    function getListaFinalPorUsuario() {

        $.ajax({
            url: "ListaFinalPorUsuario", //Your path should be here
            type: "post",
            success: function (datos) {

                if (datos != null) {

                    listaFinalPorUsuario = JSON.parse(datos);

                    if (listaFinalPorUsuario.length === 0) {
                        console.log(listaFinalPorUsuario);
                    } else {
                        console.log(listaFinalPorUsuario);
                    }

                }
            }
        }
        );
    }

    const contenedorPerfilRiesgo = ((perfil) => {
        arrayObjetosAgrupadosOcupacion = contarOcupacionesPorPerfil(resultado, perfil);

        const resultadosAgrupados = agruparPorOcupacion(resultado, perfil);

        $('#container').show();

        let totalNombres = resultadosAgrupados.reduce((acumulador, objeto) => {
            return acumulador + objeto.Resultados.length;
        }, 0);

        var $perfil = $("<h1>").text(perfil + " (" + totalNombres + ")");
        $perfil.addClass("perfil");
        $("#container").append($perfil);


        resultadosAgrupados.forEach(function (resultado) {
            var ocupacion = resultado.Ocupacion;
            var unidadesOrganizativas = resultado.Resultados.reduce(function (acc, resultado) {
                var unidadOrganizativa = resultado.Unidad_Organizativa;
                var posicion = resultado.Posicion;
                var nombre = resultado.Nombre;
                var numeroEmpleado = resultado.Nº_Empleado;

                if (!acc[unidadOrganizativa]) {
                    acc[unidadOrganizativa] = [];
                }
                acc[unidadOrganizativa].push({ posicion, nombre, numeroEmpleado });
                return acc;
            }, {});

            var $ocupacion = $("<h2>").text(ocupacion);
            $ocupacion.addClass("ocupacion");
            var $unidadesOrganizativas = $("<ul>").hide();

            Object.keys(unidadesOrganizativas).forEach(function (unidadOrganizativa) {
                var $unidadOrganizativa = $("<h3>").text(unidadOrganizativa);
                $unidadOrganizativa.addClass("unidad-organizativa");
                var $posiciones = $("<ul>").hide();
                unidadesOrganizativas[unidadOrganizativa].forEach(function (item) {
                    var $posicion = $("<li>").text(item.posicion + ' - ' + item.nombre);

                    // Agreguo el atributo Nº_Empleado
                    $posicion.attr({
                        'data-num_empleado_li': item.numeroEmpleado,
                    });

                    // Agreguo el elemento input checkbox
                    var $inputCheckbox = $("<input>").attr({
                        type: "checkbox",
                        'data-num_empleado_chk': item.numeroEmpleado,
                    }).click(function (event) {
                        verificarCheck(event, $(this));
                    });

                    // Se establece el valor del atributo checked
                    if (listaFinalPorUsuario.includes(item.numeroEmpleado)) {
                        $inputCheckbox.prop('checked', true);
                    }

                    $posicion.append($inputCheckbox);

                    $posicion.addClass("posicion");

                    $posiciones.append($posicion);
                });

                $unidadOrganizativa.click(function () {
                    $posiciones.toggle();
                });

                // Agregar la cantidad de etiquetas <li> dentro de la etiqueta <ul>
                var cantidadLi = $posiciones.find('li').length;
                $unidadOrganizativa.text($unidadOrganizativa.text() + " (" + cantidadLi + ")");

                $unidadesOrganizativas.append($unidadOrganizativa).append($posiciones);
            });

            // Agregar la cantidad de etiquetas <h3> dentro de la etiqueta <h2>
            var cantidadLiOcupaciones = $unidadesOrganizativas.find('li').length;
            $ocupacion.text($ocupacion.text() + " (" + cantidadLiOcupaciones + ")");

            $perfil.click(function () {
                $ocupacion.toggle();
            });

            $ocupacion.click(function () {
                $unidadesOrganizativas.toggle();
            });

            $("#container").append($ocupacion).append($unidadesOrganizativas);
        });
    });

    function verificarCheck(event, $inputCheckbox) {

        if ($inputCheckbox.prop('checked')) {
            console.log('El checkbox está marcado');
        } else {
            console.log('El checkbox no está marcado');


            if ($.inArray($inputCheckbox.attr('data-num_empleado_chk'), listaFinalPorUsuario) !== -1) {
                console.log('El valor existe en el array');

                let valorParaBorrar = $inputCheckbox.attr('data-num_empleado_chk');
                //esta llamada es para borrar el registro, si es que existe en BD
                $.ajax({
                    url: "BorrarPersonaDeListaFinal", //Your path should be here
                    data: { valorParaBorrar: valorParaBorrar },
                    type: "post",
                    success: function (datos) {

                        if (datos != null) {

                            resultadoListaFinal = JSON.parse(datos);

                            if (resultadoListaFinal.length === 0) {

                                console.log("no se pudo borrar");

                            } else {
                               
                                console.log("borrado");


                            }
                        }
                    }
                });

            } else {
                console.log('El valor no existe en el array');
            }


        }

    }

    function empleadosListaFinal(event, $inputCheckbox) {

        const arrayInts = listaFinalPorUsuario.map(str => parseInt(str));
        console.log(arrayInts);
        listaNumeroEmpleadoSeleccionados = [...arrayInts];
        console.log(listaNumeroEmpleadoSeleccionados);

        var numEmpleado = $inputCheckbox.data('num_empleado_chk');

        if ($inputCheckbox.is(':checked')) {
            if (!listaNumeroEmpleadoSeleccionados.includes(numEmpleado)) {
                listaNumeroEmpleadoSeleccionados.push(numEmpleado);
            }
        } else {
            listaNumeroEmpleadoSeleccionados = listaNumeroEmpleadoSeleccionados.filter(item => item !== numEmpleado);
        }
    }

    function agruparEmpleadosPorPerfilDeRiesgo(empleados) {
        let agrupado = {};

        for (let i = 0; i < empleados.length; i++) {
            let perfil = empleados[i].Perfil_de_riesgo;
            if (!agrupado[perfil]) {
                agrupado[perfil] = [];
            }
            agrupado[perfil].push(empleados[i]);
        }

        return agrupado;
    }

    $('#btnAnalizar').on('click', async function () {

        const misDatos = await getDatosFiltrados();
    });

});

function guardarSeleccionados() {

    const valores = $("input[type='checkbox'][data-num_empleado_chk]:checked").map(function () {
        return $(this).data("num_empleado_chk");
    }).get();

    console.log(valores);

    listaNumeroEmpleadoSeleccionados = [...valores];


    $.ajax({
        url: "GuardarElementosSeleccionados", //Your path should be here
        data: { listaNumeroEmpleadoSeleccionados: listaNumeroEmpleadoSeleccionados },
        type: "post",
        success: function (datos) {

            if (datos != null) {

                resultadoListaFinal = JSON.parse(datos);

                if (resultadoListaFinal.length === 0) {

                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: 'No se pudo guardar los elementos seleccionados',
                        showConfirmButton: false,
                        timer: 2000
                    });

                } else {
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: 'Elementos seleccionados, guardados!!!',
                        showConfirmButton: false,
                        timer: 1000
                    });


                }
            }
        }
    });
}