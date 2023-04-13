<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Indicadores</title>
        <title>Midas-Indicador </title>
    <script type="text/javascript">


        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "btnAddIndicador")
                    $("#hdFormularioEjecutado").val("btnAddIndicador");
                if (val == "ctl00_MainContent_btnImprimir")
                    $("#hdFormularioEjecutado").val("btnImprimir");
                if (val == "ctl00_MainContent_btnImprimirCatalogo")
                    $("#hdFormularioEjecutado").val("btnImprimirCatalogo");

            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });


        });

        
       
    </script>
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

            for (int i = 2008; i <= DateTime.Now.Year; i++)
            {
                ListItem itemAnio = new ListItem();
                itemAnio.Value = i.ToString();
                itemAnio.Text = i.ToString();
                ddlAnio.Items.Insert(0, itemAnio);
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
            
            if (ViewData["indicadores"] != null)
            {
                DatosPedidos.DataSource = ViewData["indicadores"];
                DatosPedidos.DataBind();
            }

            if (Session["EditarIndicadoresResultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarIndicadoresResultado"].ToString() + "' });", true);
                Session.Remove("EditarIndicadoresResultado");
            }

            if (Session["EditarIndicadoresError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarIndicadoresError"].ToString() + "' });", true);
                Session.Remove("EditarIndicadoresError");
            } 

        }        

    }
</script>
    <br />
    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <table width="100%">
    <tr>
        <td style="width:50%">
    <h3>Gestión de Indicadores</h3>
    </td>
        <td style="width:50%; padding-bottom:10px">
            <div style="text-align:right">
                                      <asp:Button ID="btnImprimirCatalogo" runat="server" class="btn btn-primary run-first" style="height:34px" Text="Catálogo Indicadores" />
                                      <a class="btn btn-primary run-first" href="/evr/indicadores/parametros"><i class="icon-cog"></i> <asp:Literal ID="Literal4" runat="server" Text="Parámetros" /></a>
                                    </div>
        </td>
        </tr>
    </table>

    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    
    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-signup"></i>Indicadores</h6>
                        </div>

                            <asp:GridView ID="DatosPedidos" runat="server" Visible="false">
                            </asp:GridView>

                        <div class="panel-body">

                        <div style="width:100%" class="datatablePedido">
									<center>
                                    <table class="table table-bordered">
					                    <thead>
					                        <tr> 
					                            <th>Indicador</th>    
                                                <th style="width:45px">Unidad</th>                                                                                 
                                                <th style="width:45px">V.Referencia</th>
                                                <th style="width:45px">V.Operación</th>   
                                                 <% if (permisos.permiso == true)
                                                    { %>
					                            <th style="width:45px">Editar</th>
                                                <%} %>
					                        </tr>
					                    </thead>
					                    <tbody>
                                            <% 
                                                foreach (GridViewRow item in DatosPedidos.Rows)
                                                     { %>
                                            <tr>
                                                <td class="task-desc">
                                                    <%= item.Cells[1].Text %>
                                                </td>         
                                                <td class="task-desc">
                                                    <%= item.Cells[10].Text %>
                                                </td>                                                 
                                                <% 
                                                   MIDAS.Models.VISTA_UltimasMediciones ultmed = MIDAS.Models.Datos.obtenerUltimasMediciones(int.Parse(item.Cells[0].Text), DateTime.Now.Year, centroseleccionado.id);
                                                     %>                                                 
                                                <td class="task-desc">
                                                <center>
                                                <% if (ultmed != null && ultmed.ultimovalorRef != null)
                                                   { %>
                                                    <%= ultmed.ultimovalorRef%>
                                                    <% } %>
                                                </center>
                                                </td>    
                                                <% if (ultmed != null && ultmed.ultimovalorRef != null && ultmed.ultimovalorCalc != null)
                                                   {
                                                       if ((ultmed.ultimovalorCalc < ultmed.ultimovalorRef && ultmed.tendencia == 0) || (ultmed.ultimovalorCalc > ultmed.ultimovalorRef && ultmed.tendencia == 1))
                                                       { %>
                                                <td style="background-color:Red; color:White" class="task-desc">
                                                <center>
                                                <% if (ultmed != null && ultmed.ultimovalorCalc != null)
                                                   { %>
                                                    <%= ultmed.ultimovalorCalc%>
                                                    <% } %>
                                                    </center>
                                                </td>          
                                                <% 
                                                        }
                                                       else
                                                       {
                                                           %>
                                                               <td style="background-color:Green; color:White" class="task-desc">
                                                               <center>
                                                <% if (ultmed != null && ultmed.ultimovalorCalc != null)
                                                   { %>
                                                    <%= ultmed.ultimovalorCalc%>
                                                    <% } %>
                                                    </center>
                                                </td> 
                                                               <%
                                                       }
                                                      }
                                                   else
                                                   { %>   
                                                   <td class="task-desc">
                                                   <center>
                                                   <% if (ultmed != null && ultmed.ultimovalorCalc != null)
                                                      { %>
                                                    <%= ultmed.ultimovalorCalc%>
                                                    <% } %>
                                                    </center>
                                                </td>         
                                                  <%} %>  
                                                <% if (permisos.permiso == true)
                                                   {
                                                       %>
                                                <td class="text-center">
                                                    <a href="/evr/indicadores/detalle_indicador/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-pencil"></i></a>
                                                </td>
                                                <% 
                                                   } %>
                                            </tr>
                                            <% }%>
					                            
					                    </tbody>
					                </table>
							        </center>
                                    </div>
                                 </div>      
                                 <br />                                                                                                
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
                                            </table>
                                    </div>

    </form>
</asp:Content>
