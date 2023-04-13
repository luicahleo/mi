<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Plan de Formación</title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divN1").show();
            $("#btnN1").addClass('active');
            $("#ctl00_MainContent_divN3").hide();

            $("#btnN1").click(function () {
                $("#ctl00_MainContent_divN1").show();
                $("#btnN1").addClass('active');
                $("#ctl00_MainContent_divN3").hide();
                $("#btnN3").removeClass('active');
            });

            $("#btnN3").click(function () {
                $("#ctl00_MainContent_divN1").hide();
                $("#btnN1").removeClass('active');
                $("#ctl00_MainContent_divN3").show();
                $("#btnN3").addClass('active');
            });
        });       
    </script>
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

            if (ViewData["planformacion"] != null)
            {
                grdN1.DataSource = ViewData["planformacion"];
                grdN1.DataBind();
            }

            if (ViewData["planformacionEspecifico"] != null)
            {
                grdN3.DataSource = ViewData["planformacionEspecifico"];
                grdN3.DataBind();
            }


            if (Session["EditarFormacionResultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarFormacionResultado"].ToString() + "' });", true);
                Session.Remove("EditarFormacionResultado");
            }

            if (Session["EditarFormacionError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarFormacionError"].ToString() + "' });", true);
                Session.Remove("EditarFormacionError");
            } 

        }        

    }
