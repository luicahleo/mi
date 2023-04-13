<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.procesos oProceso = new MIDAS.Models.procesos();
    MIDAS.Models.indicadores oIndicador = new MIDAS.Models.indicadores();
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

            if (ViewData["tecnologias"] != null)
            {
                ddlTecnologia.DataSource = ViewData["tecnologias"];
                ddlTecnologia.DataValueField = "id";
                ddlTecnologia.DataTextField = "nombre";
                ddlTecnologia.DataBind();

                ListItem general = new ListItem();
                general.Value = "0";
                general.Text = "No";
                ddlTecnologia.Items.Insert(0, general);
            }

            grdDocumentacion.DataSource = ViewData["documentos"];
            grdDocumentacion.DataBind();
            
            oProceso = (MIDAS.Models.procesos)ViewData["proceso"];
            oIndicador = (MIDAS.Models.indicadores)Session["indFoca"];
            
            txtNombre.Text = oProceso.nombre;
            txtDescripcion.Text = oProceso.descripcion;
            txtEntradas.Text = oProceso.objetivos;
            txtSalidas.Text = oProceso.alcance;
            txtCod.Text = oProceso.cod_proceso;
            ddlTecnologia.SelectedValue = oProceso.tecnologia.ToString();
     
        }

        if (user.perfil != 1)
        {
            txtNombre.Enabled = false;
            txtCod.ReadOnly = true;
            txtDescripcion.ReadOnly = true;
            txtEntradas.ReadOnly = true;
            txtSalidas.ReadOnly = true;
            ddlTecnologia.Enabled = false;
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
                else $("#hdFormularioEjecutado").val("btnImprimir");   
                

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
        top: 80%;
        left: 25%;
        width: 60%;
        height: 600px;
        padding: 16px;
        border: 5px solid #41b9e6;
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
					<h3>Ficha de proceso<asp:Label runat="server" ID="lblNombreProc"></asp:Label> </h3>
				</div>
			</div>
			<!-- /page header -->

			<!-- Form vertical (default) -->
						<form enctype="multipart/form-data" method="post" id="Form1" runat="server">
						    <input id="hdFormularioEjecutado" name ="hdFormularioEjecutado" type="hidden" value="Entro"/>
                            <div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-pencil"></i>Cabecera</h6></div>
                                <div class="panel-body">
						 			        <div  class="form-group">
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width:10%">
                                                            <label>Código (*)</label>
												            <asp:TextBox ID="txtCod" Width="100%" runat="server" class="form-control" ></asp:TextBox>
                                                        </td>
                                                        <td style="width:60%; padding-left:20px">
                                                            <label>Nombre (*)</label>
												            <asp:TextBox ID="txtNombre" Width="90%" runat="server" class="form-control" ></asp:TextBox>
                                                        </td>
                                                        <td style="width:30%; padding-left:20px" >
                                                            <label>Específico</label>
												            <asp:DropDownList CssClass="form-control" ID="ddlTecnologia" runat="server"> 
                                                            </asp:DropDownList>  
                                                        </td>
                                                    </tr>
                                                </table>
											</div>

                                            <div  class="form-group">
											<div class="form-group">
												<label>Entradas</label>
                                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtEntradas" runat="server" class="form-control" ></asp:TextBox>
											</div>
                                            <div class="form-group">
												<label>Salidas</label>
                                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtSalidas" runat="server" class="form-control" ></asp:TextBox>
											</div>
                                            <div class="form-group">
												<label>Objeto</label>
                                                <asp:TextBox TextMode="MultiLine" Rows="4" ID="txtDescripcion" runat="server" class="form-control" ></asp:TextBox>
											</div>
										</div> 
			                    </div>  
							</div>	
                                                       
                           <div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-paragraph-center"></i>Documentación</h6></div>
								<div class="panel-body">
									<center>
                                    <table  width="100%" >
                                        <tr>
                                            <td colspan="4">
                                                <div runat="server" id="divProc">                           
                                                    <asp:GridView ID="grdDocumentacion" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th>
                                                                                Titulo      
                                                                            </th> 
                                                                            <th>
                                                                                Nivel
                                                                            </th>
                                                                            <th>
                                                                                Tipo      
                                                                            </th>                                                                             
                                                                            <th style="width:100px">
                                                                                Fecha de aprobación
                                                                            </th>                    
                                                                            <th style="width:100px">
                                                                                Fecha de publicación
                                                                            </th>   
                                                                            <th  style="width:50px">
                                                                                Descarga
                                                                            </th>                                                                           
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <% foreach (GridViewRow item in grdDocumentacion.Rows)
                                                                           { %>
                                                                        <tr>         
                                                                            <td class="text">     
                                                                                <%= item.Cells[7].Text %>
                                                                            </td>    
                                                                            <td class="text">     
                                                                                <%= item.Cells[8].Text %> 
                                                                            </td>  
                                                                            <td class="text">     
                                                                                <%= item.Cells[10].Text %> 
                                                                            </td>             
                                                                            <td style="text-align:center" class="task-desc">
                                                                             <% if (item.Cells[5].Text != "&nbsp;")
                                                                                { %>
                                                                                <%= (DateTime.Parse(item.Cells[5].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Year).ToString()%>
                                                                            <%} %>
                                                                            </td>                     
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[6].Text != "&nbsp;")
                                                                                { %>
                                                                                <%= (DateTime.Parse(item.Cells[6].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Year).ToString()%>
                                                                            <%} %>
                                                                            </td>    
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Ver Fichero" href="/evr/documentos/ObtenerDocumento/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                            </td>  
                                                                        </tr>
                                                                        <% } %>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>
                                                </div>
                                            </td>                                            
                                        </tr>
                                    </table>
							        </center>
                                 </div>

							</div>
                            
                            <div class="form-actions text-right">
                                <asp:Button ID="btnImprimir" runat="server"  class="btn btn-primary run-first" Text="Exportar" />
                              <%
                                  if (user.perfil == 1)
                                  {
                                                      %>  
                                        <a class="btn btn-primary run-first" title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar esta ficha de proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%= oProceso.id %>");">Eliminar Proceso</a>
										<input id="GuardarProceso" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
                                        <% } %>
                                        <a href="/evr/procesos/gestion_procesos" class="btn btn-primary" >Volver</a></>
           
									</div>
                                    
                         </form>				
						<!-- /form vertical (default) -->	
			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->
</asp:Content>
