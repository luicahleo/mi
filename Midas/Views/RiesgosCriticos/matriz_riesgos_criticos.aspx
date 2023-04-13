<%@ Page Title="tipos_riesgos" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">

    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    protected void Page_Load(object sender, EventArgs e)
    {
        Tecnologias.DataSource = ViewData["tecnologias"];
        Tecnologias.DataBind();
        TiposRiesgos.DataSource = ViewData["tipos_riesgos_criticos"];
        TiposRiesgos.DataBind();
        Instalaciones.DataSource = ViewData["areas"];
        Instalaciones.DataBind();
        GVSistemas.DataSource = ViewData["sistemas"];
        GVSistemas.DataBind();
        GVNivelescuatro.DataSource = ViewData["nivelescuatro"];
        GVNivelescuatro.DataBind();
        GVEquipos.DataSource = ViewData["equipos"];
        GVEquipos.DataBind();

        versionHI.Value = ViewData["version"].ToString();

        //ConjuntoChecks.DataSource = ViewData["tipos_riesgos"];
        //ConjuntoChecks.DataBind();
        GVMatriz.DataSource = ViewData["matriz_centro"];
        GVMatriz.DataBind();



        if (Session["usuario"] != null)
        {

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (Session["TiposRiesgosResultado"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarAmbitosResultado"].ToString() + "' });", true);
            Session["TiposRiesgosResultado"] = null;
        }

        //if (TempData["Notification"] != null)
        //{
        //    var tempData = TempData["Notification"].ToString();
        //    var lista = tempData.Split(',');

        //    string js = string.Format("MostrarMensaje('{0}','{1}');", lista[0], lista[1]);
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", js, true);
        //}
    }
</script>

<asp:Content ID="pedidoHead" ContentPlaceHolderID="head" runat="server">
    <title>Evaluaciones Riesgos </title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="<%=ResolveClientUrl("~/ext/js/RiesgosCriticos/matriz_riesgos_criticos.js") %>"></script>
    <%--<link href="<%=ResolveClientUrl("~/ext/css/Riesgos/matriz_riesgos.css") %>" rel="stylesheet" />--%>
    <link rel="stylesheet" href="<%=ResolveClientUrl("~/ext/css/RiesgosCriticos") %>/matriz_riesgos_criticos.css?v=1.123"/>
    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>Matriz de riesgos criticos<small>Tipos de riesgos registrados en el sistema.</small></h3>
        </div>
    </div>
    <!-- /page header -->

    <form id="formRiesgos" runat="server">
        <asp:GridView ID="Tecnologias" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="TiposRiesgos" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="Instalaciones" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="ConjuntoChecks" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="GVSistemas" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="GVEquipos" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="GVNivelescuatro" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="GVMatriz" runat="server" Visible="false" ClientIDMode="Static">
        </asp:GridView>
        <asp:HiddenField ID="versionHI" runat="server" ClientIDMode="Static" />

    </form>
    <!-- Tasks table -->


    <div class="block">
        <center>
            <%--<div class="loader"></div>--%>
            <div id="dialog" style="background: white;" hidden>
            </div>
            <div style="width: 95%" class="dataTable">
                <table id="datatableMatrizRiesgos" class="table table-bordered table-hover table-dark">
                    <thead>
                        <tr style="background-color: #41b9e6;">
                            <th width="500" nowrap style="font-size: 12px; background-color: #2f5496">Tipo de Riesgo<br />
                                __________________<br />
                                <br />
                                Tecnologías</th>
                            <% 
                                foreach (GridViewRow item in TiposRiesgos.Rows)
                                {
                            %>
                            <th class="text-center" style="background-color: #2f5496" tiporiesgocritico="<%=item.Cells[0].Text %>" title="<%= item.Cells[1].Text%>">
                                <div>
                                        <h2 style="color: white; font-size:12px;" ><%= item.Cells[1].Text%></h2>
                                </div>
                                <%--                                <div>
                                         <img id="Riesgo_<%=item.Cells[0].Text%>" style="width: 30px; height: 30px;" src="<%= MIDAS.Models.Datos.ObtenerImagenRiesgo(int.Parse(item.Cells[0].Text)) %>" action="GuardarImagen" method="post" enctype="multipart/form-data">
                                        </div>--%>
                            </th>
                            <%
                                }

                            %>

                            <%-- <th class="task-desc icon-plus" style="text-align: center; color: white;background-color:#2f5496; font-size: 12px;" font-weight: bold;" title="Nuevo">
                            </th>--%>
                            <th class="task-desc icon-pencil" style="text-align: center; color: white;background-color:#2f5496; font-size: 12px;" font-weight: bold" title="Editar">
                            </th>
                            <th class="task-desc icon-cancel" style="text-align: center; color: white;background-color:#2f5496; font-size: 12px;" font-weight: bold" title="Eliminar">
                            </th>
                            <%--<th class="task-desc icon-camera2" style="text-align: center; color: white;background-color:#2f5496; font-size: 12px;" font-weight: bold" title="Foto">
                            </th>--%>
                        </tr>
                    </thead>

                    <tbody>
                        <%
                            foreach (GridViewRow tecno in Tecnologias.Rows)
                            {%>
                                <tr style="background-color: #ff0f64" class="Tecnologia" tecnologia="<%=tecno.Cells[0].Text %>" version='<%=ViewData["version"] %>'>
                                    <td columnaprincipal="" colspan="10" style="font-size: 15px; font-weight: bold; color: white;"><%=tecno.Cells[1].Text %></td>
                                    <td colspan="1" style="font-size: 15px; font-weight: bold; color: white;">
                                        <i class="icon-arrow-up" style="color: white; float: right"></i>
                                    </td>
                                </tr>
                                <% 
                                    List<MIDAS.Models.areanivel1> areas = (List<MIDAS.Models.areanivel1>)ViewData["areas"];
                                    List<MIDAS.Models.areanivel2> sistemas = (List<MIDAS.Models.areanivel2>)ViewData["sistemas"];
                                    List<MIDAS.Models.areanivel3> equipos = (List<MIDAS.Models.areanivel3>)ViewData["equipos"];
                                    List<MIDAS.Models.areanivel4> nivelescuatro = (List<MIDAS.Models.areanivel4>)ViewData["nivelescuatro"];
                                    List<MIDAS.Models.areas_imagenes> imagenes_areas = (List<MIDAS.Models.areas_imagenes>)ViewData["imagenesAreas"];
                                    List<MIDAS.Models.areas2_imagenes> imagenes_sistemas = (List<MIDAS.Models.areas2_imagenes>)ViewData["imagenesSistemas"];
                                    List<MIDAS.Models.equipos_imagenes> imagenes_equipos = (List<MIDAS.Models.equipos_imagenes>)ViewData["imagenesEquipos"];
                                    List<MIDAS.Models.areas4_imagenes> imagenes_areas4 = (List<MIDAS.Models.areas4_imagenes>)ViewData["imagenesareascuatro"];

                                    //Intalaciones son las areas
                                    foreach (GridViewRow item in Instalaciones.Rows)
                                    {
                                        if (item.Cells[4].Text == tecno.Cells[0].Text)
                                        {%>
                                <tr style="background-color: #426ee5; font-weight: bold; color: white;" class="AREA" id="AREA_<%=item.Cells[0].Text%>" area="<%=item.Cells[0].Text%>" tecnologia="<%=tecno.Cells[0].Text %>" version='<%=ViewData["version"] %>'>
                                    <td class="task-desc" columnaprincipal="<%= item.Cells[2].Text %>" style="width: 500px; overflow: auto; font-size: 9px;">
                                        <%=   item.Cells[2].Text %>
                                        <%
                                            if (sistemas.Where(x => x.id_areanivel1 == int.Parse(item.Cells[0].Text)).Count() > 0)
                                            {%>
                                        <i class="icon-arrow-up" style="color: #ff0f64; height: 5px; float: right; font-size: 10px;" hidden></i>
                                        <i class="icon-arrow-down" style="color: #ff0f64; height: 5px; float: right; font-size: 10px;"></i>
                                        <%}%>  
                             
                                    </td>

                                    <% for (int z = 0; z < TiposRiesgos.Rows.Count; z++)
                                        {
                                            var numeroRiesgo = z + 1;%>
                                    <td class="task-desc" style="text-align: center;" title="<%= TiposRiesgos.Rows[z].Cells[1].Text.ToString()%>">
                                        <input disabled class="form-check-input" type="checkbox" value="" id="<%="chk_" + item.Cells[0].Text + "_" + numeroRiesgo.ToString() %>" atributoPrueba="hola" />
                                    </td>
                                    <% } %>

                                    <%--<td class="task-desc" style="text-align: center;">
                                        <div class="dropdown">
                                            <button class="dropbtn icon-plus" style="color: white; background-color: transparent;"></button>
                                            <div id="DropArea" class="dropdown-content">
                                                <a class="Nueva_Area">Nivel Actual</a>
                                                <a class="Nuevo_Sistema">Subnivel</a>
                                            </div>
                                        </div>

                                    </td>--%>
                                    <td class="task-desc" style="text-align: center;">
                                        <%  if (sistemas.Where(x => x.id_areanivel1 == int.Parse(item.Cells[0].Text)).Count() == 0)
                                            {%>
                                        <%--<a class="EditarRegistro" id="<%="btn_edit_" + item.Cells[1].Text  %>" style="display:none;">
                                            <i class="icon-pencil" style="color: gray; display:none;" ></i>
                                        </a>--%>
                                        <% }
                                            else
                                            { %>
                                        <%-- Boton editar areas, para la matrizCritica no lo necesitamos --%>
                                        <%--<a class="EditarRegistro" conhijos="si" id="<%="btn_edit_" + item.Cells[1].Text  %>">
                                            <i class="icon-pencil" style="color: gray;"></i>
                                        </a>--%>
                                        <% } %>
                                    </td>

                                    <td class="task-desc" style="text-align: center;">
                                        <%--<a class="Eliminar" id="<%="btn_del_" + item.Cells[1].Text  %>">
                                            <i class="icon-remove" style="color: red;"></i>
                                        </a>--%>
                                    </td>
                                    <%--<td class="task-desc" style="text-align: center;">
                                        <%if (imagenes_areas.Where(x => x.id_areanivel1 == int.Parse(item.Cells[0].Text)).Count() > 0)
                                            { %>
                                        <a class="Subir" id="<%="btn_sub_" + item.Cells[0].Text  %>">
                                            <i class="icon-camera" style="color: green;" identificador="<%=item.Cells[0].Text%>"></i>
                                        </a>
                                        <% }
                                            else
                                            { %>
                                        <a class="Subir" id="<%="btn_sub_" + item.Cells[0].Text  %>">
                                            <i class="icon-camera2" style="color: dimgray;" identificador="<%=item.Cells[0].Text%>"></i>
                                        </a>

                                        <% } %>
                                    </td>--%>
                                </tr>
                                <%
                                    foreach (GridViewRow itemS in GVSistemas.Rows)
                                    {

                                        if (itemS.Cells[3].Text == item.Cells[0].Text)
                                        {

                                %>
                                <tr style="background-color: #b4c6e7;" hidden class="SISTEMA" id="AREA_<%=item.Cells[0].Text%>_SISTEMA_<%=itemS.Cells[0].Text%>" area="<%=item.Cells[0].Text%>" sistema="<%=itemS.Cells[0].Text%>" tecnologia="<%=tecno.Cells[0].Text %>">
                                    <td class="" columnaprincipal="<%= item.Cells[2].Text %>" style="font-weight: bold; font-size: 9px;">


                                        <%= itemS.Cells[2].Text %>

                                        <%

                                            if (equipos.Where(x => x.id_areanivel2 == int.Parse(itemS.Cells[0].Text)).Count() > 0)
                                            {%>
                                        <i class="icon-arrow-up" style="color: #ff0f64; float: right; font-size: 10px;" hidden></i>
                                        <i class="icon-arrow-down" style="color: #ff0f64; float: right; font-size: 10px;"></i>
                                        <% }%>                   
                                
                            
                                    </td>

                                    <%
                                        for (int i = 0; i < TiposRiesgos.Rows.Count; i++)
                                        {
                                            var numeroRiesgo = i + 1;%>
                                    <td class="" style="text-align: center;" title="<%= TiposRiesgos.Rows[i].Cells[1].Text.ToString()%>">
                                        <input disabled class="form-check-input" type="checkbox" value="" id="<%="chk_" + itemS.Cells[0].Text + "_" + numeroRiesgo.ToString() %>" onchange="SetCheckElemento(this);" />
                                    </td>
                                    <%}%>
                                    <%--<td class="task-desc" style="text-align: center;">
                                        <div class="dropdown">
                                            <button class="dropbtn icon-plus" style="color: white; background-color: transparent;"></button>
                                            <div id="DropSistema" class="dropdown-content">
                                                <a class="Nuevo_Sistema">Nivel Actual</a>
                                                <a class="Nuevo_Equipo">Subnivel</a>
                                            </div>
                                        </div>

                                    </td>--%>
                                     <%if (equipos.Where(x => x.id_areanivel2 == int.Parse(itemS.Cells[0].Text)).Count() == 0)
                                         {%>
                                                <td class="task-desc" style="text-align: center;">
                                        
                                                    <% if (equipos.Where(x => x.id_areanivel2 == int.Parse(itemS.Cells[0].Text)).Count() == 0)
                                                        {%>
                                                    <a class="EditarRegistro" id="<%="btn_edit_" + itemS.Cells[1].Text%>">
                                                        <i class="icon-pencil" style="color: gray;"></i>
                                                    </a>
                                                    <% }
                                                        else
                                                        {%>
                                                    <a class="EditarRegistro" conhijos="si" id="<%="btn_edit_" + itemS.Cells[1].Text%>">
                                                        <i class="icon-pencil" style="color: gray;"></i>
                                                    </a>
                                                    <%} %>
                                                </td>
                                                <td class="task-desc" style="text-align: center;">
                                                    <a class="Cancel" id="<%="btn_del_" + itemS.Cells[1].Text  %>">
                                                        <i class="icon-cancel" style="color: red;"></i>
                                                    </a>
                                                </td>
                                    <%}
                                        else
                                        {%> 
                                        <td class="task-desc" style="text-align: center;">
                                        </td>
                                        <td class="task-desc" style="text-align: center;">
                                        </td>
                                    <%} %>

                                    <%--<td class="task-desc" style="text-align: center;">
                                        <%if (imagenes_sistemas.Where(x => x.id_areanivel2 == int.Parse(itemS.Cells[0].Text)).Count() > 0)
                                            { %>
                                        <a class="Subir" id="<%="btn_sub_2_" + itemS.Cells[0].Text  %>">
                                            <i class="icon-camera" style="color: green;" identificador="<%=itemS.Cells[0].Text%>"></i>
                                        </a>
                                        <% }
                                            else
                                            { %>
                                        <a class="Subir" id="<%="btn_sub_2_" + itemS.Cells[0].Text  %>">
                                            <i class="icon-camera2" style="color: dimgray;" identificador="<%=itemS.Cells[0].Text%>"></i>
                                        </a>

                                        <% } %>
                                    </td>--%>
                                </tr>
                                <% 
                                    foreach (GridViewRow itemE in GVEquipos.Rows)
                                    {

                                        if (itemE.Cells[3].Text == itemS.Cells[0].Text)
                                        {

                                %>
                                <tr style="background-color: #f4b083; font-weight: bold;  font-size: 9px;" hidden class="EQUIPO" id="AREA_<%=item.Cells[0].Text%>_SISTEMA_<%=itemS.Cells[0].Text%>_EQUIPO_<%=itemE.Cells[0].Text%>" area="<%=item.Cells[0].Text%>" sistema="<%=itemS.Cells[0].Text%>" equipo="<%=itemE.Cells[0].Text%>" tecnologia="<%=tecno.Cells[0].Text %>">
                                    <td class="task-desc" columnaprincipal="<%= item.Cells[2].Text %>">
                                        <%=itemE.Cells[2].Text %>
                                        <% if (nivelescuatro.Where(x => x.id_areanivel3 == int.Parse(itemE.Cells[0].Text)).Count() > 0)
                                            {%>
                                        <i class="icon-arrow-up" style="color: #ff0f64; float: right; font-size: 10px;" hidden></i>
                                        <i class="icon-arrow-down" style="color: #ff0f64; float: right; font-size: 10px;"></i>
                                        <% }%>        
                                
                                    </td>



                                    <%

                                        for (int i = 0; i < TiposRiesgos.Rows.Count; i++)
                                        {
                                            var numeroRiesgo = i + 1;%>
                                    <td class="task-desc" style="text-align: center;" title="<%= TiposRiesgos.Rows[i].Cells[1].Text.ToString()%>">
                                        <input disabled class="form-check-input" type="checkbox" value="" id="<%="chk_" + itemS.Cells[1].Text + "_" + itemE.Cells[1].Text + "_" + numeroRiesgo.ToString() %>" />
                                    </td>
                                    <%}%>
                                    <%--<td class="task-desc" style="text-align: center;">
                                        <div class="dropdown">
                                            <button class="dropbtn icon-plus" style="color: white; background-color: transparent;"></button>
                                            <div id="DropEquipo" class="dropdown-content">
                                                <a class="Nuevo_Equipo">Nivel Actual</a>
                                                <a class="Nuevo_NIVELCUATRO">Subnivel</a>
                                            </div>
                                        </div>
                                    </td>--%>
                                    <% if (nivelescuatro.Where(x => x.id_areanivel3 == int.Parse(itemE.Cells[0].Text)).Count() == 0)
                                        {%>
                                    <td class="task-desc" style="text-align: center;">
                                        <% if (nivelescuatro.Where(x => x.id_areanivel3 == int.Parse(itemE.Cells[0].Text)).Count() == 0)
                                            {%>
                                        <a class="EditarRegistro" id="<%="btn_edit_" + itemE.Cells[1].Text%>">
                                            <i class="icon-pencil" style="color: gray;"></i>
                                        </a>
                                        <% }
                                            else
                                            {%>
                                        <a class="EditarRegistro" conhijos="si" id="<%="btn_edit_" + itemE.Cells[1].Text%>" atributo="Prueba">
                                            <i class="icon-pencil" style="color: gray;"></i>
                                        </a>
                                        <%} %>
                                    </td>
                                    <td class="task-desc" style="text-align: center;">
                                        <a class="Cancelar" id="<%="btn_del_" + itemE.Cells[1].Text  %>">
                                            <i class="icon-cancel" style="color: red;"></i>
                                        </a>
                                    </td>
                                    <%}
                                    else
                                    { %>
                                     <td class="task-desc" style="text-align: center;">
                                        </td>
                                        <td class="task-desc" style="text-align: center;">
                                        </td>
                                    <%} %>
                                    <%--<td class="task-desc" style="text-align: center;">
                                        <%if (imagenes_equipos.Where(x => x.id_areanivel3 == int.Parse(itemE.Cells[0].Text)).Count() > 0)
                                            { %>
                                        <a class="Subir" id="<%="btn_sub_3_" + itemE.Cells[0].Text  %>">
                                            <i class="icon-camera" style="color: green;" identificador="<%=itemE.Cells[0].Text%>"></i>
                                        </a>
                                        <% }
                                            else
                                            { %>
                                        <a class="Subir" id="<%="btn_sub_3_" + itemE.Cells[0].Text  %>">
                                            <i class="icon-camera2" style="color: dimgray;" identificador="<%=itemE.Cells[0].Text%>"></i>
                                        </a>
                                        <% } %>
                                    </td>--%>
                                </tr>
                                <%foreach (GridViewRow item4 in GVNivelescuatro.Rows)
                                    {
                                        if (item4.Cells[3].Text == itemE.Cells[0].Text)
                                        { %>
                                <tr style="background-color: #fbe4d5; font-weight: bold;  font-size: 9px;" hidden class="NIVELCUATRO"
                                    id="AREA_<%=item.Cells[0].Text%>_SISTEMA_<%=itemS.Cells[0].Text%>_EQUIPO_<%=itemE.Cells[0].Text%>_NIVELCUATRO_<%=item4.Cells[0].Text%>"
                                    area="<%=item.Cells[0].Text%>" sistema="<%=itemS.Cells[0].Text%>" equipo="<%=itemE.Cells[0].Text%>"
                                    nivelcuatro="<%=item4.Cells[0].Text%>"
                                    tecnologia="<%=tecno.Cells[0].Text %>">

                                    <td class="task-desc" columnaprincipal="<%= item.Cells[2].Text %>">
                                        <%=item4.Cells[2].Text %>
                                    </td>

                                    <%

                                        for (int i = 0; i < TiposRiesgos.Rows.Count; i++)
                                        {
                                            var numeroRiesgo = i + 1;%>
                                    <td class="task-desc" style="text-align: center;" title="<%= TiposRiesgos.Rows[i].Cells[1].Text.ToString()%>">
                                        <input disabled class="form-check-input" type="checkbox" value="" id="<%="chk_" + itemS.Cells[1].Text + "_" + itemE.Cells[1].Text + "_" + item4.Cells[1].Text + "_" + numeroRiesgo.ToString() %>" />
                                    </td>
                                    <%}%>
                                    <%--<td class="task-desc" style="text-align: center;">
                                                          <a id="<%="btn_new_" + item4.Cells[1].Text  %>" name="Nuevo_NivelCuatro">
                                                                                    <i class="icon-plus" style="color: white;"></i>
                                                                                </a>
                                    </td>--%>

                                    <td class="task-desc" style="text-align: center;">
                                        <a class="EditarRegistro" id="<%="btn_edit_" + item4.Cells[1].Text  %>">
                                            <i class="icon-pencil   " style="color: gray;"></i>
                                        </a>
                                    </td>
                                    <td class="task-desc" style="text-align: center;">
                                        <a class="Cancel" id="<%="btn_cancel_" + item4.Cells[1].Text  %>">
                                            <i class="icon-cancel" style="color: red;"></i>
                                        </a>
                                    </td>
                                    <%--<td class="task-desc" style="text-align: center;">
                                        <%if (imagenes_areas4.Where(x => x.id_areanivel4 == int.Parse(item4.Cells[0].Text)).Count() > 0)
                                            { %>
                                        <a class="Subir" id="<%="btn_sub_4_" + item4.Cells[0].Text  %>">
                                            <i class="icon-camera" style="color: green;" identificador="<%=item4.Cells[0].Text%>"></i>
                                        </a>
                                        <% }
                                            else
                                            { %>
                                        <a class="Subir" id="<%="btn_sub_4_" + item4.Cells[0].Text  %>">
                                            <i class="icon-camera2" style="color: dimgray;" identificador="<%=item4.Cells[0].Text%>"></i>
                                        </a>

                                        <% } %>
                                    </td>--%>
                                </tr>


                                    <%}
                                                        }
                                                    }
                                                }
                                            }
                                        }%>

                                 <%
                                         }
                                     }
                                     if (user.perfil == 1 || user.perfil == 3)
                                     {
                                    %>
                                    <%-- <td class="text-center">
                                                            <a href="/evr/Configuracion/tipos_riesgos/<%= item.Cells[0].Text %>" title="Editar"><i class="icon-search3"></i></a>
                                                        </td>
                                                        <td class="text-center">
                                                            <a onclick="return confirm('¿Está seguro que desea eliminar el registro?');" href="/evr/Configuracion/tipos_riesgos/<%= item.Cells[0].Text %>" title="Eliminar"><i class="icon-remove"></i></a>
                                                        </td>--%>
                                    <% }
                                        } %>
                    </tbody>
                </table>
            </div>


            <!-- Modal -->
            <div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLongTitle">Añadir Tecnología</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <select id="combotecnologias"></select>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Salir</button>
                            <button onclick="CreaTecnologia()" type="button" class="btn btn-primary">Seleccionar Tecnologías</button>
                        </div>
                    </div>
                </div>
            </div>



        </center>
    </div>
    <!-- /tasks table -->

    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left"></div>
        <div style="text-align: right">
            <button type="button" class="btn btn-primary run-first" data-toggle="modal" data-target="#exampleModalCenter">
                Añadir tecnología
            </button>

        </div>
        <br />
        <div style="text-align: right">
            <% if (user.perfil == 1 || user.perfil == 3)
                { %>
            <%--        <a href="/evr/Riesgos/finalizar_version/<%=ViewData["version"].ToString()%>" title="Volver" class="btn btn-primary run-first">Finalizar Evaluacion de Riesgos</a>--%>
            <% } %>
            <%--<a href="/evr/Riesgos/lista_matrices" title="Volver" class="btn btn-primary run-first">Guardar</a>--%>
        </div>
        <p>
            <br />
        </p>
        <div id="circle angled">
        </div>
        <div class="spinner-border"></div>
    </div>
    <!-- /footer -->
</asp:Content>