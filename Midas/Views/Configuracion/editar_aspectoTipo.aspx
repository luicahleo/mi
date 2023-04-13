<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">   
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.aspecto_parametro oTipoAspecto;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (!IsPostBack)
        {
            oTipoAspecto = (MIDAS.Models.aspecto_parametro)ViewData["parametrosAmbientales"];
            if (oTipoAspecto != null)
            {
                hdnIdDocumento.Value = oTipoAspecto.id_parametro.ToString();
                txtNombre.Text = oTipoAspecto.nombre.ToString();
                ddlTipo.Text = oTipoAspecto.id_aspecto.ToString();
            }
        }

        if (Session["EdicionAmbitoMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionAmbitoMensaje"].ToString() + "' });", true);
            Session["EdicionAmbitoMensaje"] = null;
        }

        if (Session["EdicionAmbitoError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionAmbitoError"].ToString() + "' });", true);
            Session["EdicionAmbitoError"] = null;
        }

    }
    
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-AspectoTipo </title>
    <script type="text/javascript">


        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarAspectoTipo")
                    $("#hdFormularioEjecutado").val("GuardarAspectoTipo");

            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });          


        });

        
       
    </script>
</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
	   <div class="page-title">
            <h3>
                Edición de Aspectos
            </h3>
       </div> 
    </div>
    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form role="form" action="#" runat="server">
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-document"></i>Datos generales</h6>
        </div>
        <div class="panel-body">
            <table style="width:100%">
                <tr>
                    <td>
                        <div class="form-group">
                            <label>Aspecto</label>
                                <asp:DropDownList Width="98%" CssClass="form-control" ID="ddlTipo" runat="server"> 
                                        <asp:ListItem Value="1" Text="Emisiones Reguladas"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="Vertidos Regulados"></asp:ListItem>                                        
                                        <asp:ListItem Value="9" Text="Gestion de residuos no peligrosos"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="Consumo de combustibles"></asp:ListItem>
                                        <asp:ListItem Value="12" Text="Consumo de electricidad"></asp:ListItem>
                                </asp:DropDownList>
                       </div>
                    </td> 
                    <td >
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="hdnIdDocumento" />
                        <label>
                            Nombre Del Parámetro:</label>
                        <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>                                  
                    </tr>
            </table>
            
        </div>
    </div>
   
    <!-- /modal with table -->
    <div class="form-actions text-right">
        <input id="GuardarAspectoTipo" type="submit" value="Guardar datos" class="btn btn-primary run-first">        
        <a href="/evr/configuracion/aspectosParametros" title="Volver" class="btn btn-primary run-first">Volver</a>
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
