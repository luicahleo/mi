<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros oCentro;
    MIDAS.Models.areanivel1 oArea;
    MIDAS.Models.areanivel2 oSistema;
    MIDAS.Models.areanivel3 oEquipo;
    
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

        }

        if (Session["EditarAreaResultado"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarAreaResultado"].ToString() + "' });", true);
            Session.Remove("EditarAreaResultado");
        }

        if (Session["EditarAreaError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarAreaError"].ToString() + "' });", true);
            Session.Remove("EditarAreaError");
        }

        if (ViewData["ubicaciones"] != null)
        {
            ddlUbicacion.DataSource = ViewData["ubicaciones"];
            ddlUbicacion.DataValueField = "id_provincia";
            ddlUbicacion.DataTextField = "nombre_provincia";
            ddlUbicacion.DataBind();
        }

        if (ViewData["unidades"] != null)
        {
            ddlTipo.DataSource = ViewData["unidades"];
            ddlTipo.DataValueField = "id";
            ddlTipo.DataTextField = "nombre";
            ddlTipo.DataBind();
        }

        if (!IsPostBack)
        {
            oCentro = (MIDAS.Models.centros)ViewData["EditarCentro"];
            oArea = (MIDAS.Models.areanivel1)ViewData["EditarArea"];
            oSistema = (MIDAS.Models.areanivel2)ViewData["EditarSistema"];
            oEquipo = (MIDAS.Models.areanivel3)ViewData["EditarEquipo"];

            if (oCentro != null && oArea !=null && oSistema !=null)
            {
                hdnIdCentral.Value = oCentro.id.ToString();
                txtSiglas.Text = oCentro.siglas.ToString();
                txtSiglas.ReadOnly = true;
                txtNombre.Text = oCentro.nombre.ToString();
                txtNombre.ReadOnly = true;
                ddlTipo.SelectedValue = oCentro.tipo.ToString();
                ddlTipo.Enabled= false;
                ddlUbicacion.SelectedValue = oCentro.provincia.ToString();
                ddlUbicacion.Enabled=false;

                CodigoArea.Text = oArea.codigo.ToString();
                CodigoArea.ReadOnly = true;
                NombreArea.Text = oArea.nombre.ToString();
                NombreArea.ReadOnly = true;

                sistemacodigo.Text = oSistema.codigo.ToString();
                sistemacodigo.ReadOnly=true;
                sistemanombre.Text = oSistema.nombre.ToString();
                sistemanombre.ReadOnly=true;
                ViewData["idCentro"] = oCentro.id;
            }
            else
            {
                hdnIdCentral.Value = "0";
            }

            if (oEquipo != null)
            {
                equipocodigo.Text = oEquipo.codigo.ToString();
                equiponombre.Text = oEquipo.nombre.ToString();
                //equipoidSistema.Text = oEquipo.id_areanivel2.ToString();
                equipohidden.Value=oEquipo.id_areanivel2.ToString();
            }

            if (Session["CentroErroneo"] != null)
            {
                oCentro = (MIDAS.Models.centros)Session["CentroErroneo"];
                hdnIdCentral.Value = oCentro.id.ToString();
                txtSiglas.Text = oCentro.siglas.ToString();
                txtSiglas.ReadOnly = true;
                txtNombre.Text = oCentro.nombre.ToString();
                txtNombre.ReadOnly = true;
                ddlTipo.SelectedValue = oCentro.tipo.ToString();
                ddlTipo.Enabled = false;
                ddlUbicacion.SelectedValue = oCentro.ubicacion.ToString();
                ddlUbicacion.Enabled = false;
                Session.Remove("CentroErroneo");
            }
        }

        if (Session["EdicionCentroMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionCentroMensaje"].ToString() + "' });", true);
            Session["EdicionCentroMensaje"] = null;
        }

        if (Session["EdicionCentroError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionCentroError"].ToString() + "' });", true);
            Session["EdicionCentroError"] = null;
        }


    }


