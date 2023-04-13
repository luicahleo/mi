<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.materialdivulgativo oMaterial = new MIDAS.Models.materialdivulgativo();
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

        if (ViewData["riesgos"] != null)
        {
            ddlRiesgos.DataSource = ViewData["riesgos"];
            ddlRiesgos.DataValueField = "id";
            ddlRiesgos.DataTextField = "riesgo";
            ddlRiesgos.DataBind();
        }

        if (ViewData["tipodocs"] != null)
        {
            ddlTipoDoc.DataSource = ViewData["tipodocs"];
            ddlTipoDoc.DataValueField = "id";
            ddlTipoDoc.DataTextField = "tipo";
            ddlTipoDoc.DataBind();
        }

        if (ViewData["centros"] != null)
        {
            ddlCentro.DataSource = ViewData["centros"];
            ddlCentro.DataValueField = "id";
            ddlCentro.DataTextField = "nombre";
            ddlCentro.DataBind();
        }
        
        if (!IsPostBack)
        {
            

            oMaterial = (MIDAS.Models.materialdivulgativo)ViewData["material"];

            if (oMaterial != null)
            {
                txtCodigo.Text = oMaterial.codigo;
                txtNombre.Text = oMaterial.titulo;
                ddlTipoDoc.SelectedValue = oMaterial.tipodoc.ToString();
                txtFechaPublicacion.Text = oMaterial.fechapub.ToString().Replace(" 0:00:00", "");
                ddlRiesgos.SelectedValue = oMaterial.riesgoasoc.ToString();
                ddlCentro.SelectedValue = oMaterial.idcentral.ToString();
                if (oMaterial.enlace != null)
                    lblEnlace.Text = oMaterial.enlace.Replace(Server.MapPath("~/Materiales") + "\\" + oMaterial.id + "\\", "");                
            }
        }

        if (permisos.permiso == false)
        {
            desactivarCampos();
        }                                                      

        if (Session["EdicionMaterialMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionMaterialMensaje"].ToString() + "' });", true);
            Session["EdicionMaterialMensaje"] = null;
        }
        if (Session["EdicionMaterialError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionMaterialError"].ToString() + "' });", true);
            Session["EdicionMaterialError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        txtCodigo.ReadOnly = true;
        txtNombre.ReadOnly = true;
        ddlTipoDoc.Enabled = false;
        txtFechaPublicacion.ReadOnly = true;
        ddlRiesgos.Enabled = false;
        ddlCentro.Enabled = false;
        
        consulta = 1;
    }
    
</script>
<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Material</title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            var tiposeleccionado = $('#ctl00_MainContent_ddlTipoDoc').val();

            $("#divRiesgos").hide();
            $("#divCentros").hide();

            if (tiposeleccionado == 1) {
                $("#divRiesgos").show();
                $("#divCentros").hide();
            }
            if (tiposeleccionado == 4) {
                $("#divRiesgos").hide();
                $("#divCentros").show();
            }

            $("#ctl00_MainContent_ddlTipoDoc").change(function () {
                var tiposeleccionado = $('#ctl00_MainContent_ddlTipoDoc').val();


                $("#divRiesgos").hide();
                $("#divCentros").hide();

                if (tiposeleccionado == 1) {
                    $("#divRiesgos").show();
                    $("#divCentros").hide();
                }
                if (tiposeleccionado == 4) {
                    $("#divRiesgos").hide();
                    $("#divCentros").show();
                }

            });

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarMaterial")
                    $("#hdFormularioEjecutado").val("GuardarMaterial");
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
                Detalle del material<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
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
                        <td style="width: 55%">
                            <label>
                                T�tulo (*)</label>
                            <asp:TextBox ID="txtNombre"  Width="98%" runat="server" class="form-control" ></asp:TextBox>
                        </td>
                        <td style="width: 25%">
                            <label>Tipo de documento</label>
                            <asp:DropDownList runat="server" ID="ddlTipoDoc" class="form-control" Width="96%"></asp:DropDownList>
                        </td>
                        <td style="width: 10%">
                            <label>
                                F.Publicaci�n (*)</label>
                            <asp:TextBox ID="txtFechaPublicacion" Width="100%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                            <div id="divRiesgos">
                                <label>Riesgo asociado</label>
                                <asp:DropDownList runat="server" ID="ddlRiesgos" class="form-control" Width="60%"></asp:DropDownList>
                            </div>
                            <div id="divCentros">
                                <label>Centro de trabajo</label>
                                <asp:DropDownList runat="server" ID="ddlCentro" class="form-control" Width="60%"></asp:DropDownList>
                            </div>                            
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
                            <% if (oMaterial != null && oMaterial.enlace != null)
                               { %>
                               <center>
                            <a title="Ver Informe" href="/evr/Home/ObtenerMaterial/<%=oMaterial.id %>");"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
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
        <input id="GuardarMaterial" type="submit" value="Guardar Material" class="btn btn-primary run-first" />
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
