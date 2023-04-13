<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">  
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros oCentro;
    MIDAS.Models.areanivel1 oArea;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            DatosPedidos.DataSource = ViewData["areas"];
            DatosPedidos.DataBind();
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
            if (oCentro != null)
            {
                hdnIdCentral.Value = oCentro.id.ToString();
                txtSiglas.Text = oCentro.siglas.ToString();
                txtNombre.Text = oCentro.nombre.ToString();
                ddlTipo.SelectedValue = oCentro.tipo.ToString();
                ddlUbicacion.SelectedValue = oCentro.provincia.ToString();

                ViewData["idCentro"] = oCentro.id;
            }
            else
            {
                hdnIdCentral.Value = "0";
            }

            oArea = (MIDAS.Models.areanivel1)ViewData["EditarArea"];
            if (oArea != null)
            {
                hdnIdArea.Value = oArea.id.ToString();

                ViewData["idArea"] = oArea.id;
            }
            else
            {
                hdnIdCentral.Value = "0";
            }

            if (Session["CentroErroneo"] != null)
            {
                oCentro = (MIDAS.Models.centros)Session["CentroErroneo"];
                hdnIdCentral.Value = oCentro.id.ToString();
                txtSiglas.Text = oCentro.siglas.ToString();
                txtNombre.Text = oCentro.nombre.ToString();
                ddlTipo.SelectedValue = oCentro.tipo.ToString();
                ddlUbicacion.SelectedValue = oCentro.ubicacion.ToString();


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


    protected void addArea_Click(object sender, EventArgs e)
    {
        oArea.codigo = areacodigo.Text;
    }
</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
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
    <form role="form" action="#" runat="server" method="post">
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
                         <asp:HiddenField runat="server" ID="hdnIdArea" />
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

        <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
                            </asp:GridView>
        <!-- Tasks table -->
        
	    <div class="panel panel-default" style="margin-left:10px;">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-office"></i>Datos de área</h6>
        </div>
                                <center>
					            <div style="width:95%" class="datatablePedido">
					                <table class="table table-bordered">
					                    <thead>
					                        <tr>
                                                <th style="width:45px">Código</th>
                                                <th>Nombre</th>
                                                <% if (user.perfil == 1 || user.perfil == 3)
                                                   { %>
					                            <th style="width:45px">Editar</th>
                                                <th style="width:45px">Borrar</th>
                                                <% } %>
					                        </tr>
					                    </thead>
					                    <tbody>
                                            <% 
                                                foreach (GridViewRow item in DatosPedidos.Rows)
                                                     { %>
                                            <tr>
                                                <td class="task-desc">
                                                    <%= item.Cells[1].Text %>
                                                </td> 
                                                <td class="task-desc">
                                                    <%= item.Cells[2].Text %>
                                                </td>         
                                                <% if (user.perfil == 1 || user.perfil == 3)
                                                   { %>                                    
                                                <td class="text-center">
                                                    <a href="/evr/Configuracion/editar_area/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <td class="text-center">
                                                    <a onclick="if(!confirm('¿Está seguro de que desea eliminar este area?')) return false;" href="/evr/Configuracion/eliminar_area/<%=item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
                                                </td>
                                                <% } %>
                                            </tr>
                                            <% }%>
					                             <% if (user.perfil == 1 || user.perfil == 3)
                                                            { %>
					                            <tr>
                                                    <td class="text-center">
                                                        <div class="form-group">                                 
                                                            <asp:TextBox ID="areacodigo" runat="server" class="form-control"></asp:TextBox>
                                                        </div>
                                                    </td>
                                                    <td class="text-center">
                                                        <div class="form-group">                                 
                                                            <asp:TextBox ID="areanombre" runat="server" class="form-control"></asp:TextBox>
                                                        </div>
                                                    </td>
                                                    <td class="text-center">                                               
                                                    </td>
                                                    <td style="padding-right:15px">
                                                        <div style="text-align:right">  
                                                            <input id="GuardarArea" type="submit" value="Añadir Área" class="btn btn-primary run-first" name="submit">  
                                                        </div>
                                                    </td>
                                                </tr>
                                             <% } %>
					                    </tbody>
					                </table>                                    
					            </div>
                                </center>
				            </div>
        
				        <!-- /tasks table -->

    <!-- /modal with table -->
    <div class="form-actions text-right">
        <input id="GuardarUsuario" type="submit" value="Guardar datos" class="btn btn-primary run-first" name="submit">                                                          
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
