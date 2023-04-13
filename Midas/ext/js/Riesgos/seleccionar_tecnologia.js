$(document).ready(function () {
    $("#MenuMatrizRiesgos").css('color', 'black');
    $("#MenuMatrizRiesgos").css('background-color', '#ebf1de');
    $("#MenuMatrizRiesgos").css('font-weight', 'bold');
    $('.loader').hide();
    $("#TABLA_TECNOLOGIAS").dataTable().fnDestroy();
    // $('#TABLA_TECNOLOGIAS').DataTable();
    $('#TABLA_TECNOLOGIAS').DataTable({

        aLengthMenu: [
            [-1],
            ["All"]
        ],
        iDisplayLength: -1


    });

});

function AbrirDDL() {
    document.getElementById("myDropdown").classList.toggle("show");
}
$('.dropbtn').on('click', function () {
    var padre = $(this).closest('div');
    var boton = padre.find('div[id="DropArea"]');
    if (typeof ($(boton)[0]) == "undefined") {
        boton = padre.find('div[id="DropSistema"]');
    }
    $(boton)[0].classList.toggle("show");
});


// Close the dropdown if the user clicks outside of it
window.onclick = function (event) {
    if (!event.target.matches('.dropbtn')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}
function CrearMatriz() {
    var array = new Array();

    $('#TABLA_TECNOLOGIAS').find('input[type="checkbox"]').each(function () {
        var ent = $(this)[0].id;

        var $chk = $(this).find('[type=checkbox]');

        if ($(this).prop("checked")) {
            array.push(ent);
        }

    });
    $('.loader').show();

    if (array.length > 0) {

        $.ajax({
            url: "CrearMatrizMaestros",
            data: { matrizMaestro: array },
            type: "post",
            success: function (datos) {
                if (datos != null) {

                }


            }
        });

    }
}