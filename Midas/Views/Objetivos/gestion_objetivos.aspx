<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>


<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Objetivos</title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_MainContent_divN1").show();
            $("#btnN1").addClass('active');
            $("#ctl00_MainContent_divN3").hide();
            $("#ctl00_MainContent_divN2").hide();


            $("#btnN1").click(function () {
                $("#ctl00_MainContent_divN1").show();
                $("#btnN1").addClass('active');
                $("#ctl00_MainContent_divN3").hide();
                $("#ctl00_MainContent_divN2").hide();
                $("#btnN3").removeClass('active');
                $("#btnN2").removeClass('active');
            });

            $("#btnN2").click(function () {
                $("#ctl00_MainContent_divN1").hide();
                $("#ctl00_MainContent_divN3").hide();
                $("#btnN1").removeClass('active');
                $("#btnN3").removeClass('active');
                $("#ctl00_MainContent_divN2").show();
                $("#btnN2").addClass('active');
            });

            $("#btnN3").click(function () {
                $("#ctl00_MainContent_divN1").hide();
                $("#ctl00_MainContent_divN2").hide();
                $("#btnN1").removeClass('active');
                $("#btnN2").removeClass('active');
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

            for (int i = 2008; i <= DateTime.Now.Year; i++)
            {
                ListItem itemAnio = new ListItem();
                itemAnio.Value = i.ToString();
                itemAnio.Text = i.ToString();
                ddlAnio.Items.Insert(0, itemAnio);
            }
            ListItem todos = new ListItem();
            todos.Value = "0";
            todos.Text = "Todos";
            ddlAnio.Items.Insert(0, todos);

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            if (user.perfil == 2)
                permisos = MIDAS.Models.Datos.ObtenerPermisos(user.idUsuario, centroseleccionado.id);
            else
            {
                permisos.idusuario = user.idUsuario;
                permisos.idcentro = centroseleccionado.id;
                permisos.permiso = true;
            }

            if (ViewData["objetivosGenericos"] != null)
            {
                grdN1.DataSource = ViewData["objetivosGenericos"];
                grdN1.DataBind();
            }

            if (ViewData["objetivosUnidad"] != null)
            {
                grdN2.DataSource = ViewData["objetivosUnidad"];
                grdN2.DataBind();
            }

            if (ViewData["objetivosEspecificos"] != null)
            {
                grdN3.DataSource = ViewData["objetivosEspecificos"];
                grdN3.DataBind();
            }



            if (Session["EditarObjetivosResultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarObjetivosResultado"].ToString() + "' });", true);
                Session.Remove("EditarObjetivosResultado");
            }

            if (Session["EditarObjetivosError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarObjetivosError"].ToString() + "' });", true);
                Session.Remove("EditarObjetivosError");
            }

        }

    }
