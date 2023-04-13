<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.enlaces oEnlace = new MIDAS.Models.enlaces();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    int consulta = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (ViewData["ambitos"] != null)
        {
            ddlAmbito.DataSource = ViewData["ambitos"];
            ddlAmbito.DataValueField = "id";
            ddlAmbito.DataTextField = "nombre_ambito";
            ddlAmbito.DataBind();

            ListItem itemAmbitoGeneral = new ListItem();
            itemAmbitoGeneral.Text = "General";
            itemAmbitoGeneral.Value = "0";
            ddlAmbito.Items.Insert(0, itemAmbitoGeneral);

        }
        
        if (!IsPostBack)
        {
            if (Session["usuario"] != null || Session["CentralElegida"] == null)
            {
                user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            }

            oEnlace = (MIDAS.Models.enlaces)ViewData["enlace"];

            if (oEnlace != null)
            {
                txtNombre.Text = oEnlace.titulo;
                txtURL.Text = oEnlace.url;
                ddlAmbito.SelectedValue = oEnlace.ambito.ToString();
                if (oEnlace.enlace != null)
                    lblEnlace.Text = oEnlace.enlace.Replace(Server.MapPath("~/Enlaces") + "\\" + oEnlace.id + "\\", "");                
            }
        }

        if ((user.perfil != 1 && user.perfil != 3))
        {
            desactivarCampos();
        }                                                      

        if (Session["EdicionEnlaceMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionEnlaceMensaje"].ToString() + "' });", true);
            Session["EdicionEnlaceMensaje"] = null;
        }
        if (Session["EdicionEnlaceError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionEnlaceError"].ToString() + "' });", true);
            Session["EdicionEnlaceError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        txtNombre.ReadOnly = true;
        
        consulta = 1;
    }
    
</script>
<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Link</title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarEnlace")
                    $("#hdFormularioEjecutado").val("GuardarEnlace");
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
                Detalle del link<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
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
                        <td style="width: 55%">
                            <label>
                               Título (*)</label>
                            <asp:TextBox ID="txtNombre"  Width="99%" runat="server" class="form-control" ></asp:TextBox>
                        </td>
                        <td style="width: 25%">
                            <label>
                               URL</label>
                            <asp:TextBox ID="txtURL"  Width="99%" runat="server" class="form-control" ></asp:TextBox>
                        </td>
                        <td style="width: 20%">
                            <label>
                                Ámbito</label>
                            <asp:DropDownList runat="server" ID="ddlAmbito" class="form-control" Width="95%"></asp:DropDownList>
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
                            <% if (oEnlace != null && oEnlace.enlace != null)
                               { %>
                               <center>
                            <a title="Ver Informe" href="/evr/Home/ObtenerEnlace/<%=oEnlace.id %>");"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
                            <asp:Label ID="lblEnlace" style="margin-top:10px" runat="server" Text=""></asp:Label></center>
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
        <input id="GuardarEnlace" type="submit" value="Guardar Link" class="btn btn-primary run-first" />
        <% } %>
        <a href="/evr/Home/enlaces" title="Volver" class="btn btn-primary run-first">Volver</a>
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
