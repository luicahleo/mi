<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Documentos</title>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#ctl00_MainContent_divN1").show();
            $("#btnN1").addClass('active');
            //$("#ctl00_MainContent_ddlTecnologia").prop('disabled', true);     
            $("#ctl00_MainContent_divN2").hide();
            $("#ctl00_MainContent_divN3").hide();


            $("#btnN1").click(function () {
                $("#ctl00_MainContent_divN1").show();
                $("#btnN1").addClass('active');
                $("#ctl00_MainContent_divN2").hide();
                $("#btnN2").removeClass('active');
                $("#ctl00_MainContent_divN3").hide();
                $("#btnN3").removeClass('active');
            });

            $("#btnN2").click(function () {
                $("#ctl00_MainContent_divN1").hide();
                $("#btnN1").removeClass('active');
                $("#ctl00_MainContent_divN2").show();
                $("#btnN2").addClass('active');
                $("#ctl00_MainContent_divN3").hide();
                $("#btnN3").removeClass('active');
            });

            $("#btnN3").click(function () {
                $("#ctl00_MainContent_divN1").hide();
                $("#btnN1").removeClass('active');
                $("#ctl00_MainContent_divN2").hide();
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
    string tecnologia;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {            
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            if (Session["CentralElegida"] != null)
            {
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            }
            
            if (ViewData["materiales"] != null)
            {
                grdN1.DataSource = ViewData["materiales"];
                grdN1.DataBind();
            }
            if (ViewData["informes"] != null)
            {
                grdN2.DataSource = ViewData["informes"];
                grdN2.DataBind();
            }
            if (ViewData["evaluaciones"] != null)
            {
                grdN3.DataSource = ViewData["evaluaciones"];
                grdN3.DataBind();
            }

            if (Session["EditarApoyoPRLResultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarApoyoPRLResultado"].ToString() + "' });", true);
                Session.Remove("EditarApoyoPRLResultado");
            }

            if (Session["EditarApoyoPRLError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarApoyoPRLError"].ToString() + "' });", true);
                Session.Remove("EditarApoyoPRLError");
            } 

        }        

    }
