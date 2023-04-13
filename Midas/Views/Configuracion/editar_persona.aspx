<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

        }

        if (ViewData["persona"] != null)
        {

            var persona = (MIDAS.Models.personas)ViewData["persona"];

            txtNombre.Text = persona.Nombre;
            txtPerfilRiesgo.Text = persona.Perfil_de_riesgo;
            txtEmpresa.Text = persona.Empresa;
            txtCentroTrabajo.Text = persona.Centro_de_trabajo;
            txtActividad.Text = persona.Actividad;
            txtUnidadOrganizativa.Text = persona.Unidad_Organizativa;
            txtOcupacion.Text = persona.Ocupacion;
            txtPosicion.Text = persona.Posicion;
            txtEmpleado.Text = persona.Nº_Empleado;
            txtDni.Text = persona.DNI;
            txtJefeDirecto.Text = persona.Jefe_Directo;

        }

    }
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Centro </title>

</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="<%=ResolveClientUrl("~/ext/js/Configuracion/editar_centro.js") %>"></script>
    <link href="<%=ResolveClientUrl("~/ext/css/Configuracion/editar_centro.css") %>" rel="stylesheet" />

    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3></h3>
        </div>
    </div>

    <!-- /page header -->
    <form action="/" method="post" runat="server">
        <asp:GridView ID="DatosPedidos" runat="server" Visible="false"></asp:GridView>


        <div class="panel-body">
            <table style="width: 100%">
                <tr>
                    <td style="padding-right: 15px">
                        <div class="form-group">
                            <label>
                                Nombres y Apellidos:</label>
                            <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>
                    <td style="padding-right: 15px">
                        <div class="form-group">
                            <label>
                                Perfil de Riesgo:</label>
                            <asp:TextBox ID="txtPerfilRiesgo" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>
                </tr>

                <tr>
                    <td style="padding-right: 15px">
                        <div class="form-group">
                            <label>
                                Empresa:</label>
                            <asp:TextBox ID="txtEmpresa" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>
                    <td style="padding-right: 15px">
                        <div class="form-group">
                            <label>
                                Centro de Trabajo:</label>
                            <asp:TextBox ID="txtCentroTrabajo" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>
                </tr>

                <tr>
                    <td style="padding-right: 15px">
                        <div class="form-group">
                            <label>
                                Actividad:</label>
                            <asp:TextBox ID="txtActividad" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>
                    <td style="padding-right: 15px">
                        <div class="form-group">
                            <label>
                                Unidad Organizativa:</label>
                            <asp:TextBox ID="txtUnidadOrganizativa" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right: 15px">
                        <div class="form-group">
                            <label>
                                Ocupación:</label>
                            <asp:TextBox ID="txtOcupacion" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>
                    <td style="padding-right: 15px">
                        <div class="form-group">
                            <label>
                                Posición:</label>
                            <asp:TextBox ID="txtPosicion" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right: 15px">
                        <div class="form-group">
                            <label>
                                Nº Empleado:</label>
                            <asp:TextBox ID="txtEmpleado" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>
                    <td style="padding-right: 15px">
                        <div class="form-group">
                            <label>
                                Actividad Funcional:</label>
                            <asp:TextBox ID="txtActividadFuncional" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding-right: 15px">
                        <div class="form-group">
                            <label>
                                DNI:</label>
                            <asp:TextBox ID="txtDni" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>
                    <td style="padding-right: 15px">
                        <div class="form-group">
                            <label>
                                Jefe Directo:</label>
                            <asp:TextBox ID="txtJefeDirecto" runat="server" class="form-control"></asp:TextBox>
                        </div>
                    </td>
                </tr>

            </table>
        </div>


         <div class="form-actions text-right">
            <input id="GuardarUsuario" type="submit" value="Guardar datos" class="btn btn-primary run-first" runat="server" clientidmode="Static">
            <a href="/evr/configuracion/personas" title="Volver" class="btn btn-primary run-first">Volver</a>
        </div>


    </form>
    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left">
        </div>
    </div>
    <!-- /footer -->
</asp:Content>
