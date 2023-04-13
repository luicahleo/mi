<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Auditorías</title>
</asp:Content>
<asp:Content ID="principalContent" ContentPlaceHolderID="MainContent" runat="server">
<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
    int consulta = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            if (Session["CentralElegida"] != null)
            {
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            }            

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            if (user.perfil == 2)
                permisos = MIDAS.Models.Datos.ObtenerPermisos(user.idUsuario, centroseleccionado.id);
            else
            {
                permisos.idusuario = user.idUsuario;
                permisos.idcentro = centroseleccionado.id;
                permisos.permiso = true;
            }  

            if (ViewData["auditorias"] != null)
            {
                grdAuditorias.DataSource = ViewData["auditorias"];
                grdAuditorias.DataBind();
            }

            if (Session["EditarAuditoriaResultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarAuditoriaResultado"].ToString() + "' });", true);
                Session.Remove("EditarAuditoriaResultado");
            }

            if (permisos.permiso != true)
            {
                consulta = 1;
            }

            if (Session["EditarAuditoriaError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarAuditoriaError"].ToString() + "' });", true);
                Session.Remove("EditarAuditoriaError");
            } 

        }        

    }
</script>
    <br />
    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <table style="width:100%">
        <tr>
            <td style="width:50%">
                <h3>Auditorías</h3>
            </td>
<%--            <td style="width:50%; padding-bottom:10px">
                <a href="/evr/configuracion/ObtenerProgramaAuditoria" style="width:200px; float:right; background-color:#ff0f64; border-color:#ff0f64" class="btn btn-primary" id="A1" title="Descargar programa de auditoria">Descargar programa de auditoria</a>                                               
            </td>--%>
        </tr>
    </table>
    
    
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    
    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-signup"></i>Listado de auditorías</h6>
                        </div>
                        <div class="panel-body">
									<center>
                                    <table  width="100%" >
                                        <tr>
                                            <td colspan="4">
                                                <div runat="server" id="divN1">
								                    <asp:GridView ID="grdAuditorias" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="width:150px">
                                                                                Central      
                                                                            </th> 
                                                                            <th style="width:125px">
                                                                                Tipo de auditoría      
                                                                            </th>   
                                                                            <th style="width:125px">
                                                                                Referenciales      
                                                                            </th>          
                                                                            <th>
                                                                                Plan de auditoría
                                                                            </th>   
                                                                            <th>
                                                                                Informe de auditoría
                                                                            </th>   
                                                                            <th style="width:100px">
                                                                                Fecha inicio
                                                                            </th>  
                                                                            <th style="width:100px">
                                                                                Fecha fin
                                                                            </th>
                                                                            <% if (consulta != 1)
                                       { %>
                                                                            <th  style="width:50px">
                                                                                Editar
                                                                            </th>
                                                                            <% }
                                                                               else
                                                                               { %>
                                                                                <th  style="width:50px">
                                                                                Consultar
                                                                            </th>
                                                                               <% } %>
                                                                            <% if (consulta != 1)
                                       { %>
                                                                            <th  style="width:50px">
                                                                                Borrar
                                                                            </th>
                                                                            <% } %>

                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <% 
                                                                        foreach (GridViewRow item in grdAuditorias.Rows)
                                                                           { %>
                                                                        <tr>
                                                                        <td class="text">
                                                                        <%=  
                                                                                    item.Cells[1].Text
                                
                                                                                    %>
 
                                                                            </td>   
                                                                            <td class="text">
                                                                                <%=  
                                                                                    item.Cells[4].Text
                                
                                                                                    %>
 
                                                                            </td> 
                                                                            <td class="text">
                                                                                <%=  
                                                                                    item.Cells[9].Text
                                
                                                                                    %>
 
                                                                            </td> 
                                                                            <td style="width:50px" class="text-center">
                                                                                <% if (item.Cells[5].Text != "&nbsp;")
                                                                                   { %>
                                                                                <a title="Ver Fichero" href="/evr/Auditorias/ObtenerProgramaAuditoria/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                                <% } %>
                                                                            </td>  
                                                                            <td style="width:50px" class="text-center">
                                                                                <% if (item.Cells[6].Text != "&nbsp;")
                                                                                   { %>
                                                                                <a title="Ver Fichero" href="/evr/Auditorias/ObtenerInformeAuditoria/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                                <% } %>
                                                                            </td> 
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[2].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[2].Text).ToString("yyyy/MM/dd")%></span>   
                                                                                <%= (DateTime.Parse(item.Cells[2].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[2].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[2].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>                     
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[3].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[3].Text).ToString("yyyy/MM/dd")%></span>   
                                                                                <%= (DateTime.Parse(item.Cells[3].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[3].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[3].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>                                                                
                                                                            
                                       
                                                                            <% 
                                                                            if (consulta != 1)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/Auditorias/detalle_auditoria/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% }
                                                                               else
                                                                               { %>    
                                                                               <td style="width:50px" class="text-center">
                                                                                <a title="Consultar" href="/evr/Auditorias/detalle_auditoria/<%=item.Cells[0].Text %>");"><i class="icon-search"></i></a>
                                                                            </td> 
                                                                               <% } %>    
                                                                            <% if (consulta != 1)
                                                                               { %>                                                                  
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Auditoría" onclick="if(!confirm('¿Está seguro de que desea eliminar esta auditoría?')) return false;" href="/evr/Auditorias/eliminar_auditoria/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
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
                                    <% if (consulta != 1)
                                       { %>
                                    <a href="/evr/Auditorias/detalle_auditoria/0" class="btn btn-primary" title="Nueva Auditoría">Nueva Auditoría</a>
                                    
                                    <% } %></div>

    
    </form>
</asp:Content>
