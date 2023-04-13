<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.auditores oAuditor;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }


        if (!IsPostBack)
        {
            oAuditor = (MIDAS.Models.auditores)ViewData["EditarAuditor"];
            if (oAuditor != null)
            {
                hdnIdDocumento.Value = oAuditor.id.ToString();
                txtNombre.Text = oAuditor.nombre.ToString();
                txtEmpresa.Text = oAuditor.empresa.ToString();
            }
        }

        if (Session["EdicionAuditorMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionAuditorMensaje"].ToString() + "' });", true);
            Session["EdicionAuditorMensaje"] = null;
        }

        if (Session["EdicionAuditorError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionAuditorError"].ToString() + "' });", true);
            Session["EdicionAuditorError"] = null;
        }


    }
    
    
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Auditor </title>
</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
				<div class="page-title">
            <h3>
                Edición de auditores
               </h3>
                </div>  </div>

    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">    
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-document"></i>Datos generales</h6>
        </div>
        <div class="panel-body">
            <table style="width:100%">
                <tr>
                    <td style="width:50%; padding-right:10px">
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="hdnIdDocumento" />
                        <label>
                            Nombre</label>
                        <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>     
                    <td style="width:50%">
                    <div class="form-group">
                        <label>
                            Empresa</label>
                        <asp:TextBox ID="txtEmpresa" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>                             
                    </tr>
                    <tr>
                        <td colspan="2" class="form-group" style="width: 50%; padding-bottom:20px">
                             <center>
                            <label>
                            Cualificación:</label>
                            <input type="file" id="fileProgramaAuditoria" name="file" style="margin-top:10px" /></center>
                        </td>
                    </tr>
                    <% if (oAuditor != null && oAuditor.cv != null && oAuditor.cv != string.Empty)
                       { %>
                    <tr style="padding-top:20px" >
                        <td colspan="2">
                               <center>
                            <a title="Ver Plan Auditoría" href="/evr/configuracion/ObtenerCualificacion/<%= oAuditor.id %>"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
                            </center>
                        </td>
                        </tr>  
                        <% } %>
            </table>
            
        </div>
    </div>
   
    <!-- /modal with table -->
    <div class="form-actions text-right">
        <input id="GuardarReferencial" type="submit" value="Guardar datos" class="btn btn-primary run-first">        
        <a href="/evr/configuracion/auditores" title="Volver" class="btn btn-primary run-first">Volver</a>
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
