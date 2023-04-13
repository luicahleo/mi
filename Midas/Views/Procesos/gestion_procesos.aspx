<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
        
    protected void Page_Load(object sender, EventArgs e)
    {
        List<MIDAS.Models.procesos> listaPadres = new List<MIDAS.Models.procesos>();
        
        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            if (Session["CentralElegida"] != null)
            {
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                if (ViewData["tecnologias"] != null)
                {                    
                    ddlTecnologia.DataSource = ViewData["tecnologias"];
                    ddlTecnologia.DataValueField = "id";
                    ddlTecnologia.DataTextField = "nombre";
                    ddlTecnologia.DataBind();

                    ListItem general = new ListItem();
                    general.Value = "0";
                    general.Text = "No";
                    ddlTecnologia.Items.Insert(0, general);
                }                
                listaPadres = (List<MIDAS.Models.procesos>)ViewData["padres"];

                ddlDependencia.DataSource = listaPadres;
                ddlDependencia.DataTextField = "nombre";
                ddlDependencia.DataValueField = "id";
                ddlDependencia.DataBind();

                ListItem ninguno = new ListItem();
                ninguno.Value = "0";
                ninguno.Text = "---";
                ddlDependencia.Items.Insert(0, ninguno);

                if (Session["nivel"] != null)
                {
                    ddlNivel.SelectedValue = Session["nivel"].ToString();
                }
                if (Session["tipo"] != null)
                {
                    ddlTipo.SelectedValue = Session["tipo"].ToString();
                }

                if (Session["EdicionGPMensaje"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionGPMensaje"].ToString() + "' });", true);
                    Session["EdicionGPMensaje"] = null;
                }
                if (Session["errorGPMensaje"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["errorGPMensaje"].ToString() + "' });", true);
                    Session["errorGPMensaje"] = null;
                }
            }
        }
    }
</script>

<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <style>
        .Rotate-90
        {
          -webkit-transform: rotate(-90deg);
          -moz-transform: rotate(-90deg);
          -ms-transform: rotate(-90deg);
          -o-transform: rotate(-90deg);
          transform: rotate(-90deg);
 
          -webkit-transform-origin: 50% 50%;
          -moz-transform-origin: 50% 50%;
          -ms-transform-origin: 50% 50%;
          -o-transform-origin: 50% 50%;
          transform-origin: 50% 50%;
 
          font-size: 35px;
          font-color:#41b9e6;
          color:#41b9e6;
          width: 100px;
          position: relative;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divFicha").hide();

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarElemento")
                    $("#hdFormularioEjecutado").val("GuardarElemento");
            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });

            $('#ctl00_MainContent_ddlNivel').change(function () {
                $("#hdFormularioEjecutado").val("CambiaNivel");
            });
        });       
    </script>
    <title>DIMAS</title>
