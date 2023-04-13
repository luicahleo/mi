<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.documento_historico_criticos oDocumentoH = new MIDAS.Models.documento_historico_criticos();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            DatosVersiones.DataSource = ViewData["versiones"];
            DatosVersiones.DataBind();

            MIDAS.Models.documento_historico_criticos doch = MIDAS.Models.Datos.ObtenerUltimoDocumentoHistoricoCriticos(int.Parse(Session["CentralElegida"].ToString()));

            if (doch != null)
            {
                Session["DocuhistoricoLastVersionCritico"] = doch.id;
            }
            else {
                Session["DocuhistoricoLastVersionCritico"] = "";
            }

            var listaDoc = MIDAS.Models.Datos.ListaDocumentoHistoricoDefinitivoCritico(int.Parse(Session["CentralElegida"].ToString()));
            datosDocumentosH.DataSource = listaDoc;
            datosDocumentosH.DataBind();




            if (ViewData["textoUltimoBoradorCritico"] != null)
            {
                textooculto.Value = ViewData["textoUltimoBoradorCritico"].ToString();
            }
        }

        if (Session["EliminarSistema"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EliminarSistema"].ToString() + "' });", true);
            Session["EliminarSistema"] = null;
        }

        if (Session["GenerarDocumento"] != null)
        {

            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["GenerarDocumento"].ToString() + "' });", true);
            Session["GenerarDocumento"] = null;

        }
    }
</script>

