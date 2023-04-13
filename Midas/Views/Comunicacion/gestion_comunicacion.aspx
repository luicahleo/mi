<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Comunicación</title>
    <script type="text/javascript">
        $(document).ready(function () {

            
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

            if (ViewData["partes"] != null)
            {
                grdN5.DataSource = ViewData["partes"];
                grdN5.DataBind();
            }

            if (Session["EditarComunicacionResultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarComunicacionResultado"].ToString() + "' });", true);
                Session.Remove("EditarComunicacionResultado");
            }

            if (Session["EditarComunicacionError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarComunicacionError"].ToString() + "' });", true);
                Session.Remove("EditarComunicacionError");
            } 

        }        

    }
</script>
    <br />
    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <h3>Gestión de la Comunicación</h3>
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    
    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-signup"></i>Partes de comunicación de riesgos</h6>
                        </div>
                        <div class="panel-body">
									<center>
                                    <table  width="100%" >
                                        <tr>
                                            <td>
                                                <div runat="server" id="divN5">
								                    <asp:GridView ID="grdN5" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th>
                                                                                Nº Comunicación      
                                                                            </th> 
                                                                            <th>
                                                                                Empresa      
                                                                            </th> 
                                                                            <th>
                                                                                Instalación/Equipo      
                                                                            </th> 
                                                                            <th>
                                                                                Trabajo     
                                                                            </th>                                                                                                                                                    
                                                                             <th>
                                                                                Asunto     
                                                                            </th>   
                                                                            <th  style="width:50px">
                                                                                Editar
                                                                            </th>
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
                                                                        foreach (GridViewRow item in grdN5.Rows)
                                                                           { %>
                                                                        <tr>
                                                                        <td class="text">
                                                                        <%=  
                                                                                    item.Cells[1].Text
                                
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
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[5].Text
                                
                                                                                    %>
 
                                                                            </td>        
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[21].Text
                                
                                                                                    %>
 
                                                                            </td>   
                                                                                                                                            
                                                                            

                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/Comunicacion/detalle_parte/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                          
                                                                            <% if (permisos.permiso == true)
                                                                             { %>                                                               
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Parte" onclick="if(!confirm('¿Está seguro de que desea eliminar este parte?')) return false;" href="/evr/Comunicacion/eliminar_parte/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>      
                                                                            <% } %>               
                                                                            <% } %>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>     
                                                        
                                                           <div style="text-align:right">
                                                        <asp:Button ID="Button4" runat="server" class="btn btn-primary run-first" Text="Exportar" />
                                                               <% if (permisos.permiso == true)
                                                           { %>
                                                        <a href="/evr/Comunicacion/detalle_parte/0" class="btn btn-primary" title="Nuevo Parte">Nuevo Parte</a>
                                                               <% } %> 
                                                        </div>
                                                        
                                                        <br />                                                  
                                                 </div>                                                                                 
                                            </td>                                            
                                        </tr>
                                    </table>
							        </center>
                                 </div>                                    
                                 </div>
                                 

    
    </form>
</asp:Content>