</asp:Content>
<asp:Content ID="principalContent" ContentPlaceHolderID="MainContent" runat="server">
    <form runat="server" id="form1">
    <input id="hdFormularioEjecutado" name ="hdFormularioEjecutado" type="hidden" value="Entro"/>
    <!-- Page header -->
    <div class="page-header">
        <table width="100%">
            <tr>    
                <td align="left">
                    <div>
                    <% var urlActual = Page.Request.Url.AbsoluteUri;
                       if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                        {  %>
                        <table style="margin-top:25px; ">
                            <tr>
                                <td style="padding-left:25px">
                                    <label>Tipo</label>
                                    <asp:DropDownList CssClass="form-control" AutoPostBack="true" ID="ddlTipo" runat="server"> 
                                        <asp:ListItem Value="E" Text="Estratégico"></asp:ListItem>
                                        <asp:ListItem Value="O" Text="Operativo"></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Soporte/Apoyo"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="padding-left:25px">
                                    <label>Nivel</label>
                                    <asp:DropDownList CssClass="form-control" AutoPostBack="true" ID="ddlNivel" runat="server">                      
                                        <asp:ListItem Value="M" Text="Value Chain/Process Area"></asp:ListItem>
                                        <asp:ListItem Value="S" Text="Macroproceso"></asp:ListItem>
                                        <asp:ListItem Value="F" Text="Proceso"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td id="divPadre" style="padding-left:25px">
                                    <label>Dependencia</label>
                                    <asp:DropDownList CssClass="form-control" ID="ddlDependencia" runat="server"> 
                                    </asp:DropDownList>                                   
                                </td>
                                <td style="padding-left:25px">
                                    <label>Específico unidad de negocio</label>
                                    <asp:DropDownList CssClass="form-control" ID="ddlTecnologia" runat="server"> 
                                    </asp:DropDownList>  
                                </td>
                                <td style="padding-left:25px">
                                    <label>Código (*)</label>
                                    <asp:TextBox  CssClass="form-control" ID="txtCodigo" runat="server"></asp:TextBox>
                                </td>  
                                <td style="padding-left:25px">
                                    <label>Nombre (*)</label>
                                    <asp:TextBox  CssClass="form-control" ID="txtNombre" runat="server"></asp:TextBox>
                                </td>                                
                                <td style="padding-left:25px; padding-top:22px">
                                    
                                    <input id="GuardarElemento" type="submit" value="Añadir" class="btn btn-primary run-first">
                                    
                                </td>
                                <td style="padding-left:25px; padding-top:22px; float:right">
									<a  target="_blank" href="/evr/procesos/impresion_procesos/0" class="btn btn-primary" style="float:right;" id="A1" title="Imprimir">Imprimir</a>
								</td>                                
                            </tr>
                        </table> <hr /><% }
                        else
                        { %>
                        <br />
                        <table style="float:right;">
							<tr>
							<td>
							<a  target="_blank" href="/evr/procesos/impresion_procesos/0" class="btn btn-primary" style="float:right;" id="A2" title="Imprimir">Imprimir</a>
							</td>
							</tr>
							</table>
                                   <%} %></div>
                                   </td></tr>
                                   </table>                       
                       <%--e5f6ff--%>
                        <div id="canvas" style="background-color:#FFF; padding:15px; border-radius:25px; ">
                        <table width="100%" style="min-height:155px">
                        <tr>
                            <td style="width:95%">
                            <table width="100%">
                                <tr>                              
                              <% 
                                List<MIDAS.Models.procesos> listaProcesos1 = (List<MIDAS.Models.procesos>)ViewData["procesosE"];
                                List<MIDAS.Models.procesos> listaProcesosHijos = (List<MIDAS.Models.procesos>)ViewData["procesosT"];
                              %>
                                <% 
                                    foreach (MIDAS.Models.procesos proc in listaProcesos1)
                                    {
                                        %>
                                        <%--VALUECHAIN--%>
                                        <% if (proc.nivel == "M")
                                           { %>
                                           <td style="width:<%=100/listaProcesos1.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#0555FA;margin-right:15px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:White;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <% if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                            {
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                                <% 
                                               List<MIDAS.Models.procesos> listaHijos2 = listaProcesosHijos.Where(x=>x.padre == proc.id).ToList();
                                               if (listaHijos2.Count > 0)
                                               { %>
                                               <tr>
                                                   <td>
                                                   <table>
                                                    <% foreach (MIDAS.Models.procesos procHijo in listaHijos2)
                                                        {
                                                    %> 
                                                    <tr>
                                                        <td style="width:50%">
                                                        <center>
                                                                <div style="border-radius: 15px; background-color:#ede6e8; min-height:70px ; margin-right:15px; margin-top:10px; padding:10px;  padding-top:10px; width:90%">
                                                                <table width="100%" style="padding:25px;">
                                                                <tr>
                                                                    <td  style="color:#FFFFFF;text-align:left; width:100%; padding-left:10px">
                                                                    <% if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                                        {
                                                                     %>
                                                                         <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=procHijo.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                                         <% } %>
                                                                        <a style="text-decoration:none; color:Black; margin-left:5px" href="/evr/procesos/detalle_proceso/<%= procHijo.id %>">
                                                                        <%= procHijo.cod_proceso + " - " + procHijo.nombre%>
                                                                        </a>
                                                                    </td>
                                                                </tr>
                                                                 <% 
                                                                   List<MIDAS.Models.procesos> listaHijosSub2 = listaProcesosHijos.Where(x=>x.padre == procHijo.id).ToList();
                                                                   if (listaHijosSub2.Count > 0)
                                                                   { %>
                                                                    <tr>
                                                                       <td style="padding-top:10px;" width="100%">

                                                                        <%
                                                                            foreach (MIDAS.Models.procesos procHijoSub in listaHijosSub2)
                                                                            {
                                                                                %>
                                                                                    <div style="border-radius: 15px;background-color:#41b9e6; min-height:70px; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                                                                    <%
                                                                                        if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                                                        {
                                                                                     %>
                                                                                     <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=procHijoSub.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                                                     <% } %>
                                                                                     <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= procHijoSub.id %>">
                                                                                     <%= procHijoSub.cod_proceso + " - " + procHijoSub.nombre%></a></div> <br />
                                                                                 <%   } %> 
                                                                        </td>
                                                                        </tr>
                                                                        <% } %>
                                                                        </table></div>
                                                                        </center>
                                                        </td>
                                                        </tr>
                                                    <% } %>
                                                    </table>
                                                    </td></tr></table></center></div>
                                                    </td>
                                               <%
                                               } %>
                                           <% } %>
                                           
                                           <%--MACROPROCESO--%>
                                           <% if (proc.nivel == "S")
                                           { %>
                                           <td style="width:<%=100/listaProcesos1.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#ede6e8;margin-right:15px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:Black;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <%
                                                            if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                            {
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                                <% 
                                               List<MIDAS.Models.procesos> listaHijos2 = listaProcesosHijos.Where(x=>x.padre == proc.id).ToList();
                                               if (listaHijos2.Count > 0)
                                               { %>
                                                <tr>
                                                   <td>
                                                   <table width="100%">
                                                    <%
                                                        int countLinea = 1;
                                                        foreach (MIDAS.Models.procesos procHijo in listaHijos2)
                                                        {
                                                    %> 

                                                       <tr>

                                                        <td style="width:50%">
                                                        <center>
                                                                <div style="border-radius: 15px; background-color:#41b9e6; min-height:70px; margin-right:15px; margin-top:10px; padding:10px;  padding-top:10px; width:90%">
                                                                <table width="100%" style="padding:25px;">
                                                                <tr>
                                                                    <td  style="color:#FFFFFF;text-align:left; width:100%; padding-left:10px">
                                                                    <%
                                                                        if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                                        {
                                                      %>
                                                                        <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=procHijo.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                                        <% } %>
                                                                        <a style="text-decoration:none; color:Black; margin-left:5px" href="/evr/procesos/detalle_proceso/<%= procHijo.id %>">
                                                                        <%= procHijo.cod_proceso + " - " + procHijo.nombre%>
                                                                        </a>
                                                                    </td>
                                                                </tr>                                                                
                                                                </table>
                                                                </div></center>
                                                        </td>

                                                            </tr>
                                                    <% } %>
                        
                                                    </table></center></div>
                                               <%
                                               } %>
                                               </td>

                                           <% } %>

                                           <%--PROCESO--%>
                                           <% if (proc.nivel == "F")
                                           { %>
                                           <td style="width:<%=100/listaProcesos1.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#41b9e6;margin-right:15px; min-height:70px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:Black;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <%
                                                            if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                            {
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                                    </table></center></div>

                                                </td>
                                               <%
                                               } %>
                                               
                                           <% } %>
                                        </tr></table></td>
                            <td>
                             <div class="Rotate-90" style="top:1px; font-size:x-large">Estratégicos</div>
                             </td></tr></table>
                             <%--FIN PROCESOS ESTRATEGICOS--%>
                                  <hr style="border-color:#41b9e6;" />

                            <%--INICIO PROCESOS OPERATIVOS--%>
                        <table width="100%" style="min-height:155px">
                        <tr>
                            <td style="width:95%">
                            <table width="100%">
                                <tr>
                              
<% 
                                List<MIDAS.Models.procesos> listaProcesos2 = (List<MIDAS.Models.procesos>)ViewData["procesosO"];
                              %>
                                <% 
                                    foreach (MIDAS.Models.procesos proc in listaProcesos2)
                                    {
                                        %>
                                        <%--VALUECHAIN--%>
                                        <% if (proc.nivel == "M")
                                           { %>
                                           <td style="width:<%=100/listaProcesos2.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#0555FA;margin-right:15px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:White;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <%
                                                            if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                            {
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                                <% 
                                               List<MIDAS.Models.procesos> listaHijos2 = listaProcesosHijos.Where(x=>x.padre == proc.id).ToList();
                                               if (listaHijos2.Count > 0)
                                               { %>
                                               <tr>
                                                   <td>
                                                   <table>
                                                   <tr>
                                                    <%
                                                        foreach (MIDAS.Models.procesos procHijo in listaHijos2)
                                                        {
                                                    %> 
                                                    
                                                        <td style="width:<%= 100/listaHijos2.Count %>%">
                                                        <center>
                                                                <div style="border-radius: 15px; min-height:70px; background-color:#ede6e8; margin-right:15px; margin-top:10px; padding:10px;  padding-top:10px; width:90%">
                                                                <table width="100%" style="padding:25px;">
                                                                <tr>
                                                                    <td  style="color:#FFFFFF;text-align:left; width:100%; padding-left:10px">
                                                                    <%
                                                                        if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                                        {
                                                      %>
                                                                        <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=procHijo.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                                        <% } %>
                                                                        <a style="text-decoration:none; color:Black; margin-left:5px" href="/evr/procesos/detalle_proceso/<%= procHijo.id %>">
                                                                        <%= procHijo.cod_proceso + " - " + procHijo.nombre%>
                                                                        </a>
                                                                    </td>
                                                                </tr>
                                                                 <% 
                                                                   List<MIDAS.Models.procesos> listaHijosSub2 = listaProcesosHijos.Where(x=>x.padre == procHijo.id).ToList();
                                                                   if (listaHijosSub2.Count > 0)
                                                                   { %>
                                                                    <tr>
                                                                       <td style="padding-top:10px;" width="100%">

                                                                        <%
                                                                            foreach (MIDAS.Models.procesos procHijoSub in listaHijosSub2)
                                                                            {
                                                                                %>
                                                                                    <div style="border-radius: 15px; min-height:70px;background-color:#41b9e6; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                                                                     <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= procHijoSub.id %>">
                                                                                     <%= procHijoSub.cod_proceso + " - " + procHijoSub.nombre%></a></div> <br />
                                                                                 <%   
                                                                            }
                                                                        %> 

                                                                        </td>
                                                                        </tr>
                                                                        <% } %>
                                                                        </table></div>
                                                                        </center>
                                                        </td>
                                                       
                                                    <% } %> </tr>
                                                    </table>
                                                    </td></tr></table></center></div>
                                                    </td>
                                               <%
                                               } %>
                                           <% } %>
                                           
                                           <%--MACROPROCESO--%>
                                           <% if (proc.nivel == "S")
                                           { %>
                                           <td style="width:<%=100/listaProcesos2.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#ede6e8; min-height:70px;margin-right:15px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:Black;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <%
                                                            if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                            {
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                                <% 
                                               List<MIDAS.Models.procesos> listaHijos2 = listaProcesosHijos.Where(x=>x.padre == proc.id).ToList();
                                               if (listaHijos2.Count > 0)
                                               { %>
                                                <tr>
                                                   <td>
                                                   <table>
                                                    <%
                                                        int countLinea = 1;
                                                        foreach (MIDAS.Models.procesos procHijo in listaHijos2)
                                                        {
                                                    %> 

                                                       <tr>

                                                        <td style="width:50%">
                                                        <center>
                                                                <div style="border-radius: 15px; background-color:#41b9e6; min-height:70px; margin-right:15px; margin-top:10px; padding:10px;  padding-top:10px; width:90%">
                                                                <table width="100%" style="padding:25px;">
                                                                <tr>
                                                                    <td  style="color:#FFFFFF;text-align:left; width:100%; padding-left:10px">
                                                                    <%
                                                                        if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                                        {
                                                      %>
                                                                        <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=procHijo.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                                        <% } %>
                                                                        <a style="text-decoration:none; color:Black;margin-left:5px" href="/evr/procesos/detalle_proceso/<%= procHijo.id %>">
                                                                        <%= procHijo.cod_proceso + " - " + procHijo.nombre%>
                                                                        </a>
                                                                    </td>
                                                                </tr>                                                                
                                                                </table>
                                                                </div></center>
                                                        </td>

                                                            </tr>
                                                    <% } %>
                        
                                                    </table></center></div>
                                               <%
                                               } %>
                                               </td>

                                           <% } %>

                                           <%--PROCESO--%>
                                           <% if (proc.nivel == "F")
                                           { %>
                                           <td style="width:<%=100/listaProcesos2.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#41b9e6;margin-right:15px; min-height:70px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:Black;text-align:left; padding-left:10px">
                                                    <%
                                                            if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                            {
                                                      %>
                                                    <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                    <% } %>
                                                   <a style="text-decoration:none; color:Black;margin-left:5px" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <%
                                                            if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                            {
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                                    </table></center></div>

                                                </td>
                                               <%
                                               } %>
                                               
                                           <% } %>
                                        </tr></table></td>
                            <td>
                             <div class="Rotate-90" style="top:1px; font-size:x-large">Operativos</div>
                             </td></tr></table>

                                   <hr  style="border-color:#41b9e6;" />

                        <table width="100%" style="min-height:155px">
                        <tr>
                            <td style="width:95%">
                            <table width="100%">
                                <tr>
                              
<% 
                                List<MIDAS.Models.procesos> listaProcesos3 = (List<MIDAS.Models.procesos>)ViewData["procesosS"];
                              %>
                                <% 
                                    foreach (MIDAS.Models.procesos proc in listaProcesos3)
                                    {
                                        %>
                                        <%--VALUECHAIN--%>
                                        <% if (proc.nivel == "M")
                                           { %>
                                           <td style="100%">
                                                <div style="border-radius: 15px; background-color:#0555FA;margin-right:15px; margin-top:15px ; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:White;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <%
                                                            if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                            {
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                                <% 
                                               List<MIDAS.Models.procesos> listaHijos2 = listaProcesosHijos.Where(x=>x.padre == proc.id).ToList();
                                               if (listaHijos2.Count > 0)
                                               { %>
                                               <tr>
                                                   <td>
                                                   <table width="100%"><tr>
                                                    <%
                                                        foreach (MIDAS.Models.procesos procHijo in listaHijos2)
                                                        {
                                                    %> 
                                                    
                                                        <td style="width:<%= 100/listaHijos2.Count %>%">
                                                        <center>
                                                                <div style="border-radius: 15px; background-color:#ede6e8; min-height:70px; margin-right:15px; margin-top:10px; padding:10px;  padding-top:10px; width:90%">
                                                                <table width="100%" style="padding:25px;">
                                                                <tr>
                                                                    <td  style="color:#FFFFFF;text-align:left; width:100%; padding-left:10px">
                                                                    <%
                                                            if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                            {
                                                      %>
                                                                        <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=procHijo.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                                        <% } %>
                                                                        <a style="text-decoration:none; color:Black;margin-left:5px" href="/evr/procesos/detalle_proceso/<%= procHijo.id %>">
                                                                        <%= procHijo.cod_proceso + " - " + procHijo.nombre%>
                                                                        </a>
                                                                    </td>
                                                                </tr>
                                                                 <% 
                                                                   List<MIDAS.Models.procesos> listaHijosSub2 = listaProcesosHijos.Where(x=>x.padre == procHijo.id).ToList();
                                                                   if (listaHijosSub2.Count > 0)
                                                                   { %>
                                                                    <tr>
                                                                       <td style="padding-top:10px;" width="100%">

                                                                        <%
                                                                            foreach (MIDAS.Models.procesos procHijoSub in listaHijosSub2)
                                                                            {
                                                                                %>
                                                                                    <div style="border-radius: 15px; min-height:70px;background-color:#41b9e6; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                                                                    
                                                                                     <a style="text-decoration:none; color:#000" href="/evr/procesos/detalle_proceso/<%= procHijoSub.id %>">
                                                                                     <%= procHijoSub.cod_proceso + " - " + procHijoSub.nombre%></a></div> <br />
                                                                                 <%   
                                                                            }
                                                                        %> 

                                                                        </td>
                                                                        </tr>
                                                                        <% } %>
                                                                        </table></div>
                                                                        </center>
                                                        </td>
                                                        
                                                    <% } %></tr>
                                                    </table>
                                                    </td></tr></table></center></div>
                                                    </td>
                                               <%
                                               } %>
                                               </tr></tr>
                                           <% } %>
                                           
                                           <%--MACROPROCESO--%>
                                           <% if (proc.nivel == "S")
                                           { %>
                                           <td style="width:<%=100/listaProcesos3.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#ede6e8;margin-right:15px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:Black;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <%
                                                            if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                            {
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                                <% 
                                               List<MIDAS.Models.procesos> listaHijos2 = listaProcesosHijos.Where(x=>x.padre == proc.id).ToList();
                                               if (listaHijos2.Count > 0)
                                               { %>
                                                <tr>
                                                   <td>
                                                   <table>
                                                    <%
                                                        int countLinea = 1;
                                                        foreach (MIDAS.Models.procesos procHijo in listaHijos2)
                                                        {
                                                    %> 

                                                       <tr>

                                                        <td style="width:50%">
                                                        <center>
                                                                <div style="border-radius: 15px; background-color:#41b9e6; margin-right:15px; margin-top:10px; padding:10px;  padding-top:10px; width:90%">
                                                                <table width="100%" style="padding:25px;">
                                                                <tr>
                                                                    <td  style="color:#FFFFFF;text-align:left; width:100%; padding-left:10px">
                                                                        <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= procHijo.id %>">
                                                                        <%= procHijo.cod_proceso + " - " + procHijo.nombre%>
                                                                        </a>
                                                                    </td>
                                                                </tr>                                                                
                                                                </table>
                                                                </div></center>
                                                        </td>

                                                            </tr>
                                                    <% } %>
                        
                                                    </table></center></div>
                                               <%
                                               } %>
                                               </td>

                                           <% } %>

                                           <%--PROCESO--%>
                                           <% if (proc.nivel == "F")
                                           { %>
                                           <td style="width:<%=100/listaProcesos3.Count  %>%">
                                                <div style="border-radius: 15px; background-color:#41b9e6;margin-right:15px; padding:10px;  padding-top:10px"><center>
                                                <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:Black;text-align:left; padding-left:10px">
                                                   <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <%
                                                            if (user.perfil == 1 && centroseleccionado.tipo == 4 && !urlActual.Contains("/0"))
                                                            {
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                                    </table></center></div>

                                                </td>
                                               <%
                                               } %>
                                               
                                           <% } %>
                                        </tr></table></td>
                            <td>
                             <div class="Rotate-90" style="top:1px; font-size:x-large">Soporte</div>
                             </td></tr></table>
                    </div>


                    </div>
</form>

    <!-- /page header -->
    <!-- Page tabs -->
   
        

   
    

    <!-- Footer -->
<%--    <div class="footer clearfix">
        <div class="pull-left">
            </div>
    </div>--%>
    <!-- /footer -->
</asp:Content>
