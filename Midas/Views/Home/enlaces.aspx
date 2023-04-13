<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Enlaces de interés</title>

</asp:Content>
<asp:Content ID="principalContent" ContentPlaceHolderID="MainContent" runat="server">
<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());            
        }
        if (Session["CentralElegida"] != null)
        {
            centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
        }  
        
        //INSPECCIONES
        DatosPedidos.DataSource = ViewData["enlaces"];
        DatosPedidos.DataBind();

    }
</script>
    <br />    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    

    <h2>Links</h2>
    <br />
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
    </asp:GridView>
    <div class="block">
        <div class="datatablePedido">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>
                            Título     
                        </th>  
                        <th style="width:200px">
                            URL     
                        </th>     
                        <th style="width:200px">
                            Ámbito     
                        </th>                       
                        <th style="width:50px">
                        <center>
                            Fichero
                            </center>
                        </th>
                        <% if (user.perfil == 1 && centroseleccionado.tipo == 4)
                           { %>
                           <th  style="width:50px">
                        <center>
                            Editar
                            </center>
                        </th>
                        <th  style="width:50px">
                        <center>
                            Borrar
                            </center>
                        </th>
                        <% } %>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (GridViewRow item in DatosPedidos.Rows)
                       { %>
                    <tr>                        
                        <td class="text">
     
                            <%=  
                                item.Cells[1].Text
                                
                                %>
 
                        </td>   
                        <td class="text">
                            <% if (item.Cells[2].Text != "&nbsp;")
                               {  %>

                                    <% if (item.Cells[2].Text.Contains("http"))
                                       { %>
                                        <a target="_blank" href="<%=item.Cells[2].Text%>"><%=item.Cells[2].Text%></a>
                                    <% }
                                       else
                                       { %>
                                        <a target="_blank" href="http://<%=item.Cells[2].Text%>"><%=item.Cells[2].Text%></a>
                                    <% } %>
                            <% } %>
                        </td>   
                        <td class="text">
                            <center>

                                <% if (item.Cells[3].Text != "&nbsp;")
                                   {  %>
                                        <%=item.Cells[3].Text%>
                                <%
                                    }
                                   else
                                   { %>
                                        General
                                <% } %>
                            </center>
                        </td>                    
                        <td style="width:50px" class="text-center">
                        <center>
                        <%if (item.Cells[4].Text != "&nbsp;")
                          { %> 
                            <a title="Ver Fichero" href="/evr/Home/ObtenerEnlace/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                            <% } %>
                            </center>
                        </td>      
                        <% if (user.perfil == 1 && centroseleccionado.tipo == 4)
                           { %>
                        <td style="width:50px" class="text-center">
                        <center>
                            <a title="Editar Enlace" href="/evr/Home/detalle_enlace/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                            </center>
                        </td>    
                        <td style="width:50px" class="text-center">
                        <center>
                            <a title="Eliminar Enlace" href="/evr/Home/Eliminar_enlace/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                            </center>
                        </td>                     
                        <% } %>
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
    </div>

    <div class="form-actions text-right">
        <% if (user.perfil == 1 && centroseleccionado.tipo == 4)
                           { %>
        <a href="/evr/Home/detalle_enlace/0" class="btn btn-primary" title="Nuevo enlace">Nuevo link</a>   
        <% } %>
    </div>
   
    </form>
</asp:Content>