</script>
    <br />
    
    <form enctype="multipart/form-data" method="post" id="Form1" runat="server">
    <h3>Apoyo PRL</h3>
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    
    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h6 class="panel-title">
                                <i class="icon-file"></i>Documentos subidos</h6>
                        </div>
                        <div class="panel-body">
									<center>
                                    <table width="100%">
                                        <tr>
                                            <td style="width:33%">
                                                <asp:HiddenField ID="hdnTipoCentro" runat="server" />
                                                <center>
                                                <input id="btnN1" style="width:240px; margin-right:10px" type="button" value="Material Divulgativo" class="btn btn-primary run-first" /></center>
                                            </td>
                                             
                                            <td style="width:33%">
                                                <center>
                                                <input id="btnN2" style="width:240px; margin-right:10px" type="button" value="Informes de Seguridad" class="btn btn-primary run-first"/>
                                                </center>
                                            </td>
                                            <td style="width:33%">
                                                <center>
                                                <input id="btnN3" style="width:240px" type="button" value="Evaluaciones de Riesgo" class="btn btn-primary run-first"/></center>
                                            </td>

                                        </tr>
                                    </table>
                                    <br />
                                    <table  width="100%" >
                                        <tr>
                                            <td colspan="4">
                                                <div runat="server" id="divN1">
                                                    <h5>Material Divulgativo</h5>
								                    <asp:GridView ID="grdN1" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="width:70px">
                                                                                Código      
                                                                            </th> 
                                                                            <th>
                                                                                Titulo      
                                                                            </th> 
                                                                            <th>
                                                                                Tipo de documento     
                                                                            </th> 
                                                                            <th>
                                                                                Riesgo asociado
                                                                            </th>
                                                                            <th>
                                                                                Centro de trabajo
                                                                            </th>                    
                                                                            <th style="width:100px">
                                                                                Fecha de publicación
                                                                            </th>   
                                                                            <th  style="width:50px">
                                                                                Descargas
                                                                            </th>
                                                                            <th  style="width:50px">
                                                                                Enlace
                                                                            </th>
                                                                            <% if (centroseleccionado.tipo == 4)
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Editar
                                                                            </th>
                                                                            <% } %>
                                                                            <% if (centroseleccionado.tipo == 4)
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
                                                                        <%=  
                                                                                    item.Cells[2].Text
                                
                                                                                    %>
 
                                                                            </td>    
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
                                                                             <%=  
                                                                                    item.Cells[5].Text
                                
                                                                                    %>
                                                                            </td>                     
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[6].Text != "&nbsp;")
                                                                               { %>
                                                                                <%= (DateTime.Parse(item.Cells[6].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[6].Text).Year).ToString()%>
                                                                            <% } %>
                                                                            </td>   
                                                                            <td style="text-align:center" class="task-desc">
                                                                             <%=  
                                                                                    item.Cells[8].Text
                                
                                                                                    %>
                                                                            </td> 
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Ver Fichero" href="/evr/Home/ObtenerMaterial/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                            </td>  
                                                                            <% if (centroseleccionado.tipo == 4)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/Home/detalle_material/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% } %>        
                                                                            <% if (centroseleccionado.tipo == 4)
                                                                               { %>                                                                  
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Fichero" onclick="if(!confirm('¿Está seguro de que desea eliminar este documento?')) return false;" href="/evr/Home/eliminar_material/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>      
                                                                            <% } %>               
                                                                            <% } %>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                                <br />

                                                                <% if (centroseleccionado.tipo == 4)
                                                                       { %>
                                                                       <div style="text-align:right">
                               
                                                                    <a href="/evr/Home/detalle_material/0" class="btn btn-primary" id="btnNuevo" title="Nuevo Material">Nuevo Material</a>
                                                                    </div>
                                                                    <% } %>
                                                            </div>
                                                        </div>
                                                        
                                                 </div>
                                                 
                                                <div runat="server" id="divN2">
                                                    <h5>Informes de Seguridad</h5>
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
                                                                            <th>
                                                                                Tipo de documento     
                                                                            </th> 
                                                                            <th>
                                                                                Elaborado por
                                                                            </th>
                                                                            <th>
                                                                                Mes informe
                                                                            </th>                    
                                                                            <th>
                                                                                Año informe
                                                                            </th>  
                                                                            <th style="width:100px">
                                                                                Fecha de publicación
                                                                            </th>  
                                                                            <th  style="width:50px">
                                                                                Descargas
                                                                            </th>
                                                                            <th  style="width:50px">
                                                                                Enlace
                                                                            </th>
                                                                            <% if (centroseleccionado.tipo == 4)
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Editar
                                                                            </th>
                                                                            <% } %>
                                                                            <% if (centroseleccionado.tipo == 4)
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Borrar
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
                                                                                    item.Cells[1].Text
                                
                                                                                    %>
 
                                                                            </td>    
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
                                                                            <td class="text">
     
                                                                                <%=  
                                                                                    item.Cells[4].Text
                                
                                                                                    %>
 
                                                                            </td>    
                                                                            <td class="text">   
                                                                                <% switch (item.Cells[5].Text)
                                                                                   {
                                                                                       case "1": %>
                                                                                         Enero
                                                                                         <%
                                                                                    break;
                                                                                       case "2": %>
                                                                                         Febrero
                                                                                         <%
                                                                                    break;
                                                                                       case "3": %>
                                                                                         Marzo
                                                                                         <%
                                                                                    break;
                                                                                       case "4": %>
                                                                                         Abril
                                                                                         <% break;
                                                                                       case "5": %>
                                                                                         Mayo
                                                                                         <% break;
                                                                                       case "6": %>
                                                                                         Junio
                                                                                         <% break;
                                                                                       case "7": %>
                                                                                         Julio
                                                                                         <% break;
                                                                                       case "8": %>
                                                                                         Agosto
                                                                                         <% break;
                                                                                       case "9": %>
                                                                                         Septiembre
                                                                                         <% break;
                                                                                       case "10": %>
                                                                                         Octubre
                                                                                         <% break;
                                                                                       case "11": %>
                                                                                         Noviembre
                                                                                         <% break;
                                                                                       case "12": %>
                                                                                         Diciembre
                                                                                         <% break;
                                                                                   } %>
                                                                                
 
                                                                            </td>          
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <%=  
                                                                                    item.Cells[6].Text
                                
                                                                                    %>
                                                                            </td>  
                                                                            
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[7].Text != "&nbsp;")
                                                                               { %>
                                                                                <%= (DateTime.Parse(item.Cells[7].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[7].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[7].Text).Year).ToString()%>
                                                                            <% } %>
                                                                            </td>                
                                                                            <td style="text-align:center" class="task-desc">
                                                                             <%=  
                                                                                    item.Cells[9].Text
                                
                                                                                    %>
                                                                            </td> 
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Ver Fichero" href="/evr/Home/ObtenerInformeSeguridad/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                            </td>  
                                                                            <% if (centroseleccionado.tipo == 4)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/Home/detalle_informe/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% } %>  
                                                                            <% if (centroseleccionado.tipo == 4)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Fichero" onclick="if(!confirm('¿Está seguro de que desea eliminar este documento?')) return false;" href="/evr/Home/eliminar_informeseguridad/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>     
                                                                            <% } %>                
                                                                            <% } %>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                                <br />

                                                                <% if (centroseleccionado.tipo == 4)
                                                                       { %>
                                                                       <div style="text-align:right">
                               
                                                                    <a href="/evr/Home/detalle_informe/0" class="btn btn-primary" id="A1" title="Nuevo Informe">Nuevo Informe</a>
                                                                    </div>
                                                                    <% } %>

                                                            </div>
                                                        </div>
                                                        
                                                 </div>

                                                <div runat="server" id="divN3">
                                                    <h5>Evaluaciones de Riesgos</h5>
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
                                                                            <th>
                                                                                Tipo de documento     
                                                                            </th> 
                                                                            <th>
                                                                                Centro de trabajo
                                                                            </th>
                                                                            <th>
                                                                                Elaborado por
                                                                            </th>    
                                                                            <th>
                                                                                Empresa
                                                                            </th>                 
                                                                            <th>
                                                                                Fecha de publicación
                                                                            </th>   
                                                                            <th  style="width:50px">
                                                                                Descargas
                                                                            </th>
                                                                            <th  style="width:50px">
                                                                                Enlace
                                                                            </th>
                                                                            <% if (centroseleccionado.tipo == 4)
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Editar
                                                                            </th>
                                                                            <% } %>
                                                                            <% if (centroseleccionado.tipo == 4)
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
                                                                                    item.Cells[1].Text
                                
                                                                                    %>
 
                                                                            </td>  
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
                                                                            <td class="text">
     
                                                                                <%=  
                                                                                    item.Cells[4].Text
                                
                                                                                    %>
 
                                                                            </td>               
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <%=  
                                                                                    item.Cells[5].Text
                                
                                                                                    %>
                                                                            </td>   
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <%=  
                                                                                    item.Cells[6].Text
                                
                                                                                    %>
                                                                            </td>                   
                                                                            <td style="text-align:center" class="task-desc">
                                                                            <% if (item.Cells[7].Text != "&nbsp;")
                                                                               { %>
                                                                                <%= (DateTime.Parse(item.Cells[7].Text).Day).ToString() + "/" + (DateTime.Parse(item.Cells[7].Text).Month).ToString() + "/" + (DateTime.Parse(item.Cells[7].Text).Year).ToString()%>
                                                                            <% } %>
                                                                            </td>    
                                                                            <td style="text-align:center" class="task-desc">
                                                                             <%=  
                                                                                    item.Cells[9].Text
                                
                                                                                    %>
                                                                            </td> 
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Ver Fichero" href="/evr/Home/ObtenerEvaluacionRiesgos/<%=item.Cells[0].Text %>");"><i class="icon-download"></i></a>
                                                                            </td>  
                                                                            <% if (centroseleccionado.tipo == 4)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/Home/detalle_evaluacion/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% } %>  
                                                                            <% if (centroseleccionado.tipo == 4)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Fichero" onclick="if(!confirm('¿Está seguro de que desea eliminar este documento?')) return false;" href="/evr/Home/eliminar_evaluacionriesgos/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>     
                                                                            <% } %>                
                                                                            <% } %>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                                <br />
                                                                <% if (centroseleccionado.tipo == 4)
                                                                       { %>
                                                                       <div style="text-align:right">
                               
                                                                    <a href="/evr/Home/detalle_evaluacion/0" class="btn btn-primary" id="A2" title="Nueva Evaluación">Nueva Evaluación</a>
                                                                    </div>
                                                                    <% } %>

                                                            </div>
                                                        </div>
                                                        
                                                 </div>

                                            </td>                                            
                                        </tr>
                                    </table>
							        </center>
                                 </div>                                    
    </div>
                                 
                                 

    
    </form>
</asp:Content>