</script>
    <br />    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <h3>Gestión de la Formación</h3>
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    
    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-signup"></i>Plan de Formación</h6>
                        </div>
                        <div class="panel-body">
									<center>
                                    <table width="50%">
                                        <tr>
                                        <% 
                                            if (centroseleccionado.tipo != 4)
                                            { %>
                                            <td style="width:33%">
                                                <center>
                                                <input id="btnN1" style="width:230px; margin-right:10px" type="button" value="Genérica" class="btn btn-primary run-first" /></center>
                                            </td>                                           
                                            <td style="width:33%">
                                            
                                                <center>
                                                <input id="btnN3" style="width:230px" type="button" value="Específica (<%= centroseleccionado.nombre  %>)" class="btn btn-primary run-first"/></center>
                                                <% }
                                            else
                                            {
                                              %>
                                                <td style="width:33%">
                                                <center>
                                                <input id="Button1" style="width:230px;" type="button" value="Genérica" class="btn btn-primary run-first" /></center>
                                            </td> 
                                                                                               
                                                 <%
                                            } %>
                                            </td>
                                        </tr>
                                    </table>
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
                                                                                Año      
                                                                            </th> 
                                                                            <th>
                                                                                Denominación      
                                                                            </th> 
                                                                            <th style="width:50px">
                                                                                P.Inicial
                                                                            </th>                    
                                                                            <th style="width:100px">
                                                                                Fecha reg. inicio
                                                                            </th>  
                                                                            <th style="width:100px">
                                                                                Fecha reg. ejecutado
                                                                            </th>
                                                                            <th style="width:50px">
                                                                                P.Ejecutada
                                                                            </th> 
                                                                            <th style="width:50px">
                                                                                Valoración
                                                                            </th>  
                                                                            <th style="width:50px">
                                                                                Ejecución (%)
                                                                            </th>  
                                                                            <th style="width:50px">
                                                                                Horas
                                                                            </th>  
                                                                            <% if (centroseleccionado.tipo == 4 && user.perfil == 1)
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
                                                                            <% if (centroseleccionado.tipo == 4 && user.perfil == 1)
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
                                                                                    item.Cells[2].Text
                                
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
                                                                                <a title="Ver Fichero" href="/evr/Formacion/ObtenerPlanInicial/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                                <% } %>
                                                                            </td>  
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[5].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[5].Text).ToString("yyyy/MM/dd")%></span>   
                                                                                <%= (DateTime.Parse(item.Cells[5].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>                     
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[6].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[6].Text).ToString("yyyy/MM/dd")%></span>   
                                                                                <%= (DateTime.Parse(item.Cells[6].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>   
                                                                            <td style="width:50px" class="text-center">
                                                                                <% if (item.Cells[7].Text != "&nbsp;")
                                                                                   { %>
                                                                                <a title="Ver Fichero" href="/evr/Formacion/ObtenerPlanEjecutada/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                                <% } %>
                                                                            </td> 
                                                              
                                                                                <% switch (item.Cells[8].Text)
                                                                                   {
                                                                                       case "1":
                                                                                           %>
                                                                                            <td style="color: Green" class="task-desc">
                                                                                           Eficaz
                                                                                           </td>
                                                                                        <% break; 
                                                                                       case "2":                                                                                          
                                                                                           %>
                                                                                            <td style="color: Red" class="task-desc">
                                                                                           No eficaz
                                                                                           </td>
                                                                                        <% break; 
                                                                                       default:                                                                                          
                                                                                           %>
                                                                                            <td class="task-desc">
                                                                                          
                                                                                           </td>
                                                                                        <%
                                                                                            break;
                                                                                   }  %> 
                                                                            <td style="width:50px" class="text-center">
                                                                                <% 
                                                                                    double ejecut = 0;
                                                                                    double planif = 0;

                                                                                    if (item.Cells[11].Text != "&nbsp;")
                                                                                    {
                                                                                        ejecut = double.Parse(item.Cells[11].Text);
                                                                                    }
                                                                                    else
                                                                                        ejecut = 0;

                                                                                    if (item.Cells[12].Text != "&nbsp;")
                                                                                    {
                                                                                        planif = double.Parse(item.Cells[12].Text);
                                                                                    }
                                                                                    else
                                                                                        planif = 0;

                                                                                    double porcentaje = 0;

                                                                                    if (planif != 0)
                                                                                    {
                                                                                        porcentaje = (100 / planif) * ejecut;
                                                                                        porcentaje = Math.Round(porcentaje, 0);
                                                                                    }
                                                                                
                                                                                 %>
                                                                                 <%= porcentaje.ToString() %>
                                                                            </td>
                                                                            <td style="width:50px" class="text-center">
                                                                                <% 
                                                                                    int totalhoras = 0;
                                                                                    int horascalidad = 0;
                                                                                    int horasmedioamb = 0;
                                                                                    int horassegysalud = 0;
                                                                                    int horasotras = 0;
                                                                                
                                                                                    if (item.Cells[13].Text != "&nbsp;")
                                                                                    {
                                                                                        horascalidad = int.Parse(item.Cells[13].Text);
                                                                                    }
                                                                                    else
                                                                                        horascalidad = 0;
                                                                                
                                                                                    if (item.Cells[14].Text != "&nbsp;")
                                                                                    {
                                                                                        horasmedioamb = int.Parse(item.Cells[14].Text);
                                                                                    }
                                                                                    else
                                                                                        horasmedioamb = 0;
                                                                                
                                                                                    if (item.Cells[15].Text != "&nbsp;")
                                                                                    {
                                                                                        horassegysalud = int.Parse(item.Cells[15].Text);
                                                                                    }
                                                                                    else
                                                                                        horassegysalud = 0;
                                                                                
                                                                                    if (item.Cells[16].Text != "&nbsp;")
                                                                                    {
                                                                                        horasotras = int.Parse(item.Cells[16].Text);
                                                                                    }
                                                                                    else
                                                                                        horasotras = 0;
                                                                                
                                                                                    totalhoras = horascalidad + horasmedioamb + horassegysalud + horasotras;
                                                                                 %>
                                                                                 <%= totalhoras %>
                                                                            </td>
                                                                            <% 
                                                                            if (centroseleccionado.tipo == 4 && user.perfil == 1)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/Formacion/detalle_formacion/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% }
                                                                               else
                                                                               { %>    
                                                                               <td style="width:50px" class="text-center">
                                                                                <a title="Consultar" href="/evr/Formacion/detalle_formacion/<%=item.Cells[0].Text %>");"><i class="icon-search"></i></a>
                                                                            </td> 
                                                                               <% } %>    
                                                                            <% if (centroseleccionado.tipo == 4 && user.perfil == 1)
                                                                               { %>                                                                  
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Plan" onclick="if(!confirm('¿Está seguro de que desea eliminar este plan de formación?')) return false;" href="/evr/Formacion/eliminar_planformacion/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>      
                                                                            <% } %>               
                                                                            <% } %>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>                                                        
                                                 </div>

                                                 <% 
                                                     if (centroseleccionado.tipo != 4)
                                                    { %>
                                                 <div runat="server" id="divN3">
								                    <asp:GridView ID="grdN3" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                <thead>
                                                                        <tr>
                                                                            <th style="width:50px">
                                                                                Año      
                                                                            </th> 
                                                                            <th>
                                                                                Denominación      
                                                                            </th> 
                                                                            <th style="width:50px">
                                                                                P.Inicial
                                                                            </th>                    
                                                                            <th style="width:100px">
                                                                                Fecha reg. inicio
                                                                            </th>  
                                                                            <th style="width:100px">
                                                                                Fecha reg. ejecutado
                                                                            </th>
                                                                            <th style="width:50px">
                                                                                P.Ejecutada
                                                                            </th> 
                                                                            <th style="width:50px">
                                                                                Valoración
                                                                            </th>  
                                                                            <th style="width:50px">
                                                                                Ejecución (%)
                                                                            </th>  
                                                                            <th style="width:50px">
                                                                                Horas
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
                                                                            foreach (GridViewRow item in grdN3.Rows)
                                                                           { %>
                                                                        <tr>
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
                                                                            <td style="width:50px" class="text-center">
                                                                             <% if (item.Cells[4].Text != "&nbsp;")
                                                                                { %>
                                                                                <a title="Ver Fichero" href="/evr/Formacion/ObtenerPlanInicial/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                                <% } %>
                                                                            </td>  
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[5].Text != "&nbsp;")
                                                                               { %>
                                                                                <%= (DateTime.Parse(item.Cells[5].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[5].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>                     
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[6].Text != "&nbsp;")
                                                                               { %>
                                                                                <%= (DateTime.Parse(item.Cells[6].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>   
                                                                            <td style="width:50px" class="text-center">
                                                                                <% if (item.Cells[7].Text != "&nbsp;")
                                                                                   { %>
                                                                                <a title="Ver Fichero" href="/evr/Formacion/ObtenerPlanEjecutada/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                                <% } %>
                                                                            </td> 
                                                              
                                                                                <% switch (item.Cells[8].Text)
                                                                                   {
                                                                                       case "1":
                                                                                           %>
                                                                                            <td style="color: Green" class="task-desc">
                                                                                           Eficaz
                                                                                           </td>
                                                                                        <% break; 
                                                                                       case "2":                                                                                          
                                                                                           %>
                                                                                            <td style="color: Red" class="task-desc">
                                                                                           No eficaz
                                                                                           </td>
                                                                                        <% break; 
                                                                                       default:                                                                                          
                                                                                           %>
                                                                                            <td class="task-desc">
                                                                                          
                                                                                           </td>
                                                                                        <%
                                                                                            break;
                                                                                   }  %> 
                                                                            <td style="width:50px" class="text-center">
                                                                                <% 
                                                                                    double ejecut = 0;
                                                                                    double planif = 0;

                                                                                    if (item.Cells[11].Text != "&nbsp;")
                                                                                    {
                                                                                        ejecut = double.Parse(item.Cells[11].Text);
                                                                                    }
                                                                                    else
                                                                                        ejecut = 0;

                                                                                    if (item.Cells[12].Text != "&nbsp;")
                                                                                    {
                                                                                        planif = double.Parse(item.Cells[12].Text);
                                                                                    }
                                                                                    else
                                                                                        planif = 0;

                                                                                    double porcentaje = 0;

                                                                                    if (planif != 0)
                                                                                    {
                                                                                        porcentaje = (100 / planif) * ejecut;
                                                                                        porcentaje = Math.Round(porcentaje, 0);
                                                                                    }
                                                                                
                                                                                 %>
                                                                                 <%= porcentaje.ToString() %>
                                                                            </td>
                                                                            <td style="width:50px" class="text-center">
                                                                                <% 
                                                                                    int totalhoras = 0;
                                                                                    int horascalidad = 0;
                                                                                    int horasmedioamb = 0;
                                                                                    int horassegysalud = 0;
                                                                                    int horasotras = 0;
                                                                                
                                                                                    if (item.Cells[13].Text != "&nbsp;")
                                                                                    {
                                                                                        horascalidad = int.Parse(item.Cells[13].Text);
                                                                                    }
                                                                                    else
                                                                                        horascalidad = 0;
                                                                                
                                                                                    if (item.Cells[14].Text != "&nbsp;")
                                                                                    {
                                                                                        horasmedioamb = int.Parse(item.Cells[14].Text);
                                                                                    }
                                                                                    else
                                                                                        horasmedioamb = 0;
                                                                                
                                                                                    if (item.Cells[15].Text != "&nbsp;")
                                                                                    {
                                                                                        horassegysalud = int.Parse(item.Cells[15].Text);
                                                                                    }
                                                                                    else
                                                                                        horassegysalud = 0;
                                                                                
                                                                                    if (item.Cells[16].Text != "&nbsp;")
                                                                                    {
                                                                                        horasotras = int.Parse(item.Cells[16].Text);
                                                                                    }
                                                                                    else
                                                                                        horasotras = 0;
                                                                                
                                                                                    totalhoras = horascalidad + horasmedioamb + horassegysalud + horasotras;
                                                                                 %>
                                                                                 <%= totalhoras %>
                                                                            </td>
                                                                            <% if (permisos.permiso == true)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/Formacion/detalle_formacion/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% }
                                                                               else
                                                                               { %>    
                                                                               <td style="width:50px" class="text-center">
                                                                                <a title="Consultar" href="/evr/Formacion/detalle_formacion/<%=item.Cells[0].Text %>");"><i class="icon-search"></i></a>
                                                                            </td> 
                                                                               <% } %>    
                                                                            <% if (permisos.permiso == true)
                                                                               { %>                                                                
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Plan" onclick="if(!confirm('¿Está seguro de que desea eliminar este plan de formación?')) return false;" href="/evr/Formacion/eliminar_planformacion/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>      
                                                                            <% } %>               
                                                                            <% } %>
                                                                        </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        
                                                 </div>        
                                                 <% } %>                           
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
                                    <a href="/evr/Formacion/detalle_formacion/0" class="btn btn-primary" title="Nuevo Plan de Formación">Nuevo Plan de Formación</a>                                    
                                    <% } %></div>    
    </form>
</asp:Content>
