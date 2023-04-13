<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Emergencias</title>
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

            if (ViewData["emergencias"] != null)
            {
                grdN1.DataSource = ViewData["emergencias"];
                grdN1.DataBind();
            }

            if (Session["EditarEmergenciasResultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarEmergenciasResultado"].ToString() + "' });", true);
                Session.Remove("EditarEmergenciasResultado");
            }

            if (Session["EditarEmergenciasError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarEmergenciasError"].ToString() + "' });", true);
                Session.Remove("EditarEmergenciasError");
            } 

        }        

    }
</script>
    <br />    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <h3>Simulacros</h3>
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    
    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-signup"></i>Simulacros</h6>
                        </div>
                        <div class="panel-body">
                        <br />
                        <label>Para el registro de eventos ambientales, de seguridad y salud o de calidad acceda al módulo de Comunicación o pulse <a href="/evr/Comunicacion/gestion_comunicacion">aquí</a> para acceder.</label>
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
                                                                            <th style="width:60px">
                                                                                Año      
                                                                            </th> 
                                                                            <th>
                                                                                Descripción      
                                                                            </th> 
                                                                            <th>
                                                                                Personal implicado      
                                                                            </th>
                                                                            <th>
                                                                                Responsable      
                                                                            </th>
                                                                            <th style="width:50px">
                                                                                Informe
                                                                            </th>                    
                                                                            <th style="width:100px">
                                                                                Fecha planificada
                                                                            </th>  
                                                                            <th style="width:100px">
                                                                                Fecha realización
                                                                            </th>
                                                                            <% if (permisos.permiso == true)
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
                                                                            <% if (permisos.permiso == true)
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
                                                                                    item.Cells[1].Text
                                
                                                                                    %>
 
                                                                            </td>   
                                                                        <td class="text">
                                                                        <%=  
                                                                                    item.Cells[2].Text
                                
                                                                                    %>
 
                                                                            </td>    
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[3].Text
                                
                                                                                    %>
 
                                                                            </td>  
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[4].Text
                                
                                                                                    %>
 
                                                                            </td>  
                                                                            <td style="width:50px" class="text-center">
                                                                             <% if (item.Cells[5].Text != "&nbsp;")
                                                                                { %>
                                                                                <a title="Ver Fichero" href="/evr/emergencias/ObtenerInforme/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                                <% } %>
                                                                            </td>  
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[6].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[6].Text).ToString("yyyy/MM/dd")%></span>   
                                                                                <%= (DateTime.Parse(item.Cells[6].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>                     
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[7].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[7].Text).ToString("yyyy/MM/dd")%></span>   
                                                                                <%= (DateTime.Parse(item.Cells[7].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[7].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[7].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>
                                                                            <% 
                                                                                if (permisos.permiso == true)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/emergencias/detalle_emergencia/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% }
                                                                               else
                                                                               { %>    
                                                                               <td style="width:50px" class="text-center">
                                                                                <a title="Consultar" href="/evr/emergencias/detalle_emergencia/<%=item.Cells[0].Text %>");"><i class="icon-search"></i></a>
                                                                            </td> 
                                                                               <% } %>    
                                                                            <% if (permisos.permiso == true)
                                                                               { %>                                                                  
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Emergencia" onclick="if(!confirm('¿Está seguro de que desea eliminar este registro de emergencia?')) return false;" href="/evr/emergencias/eliminar_emergencia/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
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
                                 <% if (permisos.permiso == true)
                                       { %>     
                                    <a href="/evr/emergencias/detalle_emergencia/0" class="btn btn-primary" title="Nueva Emergencia">Nuevo Simulacro</a>                                    
                                    <% } %></div>    
    </form>
</asp:Content>
