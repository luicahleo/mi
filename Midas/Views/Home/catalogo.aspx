<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Catálogo</title>

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
        DatosPedidos.DataSource = ViewData["catalogonormas"];
        DatosPedidos.DataBind();

        if (Session["EdicionNormaMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionNormaMensaje"].ToString() + "' });", true);
            Session["EdicionNormaMensaje"] = null;
        }
        if (Session["EdicionNormaError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionNormaError"].ToString() + "' });", true);
            Session["EdicionNormaError"] = null;
        }

    }
</script>
    <br />    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    

    <h2>Normas</h2>
    <br />

    <label>Estas Normas y documentos forman parte de la biblioteca de Endesa, y solo está permitido su uso en este ámbito</label>

    <br /><br />
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
    </asp:GridView>
    <div class="block">
        <div class="datatablePedido">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th  style="width:175px">
                            Código      
                        </th>  
                        <th>
                            Descripción     
                        </th>     
                        <th>
                            Edición     
                        </th>   
                        <th style="width:45px">
                            Descargas      
                        </th>                     
                        <th  style="width:50px">
                        <center>
                            Enlace
                            </center>
                        </th>
                        <% if (user.perfil == 1)
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
                            <%=  
                                item.Cells[2].Text
                                
                                %>
                        </td>   
                        <td class="text">
                            <center>
                            <%=  
                                item.Cells[3].Text
                                
                                %>
                            </center>
                        </td> 
                        <td class="text">
                            <center>
                            <%=  
                                item.Cells[4].Text
                                
                                %>
                            </center>
                        </td>                    
                        <td style="width:50px" class="text-center">
                        <center>
                        <%if (item.Cells[5].Text != "&nbsp;")
                          { %> 
                            <a title="Ver Fichero" href="/evr/Home/ObtenerNorma/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                            <% } %>
                            </center>
                        </td>      
                        <% if (user.perfil == 1)
                           { %>
                        <td style="width:50px" class="text-center">
                            <center>
                            <a title="Editar Norma" href="/evr/Home/detalle_norma/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                            </center>
                        </td>    
                        <td style="width:50px" class="text-center">
                            <center>
                            <a title="Eliminar Norma" href="/evr/Home/Eliminar_Norma/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
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
        <% if (user.perfil == 1)
                           { %>
        <a href="/evr/Home/detalle_norma/0" class="btn btn-primary" title="Nueva Emergencia">Nueva Norma</a>   
        <% } %>
    </div>
   
    </form>
</asp:Content>
