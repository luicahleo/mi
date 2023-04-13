<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">

    
    protected void Page_Load(object sender, EventArgs e)
    {
        List<MIDAS.Models.procesos> listaPadres = new List<MIDAS.Models.procesos>();

        listaPadres = (List<MIDAS.Models.procesos>)ViewData["padres"];


        if (Session["tipo"] != null)
        {
            ddlTipo.SelectedValue = Session["tipo"].ToString();
        }

        if (Session["EdicionEstMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionEstMensaje"].ToString() + "' });", true);
            Session["EdicionEstMensaje"] = null;
        }
        if (Session["errorEstMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["errorEstMensaje"].ToString() + "' });", true);
            Session["errorEstMensaje"] = null;
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
                        if (Session["permisoEst"].ToString() == "2")
                        {
                                    
                                     %>
                        <table style="margin-top:25px; ">
                            <tr>
                                <td style="padding-left:25px">
                                    <label>Perspectiva</label>
                                    <asp:DropDownList CssClass="form-control" ID="ddlTipo" runat="server"> 
                                        <asp:ListItem Value="C" Text="Cliente"></asp:ListItem>
                                        <asp:ListItem Value="I" Text="Interna"></asp:ListItem>
                                        <asp:ListItem Value="A" Text="Aprendizaje y crecimiento"></asp:ListItem>
                                        <asp:ListItem Value="R" Text="Responsabilidad social"></asp:ListItem>
                                        <asp:ListItem Value="F" Text="Financiera"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="padding-left:25px">
                                    <label>Nombre</label>
                                    <asp:TextBox  CssClass="form-control" ID="txtNombre" runat="server"></asp:TextBox>
                                </td>                                
                                <td style="padding-left:25px; padding-top:22px">
                                    
                                    <input id="GuardarElemento" type="submit" value="Añadir" class="btn btn-primary run-first">
                                </td>
                                
                            </tr>
                        </table> <hr /><% }
                        else
                        { %>
                        <br />
                        

                                   <%} %>
                       
                       <%--e5f6ff--%>
                        <div id="canvas" style="background-color:#FFF; padding:15px; border-radius:25px;">

                        <%--Perspectiva cliente--%>
                        <table width="100%" style="min-height:155px">
                        <tr>
                            <td style="width:90%">
                            <table width="100%">
                                <tr>
                              
                            <% 
                                List<MIDAS.Models.objetivos> listaObjetivos = (List<MIDAS.Models.objetivos>)ViewData["objetivosC"];

                                int count1 = 0;
                                foreach (MIDAS.Models.objetivos obj in listaObjetivos)
                                {

                                    count1 = count1 + 1;
                                    if (count1 == 5)
                                    {
                                                %>
                                                    <tr>
                                                    
                                                <%
                                            count1 = 0;
                                    }
                                        
                                        %>
                                        <td width="25%">


                                            <div style="height:150px; border-radius: 50%; max-width:255px; background-color:#0555FA;margin-right:15px; font-weight:bold; padding:25px; padding-top:10px; width:90%">
                                            <center>
                                            <div style="text-align:center"> 
                                                     <%
                                                        if (Session["permisoEst"].ToString() == "2")
                                                        {
                                                      %>
                                                 
                                                     <a title="Eliminar Objetivo" onclick="if(!confirm('¿Está seguro de que desea eliminar este objetivo?')) return false;" href="/evr/Home/Eliminar_Objetivo/<%=obj.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                     <% }
														else {%> <br/>
														<%} %>
                                                     </div>
    
                                            <table width="100%">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:center; padding-top:5px; width:80%">
                                                        <a style="text-decoration:none; color:White; vertical-align:middle;" href="/evr/Home/detalle_objetivo/<%= obj.id %>">
                                                         <%= obj.Nombre%></a>
                                                    </td>
          
                                                </tr>
                                                </table>
                                                
                                                
                                            
                                           
                                            </center>
                                            <br />
                                            </div>                                            
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
                                 %>              
                                 </tr>
                            </table>
                            </td>
                            <td>
                             <div class="Rotate-90" style="top:1px; font-size:x-large">Cliente</div>
                             </td></tr></table>
                                  <hr style="border-color:#41b9e6;" />


                        <%--Perspectiva interna--%>
                        <table width="100%" style="min-height:155px">
                        <tr>
                            <td style="width:90%">
                            <table width="100%">
                                <tr>
                              
                            <% 
                                listaObjetivos = (List<MIDAS.Models.objetivos>)ViewData["objetivosI"];

                                count1 = 0;
                                foreach (MIDAS.Models.objetivos obj in listaObjetivos)
                                {

                                    count1 = count1 + 1;
                                    if (count1 == 5)
                                    {
                                                %>
                                                    <tr>
                                                    
                                                <%
                                            count1 = 0;
                                    }
                                        
                                        %>
                                        <td width="25%">


                                            <div style="height:150px; border-radius: 50%; max-width:255px ; background-color:#0555FA;margin-right:15px; font-weight:bold; padding:25px; padding-top:10px; width:90%">
                                            <center>
                                            <div style="text-align:center"> 
                                                     <%
                                                        if (Session["permisoEst"].ToString() == "2")
                                                        {
                                                      %>
                                                 
                                                     <a title="Eliminar Objetivo" onclick="if(!confirm('¿Está seguro de que desea eliminar este objetivo?')) return false;" href="/evr/Home/Eliminar_Objetivo/<%=obj.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                     <% }
														else {%> <br/>
														<%} %>
                                                     </div>
                                                 
                                            <table width="100%">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:center; padding-top:5px; width:80%">
                                                        <a style="text-decoration:none; color:White; vertical-align:middle;" href="/evr/Home/detalle_objetivo/<%= obj.id %>">
                                                         <%= obj.Nombre%></a>
                                                    </td>
          
                                                </tr>
                                                </table>
                                                
                                                
                                            
                                           
                                            </center>
                                            <br />
                                            </div>                                            
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
                                 %>              
                                 </tr>
                            </table>
                            </td>
                            <td>
                             <div class="Rotate-90" style="top:1px; font-size:x-large">Interna</div>
                             </td></tr></table>
                                  <hr style="border-color:#41b9e6;" />

                           <%--Perspectiva aprendizaje--%>
                        <table width="100%" style="min-height:155px">
                        <tr>
                            <td style="width:90%">
                            <table width="100%">
                                <tr>
                              
                            <% 
                                listaObjetivos = (List<MIDAS.Models.objetivos>)ViewData["objetivosA"];

                                count1 = 0;
                                foreach (MIDAS.Models.objetivos obj in listaObjetivos)
                                {

                                    count1 = count1 + 1;
                                    if (count1 == 5)
                                    {
                                                %>
                                                    <tr>
                                                    
                                                <%
                                            count1 = 0;
                                    }
                                        
                                        %>
                                        <td width="25%">


                                            <div style="height:150px; border-radius: 50%; max-width:255px; background-color:#0555FA;margin-right:15px; font-weight:bold; padding:25px; padding-top:10px; width:90%">
                                            <center>
                                            <div style="text-align:center"> 
                                                     <%
                                                        if (Session["permisoEst"].ToString() == "2")
                                                        {
                                                      %>
                                                 
                                                     <a title="Eliminar Objetivo" onclick="if(!confirm('¿Está seguro de que desea eliminar este objetivo?')) return false;" href="/evr/Home/Eliminar_Objetivo/<%=obj.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                     <% }
														else {%> <br/>
														<%} %>
                                                     </div>
                                               
                                            <table width="100%">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:center; padding-top:5px; width:80%">
                                                        <a style="text-decoration:none; color:White; vertical-align:middle;" href="/evr/Home/detalle_objetivo/<%= obj.id %>">
                                                         <%= obj.Nombre%></a>
                                                    </td>
          
                                                </tr>
                                                </table>
                                                
                                                
                                            
                                           
                                            </center>
                                            <br />
                                            </div>                                            
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
                                 %>              
                                 </tr>
                            </table>
                            </td>
                            <td>
                             <div class="Rotate-90" style="top:15px; font-size:x-large">Aprendizaje</div>
                             </td></tr></table>
                                  <hr style="border-color:#41b9e6;" />


                                  <%--Perspectiva responsabilidad--%>
                        <table width="100%" style="min-height:155px">
                        <tr>
                            <td style="width:90%">
                            <table width="100%">
                                <tr>
                              
                            <% 
                                listaObjetivos = (List<MIDAS.Models.objetivos>)ViewData["objetivosR"];

                                count1 = 0;
                                foreach (MIDAS.Models.objetivos obj in listaObjetivos)
                                {

                                    count1 = count1 + 1;
                                    if (count1 == 5)
                                    {
                                                %>
                                                    <tr>
                                                    
                                                <%
                                            count1 = 0;
                                    }
                                        
                                        %>
                                        <td width="25%">


                                            <div style="height:150px; border-radius: 50%; max-width:255px; background-color:#0555FA;margin-right:15px; font-weight:bold; padding:25px; padding-top:10px; width:90%">
                                            <center>
                                            <div style="text-align:center"> 
                                                     <%
                                                        if (Session["permisoEst"].ToString() == "2")
                                                        {
                                                      %>
                                                 
                                                     <a title="Eliminar Objetivo" onclick="if(!confirm('¿Está seguro de que desea eliminar este objetivo?')) return false;" href="/evr/Home/Eliminar_Objetivo/<%=obj.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                     <% }
														else {%> <br/>
														<%} %>
                                                     </div>
                                                   
                                            <table width="100%">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:center; padding-top:5px; width:80%">
                                                        <a style="text-decoration:none; color:White; vertical-align:middle;" href="/evr/Home/detalle_objetivo/<%= obj.id %>">
                                                         <%= obj.Nombre%></a>
                                                    </td>
          
                                                </tr>
                                                </table>
                                                
                                                
                                            
                                           
                                            </center>
                                            <br />
                                            </div>                                            
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
                                 %>              
                                 </tr>
                            </table>
                            </td>
                            <td>
                             <div class="Rotate-90" style="top:45px; font-size:x-large">Responsabilidad</div>
                             </td></tr></table>
                                  <hr style="border-color:#41b9e6;" />

                                  <%--Perspectiva financiera--%>
                        <table width="100%" style="min-height:155px">
                        <tr>
                            <td style="width:90%">
                            <table width="100%">
                                <tr>
                              
                            <% 
                                listaObjetivos = (List<MIDAS.Models.objetivos>)ViewData["objetivosF"];

                                count1 = 0;
                                foreach (MIDAS.Models.objetivos obj in listaObjetivos)
                                {

                                    count1 = count1 + 1;
                                    if (count1 == 5)
                                    {
                                                %>
                                                    <tr>
                                                    
                                                <%
                                            count1 = 0;
                                    }
                                        
                                        %>
                                        <td width="25%">


                                            <div style="height:150px; border-radius: 50%; max-width:255px; background-color:#0555FA;margin-right:15px; font-weight:bold; padding:25px; padding-top:10px; width:90%">
                                            <center>
                                            <div style="text-align:center"> 
                                                     <%
                                                        if (Session["permisoEst"].ToString() == "2")
                                                        {
                                                      %>
                                                 
                                                     <a title="Eliminar Objetivo" onclick="if(!confirm('¿Está seguro de que desea eliminar este objetivo?')) return false;" href="/evr/Home/Eliminar_Objetivo/<%=obj.id %>");"><i style="color:White" class="icon-remove"></i></a>
                                                     <% }
														else {%> <br/>
														<%} %>
                                                     </div>
                                                
                                            <table width="100%">
                                                <tr>
                                                    <td  style="color:#FFFFFF;text-align:center; padding-top:5px; width:80%">
                                                        <a style="text-decoration:none; color:White; vertical-align:middle;" href="/evr/Home/detalle_objetivo/<%= obj.id %>">
                                                         <%= obj.Nombre%></a>
                                                    </td>
          
                                                </tr>
                                                </table>
                                                
                                                
                                            
                                           
                                            </center>
                                            <br />
                                            </div>                                            
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
                                 %>              
                                 </tr>
                            </table>
                            </td>
                            <td>
                             <div class="Rotate-90" style="top:10px; font-size:x-large">Financiera</div>
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
