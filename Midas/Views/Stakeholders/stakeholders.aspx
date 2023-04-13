<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Partes interesadas</title>
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

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            if (Session["CentralElegida"] != null)
            {
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            } 

            if (user.perfil == 2)
                permisos = MIDAS.Models.Datos.ObtenerPermisos(user.idUsuario, centroseleccionado.id);
            else
            {
                permisos.idusuario = user.idUsuario;
                permisos.idcentro = centroseleccionado.id;
                permisos.permiso = true;
            } 

            if (ViewData["stakeholdersN4"] != null)
            {
                grdN3.DataSource = ViewData["stakeholdersN4"];
                grdN3.DataBind();
            }

            if (Session["EditarStakeholder4Resultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarStakeholder4Resultado"].ToString() + "' });", true);
                Session.Remove("EditarStakeholder4Resultado");
            }

            if (Session["EditarStakeholder4Error"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarStakeholder4Error"].ToString() + "' });", true);
                Session.Remove("EditarStakeholder4Error");
            } 

        }        

    }
</script>
    <br />    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <h3>Gestión de partes interesadas</h3>
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    
    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-globe"></i>Partes interesadas</h6>
                        </div>
                        <div class="panel-body">
									<center>
                                    <table width="100%">
                                        <tr>
                                            <td style="width:100%">
                                                <div style="text-align:right">
                                                    <asp:Button ID="btnImprimirCatalogo" runat="server" class="btn btn-primary run-first" Text="Catálogo Partes Interesadas" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <table width="50%">
                                        <tr>
                                            <td style="width:33%">
                                                <center>
                                                <input id="btnN3" style="width:180px" type="button" value="Nivel IV" class="btn btn-primary run-first"/></center>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table  width="100%" >
                                        <tr>
                                            <td colspan="4">

                                                <div runat="server" id="divN3">
								                    <asp:GridView ID="grdN3" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th>
                                                                                Nivel III      
                                                                            </th> 
                                                                            <th>
                                                                                Denominación      
                                                                            </th>                                                                                                                                                
                                                                            <% if ((permisos.permiso == true))
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Editar
                                                                            </th>
                                                                            <% } %>
                                                                            <% if ((permisos.permiso == true))
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Borrar
                                                                            </th>
                                                                            <% } %>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <% 
                                                                        foreach (GridViewRow item in grdN3.Rows)
                                                                           { %>
                                                                        <tr>         
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
                                                                            
                                                                            <% if (permisos.permiso == true)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/stakeholders/editar_stakeholdern4/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% } %>  
                                                                            <% if (permisos.permiso == true)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Nivel" onclick="if(!confirm('¿Está seguro de que desea eliminar esta parte interesada?')) return false;" href="/evr/stakeholders/eliminar_stakeholdern4/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>     
                                                                            <% } %>                
                                                                            <% } %>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        
                                                        <div style="float:right; margin-bottom:15px">
                                                        <% if (permisos.permiso == true)
                                                           { %>
                                                        <a href="/evr/stakeholders/editar_stakeholdern4/0"  class="btn btn-primary" title="Nueva Parte Interesada">Nueva Parte Interesada</a>
                                                        
                                                        <% } %></div>
                                                 </div>
                                            </td>                                            
                                        </tr>
                                    </table>
							        </center>
                                 </div>                                 
        </div>     
    </form>
</asp:Content>
