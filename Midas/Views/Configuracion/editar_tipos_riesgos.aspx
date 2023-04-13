<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.tipos_riesgos oRiesgo;
    MIDAS.Models.areanivel1 oArea;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (ViewData["riesgos"] != null)
        {
            oRiesgo=(MIDAS.Models.tipos_riesgos )ViewData["riesgos"];
            txtDescripcionRiesgo.Text=oRiesgo.definicion;
            txtNombreRiesgo.Text=oRiesgo.codigo;
        }
    }


</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Editar Riesgo</title>
    <script type="text/javascript">


        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarRiesgo")
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
    <div class="page-header">
        <div class="page-title">
            <h3></h3>
        </div>
    </div>
    <!-- Form vertical (default) -->
    <form role="form" action="#" runat="server">
        <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
        <div class="panel panel-default" >
            <div class="panel-heading">
                <h6 class="panel-title">
                    <i class="icon-office"></i>Datos de riesgo</h6>
            </div>
            <div class="panel-body">

                <table style="width: 100%">
                    <tr>
                        <td style="padding-right: 15px">
                            <div class="form-group">
                                <asp:HiddenField runat="server" ID="hdnIdCentral" />
                                <label>
                                    TIPO RIESGO:</label>
                                <asp:TextBox id="txtNombreRiesgo" Name="txtNombreRiesgo"  runat="server" class="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                            </div>
                        </td>
                        <td style="padding-right: 15px">
                            <div class="form-group">
                                <label>
                                    DESCRIPCIÓN:</label>
                                <asp:TextBox id="txtDescripcionRiesgo" Name="txtDescripcionRiesgo" runat="server" class="form-control" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>


            <!-- /page header -->
            <!-- Form vertical (default) -->


        </div>

 
        <!-- /tasks table -->

        <!-- /modal with table -->
        <div class="form-actions text-right">
            <input id="GuardarRiesgo" type="submit" value="Guardar datos" class="btn btn-primary run-first" name="submit">
            <%--<a data-toggle="modal" id="extender" runat="server" role="button" href="#ConfirmarModalLicencia" title="Confirmar" class="btn btn-primary">Extender licencia</a>                                                    --%>
            <a href="/evr/configuracion/tipos_riesgos/" title="Volver" class="btn btn-primary run-first">Volver</a>
        </div>

    </form>
    <!-- /form vertical (default) -->
    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left">
        </div>
    </div>
    <!-- /footer -->
</asp:Content>
