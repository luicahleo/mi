<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Revisiones</title>
</asp:Content>
<asp:Content ID="principalContent" ContentPlaceHolderID="MainContent" runat="server">
<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
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

            if (ViewData["revisiones"] != null)
            {
                grdN1.DataSource = ViewData["revisiones"];
                grdN1.DataBind();
            }

            if (Session["EditarRevisionesResultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarRevisionesResultado"].ToString() + "' });", true);
                Session.Remove("EditarRevisionesResultado");
            }

            if (Session["EditarRevisionesError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarRevisionesError"].ToString() + "' });", true);
                Session.Remove("EditarRevisionesError");
            } 

        }        

    }
</script>
    <br />    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <h3>Revisiones</h3>
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    
    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-signup"></i>Revisiones energéticas</h6>
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
                                                                            <th style="width:50px">
                                                                                Año      
                                                                            </th> 
                                                                            <th style="width:100px">
                                                                                Fecha planificación
                                                                            </th>  
                                                                            <th style="width:50px">
                                                                                Planificación
                                                                            </th> 
                                                                             <th style="width:100px">
                                                                                Fecha revisión
                                                                            </th>  
                                                                            <th style="width:50px">
                                                                                Revisión
                                                                            </th> 
                                                                            <th style="width:100px">
                                                                                Responsable      
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
                                                                        <% if (item.Cells[2].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[2].Text).ToString("yyyy/MM/dd")%></span>   
                                                                                <%= (DateTime.Parse(item.Cells[2].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[2].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[2].Text).Year).ToString()%>
                                                                                <% } %>
 
                                                                            </td>    
                                                                            <td class="text">
                                                                        <% if (item.Cells[3].Text != "&nbsp;")
                                                                                { %>
                                                                                <center>
                                                                                <a title="Ver Fichero" href="/evr/revenergetica/ObtenerPlanificacion/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                                </center>
                                                                                <% } %>
 
                                                                            </td>  
                                                                            <td class="text">
                                                                        <% if (item.Cells[4].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[4].Text).ToString("yyyy/MM/dd")%></span>    
                                                                                <%= (DateTime.Parse(item.Cells[4].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[4].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[4].Text).Year).ToString()%>
                                                                                <% } %>
 
                                                                            </td>  
                                                                            <td class="text">
                                                                        <% if (item.Cells[5].Text != "&nbsp;")
                                                                                { %>
                                                                                <center>
                                                                                <a title="Ver Fichero" href="/evr/revenergetica/ObtenerRevision/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                                </center>
                                                                                <% } %>
 
                                                                            </td> 
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[6].Text
                                
                                                                                    %>
 
                                                                            </td> 

                                                                            <% 
                                                                                if (permisos.permiso == true)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/revenergetica/detalle_revision/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% }
                                                                               else
                                                                               { %>    
                                                                               <td style="width:50px" class="text-center">
                                                                                <a title="Consultar" href="/evr/revenergetica/detalle_revision/<%=item.Cells[0].Text %>");"><i class="icon-search"></i></a>
                                                                            </td> 
                                                                               <% } %>    
                                                                            <% if (permisos.permiso == true)
                                                                               { %>                                                                  
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Revision" onclick="if(!confirm('¿Está seguro de que desea eliminar este registro de revisión?')) return false;" href="/evr/revenergetica/eliminar_revision/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
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
                                    <a href="/evr/revenergetica/detalle_revision/0" class="btn btn-primary" title="Nueva Revisión">Nueva Revisión</a>                                    
                                    <% } %></div>    
    </form>
</asp:Content>
