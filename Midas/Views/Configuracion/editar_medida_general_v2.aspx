<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>




<script runat="server">  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.medidas_generales oMedida;
    MIDAS.Models.medidas_generales_imagenes oImagen;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            DatosPedidos.DataSource = ViewData["areas"];
            DatosPedidos.DataBind();
        }
        oMedida = (MIDAS.Models.medidas_generales)ViewData["EditarMedida"];

        if (oMedida != null)
        {
            //hdnIdMedida.Value = oMedida.id.ToString();
            //ImagenGrande.Checked=MIDAS.Models.Datos.EsImagenGrandeMedidaGeneral(oMedida.id);
        }


        if (Session["EditarAreaResultado"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarAreaResultado"].ToString() + "' });", true);
            Session.Remove("EditarAreaResultado");
        }

        if (Session["EditarAreaError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarAreaError"].ToString() + "' });", true);
            Session.Remove("EditarAreaError");
        }



        if (ViewData["apartados"] != null)
        {
            ddlApartados.DataSource = ViewData["apartados"];
            ddlApartados.DataValueField = "id";
            ddlApartados.DataTextField = "descripcion";
            ddlApartados.DataBind();




            if (oMedida != null)
            {

               // txtNombre.Text = oMedida.descripcion.Replace("[SALTO]", "\n");
                int apartado = 0;
                int.TryParse(oMedida.id_apartado_generales.ToString(), out apartado);
                if (apartado > 0)
                {


                    ddlApartados.SelectedIndex = apartado - 1/*MIDAS.Models.Datos.obtenerNombreApartadoGenerales(apartado)*/;
                }
            }
        }

        if (!IsPostBack)
        {
            oMedida = (MIDAS.Models.medidas_generales)ViewData["EditarMedida"];

            if (oMedida != null)
            {
                int apartado = 0;
                int.TryParse(oMedida.id_apartado_generales.ToString(), out apartado);
                ddlApartados.SelectedValue = MIDAS.Models.Datos.obtenerNombreApartadoGenerales(apartado);
                //txtNombre.Text = oMedida.descripcion.Replace("[SALTO]", "\n");
                ViewData["idMedida"] = oMedida.id;
            }



        }


    }



</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Centro </title>

    <link rel="stylesheet" href="../_css/Configuracion/editar_medida_general.css"></link>

    <script type="text/javascript" src="../js/editar_medida_general.js"></script>

    <script type="text/javascript">


        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarCentral")
                    $("#hdFormularioEjecutado").val("GuardarCentral");

            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

            $("#ctl00_MainContent_divN1").show();
            $("#btnN1").addClass('active');
            //$("#ctl00_MainContent_ddlTecnologia").prop('disabled', true);
            $("#ctl00_MainContent_divN2").hide();
            $("#ctl00_MainContent_divN3").hide();

            //comprobar url id
            var pathname = window.location.pathname;
            var aux = pathname.split('/');
            if (aux[4] == 0) {
                $("#AREAS").hide();
            }



            $("#btnN1").click(function () {
                $("#ctl00_MainContent_divN1").show();
                $("#btnN1").addClass('active');
                $("#ctl00_MainContent_divN2").hide();
                $("#btnN2").removeClass('active');
                $("#ctl00_MainContent_divN3").hide();
                $("#btnN3").removeClass('active');
            });

            $("#btnN2").click(function () {
                $("#ctl00_MainContent_divN1").hide();
                $("#btnN1").removeClass('active');
                $("#ctl00_MainContent_divN2").show();
                $("#btnN2").addClass('active');
                $("#ctl00_MainContent_divN3").hide();
                $("#btnN3").removeClass('active');

            });

            $("#btnN3").click(function () {
                $("#ctl00_MainContent_divN1").hide();
                $("#btnN1").removeClass('active');
                $("#ctl00_MainContent_divN2").hide();
                $("#btnN2").removeClass('active');
                $("#ctl00_MainContent_divN3").show();
                $("#btnN3").addClass('active');

            });

        });





    </script>