</script>
<asp:Content ID="headEditarEquipo" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Centro </title>
    <script type="text/javascript">


        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarCentral")
                    $("#hdFormularioEjecutado").val("GuardarCentral");

            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

            $("#ctl00_MainContent_divN1").show();
            $("#btnN1").addClass('active');
            //$("#ctl00_MainContent_ddlTecnologia").prop('disabled', true);
            $("#ctl00_MainContent_divN2").hide();
            $("#ctl00_MainContent_divN3").hide();

            //comprobar url id
            var pathname = window.location.pathname;
            var aux = pathname.split('/');
            if (aux[4] == 0) {
                $("#AREAS").hide();
            }



            $("#btnN1").click(function () {
                $("#ctl00_MainContent_divN1").show();
                $("#btnN1").addClass('active');
                $("#ctl00_MainContent_divN2").hide();
                $("#btnN2").removeClass('active');
                $("#ctl00_MainContent_divN3").hide();
                $("#btnN3").removeClass('active');
            });

            $("#btnN2").click(function () {
                $("#ctl00_MainContent_divN1").hide();
                $("#btnN1").removeClass('active');
                $("#ctl00_MainContent_divN2").show();
                $("#btnN2").addClass('active');
                $("#ctl00_MainContent_divN3").hide();
                $("#btnN3").removeClass('active');

            });

            $("#btnN3").click(function () {
                $("#ctl00_MainContent_divN1").hide();
                $("#btnN1").removeClass('active');
                $("#ctl00_MainContent_divN2").hide();
                $("#btnN2").removeClass('active');
                $("#ctl00_MainContent_divN3").show();
                $("#btnN3").addClass('active');

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
    
               </h3>
                </div>  </div>

    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form role="form" action="#" runat="server">
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-office"></i>Datos de central</h6>
        </div>
        <div class="panel-body">
            
            <table style="width:100%">
                <tr>
                    <td style="padding-right:15px">
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="hdnIdCentral" />
                        <label>
                            SIGLAS:</label>
                        <asp:TextBox ID="txtSiglas" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>
                    <td style="padding-right:15px">
                        <div class="form-group">
                        <label>
                            NOMBRE:</label>
                        <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>                    
                </tr>
                <tr>
                    <td style="padding-right:15px">
                        <div class="form-group">
                    <label>
                        TECNOLOGÍAS:</label>&nbsp;
                    <asp:DropDownList ID="ddlTipo" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                </div>
                    </td>
                    <td style="padding-right:15px">
                        <div class="form-group">
                    <label>
                        UBICACIÓN:</label>&nbsp;
                    <asp:DropDownList ID="ddlUbicacion" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                </div>
                    </td>
                    
                </tr>
                
                
            </table>
    </div>
  </div>

    <!-- /page header -->
    <!-- Form vertical (default) -->
   
    <div class="panel panel-default" style="margin-left:10px;" >
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-office"></i>Datos de área</h6>
        </div>
        <div class="panel-body">
            
            <table style="width:100%">
                <tr>
                    <td style="padding-right:15px">
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="hdnIdArea" />
                        <label>
                            CÓDIGO:</label>
                        <asp:TextBox ID="CodigoArea" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>
                    <td style="padding-right:15px">
                        <div class="form-group">
                        <label>
                            NOMBRE:</label>
                        <asp:TextBox ID="NombreArea" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>                    
                </tr>

            </table>
        </div>
    </div>

    <!-- /page header -->
    <!-- Form vertical (default) -->
   
    <div class="panel panel-default" style="margin-left:20px;">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-office"></i>Datos de Sistema</h6>
        </div>
        <div class="panel-body">
           
            <table style="width:100%">
                <tr>
                    <td style="padding-right:15px">
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="hdnIdSistema" />
                        <label>
                            CÓDIGO:</label>
                        <asp:TextBox ID="sistemacodigo" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>
                    <td style="padding-right:15px">
                        <div class="form-group">
                        <label>
                            NOMBRE:</label>
                        <asp:TextBox ID="sistemanombre" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>                    
                </tr>

            </table>
    </div>
        </div>
        <!-- Tasks table --> 
        <div class="panel panel-default" style="margin-left:30px;">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-office"></i>Datos de Equipo</h6>
        </div>
        <div class="panel-body">
           
            <table style="width:100%">
                <tr>
                    <td style="padding-right:15px">
                    <div class="form-group">
                    <asp:HiddenField runat="server" ID="HiddenField1" />
                        <label>
                            CÓDIGO:</label>
                        <asp:TextBox ID="equipocodigo" runat="server" class="form-control"></asp:TextBox>
                    </div>
                    </td>
                    <td style="padding-right:15px">
                        <div class="form-group">

            <%--                   <label for="Equipo">Nombre:</label>
                            <div class="form-control">
                                <%=Html.TextBox("Equipo") %>
                                </div>
                       <label>--%>
                            <label>
                            NOMBRE:</label>
                        <asp:TextBox ID="equiponombre" runat="server" class="form-control" ></asp:TextBox>
                       <asp:HiddenField ID="equipohidden" runat="server"/>
                           
                    </div>
                    </td>                    
                </tr>

            </table>
    </div>
      <%--   <div class="form-actions text-right">
        <input id="GuardarEquipo" type="submit" value="Guardar datos" class="btn btn-primary run-first">        
        <a href="/evr/configuracion/editar_sistema" title="Volver" class="btn btn-primary run-first">Volver</a>
    </div>--%>

          <div style="text-align:right">
                        <% if (user.perfil == 1 || user.perfil == 3)
                           { %>
                        <a href="/evr/Configuracion/editar_equipo/0" title="Nuevo equipo"><button class="btn btn-primary" >Guardar Equipo</button></a>                        
                        <% } %>
                        </div>
            </div>

        
         
				        <!-- /tasks table -->
        
    <!-- /modal with table -->
    <div class="form-actions text-right">
        <input id="GuardarUsuario" type="submit" value="Guardar datos" class="btn btn-primary run-first">
        <%--<a data-toggle="modal" id="extender" runat="server" role="button" href="#ConfirmarModalLicencia" title="Confirmar" class="btn btn-primary">Extender licencia</a>                                                    --%>
        <a href="/evr/configuracion/centros" title="Volver" class="btn btn-primary run-first">Volver</a>
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
