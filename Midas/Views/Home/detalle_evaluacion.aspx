<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.evaluacionriesgos oEvaluacion = new MIDAS.Models.evaluacionriesgos();
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

        if (ViewData["centros"] != null)
        {
            ddlCentro.DataSource = ViewData["centros"];
            ddlCentro.DataValueField = "id";
            ddlCentro.DataTextField = "nombre";
            ddlCentro.DataBind();
        }
        
        if (!IsPostBack)
        {
            oEvaluacion = (MIDAS.Models.evaluacionriesgos)ViewData["evaluacion"];

            if (oEvaluacion != null)
            {
                txtCodigo.Text = oEvaluacion.codigo;
                txtNombre.Text = oEvaluacion.titulo;
                ddlTipoDoc.SelectedValue = oEvaluacion.tipodoc.ToString();
                txtFechaPublicacion.Text = oEvaluacion.fechapub.ToString().Replace(" 0:00:00", "");
                ddlElaboradoPor.SelectedValue = oEvaluacion.elaboradopor.ToString();
                ddlCentro.SelectedValue = oEvaluacion.idcentral.ToString();
                txtEmpresa.Text = oEvaluacion.empresa;
                if (oEvaluacion.enlace != null)
                    lblEnlace.Text = oEvaluacion.enlace.Replace(Server.MapPath("~/EvaluacionRiesgos") + "\\" + oEvaluacion.id + "\\", "");                
            }
        }

        if (permisos.permiso == false)
        {
            desactivarCampos();
        }                                                      

        if (Session["EdicionEvaluacionMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionEvaluacionMensaje"].ToString() + "' });", true);
            Session["EdicionEvaluacionMensaje"] = null;
        }
        if (Session["EdicionEvaluacionError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionEvaluacionError"].ToString() + "' });", true);
            Session["EdicionEvaluacionError"] = null;
        }   
    }

    public void desactivarCampos()
    {
        txtCodigo.ReadOnly = true;
        txtNombre.ReadOnly = true;
        ddlTipoDoc.Enabled = false;
        txtFechaPublicacion.ReadOnly = true;
        ddlElaboradoPor.Enabled = false;
        ddlCentro.Enabled = false;
        txtEmpresa.ReadOnly = true;
        
        consulta = 1;
    }
    
</script>
<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Evaluaciones</title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divForm").hide();

            var elaborado = $('#ctl00_MainContent_ddlElaboradoPor').val();

            $("#divEmpresa").hide();

            if (elaborado != 1) {
                $("#divEmpresa").show();
            }

            $("#ctl00_MainContent_ddlElaboradoPor").change(function () {
                var elaborado = $('#ctl00_MainContent_ddlElaboradoPor').val();


                $("#divEmpresa").hide();

                if (elaborado != 1) {
                    $("#divEmpresa").show();
                }

            });

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarEvaluacion")
                    $("#hdFormularioEjecutado").val("GuardarEvaluacion");
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
                Detalle de la evaluación<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
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
                                Título (*)</label>
                            <asp:TextBox ID="txtNombre"  Width="98%" runat="server" class="form-control" ></asp:TextBox>
                        </td>
                        <td style="width: 25%">
                            <label>Tipo de documento</label>
                            <asp:DropDownList runat="server" ID="ddlTipoDoc" class="form-control" Width="96%"></asp:DropDownList>
                        </td>
                        <td style="width: 10%">
                            <label>
                                F.Publicación (*)</label>
                            <asp:TextBox ID="txtFechaPublicacion" Width="100%" runat="server" class="datepicker form-control"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%">
                    <tr>
                        <td style="width:33%">
                                <label>Elaborado por:</label>
                                <asp:DropDownList runat="server" ID="ddlElaboradoPor" class="form-control" Width="97%"></asp:DropDownList>                                                 
                        </td>
                        <td  style="width:33%">
                            <label>Centro de trabajo</label>
                                <asp:DropDownList runat="server" ID="ddlCentro" class="form-control" Width="98%"></asp:DropDownList>
                        </td>
                        <td style="width:33%">
                            <div id="divEmpresa">
                                <label>Empresa</label>
                                <asp:TextBox ID="txtEmpresa"  Width="98%" runat="server" class="form-control" ></asp:TextBox>    
                            </div> 

                                
                                           
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
                            <% if (oEvaluacion != null && oEvaluacion.enlace != null)
                               { %>
                               <center>
                            <a title="Ver Informe" href="/evr/Home/ObtenerEvaluacion/<%=oEvaluacion.id %>");"><i style="font-size:50px" class="icon-download"></i></a><br /><br />
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
        <input id="GuardarEvaluacion" type="submit" value="Guardar Evaluación" class="btn btn-primary run-first" />
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
