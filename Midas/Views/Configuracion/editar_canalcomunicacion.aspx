<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.comunicacion_canales oCanalComunicacion;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }


        if (!IsPostBack)
        {
            oCanalComunicacion = (MIDAS.Models.comunicacion_canales)ViewData["EditarCanalComunicacion"];
            if (oCanalComunicacion != null)
            {
                hdnIdDocumento.Value = oCanalComunicacion.id.ToString();
                txtNombre.Text = oCanalComunicacion.canal.ToString();
            }
        }

        if (Session["EdicionCanalComunicacionMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionCanalComunicacionMensaje"].ToString() + "' });", true);
            Session["EdicionCanalComunicacionMensaje"] = null;
        }

        if (Session["EdicionCanalComunicacionError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionCanalComunicacionError"].ToString() + "' });", true);
            Session["EdicionCanalComunicacionError"] = null;
        }


    }
    
    
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Canal Comunicación </title>
</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
				<div class="page-title">
            <h3>
                Edición de canal
               </h3>
                </div>  </div>

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
                    <td style="padding-right:15px">
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="hdnIdDocumento" />
                        <label>
                            Canal:</label>
                        <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>                                  
                    </tr>
            </table>
            
        </div>
    </div>
   
    <!-- /modal with table -->
    <div class="form-actions text-right">
        <input id="GuardarCanalComunicacion" type="submit" value="Guardar datos" class="btn btn-primary run-first">        
        <a href="/evr/configuracion/canalescomunicacion" title="Volver" class="btn btn-primary run-first">Volver</a>
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