</script>
    <br />
    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <h3>Gestión de Objetivos</h3>
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    
    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-signup"></i>Objetivos</h6>
                        </div>
                        <div class="panel-body">
									<center>
                                    <table width="100%">
                                        <tr>
                                            <td style="width:33%">
                                                <center>
                                                <input id="btnN1" style="width:350px;" type="button" value="Estratégicos" class="btn btn-primary run-first" /></center>
                                            </td>    
                                            <td style="width:33%">
                                                <center>
                                                <input id="btnN2" style="width:350px;" type="button" value="Unidad de negocio (<%= centroseleccionado.siglas  %>)" class="btn btn-primary run-first" /></center>
                                            </td>    
                                            <td style="width:33%">
                                                <center>
                                                <input id="btnN3" style="width:350px;" type="button" value="Operativos (<%= centroseleccionado.nombre  %>)" class="btn btn-primary run-first"/></center>
                                            </td>
                                           <%-- <% if (centroseleccionado.id != 25)
                                               { %>                              
                                            <td style="width:50%">
                                                <center>
                                                <input id="btnN3" style="width:350px" type="button" value="Operativos (<%= centroseleccionado.nombre  %>)" class="btn btn-primary run-first"/></center>
                                            </td>
                                            <% } %>--%>
                                        </tr>
                                    </table>
                                    <br />
                                    <table  width="100%" >
                                        <tr>
                                            <td colspan="4">
                                                <div runat="server" id="divN1">
                                                    <h5>Estratégicos</h5>
								                    <asp:GridView ID="grdN1" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="width:50px">
                                                                                Código      
                                                                            </th> 
                                                                            <th>
                                                                                Titulo      
                                                                            </th> 
                                                                            <th style="width:100px">
                                                                                Fecha estimada
                                                                            </th>                    
                                                                            <th style="width:100px">
                                                                                Fecha real
                                                                            </th>  
                                                                            <th>
                                                                                Estado
                                                                            </th>
                                                                            <th>
                                                                                Ámbito
                                                                            </th>
                                                                            <th style="width:100px;">
                                                                                Responsable
                                                                            </th>   
                                                                            <% if (centroseleccionado.id == 25 && (user.perfil == 1 || permisos.permiso == true))
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
                                                                            
                                                                            <% if (centroseleccionado.id == 25 && (user.perfil == 1 || user.idUnidad == centroseleccionado.tipo))
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Borrar
                                                                            </th>
                                                                            <% } %>
                                                                            <% if (centroseleccionado.id != 25 && (user.idUnidad == centroseleccionado.tipo))
                                                                               { %>
                                                                                <th  style="width:50px">
                                                                                Asignar
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
                                                                                    item.Cells[3].Text
                                
                                                                                    %>
 
                                                                            </td>   
                                                                        <td class="text">
                                                                        <%=  
                                                                                    item.Cells[4].Text
                                
                                                                                    %>
 
                                                                            </td>    
                
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[6].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[6].Text).ToString("yyyy/MM/dd")%></span>    
                                                                                <%= (DateTime.Parse(item.Cells[6].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>                     
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[7].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[7].Text).ToString("yyyy/MM/dd")%></span>    
                                                                                <%= (DateTime.Parse(item.Cells[7].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[7].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[7].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>   
                                                              
                                                                                <% switch (item.Cells[14].Text)
                                                                                   {
                                                                                       case "1":
                                                                                           %>
                                                                                            <td style="color: LimeGreen" class="task-desc">
																							<b>
                                                                                           Alcanza resultados esperados</b>
                                                                                           </td>
                                                                                        <% break; 
                                                                                       case "2":                                                                                          
                                                                                           %>
                                                                                            <td style="color: Red" class="task-desc">
																							<b>
                                                                                           No alcanza resultados esperados</b>
                                                                                           </td>
                                                                                        <% break; 
                                                                                       case "3":                                                                                          
                                                                                           %>
                                                                                            <td style="color: Orange" class="task-desc">
																							<b>
                                                                                           Desestimado</b>
                                                                                           </td>
                                                                                        <% break; 
                                                                                       default:                                                                                          
                                                                                           %>
                                                                                            <td class="task-desc">
                                                                                          <b>En seguimiento</b> 
                                                                                           </td>
                                                                                        <%
                                                                                            break;
                                                                                   }  %>
 
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[17].Text
                                
                                                                                    %>
 
                                                                            </td> 
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[8].Text
                                
                                                                                    %>
 
                                                                            </td> 
                                                                            <% if (centroseleccionado.id == 25 && (user.perfil == 1 || permisos.permiso == true))
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/Objetivos/detalle_objetivo/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% }
                                                                               else
                                                                               { %>    
                                                                               <td style="width:50px" class="text-center">
                                                                                <a title="Consultar" href="/evr/Objetivos/detalle_objetivo/<%=item.Cells[0].Text %>");"><i class="icon-search"></i></a>
                                                                            </td> 
                                                                               <% } %>    
                                                                            <% if (centroseleccionado.id != 25 && (permisos.permiso == true)  && (user.idUnidad == centroseleccionado.tipo))
                                                                               { %>
                                                                                <td style="width:50px" class="text-center">
                                                                                <% if (MIDAS.Models.Datos.comprobarReferencias(int.Parse(item.Cells[0].Text), centroseleccionado.id) == false)
                                                                                   { %>
                                                                                <a title="Asignar" href="/evr/Objetivos/asignar_objetivo/<%=item.Cells[0].Text %>");"><i class="icon-bubble-forward"></i></a>
                                                                                <% } %>
                                                                                </td> 
                                                                            <% } %>
                                                                            <% if (centroseleccionado.id == 25 && (user.perfil == 1 || permisos.permiso == true))
                                                                               { %>                                                                  
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Objetivo" onclick="if(!confirm('¿Está seguro de que desea eliminar este objetivo?')) return false;" href="/evr/Objetivos/eliminar_objetivo/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>      
                                                                            <% } %>               
                                                                            <% } %>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>                                                        
                                                 </div>  
                                                






                                                <div runat="server" id="divN2">
                                                    <h5>Unidad de negocio</h5>
								                    <asp:GridView ID="grdN2" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="width:50px">
                                                                                Código      
                                                                            </th> 
                                                                            <th>
                                                                                Titulo      
                                                                            </th> 
                                                                            <th style="width:100px">
                                                                                Fecha estimada
                                                                            </th>                    
                                                                            <th style="width:100px">
                                                                                Fecha real
                                                                            </th>  
                                                                            <th>
                                                                                Estado
                                                                            </th>
                                                                            <th>
                                                                                Ámbito
                                                                            </th>
                                                                            <th style="width:100px">
                                                                                Responsable
                                                                            </th>   
                                                                            <% if (centroseleccionado.id == 25 && (user.idUnidad == centroseleccionado.tipo))
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
                                                                            
                                                                            <% if (centroseleccionado.id == 25 && (user.perfil == 1 || user.idUnidad == centroseleccionado.tipo))
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Borrar
                                                                            </th>
                                                                            <% } %>
                                                                            <% if (centroseleccionado.id != 25 && user.idUnidad == centroseleccionado.tipo)
                                                                               { %>
                                                                                <th  style="width:50px">
                                                                                Asignar
                                                                            </th>
                                                                            <% } %>

                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <% 
                                                                        foreach (GridViewRow item in grdN2.Rows)
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
                
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[6].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[6].Text).ToString("yyyy/MM/dd")%></span>    
                                                                                <%= (DateTime.Parse(item.Cells[6].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>                     
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[7].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[7].Text).ToString("yyyy/MM/dd")%></span>    
                                                                                <%= (DateTime.Parse(item.Cells[7].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[7].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[7].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>   
                                                              
                                                                                <% switch (item.Cells[14].Text)
                                                                                   {
                                                                                       case "1":
                                                                                           %>
                                                                                            <td style="color: LimeGreen" class="task-desc">
																							<b>
                                                                                           Alcanza resultados esperados</b>
                                                                                           </td>
                                                                                        <% break; 
                                                                                       case "2":                                                                                          
                                                                                           %>
                                                                                            <td style="color: Red" class="task-desc">
																							<b>
                                                                                           No alcanza resultados esperados</b>
                                                                                           </td>
                                                                                        <% break; 
                                                                                       case "3":                                                                                          
                                                                                           %>
                                                                                            <td style="color: Orange" class="task-desc">
																							<b>
                                                                                           Desestimado</b>
                                                                                           </td>
                                                                                        <% break; 
                                                                                       default:                                                                                          
                                                                                           %>
                                                                                            <td class="task-desc">
                                                                                          <b>En seguimiento</b> 
                                                                                           </td>
                                                                                        <%
                                                                                            break;
                                                                                   }  %>
 
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[17].Text
                                
                                                                                    %>
 
                                                                            </td> 
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[8].Text
                                
                                                                                    %>
 
                                                                            </td> 
                                                                            <% if (centroseleccionado.id == 25 && (user.perfil == 1 || user.idUnidad == centroseleccionado.tipo))
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/Objetivos/detalle_objetivo/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% }
                                                                               else
                                                                               { %>    
                                                                               <td style="width:50px" class="text-center">
                                                                                <a title="Consultar" href="/evr/Objetivos/detalle_objetivo/<%=item.Cells[0].Text %>");"><i class="icon-search"></i></a>
                                                                            </td> 
                                                                               <% } %>    
                                                                            <% if (centroseleccionado.id != 25 && (permisos.permiso == true)  && (user.idUnidad == centroseleccionado.tipo))
                                                                               { %>
                                                                                <td style="width:50px" class="text-center">
                                                                                <% if (MIDAS.Models.Datos.comprobarReferencias(int.Parse(item.Cells[0].Text), centroseleccionado.id) == false)
                                                                                   { %>
                                                                                <a title="Asignar" href="/evr/Objetivos/asignar_objetivo/<%=item.Cells[0].Text %>");"><i class="icon-bubble-forward"></i></a>
                                                                                <% } %>
                                                                                </td> 
                                                                            <% } %>
                                                                            <% if (centroseleccionado.id == 25 && (user.perfil == 1 || user.idUnidad == centroseleccionado.tipo))
                                                                               { %>                                                                  
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Objetivo" onclick="if(!confirm('¿Está seguro de que desea eliminar este objetivo?')) return false;" href="/evr/Objetivos/eliminar_objetivo/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>      
                                                                            <% } %>               
                                                                            <% } %>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>                                                        
                                                 </div>









                                                <div runat="server" id="divN3">
                                                    <h5>Operativos</h5>
								                    <asp:GridView ID="grdN3" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                           <th style="width:50px">
                                                                                Código      
                                                                            </th> 
                                                                            <th>
                                                                                Titulo      
                                                                            </th> 
                                                                            <th style="width:100px">
                                                                                Fecha estimada
                                                                            </th>                    
                                                                            <th style="width:100px">
                                                                                Fecha real
                                                                            </th>  
                                                                            <th>
                                                                                Estado
                                                                            </th>
                                                                            <th>
                                                                                Ámbito
                                                                            </th>
                                                                            <th style="width:100px">
                                                                                Responsable
                                                                            </th>   
                                                                            <% if (user.perfil == 1 || user.idUnidad == centroseleccionado.tipo)
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
                                                                            <% if (user.perfil == 1 || user.idUnidad == centroseleccionado.tipo)
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
                
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[6].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[6].Text).ToString("yyyy/MM/dd")%></span>    
                                                                                <%= (DateTime.Parse(item.Cells[6].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>                     
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[7].Text != "&nbsp;")
                                                                               { %>
                                                                                <span style="display:none;"><%= DateTime.Parse(item.Cells[7].Text).ToString("yyyy/MM/dd")%></span>    
                                                                                <%= (DateTime.Parse(item.Cells[7].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[7].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[7].Text).Year).ToString()%>
                                                                                <% } %>
                                                                            </td>   
                                                                             <% switch (item.Cells[14].Text)
                                                                                   {
                                                                                       case "1":
                                                                                           %>
                                                                                            <td style="color: LimeGreen" class="task-desc">
																							<b>
                                                                                           Alcanza resultados esperados</b>
                                                                                           </td>
                                                                                        <% break; 
                                                                                       case "2":                                                                                          
                                                                                           %>
                                                                                            <td style="color: Red" class="task-desc">
																							<b>
                                                                                           No alcanza resultados esperados</b>
                                                                                           </td>
                                                                                        <% break; 
                                                                                       case "3":                                                                                          
                                                                                           %>
                                                                                            <td style="color: Orange" class="task-desc">
																							<b>
                                                                                           Desestimado</b>
                                                                                           </td>
                                                                                        <% break; 
                                                                                       default:                                                                                          
                                                                                           %>
                                                                                            <td class="task-desc">
                                                                                          <b>En seguimiento</b> 
                                                                                           </td>
                                                                                        <%
                                                                                            break;
                                                                                   }  %>
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[17].Text
                                
                                                                                    %>
 
                                                                            </td> 
                                                                            <td class="text">
                                                                        <%=  
                                                                                    item.Cells[8].Text
                                
                                                                                    %>
 
                                                                            </td> 
                                                                            <% if (user.idUnidad == centroseleccionado.tipo)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/Objetivos/detalle_objetivo/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% }
                                                                               else{ %>  
                                                                               <td style="width:50px" class="text-center">
                                                                                <a title="Consultar" href="/evr/Objetivos/detalle_objetivo/<%=item.Cells[0].Text %>");"><i class="icon-search"></i></a>
                                                                            </td> 
                                                                               <% } %>
                                                                            <% if (user.idUnidad == centroseleccionado.tipo)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Objetivo" onclick="if(!confirm('¿Está seguro de que desea eliminar este objetivo?')) return false;" href="/evr/Objetivos/eliminar_objetivo/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
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
                                                </td>
                                                <% if (user.idUnidad == centroseleccionado.tipo)
                                                { %>
                                                <td  style="padding-right:5px">
                                                    <a href="/evr/Objetivos/detalle_objetivo/0" class="btn btn-primary" title="Nuevo Objetivo">Nuevo Objetivo</a>
                                                </td>
                                                <% } %>   
                                            </tr>
                                        </table>  
                                        <br />
                                    </div>
                                    
    </form>
</asp:Content>
