<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Documentos</title>
    <script type="text/javascript">
        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "ctl00_MainContent_btnImprimir")
                    $("#hdFormularioEjecutado").val("btnImprimir");

                if (val == "ctl00_MainContent_btnImprimirSIG")
                    $("#hdFormularioEjecutado").val("btnImprimirSIG");

            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });
        });       
    </script>
</asp:Content>
<asp:Content ID="principalContent" ContentPlaceHolderID="MainContent" runat="server">
<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
    string tecnologia;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            if (Session["CentralElegida"] != null)
            {
                user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                
                if (user.perfil == 2)
                    permisos = MIDAS.Models.Datos.ObtenerPermisos(user.idUsuario, centroseleccionado.id);
                else
                {
                    permisos.idusuario = user.idUsuario;
                    permisos.idcentro = centroseleccionado.id;
                    permisos.permiso = true;
                }
                
                if (ViewData["tecnologias"] != null && centroseleccionado.tipo != 4)
                {
                    List<MIDAS.Models.tipocentral> listatecnologias = new List<MIDAS.Models.tipocentral>();
                    listatecnologias = (List<MIDAS.Models.tipocentral>)ViewData["tecnologias"];
                    tecnologia = listatecnologias.Where(x => x.id == centroseleccionado.tipo).First().nombre;
                }
                else
                {
                    tecnologia = "Todas las tecnologías";
                }
            }

            if (ViewData["ficherosN1"] != null)
            {
                grdN1.DataSource = ViewData["ficherosN1"];
                grdN1.DataBind();
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
    <br />
    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <h3>Gestión Documental</h3>
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    
    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-file"></i>Documentos subidos</h6>
                        </div>
                        <div class="panel-body">
									<center>

                                    <table  width="100%" >
                                        <tr>
                                            <td>
                                                <div runat="server" id="divN1">
								                    <asp:GridView ID="grdN1" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="width:70px">
                                                                                Código      
                                                                            </th> 
                                                                            <th>
                                                                                Titulo      
                                                                            </th> 
                                                                            <th>
                                                                                Tipo      
                                                                            </th> 
                                                                            <th style="width:50px">
                                                                                Versión
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
                                                                            <% if (centroseleccionado.tipo == 4 && (user.perfil == 1 || permisos.permiso == true))
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Editar
                                                                            </th>
                                                                            <% } %>
                                                                            <% if (centroseleccionado.tipo == 4 && (user.perfil == 1 || permisos.permiso == true))
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Borrar
                                                                            </th>
                                                                            <% } %>

                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <% 
                                                                        foreach (GridViewRow item in grdN1.Rows)
                                                                           { %>
                                                                        <tr>
                                                                        <td class="text">
                                                                        <%=  
                                                                                    item.Cells[16].Text
                                
                                                                                    %>
 
                                                                            </td>  
                                                                        <td class="text">
                                                                        <%=  
                                                                                    item.Cells[7].Text
                                
                                                                                    %>
 
                                                                            </td>    
                                                                            <td class="text">
     
                                                                                <%=  
                                                                                    item.Cells[10].Text
                                
                                                                                    %>
 
                                                                            </td>   
                                                                            <td class="text">
     
                                                                                <%=  
                                                                                    item.Cells[17].Text
                                
                                                                                    %>
 
                                                                            </td>              
                                                                            <td style="text-align:center" class="task-desc">
                                                                             <% if (item.Cells[5].Text != "&nbsp;")
                                                                                { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[5].Text).ToString("yyyy/MM/dd")%></span>    
                                                                                <%= (DateTime.Parse(item.Cells[5].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>                     
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[6].Text != "&nbsp;")
                                                                               { %>
                                                                               <span style="display:none;"><%= DateTime.Parse(item.Cells[6].Text).ToString("yyyy/MM/dd")%></span>    
                                                                                <%= (DateTime.Parse(item.Cells[6].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Year).ToString()%>
                                                                            <% } %>
                                                                            </td>   
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Ver Fichero" href="/evr/Documentos/ObtenerDocumento/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                            </td>  
                                                                            <% if (centroseleccionado.tipo == 4 && (user.perfil == 1 || permisos.permiso == true))
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/Documentos/editar_documento/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% } %>        
                                                                            <% 
                                                                                if (centroseleccionado.tipo == 4 && (user.perfil == 1  || permisos.permiso == true ))
                                                                               { %>                                                                  
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Fichero" onclick="if(!confirm('¿Está seguro de que desea eliminar este documento?')) return false;" href="/evr/Documentos/eliminar_documentacion/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>      
                                                                            <% } %>               
                                                                            <% } %>
                                                                        </tr>
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
                                 
                                 
                                       <div style="text-align:right">
                                           <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
                                           <% 
                                                                                if (centroseleccionado.tipo == 4 && (user.perfil == 1  || permisos.permiso == true ))
                                                                               { %>  
                                       <asp:Button ID="btnImprimirSIG" runat="server" class="btn btn-primary run-first" Text="Exportar Doc SIG" />
                                       
                                    <a href="/evr/Documentos/editar_documento/0" class="btn btn-primary" id="btnNuevo" title="Nuevo Documento">Nuevo Documento</a>
                                           <% } %>
                                    </div>
                                    

    
    </form>
</asp:Content>
