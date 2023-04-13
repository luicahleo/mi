<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="principalHead" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Partes interesadas</title>
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {          

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
                            

            if (ViewData["stakeholdersN1"] != null)
            {
                grdN1.DataSource = ViewData["stakeholdersN1"];
                grdN1.DataBind();
            }
            if (ViewData["stakeholdersN2"] != null)
            {
                grdN2.DataSource = ViewData["stakeholdersN2"];
                grdN2.DataBind();
            }
            if (ViewData["stakeholdersN3"] != null)
            {
                grdN3.DataSource = ViewData["stakeholdersN3"];
                grdN3.DataBind();
            }

            if (Session["EditarStakeholderResultado"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarStakeholderResultado"].ToString() + "' });", true);
                Session.Remove("EditarStakeholderResultado");
            }

            if (Session["EditarStakeholderError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EditarStakeholderError"].ToString() + "' });", true);
                Session.Remove("EditarStakeholderError");
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
                                    <table width="50%">
                                        <tr>
                                            <td style="width:33%">
                                                <center>
                                                <input id="btnN1" style="width:180px; margin-right:10px" type="button" value="Nivel I" class="btn btn-primary run-first" /></center>
                                            </td>
                                             
                                            <td style="width:33%">
                                                <center>
                                                <input id="btnN2" style="width:180px; margin-right:10px" type="button" value="Nivel II" class="btn btn-primary run-first"/>
                                                </center>
                                            </td>
                                            <td style="width:33%">
                                                <center>
                                                <input id="btnN3" style="width:180px" type="button" value="Nivel III" class="btn btn-primary run-first"/></center>
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
                                                                            <th>
                                                                                Denominación      
                                                                            </th>                                                                            
                                                                            <% if (user.perfil == 1)
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Editar
                                                                            </th>
                                                                            <% } %>
                                                                            <% if (user.perfil == 1)
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
                                                                            <% if (user.perfil == 1)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/configuracion/editar_stakeholdern1/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% } %>        
                                                                            <% if (user.perfil == 1)
                                                                               { %>                                                                  
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Nivel" onclick="if(!confirm('¿Está seguro de que desea eliminar este nivel?')) return false;" href="/evr/configuracion/eliminar_stakeholdern1/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>      
                                                                            <% } %>               
                                                                            <% } %>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div style="float:right; margin-bottom:15px">
                                                        <% if (user.perfil == 1 || user.perfil == 3)
                                                           { %>
                                                        <a href="/evr/configuracion/editar_stakeholdern1/0" class="btn btn-primary" title="Nueva Parte Interesada">Nueva Parte Interesada</a>
                        <a onclick="window.history.go(-1)" title="Volver" class="btn btn-primary run-first">Volver</a>
                                                        
                                                        <% } %></div>
                                                 </div>
                                                 
                                                <div runat="server" id="divN2">
								                    <asp:GridView ID="grdN2" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="width:50px">
                                                                                Nivel I      
                                                                            </th> 
                                                                            <th>
                                                                                Denominación      
                                                                            </th>                                                                             
                                                                            <% if ((user.perfil == 1 || user.perfil == 3))
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Editar
                                                                            </th>
                                                                            <% } %>
                                                                            <% if ((user.perfil == 1 || user.perfil == 3))
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
                                                                            <% if ((user.perfil == 1 || user.perfil == 3))
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/configuracion/editar_stakeholdern2/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% } %>  
                                                                            <% if ((user.perfil == 1 || user.perfil == 3))
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Nivel" onclick="if(!confirm('¿Está seguro de que desea eliminar este nivel?')) return false;" href="/evr/configuracion/eliminar_stakeholdern2/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>     
                                                                            <% } %>                
                                                                            <% } %>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <div style="float:right; margin-bottom:15px">
                                                        <% if (user.perfil == 1 || user.perfil == 3)
                                                           { %>
                                                        <a href="/evr/configuracion/editar_stakeholdern2/0" class="btn btn-primary" title="Nueva Parte Interesada">Nueva Parte Interesada</a>
                        <a onclick="window.history.go(-1)" title="Volver" class="btn btn-primary run-first">Volver</a>
                                                        
                                                        <% } %></div>
                                                 </div>

                                                <div runat="server" id="divN3">
								                    <asp:GridView ID="grdN3" runat="server" Visible="false">
                                                    </asp:GridView>
                                                    <div class="block">
                                                            <div class="datatablePedido">
                                                                <table  width="100%"  class="table table-bordered">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="width:50px">
                                                                                Nivel I      
                                                                            </th> 
                                                                            <th>
                                                                                Nivel II      
                                                                            </th> 
                                                                            <th>
                                                                                Denominación      
                                                                            </th> 
                                                                            <th style="width:50px">
                                                                                Relevancia
                                                                            </th>                                                                            
                                                                            <% if ((user.perfil == 1))
                                                                               { %>
                                                                            <th  style="width:50px">
                                                                                Editar
                                                                            </th>
                                                                            <% } %>
                                                                            <% if ((user.perfil == 1))
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
                                                                            
                                                                
                                                                                <%  
                                                                                    if (item.Cells[4].Text == "S&#237;")
                                                                                    {
                                                                                         
                                                                                    %>
                                                                                    <td style="background-color:Green; color:White" class="text">
                                                                                        Sí
                                                                                    </td>  
                                                                                    <% }
                                                                                    else
                                                                                    { %>
                                                                                    <td style="background-color:Red; color:White" class="text">
                                                                                        No
                                                                                    </td>                                                                                          
                                                                                    <% } %>    
                                                                            
                                                                            <% if (user.perfil == 1)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Editar" href="/evr/configuracion/editar_stakeholdern3/<%=item.Cells[0].Text %>");"><i class="icon-pencil"></i></a>
                                                                            </td>  
                                                                            <% } %>  
                                                                            <% if (user.perfil == 1)
                                                                               { %>
                                                                            <td style="width:50px" class="text-center">
                                                                                <a title="Eliminar Nivel" onclick="if(!confirm('¿Está seguro de que desea eliminar este nivel?')) return false;" href="/evr/configuracion/eliminar_stakeholdern3/<%=item.Cells[0].Text %>");"><i class="icon-remove"></i></a>
                                                                            </td>     
                                                                            <% } %>                
                                                                            <% } %>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        
                                                        <div style="float:right; margin-bottom:15px">
                                                        <% if (user.perfil == 1 || user.perfil == 3)
                                                           { %>
                                                        <a href="/evr/configuracion/editar_stakeholdern3/0"  class="btn btn-primary" title="Nueva Parte Interesada">Nueva Parte Interesada</a>
                        <a onclick="window.history.go(-1)" title="Volver" class="btn btn-primary run-first">Volver</a>
                                                        
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
