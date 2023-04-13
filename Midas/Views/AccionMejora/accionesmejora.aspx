<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Acciones Mejora</title>
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

            Session.Remove("ModuloAccionMejora");
            Session.Remove("ReferenciaAccionMejora");

            for (int i = 2008; i <= DateTime.Now.Year; i++)
            {
                ListItem itemAnio = new ListItem();
                itemAnio.Value = i.ToString();
                itemAnio.Text = i.ToString();
                ddlAnio.Items.Insert(0, itemAnio);
            }
            ListItem todos = new ListItem();
            todos.Value = "0";
            todos.Text = "Todos";
            ddlAnio.Items.Insert(0, todos);

            if (ViewData["accionesmejora"] != null)
            {
                grdN1.DataSource = ViewData["accionesmejora"];
                grdN1.DataBind();
            }

            if (Session["EditarAccionesMejoraResultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarAccionesMejoraResultado"].ToString() + "' });", true);
                Session.Remove("EditarAccionesMejoraResultado");
            }

            if (Session["EditarAccionesMejoraError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarAccionesMejoraError"].ToString() + "' });", true);
                Session.Remove("EditarAccionesMejoraError");
            } 

        }        

    }
</script>
    <br />    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <h3>Acciones Mejora</h3>
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    
    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-signup"></i>Acciones Mejora</h6>
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
                                                                            <th style="width:60px">
                                                                                Año      
                                                                            </th> 
                                                                            <th style="width:120px">
                                                                                Tipo      
                                                                            </th>  
                                                                            <th>
                                                                                Asunto      
                                                                            </th>
                                                                            <th>
                                                                                Ámbito
                                                                            </th>
                                                                            <th>
                                                                                Referenciales
                                                                            </th>
                                                                            <th style="width:100px">
                                                                                Fecha apertura
                                                                            </th>  
                                                                            <th style="width:100px">
                                                                                Fecha cierre
                                                                            </th>  
                                                                            <th  style="width:150px">
                                                                                Estado      
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
                                                                                    item.Cells[16].Text
                                
                                                                                    %>
 
                                                                            </td>   
                                                                            <td>
                                                                            <% 
                                                                                if (item.Cells[12].Text == "0" || item.Cells[12].Text == "&nbsp;")
                                                                               { %>
                                                                               General
                                                                            <% }
                                                                               else
                                                                               {
                                                                                   string cadenareferenciales = string.Empty;
                                                                                   int idAccion = int.Parse(item.Cells[0].Text);
                                                                                   List<MIDAS.Models.referenciales> listaReferenciales = MIDAS.Models.Datos.ListarReferencialesAsignadosAccmejora(idAccion);
                                                                                   foreach (MIDAS.Models.referenciales refer in listaReferenciales)
                                                                                   {
                                                                                       if (cadenareferenciales == string.Empty)
                                                                                       {
                                                                                           cadenareferenciales = refer.nombre;
                                                                                       }
                                                                                       else
                                                                                       {
                                                                                           cadenareferenciales = cadenareferenciales + ", " + refer.nombre;
                                                                                       }
                                                                                   }
                                                                                    %>
                                                                                        <%=cadenareferenciales %>
                                                                               <% } %>
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
                                                                        <% 
                                                                                if (item.Cells[7].Text == "0")
                                                                           { %>
                                                                        <td class="task-desc">
                                                                           <b>Abierto</b>
                                                                        </td>
                                                                        <% }%><% 
                                     
                                                    
                                                                                            if (item.Cells[7].Text == "1")
                                                                                               { %>
                                                                        <td style="color: LimeGreen" class="task-desc">
                                                                        <b>
                                                                            Cerrado eficaz</b>
                                                                        </td>
                                                                        <% } %><% 
                                     
                                                    
                                                                                            if (item.Cells[7].Text == "2")
                                                                                               { %>
                                                                        <td style="color: Red" class="task-desc">
                                                                        <b>
                                                                            Cerrado no eficaz</b>
                                                                        </td>
                                                                        <% } %>
                                                                            <% 
                                                                                if (permisos.permiso == true)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/accionmejora/detalle_accion/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% }
                                                                               else
                                                                               { %>    
                                                                               <td style="width:50px" class="text-center">
                                                                                <a title="Consultar" href="/evr/accionmejora/detalle_accion/<%=item.Cells[0].Text %>");"><i class="icon-search"></i></a>
                                                                            </td> 
                                                                               <% } %>    
                                                                            <% if (permisos.permiso == true)
                                                                               { %>                                                                  
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar acción de mejora" onclick="if(!confirm('¿Está seguro de que desea eliminar este registro de emergencia?')) return false;" href="/evr/accionmejora/eliminar_accionmejora/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>      
                                                                            <% } %>               
                                                                            
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

                                 <div style="text-align:right">
                                 <table style="float:right;">
                                    <tr>
                                        <td style="padding-right:5px; padding-top:5px ; width:50px">
                                          <label>Año:</label>
                                        </td>
                                        <td style="padding-right:5px; width:80px">
                                             <asp:DropDownList runat="server" ID="ddlAnio" class="form-control" Width="95%">
                                             </asp:DropDownList>
                                        </td>
                                        <td  style="padding-right:5px">
                                            <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar" />
                                        </td>
                                        <% if (permisos.permiso == true)
                                       { %>     
                                        <td style="padding-right:5px">
                                            <a href="/evr/accionmejora/detalle_accion/0" class="btn btn-primary" title="Nueva Acción de Mejora">Nueva Acción de Mejora</a>      
                                        </td> <% } %>
                                    </tr>

                                 </table>
                                
                                 
                                 
                                                                  
                                   </div>    
    </form>
</asp:Content>
