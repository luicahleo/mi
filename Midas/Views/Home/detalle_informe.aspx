<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.informesseguridad oInforme = new MIDAS.Models.informesseguridad();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
    int consulta = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null || Session["CentralElegida"] == null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (Session["CentralElegida"] != null)
        {
            centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
        }

        if (user.perfil == 2)
            permisos = MIDAS.Models.Datos.ObtenerPermisos(user.idUsuario, centroseleccionado.id);
        else
        {
            permisos.idusuario = user.idUsuario;
            permisos.idcentro = centroseleccionado.id;
            permisos.permiso = true;
        }

        if (ViewData["elaborado"] != null)
        {
            ddlElaboradoPor.DataSource = ViewData["elaborado"];
            ddlElaboradoPor.DataValueField = "id";
            ddlElaboradoPor.DataTextField = "elaborado";
            ddlElaboradoPor.DataBind();
        }

        if (ViewData["tipodocs"] != null)
        {
            ddlTipoDoc.DataSource = ViewData["tipodocs"];
            ddlTipoDoc.DataValueField = "id";
            ddlTipoDoc.DataTextField = "tipo";
            ddlTipoDoc.DataBind();
        }

        
        
        if (!IsPostBack)
        {
            for (int i = 2008; i <= DateTime.Now.Year + 1; i++)
            {
                ListItem itemAnio = new ListItem();
                itemAnio.Value = i.ToString();
                itemAnio.Text = i.ToString();

                ddlAnio.Items.Insert(0, itemAnio);
            }
            if (ddlAnio.Items.Count > 1)
                ddlAnio.SelectedIndex = 1;           

            oInforme = (MIDAS.Models.informesseguridad)ViewData["informe"];

            if (oInforme != null)
            {
                txtCodigo.Text = oInforme.codigo;
                txtNombre.Text = oInforme.titulo;
                ddlTipoDoc.SelectedValue = oInforme.tipodoc.ToString();
                txtFechaPublicacion.Text = oInforme.fechapub.ToString().Replace(" 0:00:00", "");
                ddlElaboradoPor.SelectedValue = oInforme.elaboradopor.ToString();
                ddlMes.SelectedValue = oInforme.mes.ToString();
                ddlAnio.SelectedValue = oInforme.anio.ToString();
                if (oInforme.enlace != null)
                    lblEnlace.Text = oInforme.enlace.Replace(Server.MapPath("~/InformesSeguridad") + "\\" + oInforme.id + "\\", "");                
            }
        }

        if (permisos.permiso == false)
        {
            desactivarCampos();
        }                                                      

        if (Session["EdicionInformeMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionInformeMensaje"].ToString() + "' });", true);
            Session["EdicionInformeMensaje"] = null;
        }
        if (Session["EdicionInformeError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionInformeError"].ToString() + "' });", true);
            Session["EdicionInformeError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        txtCodigo.ReadOnly = true;
        txtNombre.ReadOnly = true;
        ddlTipoDoc.Enabled = false;
        txtFechaPublicacion.ReadOnly = true;
        ddlElaboradoPor.Enabled = false;
        ddlMes.Enabled = false;
        ddlAnio.Enabled = false;
        
        consulta = 1;
    }
    
</script>
<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Informe</title>
    <script type="text/javascript">
        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarInforme")
                    $("#hdFormularioEjecutado").val("GuardarInforme");
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
                Detalle del informe<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
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
                        <td runat="server" id="columnaTextbox" class="form-group" style="width: 10%">
                            <label>
                                Codigo (*)</label>
                            <asp:TextBox ID="txtCodigo" Width="96%" runat="server" class="form-control"></asp:TextBox>
                        </td>
                        <td style="width: 40%">
                            <label>
                                Título (*)</label>
                            <asp:TextBox ID="txtNombre"  Width="98%" runat="server" class="form-control" ></asp:TextBox>
                        </td>
                        <td style="width: 25%">
                            <label>Tipo de documento</label>
                            <asp:DropDownList runat="server" ID="ddlTipoDoc" class="form-control" Width="96%"></asp:DropDownList>
                        </td>
                        <td style="width: 25%">
                            <label>
                                Elaborado por:</label>
                                <asp:DropDownList runat="server" ID="ddlElaboradoPor" class="form-control" Width="96%"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <label>
                                F.Publicación (*)</label>
                            <asp:TextBox ID="txtFechaPublicacion" Width="96%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                        <td style="padding-top:14px">
                            <table style="width:100%">
                                <tr>
                                    <td style="width:50%">
                                            <label>Mes</label>
                                            <asp:DropDownList runat="server" ID="ddlMes" class="form-control" Width="97%">
                                                <asp:ListItem Value="1" Text="Enero"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Febrero"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="Marzo"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="Abril"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="Mayo"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="Junio"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="Julio"></asp:ListItem>
                                                <asp:ListItem Value="8" Text="Agosto"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="Septiembre"></asp:ListItem>
                                                <asp:ListItem Value="10" Text="Octubre"></asp:ListItem>
                                                <asp:ListItem Value="11" Text="Noviembre"></asp:ListItem>
                                                <asp:ListItem Value="12" Text="Diciembre"></asp:ListItem>
                                            </asp:DropDownList>
                                    </td>
                                    <td style="width:50%">
                                        <label>Año</label>
                                        <asp:DropDownList runat="server" ID="ddlAnio" class="form-control" Width="95%">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <br />                         
                        </td>
                        <td colspan="2">
                        
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
                            <% if (oInforme != null && oInforme.enlace != null)
                               { %>
                               <center>
                            <a title="Ver Informe" href="/evr/Home/ObtenerMaterial/<%=oInforme.id %>");"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
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
        <input id="GuardarInforme" type="submit" value="Guardar Informe" class="btn btn-primary run-first" />
        <% } %>
        <a href="/evr/Home/apoyoprl" title="Volver" class="btn btn-primary run-first">Volver</a>
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
