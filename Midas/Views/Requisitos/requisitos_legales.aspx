<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Plan de Formación</title>
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

            if (ViewData["requisitos"] != null)
            {
                grdRequisitos.DataSource = ViewData["requisitos"];
                grdRequisitos.DataBind();
            }


            if (Session["EditarRequisitoResultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarRequisitoResultado"].ToString() + "' });", true);
                Session.Remove("EditarRequisitoResultado");
            }

            if (Session["EditarRequisitoError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarRequisitoError"].ToString() + "' });", true);
                Session.Remove("EditarRequisitoError");
            } 

        }        

    }
</script>
    <br />
    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <h3>Requisitos legales</h3>
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    
    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-balance"></i>Evaluaciones de cumplimiento de requisitos legales y otros requisitos</h6>
                        </div>
                        <div class="panel-body">
									<center>
                                    <table  width="100%" >
                                        <tr>
                                            <td colspan="4">
                                                <div runat="server" id="divN1">
								                    <asp:GridView ID="grdRequisitos" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="width:60px">
                                                                                Año      
                                                                            </th> 
                                                                            <th style="width:100px">
                                                                                Ámbito      
                                                                            </th> 
                                                                            <th>
                                                                                Denominación      
                                                                            </th> 
                                                                            <th style="width:50px">
                                                                                Informe Ev.
                                                                            </th>                    
                                                                            <th style="width:70px">
                                                                                Fecha Reg.
                                                                            </th>  
                                                                            <th style="width:50px">
                                                                                Num. Req
                                                                            </th>
                                                                            <th style="width:50px">
                                                                                Cumple
                                                                            </th> 
                                                                            <th style="width:50px">
                                                                                En trám.
                                                                            </th>  
                                                                            <th style="width:50px">
                                                                                No cump.
                                                                            </th>  
                                                                            <th style="width:50px">
                                                                                Obs.
                                                                            </th>  
                                                                            <th style="width:50px">
                                                                                No proc.
                                                                            </th>  
                                                                            <th style="width:50px">
                                                                                No verif.
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
                                                                        foreach (GridViewRow item in grdRequisitos.Rows)
                                                                           { %>
                                                                        <tr>
                                                                        <td class="text">
                                                                        <%=  
                                                                                    item.Cells[1].Text
                                
                                                                                    %>
 
                                                                            </td>   
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[15].Text
                                
                                                                                    %>
 
                                                                            </td>  
                                                                        <td class="text">
                                                                        <%=  
                                                                                    item.Cells[3].Text
                                
                                                                                    %>
 
                                                                            </td>    
                                                                            <td style="width:50px" class="text-center">
                                                                             <% if (item.Cells[4].Text != "&nbsp;")
                                                                                { %>
                                                                                <a title="Ver Fichero" href="/evr/Requisitos/ObtenerInformeEvaluacion/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                                <% } %>
                                                                            </td>  
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[5].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[5].Text).ToString("yyyy/MM/dd")%></span>   
                                                                                <%= (DateTime.Parse(item.Cells[5].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td> 
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[6].Text
                                
                                                                                    %>
 
                                                                            </td>
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[7].Text
                                
                                                                                    %>
 
                                                                            </td> 
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[8].Text
                                
                                                                                    %>
 
                                                                            </td>     
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[9].Text
                                
                                                                                    %>
 
                                                                            </td>                                                                                 
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[10].Text
                                
                                                                                    %>
 
                                                                            </td>    
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[11].Text
                                
                                                                                    %>
 
                                                                            </td>    
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[12].Text
                                
                                                                                    %>
 
                                                                            </td>    
                                                                            <% 
                                                                                if (permisos.permiso == true)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/Requisitos/detalle_requisito/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% }
                                                                               else
                                                                               { %>    
                                                                               <td style="width:50px" class="text-center">
                                                                                <a title="Consultar" href="/evr/Requisitos/detalle_requisito/<%=item.Cells[0].Text %>");"><i class="icon-search"></i></a>
                                                                            </td> 
                                                                               <% } %>    
                                                                            <% if (permisos.permiso == true)
                                                                               { %>                                                                  
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Requisito" onclick="if(!confirm('¿Está seguro de que desea eliminar este requisito?')) return false;" href="/evr/Requisitos/eliminar_requisito/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
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
                                       

                                   <%-- <asp:Button ID="btnImprimir" runat="server" class="btn btn-primary run-first" Text="Exportar a excel" />--%>
                                    <a href="/evr/Requisitos/detalle_requisito/0" class="btn btn-primary" title="Nueva Evaluación">Nueva Evaluación</a>
                                    
                                    <% } %></div>

    
    </form>
</asp:Content>
