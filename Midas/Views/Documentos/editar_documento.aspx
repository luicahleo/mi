<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    

    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
    string tecnologia;
    MIDAS.Models.documentacion datosDoc = new MIDAS.Models.documentacion();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            DatosPedidos.DataSource = ViewData["historial_doc"];
            DatosPedidos.DataBind();



            if (Session["CentralElegida"] != null)
            {
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));

                if (user.perfil == 2)
                    permisos = MIDAS.Models.Datos.ObtenerPermisos(user.idUsuario, centroseleccionado.id);
                else
                {
                    permisos.idusuario = user.idUsuario;
                    permisos.idcentro = centroseleccionado.id;
                    permisos.permiso = true;
                }

                if (centroseleccionado.tipo == 4)
                {
                    columnatecnologia.Visible = false;
                    if (user.perfil == 1 || permisos.permiso == true)
                    {
                        //ListItem nivel1 = new ListItem();
                        //nivel1.Value = "1";
                        //nivel1.Text = "1";
                        //ddlNivel.Items.Insert(0, nivel1);

                        //ListItem nivel2 = new ListItem();
                        //nivel2.Value = "2";
                        //nivel2.Text = "2";
                        //ddlNivel.Items.Add(nivel2);

                        if (ViewData["tecnologias"] != null)
                        {
                            ddlTecnologia.DataSource = ViewData["tecnologias"];
                            ddlTecnologia.DataValueField = "id";
                            ddlTecnologia.DataTextField = "nombre";
                            ddlTecnologia.DataBind();
                        }
                    }
                }
                else
                {
                    //ListItem nivel3 = new ListItem();
                    //nivel3.Value = "3";
                    //nivel3.Text = "3";
                    //ddlNivel.Items.Add(nivel3);

                    //if (centroseleccionado.ubicacion == 6)
                    //{
                    //    ListItem nivel4 = new ListItem();
                    //    nivel4.Value = "4";
                    //    nivel4.Text = "I.C.";
                    //    ddlNivel.Items.Add(nivel4);
                    //}

                    if (ViewData["tecnologias"] != null)
                    {
                        List<MIDAS.Models.tipocentral> listatecnologias = new List<MIDAS.Models.tipocentral>();
                        listatecnologias = (List<MIDAS.Models.tipocentral>)ViewData["tecnologias"];

                        ListItem tecnologia = new ListItem();
                        tecnologia.Value = listatecnologias.Where(x => x.id == centroseleccionado.tipo).First().id.ToString();
                        tecnologia.Text = listatecnologias.Where(x => x.id == centroseleccionado.tipo).First().nombre;
                        ddlTecnologia.Items.Insert(0, tecnologia);
                    }
                    columnatecnologia.Visible = false;
                }
            }



            if (ViewData["procesos"] != null)
            {
                ddlProcesos.DataSource = ViewData["procesos"];
                ddlProcesos.DataValueField = "id";
                ddlProcesos.DataTextField = "nombre";
                ddlProcesos.DataBind();
            }

            if (ViewData["tiposdocumento"] != null)
            {
                ddlTipo.DataSource = ViewData["tiposdocumento"];
                ddlTipo.DataValueField = "id";
                ddlTipo.DataTextField = "tipo";
                ddlTipo.DataBind();
            }

            if (ViewData["fichero"] != null)
            {
                datosDoc = (MIDAS.Models.documentacion)ViewData["fichero"];
                lblNombreDoc.Text = " - " + datosDoc.titulo;
                hdnIdDocumento.Value = datosDoc.idFichero.ToString();
                txtTitulo.Text = datosDoc.titulo;
                txtCodigo.Text = datosDoc.cod_fichero;
                txtVersion.Text = datosDoc.version;
                ddlTipo.SelectedValue = datosDoc.tipo.ToString();
                //ddlNivel.SelectedValue = datosDoc.nivel.ToString();
                if (datosDoc.nivel == 2)
                {
                    columnatecnologia.Visible = true;
                    ddlTecnologia.SelectedValue = datosDoc.tipocentral.ToString();
                }
                else
                {
                    columnatecnologia.Visible = false;
                }
                if (datosDoc.fecha_aprobacion != null)
                    txtFAprobacion.Text = datosDoc.fecha_aprobacion.ToString().Replace(" 0:00:00", "");
                if (datosDoc.fecha_publicacion != null)
                    txtFPublicacion.Text = datosDoc.fecha_publicacion.ToString().Replace(" 0:00:00", "");

                if (datosDoc.idproceso != 0)
                    ddlProcesos.SelectedValue = datosDoc.idproceso.ToString();

                if (datosDoc.idFichero != 0)
                {
                    //ddlTipo.Enabled = false;
                    //ddlNivel.Enabled = false;
                    ddlTecnologia.Enabled = false;
                }
            }
            else
            {
                datosDoc.idFichero = 0;
            }



            if (Session["EditarDocumentacionResultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarDocumentacionResultado"].ToString() + "' });", true);
                Session.Remove("EditarDocumentacionResultado");
            }

            if (Session["EditarDocumentacionError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarDocumentacionError"].ToString() + "' });", true);
                Session.Remove("EditarDocumentacionError");
            }



        }
    }
