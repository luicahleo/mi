
$(document).ready(function () {


    //mostrarModalCargarExcel();



})
    function mostrarModalCargarExcel() {
        $('#mi-modal').modal({ show: true });

}

//    const mostrarModalCargarExcel = () => {
//        $('#mi-modal').modal({ show: true });

//}

function enviarDatosExcel() {

    $("#formuploadajax").on("submit", function (e) {
        e.preventDefault();
        var f = $(this);
        var formData = new FormData(document.getElementById("formuploadajax"));
        formData.append("dato", "valor");
        //formData.append(f.attr("name"), $(this)[0].files[0]);
        $.ajax({
            url: "../Configuracion/personas/EnviarDatosExcel",
            type: "post",
            dataType: "xlsx",
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        })
            .done(function (res) {
                $("#mensaje").html("Respuesta: " + res);
            });
    });

}