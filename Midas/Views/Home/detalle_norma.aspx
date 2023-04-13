<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.normas oNorma = new MIDAS.Models.normas();
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
            
            oNorma = (MIDAS.Models.normas)ViewData["norma"];

            if (oNorma != null)
            {
                txtCodigo.Text = oNorma.codigo;
                txtNombre.Text = oNorma.nombre_norma;
                txtEdicion.Text = oNorma.edicion_norma;
                if (oNorma.enlace != null)
                    lblNorma.Text = oNorma.enlace.Replace(Server.MapPath("~/Normas") + "\\" + oNorma.id + "\\", "");                
            }
        }

        if ((user.perfil != 1 && user.perfil != 3))
        {
            desactivarCampos();
        }                                                      

        if (Session["EdicionNormaMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionNormaMensaje"].ToString() + "' });", true);
            Session["EdicionNormaMensaje"] = null;
        }
        if (Session["EdicionNormaError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionNormaError"].ToString() + "' });", true);
            Session["EdicionNormaError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        txtCodigo.ReadOnly = true;
        txtNombre.ReadOnly = true;
        txtEdicion.ReadOnly = true;
        
        consulta = 1;
    }
    
</script>
<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Normas </title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarNorma")
                    $("#hdFormularioEjecutado").val("GuardarNorma");
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
                Detalle de la norma<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
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
                <i class="icon-pencil"></i>Datos Generales</h6>
        </div>
        <div class="panel-body">
            <div class="form-group">
                <table width="100%">
                    <tr>
                        <td runat="server" id="columnaTextbox" class="form-group" style="width: 15%">
                            <label>
                                Codigo</label>
                            <asp:TextBox ID="txtCodigo" Width="92%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td style="width: 70%">
                            <label>
                                Descripción</label>
                            <asp:TextBox ID="txtNombre"  Width="99%" runat="server" class="form-control" ></asp:TextBox>
                        </td>
                        <td style="width: 15%">
                            <label>
                                Edición</label>
                            <asp:TextBox ID="txtEdicion"  Width="99%" runat="server" class="form-control" ></asp:TextBox>
                        </td>
                    </tr>
                </table>
        <br />
        <table width="100%">
                <tr>
                    <td class="form-group" style="width: 50%; padding-bottom:20px">
                    <center>
                            <label>
                                Enlace</label>
                            <input type="file" id="fileNorma" name="file" style="margin-top:10px" /></center>
                        </td>      
                    </tr>
                    <tr style="padding-top:20px">
                        <td>
                            <% if (oNorma != null && oNorma.enlace != null)
                               { %>
                               <center>
                            <a title="Ver Informe" href="/evr/Home/ObtenerNorma/<%=oNorma.id %>");"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
                            <asp:Label ID="lblNorma" style="margin-top:10px" runat="server" Text=""></asp:Label></center>
                            <% } %>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    
    
    <div class="form-actions text-right">
        <%
            if (consulta == 0) 
            {
        %>
        <input id="GuardarNorma" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
        <% } %>
        <a href="/evr/Home/catalogo" title="Volver" class="btn btn-primary run-first">Volver</a>
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