<asp:Content ID="versionesHead" ContentPlaceHolderID="head" runat="server">
    <title>DIMAS</title>
    <style>
        .margenes {
            padding: 20px;
        }



        .loader {
            position: fixed;
            left: 0px;
            top: 0px;
            width: 100%;
            height: 100%;
            z-index: 9999;
            background: url('../../Content/images/pageLoader.gif') 50% 50% no-repeat rgb(249,249,249);
            opacity: .6;
        }
    </style>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="loader"></div>
    <form action="#" runat="server" enctype="multipart/form-data" id="formularioDocumento" method="post">
        <asp:GridView ID="datosDocumentosH" runat="server" Visible="false">
        </asp:GridView>
        <!-- Page header -->
        <div class="page-header">
            <div class="page-title">
                <h3>Documentos de Riesgos <small>Generar el Documento de Riesgos para la instalación</small></h3>
            </div>
        </div>
        <!-- /page header -->

        <asp:GridView ID="DatosVersiones" runat="server" Visible="false">
        </asp:GridView>
        <asp:HiddenField ID="textooculto" runat="server" ClientIDMode="Static"></asp:HiddenField>

        <div class="block">
            <center>
                <div style="width: 95%" class="datatablePedido">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th style="width: 5%">Version</th>
                                <th style="width: 5%">Estado</th>
                                <%--  <th>Tamaño</th>--%>
                                <th style="width: 42%">Fecha ultima modificacion</th>
                                <th style="width: 42%">Usuario</th>
                                <% if (user.perfil == 1 || user.perfil == 3)
                                    { %>
                                <th style="width: 5%">Descargar</th>
                                <% } %>
                            </tr>
                        </thead>

                        <tbody>
                            <%foreach (GridViewRow item in datosDocumentosH.Rows)
                                { %>

                            <tr>
                                <td>
                                    <center><%= item.Cells[3].Text %></center>
                                </td>
                                <td>
                                    <center>
                                        <%if (item.Cells[4].Text == "1")
                                            { %>
                                        <i class="icon-checkmark"></i>
                                        <%}
                                            else
                                            { %>
                                        <i class="icon-close"></i>
                                        <%}%>
                                    </center>
                                </td>
                                <td><%= item.Cells[5].Text %></td>
                                <td><%= item.Cells[6].Text %></td>
                                <td><% if (item.Cells[7].Text == "1" && item.Cells[0].Text == Session["DocuhistoricoLastVersionCritico"].ToString())
                                        {%>
                                    <center>
                                        <button id="btnDescarga" class="btn btn-primary" style="all: unset; cursor: pointer;">
                                            <a href="#"><i class="icon-download"></i></a>
                                        </button>
                                    </center>
                                    <%--<center>
                                        <a href="/evr/DocumentoRiesgos/GenerarDocumentoRiesgos" title="Descargar" ><i class="icon-download"></i></a>
                                    </center>--%>
                                    <%}
                                        else
                                        { %>

                                    <%}%>
                                </td>

                            </tr>

                            <%} %>
                        </tbody>


                    </table>
                </div>

                <table width="100%">
                    <tr>
                        <td style="width: 20%" class="margenes">
                            <center>
                                <div id="botonVerComentarios" class="btn btn-primary" style="border-radius: 20px; width: 65%; cursor: none; background-color: #41b9e6 !important; border-color: #41b9e6 !important">
                                    <table style="width: 100%; height: 30px">

                                        <tr>
                                            <td>
                                                <center>
                                                    <span style="font-size: 13px">
                                                        <label id="label5">Generar Documento de Riesgos</label>
                                                    </span>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                        </td>
                    </tr>
                    <tr id="filaComentarios">
                        <td class="margenes">
                            <center>
                                <div style="width: 60%" class="datatablePedido">
                                    <div style="text-align: left">
                                        <label>Control de Cambios (Descripción) </label>
                                    </div>

                                    <textarea id="texto_descripcion" style="width: 100%;" name="Text1" cols="150" rows="10" onchange="activarboton()" style="white-space: pre-line;"></textarea>

                                    <div style="text-align: right">
                                        <a id="botonBorrador" onclick="generarBorrador()" title="Borrador" class="btn btn-primary run-first">Visualizar Borrador</a>
                                        <%if (ViewData["Haymatrizborrador"] != null && ViewData["Haymatrizborrador"].ToString() != "0")
                                            { %>
                                        <a id="botonDefinitivo" onclick="generarDefinitivo()" title="Definitivo" class="btn btn-primary run-first">Generar Versión Definitiva</a>
                                        <%}
                                            else
                                            {%>
                                        <button id="botonDefinitivo" onclick="generarDefinitivo()" title="Definitivo" class="btn btn-primary run-first" disabled="disabled">Generar Versión Definitiva</button>
                                        <br />
                                        <br />
                                        <label style="color: red">Para generar la Versión Definitiva es necesario tener una matriz de riesgos en estado borrador.</label>
                                        <%}%>
                                    </div>
                                </div>
                            </center>
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="width: 20%" class="margenes">
                            <center>
                                <a class="btn btn-primary" style="border-radius: 20px; width: 65%">
                                    <table style="width: 100%; height: 30px">
                                        <tr>
                                            <td>
                                                <center>
                                                    <span style="font-size: 13px">
                                                        <asp:Label ID="label3" runat="server" Text="Generar Mapa de Riesgos" />
                                                    </span>
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </a>
                            </center>
                        </td>
                    </tr>--%>
                </table>

                <br />

            </center>
        </div>
        <!-- Tasks table -->

        <div style="text-align: right">
            <% if ((user.perfil == 1 || user.perfil == 3) && DatosVersiones.Rows.Count < 1)
                { %>

            <% } %>
            <a href="/evr/Home/principal" title="Volver" class="btn btn-primary run-first">Volver</a>
        </div>
        <p>
            <br />
        </p>
        <!-- Footer -->
        <div class="footer clearfix">
            <div class="pull-left"></div>
        </div>
    </form>
    <!-- /footer -->
    <script type="text/javascript">

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
        function generarBorrador() {

            var texto = '';
            texto = $("#texto_descripcion").val();
            var textoformateado = texto.replaceAll('\n', '-salto-');
            //if ($("#textooculto").val() == textoformateado) {
            //    alert("Para generar un borrador debe actualizar lo recogido en el cuadro de control de cambios.");
            //} else {

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
                    window.open('/evr/DocumentoRiesgos/GenerarDocumentoBorradorCritico/?descrDoc=' + textoformateado + '');
                    // window.location.href = '/evr/DocumentoRiesgos/GenerarDocumentoBorrador/?descrDoc=' + textoformateado + '';
                    //setTimeout(function () {
                    //$('.loader').hide();
                }
            })



            //var input;
            //input = confirm('Se añadirá la modificación al borrador. ¿Desea continuar?:');


            //$('.loader').show();
            //if (input === false) {
            //    $('.loader').hide();
            //    return; //break out of the function early
            //} else {
            //    window.open('/evr/DocumentoRiesgos/GenerarDocumentoBorrador/?descrDoc=' + textoformateado + '');
            //    // window.location.href = '/evr/DocumentoRiesgos/GenerarDocumentoBorrador/?descrDoc=' + textoformateado + '';
            //    //setTimeout(function () {
            //    $('.loader').hide();
            //    //},10000);



            //    //$.ajax({
            //    //    url: "../GenerarDocumento",
            //    //    type: "post",
            //    //    data: { descrDoc: textoformateado, esborrador: true },
            //    //    success: function (datos) {
            //    //        window.location.href = '/evr/Home/principal';

            //    //    }
            //    //});

            //}



            //}
        }
        function generarDefinitivo() {

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

                    window.location.href = '/evr/DocumentoRiesgos/GenerarDocumentoDefinitivoCritico/?descrDoc=' + textoformateado + '';
                    console.log(window.location.href);

                }
            })

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
        //            data: { id: idVersion },
        //            type: "post",
        //            success: function (datos) {
        //                if (datos != null) {

        //                }


        //            }
        //        });

        //    }
        //}
    </script>
</asp:Content>
