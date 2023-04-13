<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.procesos oProceso = new MIDAS.Models.procesos();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["usuario"] != null)
            {

                user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            }

            if (Session["CentralElegida"] != null)
            {
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            }
            
            oProceso = (MIDAS.Models.procesos)ViewData["proceso"];

            txtNombre.Text = oProceso.nombre;
            txtCod.Text = oProceso.cod_proceso;
            txtOrden.Text = oProceso.orden.ToString();

            if (ViewData["dependencias"] != null)
            {
                List<MIDAS.Models.procesos> dependencias = new List<MIDAS.Models.procesos>();
                dependencias = (List<MIDAS.Models.procesos>)ViewData["dependencias"];

                ddlDependencia.DataSource = dependencias;
                ddlDependencia.DataTextField = "nombre";
                ddlDependencia.DataValueField = "id";
                ddlDependencia.DataBind();

                if (dependencias == null || dependencias.Count == 0)
                {
                    ListItem ninguno = new ListItem();
                    ninguno.Value = "0";
                    ninguno.Text = "---";
                    ddlDependencia.Items.Insert(0, ninguno);
                }

                if (oProceso.padre != null)
                    ddlDependencia.SelectedValue = oProceso.padre.ToString();
            }
            
        }

        if (centroseleccionado.tipo == 4)
        {
            txtNombre.Enabled = false;
            txtOrden.Enabled = false;
            txtCod.Enabled = false;
        }
                                                      

        if (Session["EdicionProcesoMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionProcesoMensaje"].ToString() + "' });", true);
            Session["EdicionProcesoMensaje"] = null;
        }
        if (Session["EdicionProcesoError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionProcesoError"].ToString() + "' });", true);
            Session["EdicionProcesoError"] = null;
        }             

    }
    
    
</script>

<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
	<title>Midas - Proceso </title>

    <script type="text/javascript">
        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarProceso")
                    $("#hdFormularioEjecutado").val("GuardarProceso");            

            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

        });
       
    </script>
    <style>
    .black_overlay{
        display: none;
        position: absolute;
        top: 0%;
        left: 0%;
        width: 100%;
        height: 100%;
        background-color: black;
        z-index:1001;
        -moz-opacity: 0.8;
        opacity:.80;
        filter: alpha(opacity=80);
    }
    .white_content {
        display: none;
        position: absolute;
        top: 40%;
        left: 25%;
        width: 50%;
        height: 50%;
        padding: 16px;
        border: 5px solid #06A4A0;
        background-color: white;
        z-index:1002;
        overflow: auto;
        border-radius:15px;
    }
</style>
</asp:Content>

<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
            <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>"/>
            
			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>Datos del proceso<asp:Label runat="server" ID="lblNombreProc"></asp:Label> <small>Cumplimentar datos del proceso</small></h3>
				</div>
			</div>
			<!-- /page header -->   

			<!-- Form vertical (default) -->
						<form id="Form1" runat="server">                           
						    <input id="hdFormularioEjecutado" name ="hdFormularioEjecutado" type="hidden" value="Entro"/>
                            <div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-pencil"></i>Datos</h6></div>
                                <div class="panel-body">
						 			        <div  class="form-group">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width:10%">
                                                            <label>Código</label>
												            <asp:TextBox ID="txtCod" Width="100%" runat="server" class="form-control" ></asp:TextBox>
                                                        </td>
                                                        <td style="width:80%; padding-left:20px">
                                                            <label>Nombre</label>
												            <asp:TextBox ID="txtNombre" Width="100%" runat="server" class="form-control" ></asp:TextBox>
                                                        </td>
                                                        <td style="width:10%; padding-left:20px">
                                                            <label>Orden</label>
												            <asp:TextBox ID="txtOrden" Width="50%" runat="server" class="form-control" ></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <label>Dependencia</label>
												            <asp:DropDownList Width="40%" CssClass="form-control" ID="ddlDependencia" runat="server"> 
                                                             </asp:DropDownList>     
                                                        </td>
                                                    </tr>
                                                </table>

												

											</div>     
                                            <br />
                                            <div class="form-group"> 
                                            </div> 
			                    </div>  
							</div>	                            
                            <div class="form-actions text-right">
                              <%
                                  if (centroseleccionado.tipo == 4)
                                  {
                                                      %>  
										<input id="GuardarProceso" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
                                        <% } %>
                                        <a href="/evr/procesos/gestion_procesos"class="btn btn-primary" >Volver</a></>      
							</div>                                    
                         </form>				
						<!-- /form vertical (default) -->
			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->
</asp:Content>
