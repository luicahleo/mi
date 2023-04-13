<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Utilidades</title>

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
        DatosPedidos.DataSource = ViewData["ficherosAdjuntos"];
        DatosPedidos.DataBind();

        if (Session["error"] != null)
        {
            lblError.Visible = true;
            Session.Remove("error");
        }


    }
</script>
    <br />    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <% if (user.perfil == 1 && centroseleccionado.tipo == 4)
       { %>
       <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-upload"></i>Subir ficheros</h6>
        </div>
        <div class="panel-body">
    <center>
    <br />
    <input type="file" id="file" name="file" />
    <br />
    <div class="form-group">
        <label>Nombre</label>
        <asp:TextBox ID="txtNombre" Width="50%" runat="server" class="form-control" ></asp:TextBox>
    </div>
    <asp:Label runat="server"  Visible="false" id="lblError" ForeColor="Red">Adjunte un fichero e indique su nombre.</asp:Label><br />
    <input id="subir" type="submit" class="btn btn-primary run-first" value="Subir" />

    </center>
    <br />
    </div>

    </div>

    <% } %>
    <h2>Utilidades</h2>
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
    </asp:GridView>
    <div class="block">
        <div class="datatablePedido">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>

                            Nombre del fichero
      
                        </th>                        
                        <th  style="width:50px">
                        <center>
                            Enlace
                            </center>
                        </th>
                        <% if (user.perfil == 1 && centroseleccionado.tipo == 4)
                           { %>
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
                        <td style="width:50px" class="text-center">
                        <center>
                            <a title="Ver Fichero" href="/evr/Home/ObtenerFichero/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                            </center>
                        </td>      
                        <% if (user.perfil == 1)
                           { %>
                        <td style="width:50px" class="text-center">
                        <center>
                            <a title="Eliminar Fichero" href="/evr/Home/Eliminar_Fichero/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                            </center>
                        </td>                     
                        <% } %>
                    </tr>
                    <% } %>
                </tbody>
            </table>
        </div>
    </div>

   
    </form>
</asp:Content>
