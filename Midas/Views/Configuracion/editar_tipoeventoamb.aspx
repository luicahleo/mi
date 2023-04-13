<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.evento_ambiental_tipo oTipoEventoAmb;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }


        if (!IsPostBack)
        {
            oTipoEventoAmb = (MIDAS.Models.evento_ambiental_tipo)ViewData["EditarTipoEventoAmb"];
            if (oTipoEventoAmb != null)
            {
                hdnIdDocumento.Value = oTipoEventoAmb.id.ToString();
                txtNombre.Text = oTipoEventoAmb.tipo.ToString();
            }
        }

        if (Session["EdicionTipoEventoAmbMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionTipoEventoAmbMensaje"].ToString() + "' });", true);
            Session["EdicionTipoEventoAmbMensaje"] = null;
        }

        if (Session["EdicionTipoEventoAmbError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionTipoEventoAmbError"].ToString() + "' });", true);
            Session["EdicionTipoEventoAmbError"] = null;
        }


    }
    
    
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Tipo Evento Ambiental </title>
</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
				<div class="page-title">
            <h3>
                Edición de tipo de evento ambiental
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
                            Tipo:</label>
                        <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>                                  
                    </tr>
            </table>
            
        </div>
    </div>
   
    <!-- /modal with table -->
    <div class="form-actions text-right">
        <input id="GuardarTipoEventoAmb" type="submit" value="Guardar datos" class="btn btn-primary run-first">        
        <a href="/evr/configuracion/tiposeventoamb" title="Volver" class="btn btn-primary run-first">Volver</a>
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