</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">

    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3></h3>
        </div>
    </div>

    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form role="form" action="#" runat="server" method="post">
        <%--<input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
        <div class="panel panel-default">
            <div class="panel-heading">
                <h6 class="panel-title">
                    <i class="icon-office"></i>Datos de medida general</h6>
                <asp:HiddenField runat="server" ID="hdnIdMedida" ClientIDMode="Static" />
            </div>--%>
            <div class="panel-body">

                <div class="container">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>APARTADOS:</label>&nbsp;
                                    <asp:DropDownList ID="ddlApartados" CssClass="form-control" runat="server">
                                    </asp:DropDownList>
                            </div>
                        </div>

                    </div>

                  <div class="row">
                    <div class="col-md-2">
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="agregarIcono" onclick="muestraOpcionesIcono(this);">
                            <label class="form-check-label" for="flexCheckDefault">
                                Agregar icono
                            </label>
                        </div>
                    </div>

                    <div id="areaIcono" style="display: none;">
                        <div class="col-md-5">
                            <br />
                            <img id="imagenPrevia" src="../../Content/images/icono.png" alt="imagen de icono" width="100"/>
                            <br />
                            <label>   Archivo: </label> <input type='file' id="imagenCargada" />
                            <br />
                            <label>
                                DESCRIPCION ICONO:</label>
                            <asp:TextBox ID="txtNombre" runat="server" class="form-control" TextMode="MultiLine" Columns="100" Rows="12" Visible="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>



                <table style="width: 100%">
                   <%-- <tr>
                        <td style="padding-right: 15px">
                            <div class="form-group">
                                <label>
                                    APARTADOS:</label>&nbsp;
                    <asp:DropDownList ID="ddlApartados" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                            </div>
                        </td>

                    </tr>
                    <tr>
                        <td style="padding-right: 15px"></td>
                        <td style="padding-right: 15px"></td>
                    </tr>--%>
                   <%-- <tr>


                        <td style="padding-right: 15px">
                            <div class="form-group">

                                <label>IMAGEN DE MEDIDA:</label>&nbsp;
                                  <%if (oMedida != null)
                                      { %>
                                <%if (!string.IsNullOrEmpty(MIDAS.Models.Datos.obtenerImagenMedidasGenerales(oMedida.id)))
                                    { %>
                                <img id="IMG" style="width: 200px; height: 200px;" src="<%= "../" + MIDAS.Models.Datos.obtenerImagenMedidasGenerales(oMedida.id)%>" action="GuardarImagen" method="post" enctype="multipart/form-data">
                                <%}
                                    else
                                    { %>
                                <img id="IMG" style="width: 200px; height: 200px;" src="" action="GuardarImagen" method="post" enctype="multipart/form-data">
                                <%}

                                    } %>
                                <asp:Image ID="imagenCentro" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:Image>
                                <%if (oMedida != null)
                                    { %>
                                <input id="file" type="file" name="img" multiple accept="image/*" class="file" onchange="file_changed(this);" onclick="this.value=null;ocultar(this);">
                                <%} %>

                                <div class="form-group" style="margin-top: 10px;">
                                    <a id="SubirCentro" class="btn btn-primary run-first" style="display: none;">Subir</a>
                                </div>
                                <div class="form-group">
                                    <label>TAMAÑO GRANDE:</label>&nbsp;
                                    <asp:CheckBox ID="ImagenGrande" runat="server" ClientIDMode="Static" Enabled="false" />
                                </div>
                            </div>
                        </td>
                        <td style="padding-right: 15px">
                            <div class="form-group">
                                <label>
                                    DESCRIPCION:</label>
                                <asp:TextBox ID="txtNombre" runat="server" class="form-control" TextMode="MultiLine" Columns="100" Rows="12"></asp:TextBox>
                            </div>
                        </td>
                        <td style="padding-right: 15px">
                            <div class="form-group">
                                <label>
                                    TAMAÑO GRANDE:</label>&nbsp;--%>
                           <%--<asp:CheckBox ID="ImagenGrande" runat="server" ClientIDMode="Static" Enabled="false" />--%>
                  <%--          </div>
                        </td>--%>

                      <%--  <td style="padding-right: 15px">
                            <div class="form-group">
                                <label>
                                    LOGO:</label>&nbsp;
                     <asp:Image ID="imagenLogo" CssClass="form-control" runat="server"></asp:Image>
                                <asp:FileUpload ID="subirLogo" runat="server" />
                                <button id="SubirLogo" disabled>Subir</button>
                            </div>
                        </td>
                    </tr>--%>
                </table>
            </div>
        </div>

        <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
        </asp:GridView>
        <!-- Tasks table -->



        <!-- /tasks table -->

        <!-- /modal with table -->
        <div class="form-actions text-right">
            <input id="GuardarUsuario" type="submit" value="Guardar datos" class="btn btn-primary run-first" name="submit">
            <a href="/evr/configuracion/medidas_generales" title="Volver" class="btn btn-primary run-first">Volver</a>
        </div>
    </form>
    <!-- /form vertical (default) -->
    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left">
        </div>
    </div>
    <!-- /footer -->
    <script type="text/javascript">
        function ocultar(valor) {

            $('#SubirCentro').hide();
            $('#SubirLogo').hide();
        }

        function muestraOpcionesIcono(opcion) {

            opcion.checked ? $('#areaIcono').show() : $('#areaIcono').hide();

        }

        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#imagenPrevia').attr('src', e.target.result);
                }

                reader.readAsDataURL(input.files[0]);
            }
        }

        $("#imagenCargada").change(function () {
            readURL(this);
        });

        $(document).ready(function () {

            $('.file').on('change', function () {
                $('#SubirCentro').show();
                $('#ImagenGrande').prop("disabled", false);

            });
            $('.filelogo').on('change', function () {
                $('#SubirLogo').show();
            });

            $('#SubirCentro').on('click', function () {
                var boton = $(this).closest('button')[0];

                var form_data = new FormData();
                var req = new XMLHttpRequest();
                var formData = new FormData();
                var photo = document.getElementById('file').files[0];
                $('#ImagenGrande').prop("disabled", true);
                var central = $('#hdnIdMedida').val();
                form_data.append("photo", photo);
                form_data.append('hdnIdMedida', central);
                form_data.append('tamano', $('#ImagenGrande').is(':checked'));
                var imagen = $.ajax({
                    url: '../GuardarImageMedidaGeneral', // point to server-side controller method
                    data: form_data,
                    cache: false,
                    contentType: false,
                    processData: false,
                    data: form_data,
                    type: 'post',
                    success: function (response) {

                        if (response != null) {
                            $.each(response, function (i, response) {

                                $('#SubirCentro').hide();
                                $('#IMG').attr("src", '../' + response.rutaImagen);
                                alert('Imagen modificada');


                            });
                        }

                    },
                    error: function (response) {
                        alert('error'); // display error response from the server
                    }
                });


            });
            $('#SubirLogo').on('click', function () {
                var boton = $(this).closest('button')[0];

                var form_data = new FormData();
                var req = new XMLHttpRequest();
                var formData = new FormData();
                var photo = document.getElementById('filelogo').files[0];
                var central = $('#hdnIdMedida').val();
                form_data.append("photo", photo);
                form_data.append('hdnIdMedida', central);

                var imagen = $.ajax({
                    url: '../GuardarImagenLogo', // point to server-side controller method
                    data: form_data,
                    cache: false,
                    contentType: false,
                    processData: false,
                    data: form_data,
                    type: 'post',
                    success: function (response) {

                        if (response != null) {
                            $.each(response, function (i, response) {

                                $('#Subirlogo').hide();
                                $('#IMGlogo').attr("src", '../' + response.rutaImagenLogo);
                                alert('Imagen modificada');


                            });
                        }

                    },
                    error: function (response) {
                        alert('error'); // display error response from the server
                    }
                });


            });
        });

    </script>
</asp:Content>
