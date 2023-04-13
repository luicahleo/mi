<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Riesgos</title>
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
            
            if (ViewData["riesgos"] != null)
            {
                grdN1.DataSource = ViewData["riesgos"];
                grdN1.DataBind();
            }

            if (Session["EditarRiesgosResultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarRiesgosResultado"].ToString() + "' });", true);
                Session.Remove("EditarRiesgosResultado");
            }

            if (Session["EditarRiesgosError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarRiesgosError"].ToString() + "' });", true);
                Session.Remove("EditarRiesgosError");
            } 

        }        

    }
</script>
    <br />
    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <table style="width:100%">
        <tr>
            <td style="width:50%">
                <h3>Gestión de Riesgos y Oportunidades</h3>
            </td>
            <td style="width:50%; padding-bottom:10px">
                <a href="/evr/riesgos/ObtenerCriterios" style="width:200px; float:right; background-color:#ff0f64; border-color:#ff0f64" class="btn btn-primary" id="A1" title="Descargar criterios de evaluación">Descargar criterios de evaluación</a>                                               
            </td>
        </tr>
    </table>
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    
    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-signup"></i>Riesgos y Oportunidades</h6>
                        </div>
                        <div class="panel-body">
									<center>
                                    <br />
                                    <table  width="100%" >
                                        <tr>
                                            <td colspan="4">
                                                <div runat="server" id="divN1">
								                    <asp:GridView ID="grdN1" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="width:60px">
                                                                                Id      
                                                                            </th> 
                                                                            <th style="width:120px">
                                                                                Tipo     
                                                                            </th> 
                                                                            <th >
                                                                                Descripcion
                                                                            </th>  
                                                                            <th>
                                                                                Vigente
                                                                            </th>
                                                                            <th style="width:120px">
                                                                                Evaluación Oportunidad
                                                                            </th>               
                                                                            <th style="width:140px">
                                                                                Relevancia Riesgo Inherente
                                                                            </th> 
                                                                            <th style="width:140px">
                                                                                Relevancia Riesgo Residual
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
                                                                        <%  
                                                                            if (item.Cells[2].Text == "1")
                                                                            {
                                                                                    %>
                                                                                        Riesgo
                                                                                    <%
                                                                            }
                                                                            else
                                                                            {
                                                                                    %>
                                                                                    Oportunidad
                                                                            <% } %>
 
                                                                            </td>    

                                                                     <td class="text"><%= item.Cells[3].Text%></td>  
                                                                     <td class="text">
                                                                        <%  
                                                                            if (item.Cells[12].Text != "1")
                                                                            {
                                                                                    %>
                                                                                        Sí
                                                                                    <%
                                                                            }
                                                                            else
                                                                            {
                                                                                    %>
                                                                                    No
                                                                            <% } %>
                                                                               </td>
                                                                     <% if (item.Cells[9].Text == "Viable")
                                                                        { %>
                                                                            <td style="background-color:Green; color:White" class="text"><center><%= item.Cells[9].Text%></center></td>  
                                                                     <% }
                                                                        else
                                                                        {
                                                                            if (item.Cells[9].Text == "No Viable")
                                                                            {
                                                                            %>
                                                                            <td style="background-color:Red; color:Black" class="text"><center><%= item.Cells[9].Text%></center></td>  
                                                                     <% 
                                                                            }
                                                                            else
                                                                            {
                                                                                %>
                                                                                    <td class="text"><center><%= item.Cells[9].Text%></center></td>  
                                                                                <%}
                                                                     } %>
                                                                     <% switch (item.Cells[5].Text)
                                                                        {
                                                                            case "BAJO":
                                                                                 %>    
                                                                                <td style="background-color:Green; color:White" class="text"><center><%=item.Cells[5].Text%></center></td>  
                                                                            <% break;
                                                                            case "MEDIO-BAJO":
                                                                                 %>    
                                                                                <td style="background: linear-gradient(to right, yellow , green); color:Black" class="text"><center><%=item.Cells[5].Text%></center></td>  
                                                                            <%
                                                                         break;
                                                                            case "MEDIO":
                                                                                 %>    
                                                                                <td style="background-color:Yellow; color:Black" class="text"><center><%=item.Cells[5].Text%></center></td>  
                                                                            <%
                                                                         break;
                                                                            case "MEDIO-ALTO":
                                                                            %>    
                                                                                <td style="background: linear-gradient(to right, yellow , red); color:Black" class="text"><center><%=item.Cells[5].Text%></center></td>  
                                                                            <%
                                                                         break;
                                                                            case "ALTO":
                                                                            %>    
                                                                                <td style="background-color:Red; color:Black" class="text"><center><%=item.Cells[5].Text%></center></td>  
                                                                            <%
                                                                         break;
                                                                            default:
                                                                               %>
                                                                                <td class="text"><center><%=item.Cells[5].Text%></center></td> 
                                                                                
                                                                                <%
                                                                         break;
                                                                        } %>



                                                                     <% switch (item.Cells[7].Text)
                                                                        {
                                                                            case "BAJO":
                                                                                 %>    
                                                                                <td style="background-color:Green; color:White" class="text"><center><%=item.Cells[7].Text%></center></td>  
                                                                            <% break;
                                                                            case "MEDIO-BAJO":
                                                                                 %>    
                                                                                <td style="background: linear-gradient(to right, yellow , green); color:Black" class="text"><center><%=item.Cells[7].Text%></center></td>  
                                                                            <%
                                                                         break;
                                                                            case "MEDIO":
                                                                                 %>    
                                                                                <td style="background-color:Yellow; color:Black" class="text"><center><%=item.Cells[7].Text%></center></td>  
                                                                            <%
                                                                         break;
                                                                            case "MEDIO-ALTO":
                                                                            %>    
                                                                                <td style="background: linear-gradient(to right, yellow , red); color:Black" class="text"><center><%=item.Cells[7].Text%></center></td>  
                                                                            <%
                                                                         break;
                                                                            case "ALTO":
                                                                            %>    
                                                                                <td style="background-color:Red; color:Black" class="text"><center><%=item.Cells[7].Text%></center></td>  
                                                                            <%
                                                                         break;
                                                                            default:
                                                                               %>
                                                                                <td class="text"><center><%=item.Cells[7].Text%></center></td> 
                                                                                
                                                                                <%
                                                                         break;
                                                                        } %>                                                                      
                                                             
                                                                            <% if (permisos.permiso == true)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/riesgos/detalle_riesgo/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% }
                                                                               else
                                                                               { %>    
                                                                               <td style="width:50px" class="text-center">
                                                                                <a title="Consultar" href="/evr/riesgos/detalle_riesgo/<%=item.Cells[0].Text %>");"><i class="icon-search"></i></a>
                                                                            </td> 
                                                                               <% } %>    
                                                                            <% if (permisos.permiso == true)
                                                                               { %>                                                                  
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Riesgo" onclick="if(!confirm('¿Está seguro de que desea eliminar este riesgo?')) return false;" href="/evr/riesgos/eliminar_riesgo/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
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
                                    <a href="/evr/riesgos/detalle_riesgo/0" class="btn btn-primary" title="Nuevo Riesgo">Nuevo Riesgo</a><% } %>    
                                    </div>
                                    
    </form>
</asp:Content>