</script>    
<asp:Content ID="pedidoHead" ContentPlaceHolderID="head" runat="server">
	<title>DIMAS</title>
    <script type="text/javascript">
        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarDocumento")
                    $("#hdFormularioEjecutado").val("GuardarDocumento");

            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

            $("#ctl00_MainContent_ddlNivel").change(function () {
                var perfilseleccionado = $('#ctl00_MainContent_ddlNivel').val();

                if (perfilseleccionado == 2) {
                    $("#ctl00_MainContent_columnatecnologia").show();
                }
                else {
                    $("#ctl00_MainContent_columnatecnologia").hide();
                }
            });
        });
</script>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">
<form enctype="multipart/form-data" method="post" id="Form1" runat="server">
<input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>Edición de documentación<asp:Label runat="server" ID="lblNombreDoc"></asp:Label></h3>
				</div>
			</div>
			<!-- /page header -->

                        <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-file"></i>Datos del documento</h6>
                        </div>
                        <div class="panel-body">
                        <asp:HiddenField runat="server" ID="hdnIdDocumento" />
                            
                            <table style="width:100%" id="tablaFicheroNuevo">
                                 <tr> 
                                     <td style="width:10%; padding-right:10px">
                                        <div class="form-group">
                                                <label>
                                                    Código</label>
                                                    <asp:TextBox ID="txtCodigo" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                    </td>         
                                    <td style="padding-right:10px">
                                        <div class="form-group">
                                                <label>
                                                    Título (*)</label>
                                                    <asp:TextBox ID="txtTitulo" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                    </td>                                      
                                   <%-- <td style="padding-right:10px; width:6%">
                                        <div class="form-group">
                                                <label>
                                                    Nivel</label>
                                                    <asp:DropDownList AutoPostBack="true" ID="ddlNivel" runat="server" class="form-control">
                                                    </asp:DropDownList>
                                            </div>
                                    </td>  --%>
                                    <td runat="server" id="columnatecnologia" style="padding-right:10px">
                                        <div class="form-group">
                                                <label>
                                                    Tecnología</label>
                                                    <asp:DropDownList AutoPostBack="true" ID="ddlTecnologia" runat="server" class="form-control">
                                                    </asp:DropDownList>
                                            </div>
                                    </td>  
                                    <td style="padding-right:10px; width:25%">
                                        <div class="form-group">
                                                <label>
                                                    Tipo</label>
                                                    <asp:DropDownList ID="ddlTipo" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                    </td>  
                                    <td style="padding-right:10px; width:10%">
                                        <div class="form-group">
                                                <label>
                                                    F.Aprobación (*)</label>
                                                <asp:TextBox Style="text-align: center" ID="txtFAprobacion" CssClass="datepicker form-control"
                                                    runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                    </td>
                                    <td style="padding-right:10px; width:10%">
                                        <div class="form-group">
                                                <label>
                                                    F.Publicación (*)</label>
                                                <asp:TextBox Style="text-align: center" ID="txtFPublicacion" CssClass="datepicker form-control"
                                                    runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div class="form-group">
                                                <label>
                                                    Proceso asociado</label>
                                                    <asp:DropDownList ID="ddlProcesos" style="width:99%" runat="server" class="form-control">
                                                    </asp:DropDownList>
                                            </div>
                                    </td>
                                    <td colspan="2">
                                    <div class="form-group">
                                                <label>
                                                    Versión (*)</label>
                                                <asp:TextBox Style="text-align: center;width:50%" ID="txtVersion" CssClass="form-control"
                                                    runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                    </td>
                                </tr>
                            </table>

                            <table>
                                <tr>    
                                    <td><label>Subir documento</label></td>      
                                    <td style="padding-left:15px">
                                        <div class="form-group">
                                        <input type="file" id="file" name="file" style="margin-top:10px" /></div>
                                    </td> 
                                 </tr>
                            </table>
                            
                        </div>
                        </div>

                        <div class="form-actions text-right">
                            <input id="GuardarDocumento" type="submit" value="Guardar documento" class="btn btn-primary run-first"/>                            
                            <a href="/evr/documentos/admrepositorio" title="Volver" class="btn btn-primary run-first">Volver</a>
                            <br />
                        </div>
                            <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
                            </asp:GridView>

                        <% if (datosDoc.idFichero != 0)
                           { %>

				        <!-- Tasks table -->
                        <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-history"></i>Histórico de versiones</h6>
                        </div>
                        <div class="panel-body">
				        	<div class="block">
                                <center>
					            <div style="width:95%" class="datatablePedido">
					                <table class="table table-bordered">
					                    <thead>
					                        <tr>
												<th>Versión del Documento</th>						  
                                                <th>Fecha Aprobación</th>
                                                <th>Fecha Publicación</th>
                                                <th style="width:100px">Descargar</th>   
                                                <% if (permisos.permiso == true)
                                                   { %>  
                                                <th style="width:100px">Eliminar</th>      
                                                <% } %>                                       
					                        </tr>
					                    </thead>
					                    <tbody>
                                            <% 
                            foreach (GridViewRow item in DatosPedidos.Rows)
                            { %>
                                            <tr>
												<td style="text-align:center; width:2%" class="task-desc">
                                                    <%=item.Cells[6].Text%>
                                                </td>											  
                                                <td style="text-align:center" class="task-desc">
                                                    <% if (item.Cells[4].Text != "&nbsp;")
                                                       { %>
                                                    <span style="display:none;"><%= DateTime.Parse(item.Cells[4].Text).ToString("yyyy/MM/dd")%></span>  
                                                    <%= (DateTime.Parse(item.Cells[4].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[4].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[4].Text).Year).ToString()%>
                                                    <% } %>
                                                </td>
                                                <td style="text-align:center" class="task-desc">
                                                    <% if (item.Cells[5].Text != "&nbsp;")
                                                       { %>
                                                    <span style="display:none;"><%= DateTime.Parse(item.Cells[5].Text).ToString("yyyy/MM/dd")%></span>  
                                                    <%= (DateTime.Parse(item.Cells[5].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Year).ToString()%>
                                                    <% } %>
                                                </td>
                                                <td style="text-align:center" class="task-desc">
                                                    <center>
                                                          <a title="Ver Fichero" href="/evr/documentos/ObtenerDocumentoHist/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                    </center>
                                                </td>                                           
                                                <% if (permisos.permiso == true)
                                                   { %>      
                                                <td style="text-align:center" class="task-desc">
                                                    <center>
                                                          <a title="Eliminar Fichero" onclick="return confirm('¿Está seguro que desea eliminar el documento?');" href="/evr/documentos/eliminar_documentacionhist/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                    </center>
                                                </td>   <% } %>
                                            </tr>
                                            <% }%>
					                            
					                    </tbody>
					                </table>
					            </div>
                                </center>
				            </div>
                        </div></div>
				        <!-- /tasks table -->
                       <% } %>

			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->

</form>
</asp:Content>
