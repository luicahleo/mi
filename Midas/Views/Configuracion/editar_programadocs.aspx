<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.documentos_programa oPrograma = new MIDAS.Models.documentos_programa();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    int consulta = 0;
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
            if (Session["usuario"] != null || Session["CentralElegida"] == null)
            {
                user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            }

            if (Session["CentralElegida"] != null)
            {
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            }

            oPrograma = (MIDAS.Models.documentos_programa)ViewData["programaDocs"];
            if (oPrograma != null)
                lblProgramaAuditoria.Text = oPrograma.nombrefichero;

        }

        if (Session["EdicionProgramaDocumentosMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionProgramaDocumentosMensaje"].ToString() + "' });", true);
            Session["EdicionProgramaDocumentosMensaje"] = null;
        }
        if (Session["EditarProgramaDocumentosError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarProgramaDocumentosError"].ToString() + "' });", true);
            Session["EditarProgramaDocumentosError"] = null;
        }   
    }
   
    
</script>
<asp:Content ID="headEditarAuditoria" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Programa Documentacion </title>
</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>
                Programa de Documentación</h3>
        </div>
    </div>
    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">    
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    <%--Datos generales --%>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-pencil"></i>Subida del programa de documentación</h6>
        </div>
        <div class="panel-body">
            <div class="form-group">
                <table width="100%">
                <tr>
                    <td class="form-group" style="width: 50%; padding-bottom:20px">
                    <center>
                            <label>
                                Programa de documentos</label>
                            <input type="file" id="fileProgramaAuditoria" name="file" style="margin-top:10px" /></center>
                        </td>      
                    </tr>   
                    <% 
                        if (oPrograma != null && oPrograma.rutaFichero != null && oPrograma.rutaFichero != string.Empty)
                       { %>
                    <tr style="padding-top:20px">
                        <td>
                               <center>
                            <a title="Ver Documentacion" href="/evr/configuracion/ObtenerProgramaDocumentos/;"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
                            <asp:Label ID="lblProgramaAuditoria" style="margin-top:10px" runat="server" Text=""></asp:Label></center>
                        </td>
                        </tr>      
                        <% } %>           
                </table>
               
            </div>
        </div>
    </div>      

    
        <%
            if (consulta == 0)
            {
        %>
        <input id="GuardarAuditoria" style="float:right;" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
        <% } %>

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
