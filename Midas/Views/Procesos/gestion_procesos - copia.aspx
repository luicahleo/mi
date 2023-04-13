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
                    <%
                        if (user.perfil == 1 && centroseleccionado.tipo == 4)
                        {                                    
                                     %>
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
                                    <label>Específico</label>
                                    <asp:DropDownList CssClass="form-control" ID="ddlTecnologia" runat="server"> 
                                    </asp:DropDownList>  
                                </td>
                                <td style="padding-left:25px">
                                    <label>Código</label>
                                    <asp:TextBox  CssClass="form-control" ID="txtCodigo" runat="server"></asp:TextBox>
                                </td>  
                                <td style="padding-left:25px">
                                    <label>Nombre</label>
                                    <asp:TextBox  CssClass="form-control" ID="txtNombre" runat="server"></asp:TextBox>
                                </td>                                
                                <td style="padding-left:25px; padding-top:22px">
                                    
                                    <input id="GuardarElemento" type="submit" value="Añadir" class="btn btn-primary run-first">
                                    
                                </td>
                                <td style="padding-left:25px; padding-top:22px; float:right">
									<a  target="_blank" href="/evr/Procesos/impresion_procesos/0" class="btn btn-primary" style="float:right;" id="A1" title="Imprimir">Imprimir</a>
								</td>
                                
                            </tr>
                        </table> <hr /><% }
                        else
                        { %>
                        <br />
                        <table style="float:right;">
							<tr>
							<td>
							<a  target="_blank" href="/evr/Procesos/impresion_procesos/0" class="btn btn-primary" style="float:right;" id="A2" title="Imprimir">Imprimir</a>
							</td>
							</tr>
							</table>

                                   <%} %>
                       
                       <%--e5f6ff--%>
                        <div id="canvas" style="background-color:#FFF; padding:15px; border-radius:25px;">
                        <table width="100%" style="min-height:155px">
                        <tr>
                            <td style="width:90%">
                            <table width="100%">
                                <tr>
                              
                            <% 
                                List<MIDAS.Models.procesos> listaProcesos = (List<MIDAS.Models.procesos>)ViewData["procesosE"];
                                List<MIDAS.Models.procesos> listahijos = new List<MIDAS.Models.procesos>();

                                int count1 = 0;
                                foreach (MIDAS.Models.procesos proc in listaProcesos)
                                {
                                    if (proc.padre == null)
                                    count1 = count1 + 1;
                                    if (proc.nivel == "S" && proc.padre == null)
                                    {
                                        if (count1 == 5)
                                            {
                                                %>
                                                    <tr>
                                                    
                                                <%
                                                count1 = 0;
                                            }
                                        
                                        %>
                                        <td width="25%">

                                            <% 
                                                 listahijos = listaProcesos.Where(x => x.padre == proc.id).ToList();
                                                 if (listahijos.Count < 1)
                                                 { %>
                                            <div style="border-radius: 15px; background-color:#FFCC99;margin-right:15px; padding:25px; padding-top:10px; width:90%">
                                            <center>
                                            <table width="100%">
                                                <tr>
                                                    <td style="text-align:left; width:70%">
                                                        <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                         <%= proc.cod_proceso + " - " + proc.nombre%></a>
                                                    </td>
                                                    <td  style="text-align:right">
                                                     <div> 
                                                     <%
                                                     if (user.perfil == 1 && centroseleccionado.tipo == 4)
                                                     {
                                                      %>
                                                     <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                     <% } %>
                                                     </div>
                                                    </td>
                                                </tr>
                                            </table>
                                           
                                            </center>
                                            <br />
                                            </div>
                                            <% }
                                                 else
                                                 { %>
                                             <div style="border-radius: 15px; border: 0.1px solid #ffcc99; background-color:#FFCC99; width:90%">
                                            <table style="border-radius: 15px;background-color:#FFCC99; width: 99%;margin-left: 2.4px;height: 99%;margin-bottom: 2px;">                                            
                                            <tr>
                                            <td>
                                            <center>
                                            <br />
                                            <%--<a style="text-decoration:none; color:#0555FA;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">--%>
                                            <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:left; width:70%; padding-left:10px; padding-left:10px">
                                                        <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                         <%= proc.cod_proceso + " - " + proc.nombre%>
                                                         </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div> 
                                                     <%
                                                     if (user.perfil == 1 && centroseleccionado.tipo == 4)
                                                     {
                                                      %>
                                                     <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                     <% } %>
                                                     </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            </center>                                           
                                            <br />
                                            </td>
                                           </tr>
                                           <tr style="border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;background-color:#FFFFFF">
                                           <td style="padding-left:10px; border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;">
                                           <br />
                                            <%
                                                     foreach (MIDAS.Models.procesos prochijo in listahijos)
                                                     {
                          
                                                 %>
                                                 <div style="border-radius: 15px;background-color:#41b9e6; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                                         <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= prochijo.id %>">
                                                         <%= prochijo.cod_proceso + " - " + prochijo.nombre%></a></div> <br />

                                                 <% } %>
                                        <br />
                                          </td> </tr>
                                            </table>        </div>                   

                                            <% } %>
                                            <br />


                                            </td> 
                                        <%
                                             if (count1 == 5)
                                            {
                                                %>
                                                    </tr>
                                                    
                                                <%
                                           
                                            }
                                    }
                                    else
                                    {

                                        if (proc.nivel == "M")
                                        {
                                            if (count1 == 5)
                                            {
                                                %>
                                                    <tr>
                                                    
                                                <%
                                            count1 = 0;
                                            }
                                 %>
                                    <td width="25%">
                                    <div style="border-radius: 15px; background-color:#0555FA;margin-right:15px; padding:10px;  padding-top:10px; width:90%"><center>
                                    <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:left; width:70%; padding-left:10px">
                                                   <a style="text-decoration:none; color:White;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <%
if (user.perfil == 1 && centroseleccionado.tipo == 4)
{
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                            </table>
                                    
                                    </center><br />
                                    
                                            <% 
                                            listahijos = listaProcesos.Where(x => x.padre == proc.id).ToList();
                                               List<MIDAS.Models.procesos> listahijossub = new List<MIDAS.Models.procesos>();

                                               foreach (MIDAS.Models.procesos prochijo in listahijos)
                                               {

                                                   listahijossub = listaProcesos.Where(x => x.padre == prochijo.id).ToList();
                                                   if (listahijossub.Count < 1)
                                                   { 
                                               
                                           %>
                                           <center>
                                           <% if (prochijo.nivel == "S")
                                              { %>
                                                <div style="border-radius: 15px; background-color:#ffcc99; font-weight:normal; padding:15px; width:99%">
                                                    <center>
                                                    <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="text-align:left; width:99%; padding-left:10px">
                                                   
                                                         <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= prochijo.id %>">  <%= prochijo.cod_proceso + " - " + prochijo.nombre%></a>
                                                    </td>
                                                </tr>
                                            </table>
                                                    
                                                   </center>
                                                    <br />
           
                                                </div>
                                                <% }
                                                   else
                                                       { %>
                                                        <div style="border-radius: 15px;background-color:#41b9e6; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                            <center>
                                                         <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= prochijo.id %>">
                                                         <%= prochijo.cod_proceso + " - " + prochijo.nombre%></a></center></div>
                                                       <% } %>
                                                </center>  <br />
                                             <% }
                                                   else
                                                   { %>
                                                 <center>
                                             <div style="border-radius: 15px; background-color:#ffcc99;font-weight:normal; width:90%">
                                            <table style="border-radius: 15px; background-color:#ffcc99;font-weight:normal; width:100%; margin-bottom:15px">                                            
                                            <tr>
                                            <td>
                                            <center>
                                            <br />
                                            <%--<a style="text-decoration:none; color:#0555FA;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">--%>
                                            <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="text-align:left; width:70%; padding-left:10px">
                                                   <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= prochijo.id %>">
                                                         <%= prochijo.cod_proceso + " - " + prochijo.nombre%></a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <%
if (user.perfil == 1 && centroseleccionado.tipo == 4)
{
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=prochijo.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--</a>--%>
                                            </center>  
                                            <br />                                         
                                            <br />
                                            </td>
                                           </tr>
                                           <tr style="border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;background-color:#FFFFFF">
                                           <td style="border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;padding-left:10px">
                                           <br />
                                            <%
foreach (MIDAS.Models.procesos prochijosub in listahijossub)
{
                          
                                                 %>
                                                         <div style="border-radius: 15px;background-color:#41b9e6; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                            <center>
                                                         <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= prochijosub.id %>">
                                                         <%= prochijosub.cod_proceso + " - " + prochijosub.nombre%></a></center></div><br />

                                                 <% } %>
                                             <br />
                      
                                          </td> </tr>
                                            </table>        </div>       </center>            

                                            <% } %>
                             

                                           <% } %>
                        
                                             <br />                                    
                                    </div><br/></td>

                                 <% }
                                        else
                                        {
                                            if (proc.padre == null)
                                            { %>
                                            <div style="border-radius: 15px;background-color:#41b9e6; width: 220px;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                            <center>
                                                         <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                         <%= proc.cod_proceso + " - " + proc.nombre%></a></center></div> <br /></td>
                                                         <%}
                                        }                                       
                                     if (count1 == 5)
                                            {
                                                %>
                                                    </tr>
                                                    
                                                <%
                                           
                                            }
                                    }
                                } %>
                               
                                 </tr>
                            </table>
                            </td>
                            <td>
                             <div class="Rotate-90" style="top:1px; font-size:x-large">Estratégicos</div>
                             </td></tr></table>
                                  <hr style="border-color:#41b9e6;" />
                        <table width="100%" style="min-height:155px">
                        <tr>
                            <td style="width:90%">
                            <table width="100%">
                                <tr>
                              
                            <% 
                                List<MIDAS.Models.procesos> listaProcesos2 = (List<MIDAS.Models.procesos>)ViewData["procesosO"];
                                List<MIDAS.Models.procesos> listahijos2 = new List<MIDAS.Models.procesos>();

                                int count2 = 0;
                                foreach (MIDAS.Models.procesos proc in listaProcesos2)
                                {
                                    if (proc.padre == null)
                                    count2 = count2 + 1;
                                    if (proc.nivel == "S" && proc.padre == null)
                                    {
                                        if (count2 == 5)
                                            {
                                                %>
                                                    <tr>
                                                    
                                                <%
                                                count2 = 0;
                                            }
                                        
                                        %>
                                        <td width="25%">

                                            <% 
                                                 listahijos2 = listaProcesos2.Where(x => x.padre == proc.id).ToList();
                                                 if (listahijos2.Count < 1)
                                                 { %>
                                            <div style="border-radius: 15px; background-color:#FFCC99;margin-right:15px; padding:25px; padding-top:10px; width:90%">
                                            <center>
                                            <table width="100%">
                                                <tr>
                                                    <td style="text-align:left; width:70%">
                                                        <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                         <%= proc.cod_proceso + " - " + proc.nombre%></a>
                                                    </td>
                                                    <td  style="text-align:right">
                                                     <div> 
                                                     <%
                                                     if (user.perfil == 1 && centroseleccionado.tipo == 4)
                                                     {
                                                      %>
                                                     <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                     <% } %>
                                                     </div>
                                                    </td>
                                                </tr>
                                            </table>
                                           
                                            </center>
                                            <br />
                                            </div>
                                            <% }
                                                 else
                                                 { %>
                                             <div style="border-radius: 15px; border: 0.1px solid #ffcc99; background-color:#FFCC99; width:90%">
                                            <table style="border-radius: 15px;background-color:#FFCC99; width: 99%;margin-left: 2.4px;height: 99%;margin-bottom: 2px;">                                            
                                            <tr>
                                            <td>
                                            <center>
                                            <br />
                                            <%--<a style="text-decoration:none; color:#0555FA;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">--%>
                                            <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:left; width:70%; padding-left:10px; padding-left:10px">
                                                        <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                         <%= proc.cod_proceso + " - " + proc.nombre%>
                                                         </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div> 
                                                     <%
                                                     if (user.perfil == 1 && centroseleccionado.tipo == 4)
                                                     {
                                                      %>
                                                     <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                     <% } %>
                                                     </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            </center>                                           
                                            <br />
                                            </td>
                                           </tr>
                                           <tr style="border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;background-color:#FFFFFF">
                                           <td style="padding-left:10px; border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;">
                                           <br />
                                            <%
                                                     foreach (MIDAS.Models.procesos prochijo in listahijos2)
                                                     {
                          
                                                 %>
                                                 <div style="border-radius: 15px;background-color:#41b9e6; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                                         <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= prochijo.id %>">
                                                         <%= prochijo.cod_proceso + " - " + prochijo.nombre%></a></div> <br />

                                                 <% } %>
                                        <br />
                                          </td> </tr>
                                            </table>        </div>                   

                                            <% } %>
                                            <br />


                                            </td> 
                                        <%
                                             if (count2 == 5)
                                            {
                                                %>
                                                    </tr>
                                                    
                                                <%
                                           
                                            }
                                    }
                                    else
                                    {

                                        if (proc.nivel == "M")
                                        {
                                            if (count2 == 5)
                                            {
                                                %>
                                                    <tr>
                                                    
                                                <%
                                            count2 = 0;
                                            }
                                 %>
                                    <td width="25%">
                                    <div style="border-radius: 15px; background-color:#0555FA;margin-right:15px; padding:10px;  padding-top:10px; width:90%"><center>
                                    <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:left; width:70%; padding-left:10px">
                                                   <a style="text-decoration:none; color:White;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <%
if (user.perfil == 1 && centroseleccionado.tipo == 4)
{
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                            </table>
                                    
                                    </center><br />
                                    
                                            <% 
                                            listahijos2 = listaProcesos2.Where(x => x.padre == proc.id).ToList();
                                               List<MIDAS.Models.procesos> listahijos2sub = new List<MIDAS.Models.procesos>();

                                               foreach (MIDAS.Models.procesos prochijo in listahijos2)
                                               {

                                                   listahijos2sub = listaProcesos2.Where(x => x.padre == prochijo.id).ToList();
                                                   if (listahijos2sub.Count < 1)
                                                   { 
                                               
                                           %>
                                           <center>
                                           <% if (prochijo.nivel == "S")
                                              { %>
                                                <div style="border-radius: 15px; background-color:#ffcc99; font-weight:normal; padding:15px; width:99%">
                                                    <center>
                                                    <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="text-align:left; width:99%; padding-left:10px">
                                                   
                                                         <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= prochijo.id %>">  <%= prochijo.cod_proceso + " - " + prochijo.nombre%></a>
                                                    </td>
                                                </tr>
                                            </table>
                                                    
                                                   </center>
                                                    <br />
           
                                                </div>
                                                <% }
                                                   else
                                                       { %>
                                                        <div style="border-radius: 15px;background-color:#41b9e6; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                            <center>
                                                         <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= prochijo.id %>">
                                                         <%= prochijo.cod_proceso + " - " + prochijo.nombre%></a></center></div>
                                                       <% } %>
                                                </center>  <br />
                                             <% }
                                                   else
                                                   { %>
                                                 <center>
                                             <div style="border-radius: 15px; background-color:#ffcc99;font-weight:normal; width:90%">
                                            <table style="border-radius: 15px; background-color:#ffcc99;font-weight:normal; width:100%; margin-bottom:15px">                                            
                                            <tr>
                                            <td>
                                            <center>
                                            <br />
                                            <%--<a style="text-decoration:none; color:#0555FA;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">--%>
                                            <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="text-align:left; width:70%; padding-left:10px">
                                                   <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= prochijo.id %>">
                                                         <%= prochijo.cod_proceso + " - " + prochijo.nombre%></a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <%
if (user.perfil == 1 && centroseleccionado.tipo == 4)
{
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=prochijo.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--</a>--%>
                                            </center>  
                                            <br />                                         
                                            <br />
                                            </td>
                                           </tr>
                                           <tr style="border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;background-color:#FFFFFF">
                                           <td style="border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;padding-left:10px">
                                           <br />
                                            <%
foreach (MIDAS.Models.procesos prochijosub in listahijos2sub)
{
                          
                                                 %>
                                                         <div style="border-radius: 15px;background-color:#41b9e6; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                            <center>
                                                         <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= prochijosub.id %>">
                                                         <%= prochijosub.cod_proceso + " - " + prochijosub.nombre%></a></center></div><br />

                                                 <% } %>
                                             <br />
                      
                                          </td> </tr>
                                            </table>        </div>       </center>            

                                            <% } %>
                             

                                           <% } %>
                        
                                             <br />                                    
                                    </div><br/></td>

                                 <% }
                                        else
                                        {
                                            if (proc.padre == null)
                                            { %>
                                            <div style="border-radius: 15px;background-color:#41b9e6; width: 220px;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                            <center>
                                                         <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                         <%= proc.cod_proceso + " - " + proc.nombre%></a></center></div> <br /></td>
                                                         <%}
                                        }                                       
                                     if (count2 == 5)
                                            {
                                                %>
                                                    </tr>
                                                    
                                                <%
                                           
                                            }
                                    }
                                } %>
                               
                                 </tr>
                            </table>
                            </td>
                            <td>
                             <div class="Rotate-90" style="top:1px; font-size:x-large">Operativos</div>
                             </td></tr></table>
                                  <hr  style="border-color:#41b9e6;" />
                        <table width="100%" style="min-height:155px">
                        <tr>
                            <td style="width:90%">
                            <table width="100%">
                                <tr>
                              
                            <% 
                                List<MIDAS.Models.procesos> listaProcesos3 = (List<MIDAS.Models.procesos>)ViewData["procesosS"];
                                List<MIDAS.Models.procesos> listahijos3 = new List<MIDAS.Models.procesos>();

                                int count3 = 0;
                                foreach (MIDAS.Models.procesos proc in listaProcesos3)
                                {
                                    if (proc.padre == null)
                                    count3 = count3 + 1;
                                    if (proc.nivel == "S" && proc.padre == null)
                                    {
                                        if (count3 == 5)
                                            {
                                                %>
                                                    <tr>
                                                    
                                                <%
                                                count3 = 0;
                                            }
                                        
                                        %>
                                        <td width="25%">

                                            <% 
                                                 listahijos3 = listaProcesos3.Where(x => x.padre == proc.id).ToList();
                                                 if (listahijos3.Count < 1)
                                                 { %>
                                            <div style="border-radius: 15px; background-color:#FFCC99;margin-right:15px; padding:25px; padding-top:10px; width:90%">
                                            <center>
                                            <table width="100%">
                                                <tr>
                                                    <td style="text-align:left; width:70%">
                                                        <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                         <%= proc.cod_proceso + " - " + proc.nombre%></a>
                                                    </td>
                                                    <td  style="text-align:right">
                                                     <div> 
                                                     <%
                                                     if (user.perfil == 1 && centroseleccionado.tipo == 4)
                                                     {
                                                      %>
                                                     <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                     <% } %>
                                                     </div>
                                                    </td>
                                                </tr>
                                            </table>
                                           
                                            </center>
                                            <br />
                                            </div>
                                            <% }
                                                 else
                                                 { %>
                                             <div style="border-radius: 15px; border: 0.1px solid #ffcc99; background-color:#FFCC99; width:90%">
                                            <table style="border-radius: 15px;background-color:#FFCC99; width: 99%;margin-left: 2.4px;height: 99%;margin-bottom: 2px;">                                            
                                            <tr>
                                            <td>
                                            <center>
                                            <br />
                                            <%--<a style="text-decoration:none; color:#0555FA;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">--%>
                                            <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:left; width:70%; padding-left:10px; padding-left:10px">
                                                        <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                         <%= proc.cod_proceso + " - " + proc.nombre%>
                                                         </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div> 
                                                     <%
                                                     if (user.perfil == 1 && centroseleccionado.tipo == 4)
                                                     {
                                                      %>
                                                     <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                     <% } %>
                                                     </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            </center>                                           
                                            <br />
                                            </td>
                                           </tr>
                                           <tr style="border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;background-color:#FFFFFF">
                                           <td style="padding-left:10px; border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;">
                                           <br />
                                            <%
                                                     foreach (MIDAS.Models.procesos prochijo in listahijos3)
                                                     {
                          
                                                 %>
                                                 <div style="border-radius: 15px;background-color:#41b9e6; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                                         <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= prochijo.id %>">
                                                         <%= prochijo.cod_proceso + " - " + prochijo.nombre%></a></div> <br />

                                                 <% } %>
                                        <br />
                                          </td> </tr>
                                            </table>        </div>                   

                                            <% } %>
                                            <br />


                                            </td> 
                                        <%
                                             if (count3 == 5)
                                            {
                                                %>
                                                    </tr>
                                                    
                                                <%
                                           
                                            }
                                    }
                                    else
                                    {

                                        if (proc.nivel == "M")
                                        {
                                            if (count3 == 5)
                                            {
                                                %>
                                                    <tr>
                                                    
                                                <%
                                            count3 = 0;
                                            }
                                 %>
                                    <td width="25%">
                                    <div style="border-radius: 15px; background-color:#0555FA;margin-right:15px; padding:10px;  padding-top:10px; width:90%"><center>
                                    <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:left; width:70%; padding-left:10px">
                                                   <a style="text-decoration:none; color:White;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                        <%= proc.cod_proceso + " - " + proc.nombre%>
                                                        </a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <%
if (user.perfil == 1 && centroseleccionado.tipo == 4)
{
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=proc.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                            </table>
                                    
                                    </center><br />
                                    
                                            <% 
                                            listahijos3 = listaProcesos3.Where(x => x.padre == proc.id).ToList();
                                               List<MIDAS.Models.procesos> listahijos3sub = new List<MIDAS.Models.procesos>();

                                               foreach (MIDAS.Models.procesos prochijo in listahijos3)
                                               {

                                                   listahijos3sub = listaProcesos3.Where(x => x.padre == prochijo.id).ToList();
                                                   if (listahijos3sub.Count < 1)
                                                   { 
                                               
                                           %>
                                           <center>
                                           <% if (prochijo.nivel == "S")
                                              { %>
                                                <div style="border-radius: 15px; background-color:#ffcc99; font-weight:normal; padding:15px; width:99%">
                                                    <center>
                                                    <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="text-align:left; width:99%; padding-left:10px">
                                                   
                                                         <a style="text-decoration:none; color:Black;" href="/evr/procesos/detalle_proceso/<%= prochijo.id %>">  <%= prochijo.cod_proceso + " - " + prochijo.nombre%></a>
                                                    </td>
                                                </tr>
                                            </table>
                                                    
                                                   </center>
                                                    <br />
           
                                                </div>
                                                <% }
                                                   else
                                                       { %>
                                                        <div style="border-radius: 15px;background-color:#41b9e6; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                            <center>
                                                         <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= prochijo.id %>">
                                                         <%= prochijo.cod_proceso + " - " + prochijo.nombre%></a></center></div>
                                                       <% } %>
                                                </center>  <br />
                                             <% }
                                                   else
                                                   { %>
                                                 <center>
                                             <div style="border-radius: 15px; background-color:#ffcc99;font-weight:normal; width:90%">
                                            <table style="border-radius: 15px; background-color:#ffcc99;font-weight:normal; width:100%; margin-bottom:15px">                                            
                                            <tr>
                                            <td>
                                            <center>
                                            <br />
                                            <%--<a style="text-decoration:none; color:#0555FA;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">--%>
                                            <table width="100%" style="padding:25px;">
                                                <tr>
                                                    <td  style="text-align:left; width:70%; padding-left:10px">
                                                   <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= prochijo.id %>">
                                                         <%= prochijo.cod_proceso + " - " + prochijo.nombre%></a>
                                                    </td>
                                                    <td  style="text-align:right; padding-right:10px">
                                                     <div>
                                                     <%
if (user.perfil == 1 && centroseleccionado.tipo == 4)
{
                                                      %>
                                                      <a title="Eliminar Proceso" onclick="if(!confirm('¿Está seguro de que desea eliminar este proceso?')) return false;" href="/evr/procesos/Eliminar_Proceso/<%=prochijo.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                      <% } %>
                                                      </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <%--</a>--%>
                                            </center>  
                                            <br />                                         
                                            <br />
                                            </td>
                                           </tr>
                                           <tr style="border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;background-color:#FFFFFF">
                                           <td style="border-bottom-left-radius: 15px; border-bottom-right-radius: 15px;padding-left:10px">
                                           <br />
                                            <%
foreach (MIDAS.Models.procesos prochijosub in listahijos3sub)
{
                          
                                                 %>
                                                         <div style="border-radius: 15px;background-color:#41b9e6; width: 99%;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                            <center>
                                                         <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= prochijosub.id %>">
                                                         <%= prochijosub.cod_proceso + " - " + prochijosub.nombre%></a></center></div><br />

                                                 <% } %>
                                             <br />
                      
                                          </td> </tr>
                                            </table>        </div>       </center>            

                                            <% } %>
                             

                                           <% } %>
                        
                                             <br />                                    
                                    </div><br/></td>

                                 <% }
                                        else
                                        {
                                            if (proc.padre == null)
                                            { %>
                                            <div style="border-radius: 15px;background-color:#41b9e6; width: 220px;height: 99%;margin-bottom: 2px; margin-left:-4px ; padding:10px">
                                            <center>
                                                         <a style="text-decoration:none; color:#000;" href="/evr/procesos/detalle_proceso/<%= proc.id %>">
                                                         <%= proc.cod_proceso + " - " + proc.nombre%></a></center></div> <br /></td>
                                                         <%}
                                        }                                       
                                     if (count3 == 5)
                                            {
                                                %>
                                                    </tr>
                                                    
                                                <%
                                           
                                            }
                                    }
                                } %>
                               
                                 </tr>
                            </table>
                            </td>
                            <td>
                             <div class="Rotate-90" style="top:1px; font-size:x-large">Soporte</div>
                             </td></tr></table>
                    </div>
                    </div>
                    </td></tr></table>
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
