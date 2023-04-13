<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            if (ViewData["tecnologias"] != null)
            {
                ddlTecnologias.DataSource = ViewData["tecnologias"];
                ddlTecnologias.DataValueField = "id";
                ddlTecnologias.DataTextField = "nombre";
                ddlTecnologias.DataBind();

            }

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            DatosRiesgos.DataSource = ViewData["riesgos"];
            DatosRiesgos.DataBind();
            DatosSituaciones.DataSource = ViewData["situaciones"];
            DatosSituaciones.DataBind();
            DatosMedidas.DataSource = ViewData["medidas"];
            DatosMedidas.DataBind();
            DatosMedidasRiesgo.DataSource = ViewData["medidasRiesgos"];
            DatosMedidasRiesgo.DataBind();
            DatosMedidasRiesgoImagen.DataSource = ViewData["medidasRiesgosImagen"];
            DatosMedidasRiesgoImagen.DataBind();
            DatosApartados.DataSource = ViewData["apartadosRiesgos"];
            DatosApartados.DataBind();


            DatosMedidasGenerales.DataSource = ViewData["medidasRiesgosGenerales"];
            DatosMedidasGenerales.DataBind();
            DatosMedidasGeneralesImagen.DataSource = ViewData["medidasRiesgosImagenGenerales"];
            DatosMedidasGeneralesImagen.DataBind();
            DatosApartadosGenerales.DataSource = ViewData["apartadosRiesgosGenerales"];
            DatosApartadosGenerales.DataBind();



            hiTecnologia.Value = ViewData["tecnologia"].ToString();
            hiCentro.Value = ViewData["centroSeleccionado"].ToString();
            DatosParametricaMedidas.DataSource = ViewData["parametricaMedidas"];
            DatosParametricaMedidas.DataBind();
        }


        if (Session["Editarsistema"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EditarMedida"].ToString() + "' });", true);
            Session["EliminarSistema"] = null;
        }
    }
</script>

<asp:Content ID="versionesHead" ContentPlaceHolderID="head" runat="server">
    <title>DIMAS</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="<%=ResolveClientUrl("~/ext/js/Riesgos/medidas_preventivas.js") %>"></script>
    <link href="<%=ResolveClientUrl("~/ext/css/Riesgos/medidas_preventivas.css") %>" rel="stylesheet" />

    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>Medidas Preventivas <small>Medidas preventivas asociadas a situaciones de riesgo.</small></h3>
        </div>
    </div>
    <!-- /page header -->
    <form id="form1" runat="server">
        <asp:GridView ID="DatosRiesgos" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosSituaciones" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosMedidas" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosMedidasRiesgo" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosMedidasRiesgoImagen" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosApartados" runat="server" Visible="false">
        </asp:GridView>

        <asp:GridView ID="DatosMedidasGenerales" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosMedidasGeneralesImagen" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosApartadosGenerales" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosParametricaMedidas" runat="server" Visible="false">
        </asp:GridView>
        <asp:HiddenField ID="hiTecnologia" runat="server" ClientIDMode="Static"></asp:HiddenField>
        <asp:HiddenField ID="hiCentro" runat="server" ClientIDMode="Static"></asp:HiddenField>
        <div class="block">
            <center>
                <div style="width: 95%" class="">
                    <asp:Label runat="server" Text="Seleccione Tecnología" Visible="false"></asp:Label>
                    <asp:DropDownList ID="ddlTecnologias" CssClass="form-control" runat="server" ClientIDMode="Static" onchange="seleccionTecnologia()" Width="10%" Visible="false">
                    </asp:DropDownList>
                </div>
            </center>
        </div>
    </form>
    <!-- Tasks table -->
    <div class="block">

        <center>
            <div class="loader"></div>
            <div style="width: 95%" class="">
                <div style="text-align: left; width: 100%">
                    <%--  <a href="/evr/Riesgos/seleccionar_tecnologia" class="btn btn-primary run-first">Crear Matriz de Riesgos </a>--%>
                    <a href="/evr/Riesgos/SeleccionarTodos/" title="" class="btn btn-primary run-first">Seleccionar Todo</a>
                    <a href="/evr/Riesgos/DesSeleccionarTodos/" title="" class="btn btn-primary run-first">Des-Seleccionar Todo</a>
                </div>

                <div style="text-align: right; width: 100%">
                    <%--  <a href="/evr/Riesgos/seleccionar_tecnologia" class="btn btn-primary run-first">Crear Matriz de Riesgos </a>--%>
                </div>
                <br />
                <table class="table table-bordered" style="font-size: 16px;">
                    <thead style="background-color: lightgrey;">
                        <tr style="background-color: lightgrey;">
                            <th class="titMedidasGenerales">Medidas Preventivas Generales <i class="icon-arrow-up" style="font-size: 10px;"></i></th>

                        </tr>
                        <tbody id="medidasGenerales">
                        </tbody>
                </table>

                <table hidden class="MedidasGeneralesCentro table-bordered" style="background-color: lavenderblush; width: 100%;">
                    <tbody>
                        <%foreach (GridViewRow apar in DatosApartadosGenerales.Rows)
                            {%>
                        <tr class="MedidasGenerales">

                            <%bool contiene = false; %>
                            <%foreach (GridViewRow medidaGeneral in DatosMedidasGenerales.Rows)
                                { %>
                            <%if (medidaGeneral.Cells[5].Text == apar.Cells[0].Text)
                                    {
                                        contiene = true;
                                    }
                                } %>


                            <%if (contiene)
                                {%>
                            <td class="text-center" style="width: 15%;">
                                <%=apar.Cells[1].Text %>
                            </td>

                            <%}%>
                            <td colspan="4">
                                <%foreach (GridViewRow medidaGeneral in DatosMedidasGenerales.Rows)
                                    { %>

                                <%if (medidaGeneral.Cells[5].Text == apar.Cells[0].Text)
                                    {

                                %>
                                <table class="table table-bordered">
                                    <tr>

                                        <td width="80%" style="max-width: 200px; overflow: auto"><%
                                                                                                     string[] stringSeparators = new string[] { "[SALTO]" };
                                                                                                     string[] lines = medidaGeneral.Cells[1].Text.Split(stringSeparators, StringSplitOptions.None);
                                                                                                     if (medidaGeneral.Cells[1].Text.Contains("[SALTO]"))
                                                                                                     {
                                                                                                         int contador = 1;
                                                                                                         foreach (string s in lines)
                                                                                                         {
                                                                                                             if (contador == 1)
                                                                                                             {%>
                                            <%=s%><br />
                                            <% }
                                                else
                                                {%>
                                            <%=" - " +s%><br />
                                            <%

                                                } %>
                                            <%contador++;
                                                    }
                                                }
                                                else
                                                {%>
                                            <%=medidaGeneral.Cells[1].Text %>
                                            <%} %>
                                                                                               
                                                                               

                                        </td>
                                        <td style="width: 10%;" class="text-center">
                                            <%if (!string.IsNullOrEmpty(MIDAS.Models.Datos.obtenerImagenMedidasGenerales(int.Parse(medidaGeneral.Cells[0].Text))))
                                                { %>
                                            <img id="IMG_<%=medidaGeneral.Cells[0].Text%>" style="width: 40px; height: 40px;" src="<%= MIDAS.Models.Datos.obtenerImagenMedidasGenerales(int.Parse(medidaGeneral.Cells[0].Text)) %>" action="GuardarImagen" method="post" enctype="multipart/form-data"></td>
                                        <% }%>

                                        <%--    <td class="text-center">
                                            <input id="file_<%=medidaGeneral.Cells[0].Text%>" type="file" name="img" multiple accept="image/*" mdgeneral="<%=medidaGeneral.Cells[0].Text%>" class="file" onchange="file_changed(this);" onclick="this.value=null;ocultar(this);"></td>
                                     
                                        <td>
                                            <button id="Subir_<%=medidaGeneral.Cells[0].Text%>" style="display: none;" class="subirImagen" mdgeneral="<%=medidaGeneral.Cells[0].Text%>">Subir</button></td>--%>
                                    </tr>
                                </table>
                                <%

                                        }
                                    } %>

                            </td>
                        </tr>
                        <tr class="MedidasGeneralesfijas">

                            <%bool contieneimg = false; %>
                            <%foreach (GridViewRow medidaGeneral in DatosMedidasGeneralesImagen.Rows)
                                { %>
                            <%if (medidaGeneral.Cells[5].Text == apar.Cells[0].Text)
                                    {
                                        contieneimg = true;
                                    }
                                } %>


                            <%if (contieneimg)
                                {%>
                            <td class="text-center" style="width: 15%;">
                                <%=apar.Cells[1].Text %> (IMÁGEN)
                            </td>

                            <%}%>
                            <td colspan="4">
                                <%foreach (GridViewRow medidaGeneral in DatosMedidasGeneralesImagen.Rows)
                                    { %>

                                <%if (medidaGeneral.Cells[5].Text == apar.Cells[0].Text)
                                    {

                                %>
                                <table class="table table-bordered">
                                    <tr>
                                        <td width="20%" class="text-center">
                                            <img id="IMG_G_<%=medidaGeneral.Cells[0].Text%>" style="width: 380px; height: 380px;"
                                                src="<%=medidaGeneral.Cells[5].Text %>"
                                                action="GuardarImagen" method="post" enctype="multipart/form-data"></td>
                                        <td width="40%" style="max-width: 300px; overflow: auto"><%
                                                                                                     string[] stringSeparators = new string[] { "[SALTO]" };
                                                                                                     string[] lines = medidaGeneral.Cells[1].Text.Split(stringSeparators, StringSplitOptions.None);
                                                                                                     if (medidaGeneral.Cells[1].Text.Contains("[SALTO]"))
                                                                                                     {
                                                                                                         int contador = 1;
                                                                                                         foreach (string s in lines)
                                                                                                         {
                                                                                                             if (contador == 1)
                                                                                                             {%>
                                            <%=s%><br />
                                            <% }
                                                else
                                                {%>
                                            <%=" - " +s%><br />
                                            <%

                                                } %>
                                            <%contador++;
                                                    }
                                                }
                                                else
                                                {%>
                                            <%=medidaGeneral.Cells[1].Text %>
                                            <%} %>
                                                                                               
                                                                               

                                        </td>


                                    </tr>
                                </table>
                                <%

                                        }
                                    } %>

                            </td>
                        </tr>
                        <%} %>
                    </tbody>
                </table>


                <br />

                <table class="table table-bordered" style="font-size: 16px;">
                    <thead style="background-color: lightgrey;">
                        <tr colspan="3" style="background-color: lightgrey;">
                            <th>Medidas Preventivas Específicas</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <% List<MIDAS.Models.parametrica_medidas> listaParametrica = MIDAS.Models.Datos.ListarParametricaMedidas();%>
                <%  
                    foreach (GridViewRow item in DatosRiesgos.Rows)
                    {

                %>
                <div class="Riesgo_<%=item.Cells[0].Text %>">
                    <table class="table" style="font-size: 16px;" riesgo="<%=item.Cells[0].Text%>">
                        <tbody>

                            <tr style="background-color: aliceblue;" class="Riesgos">
                                <td width="5%" class=" text-center desplegar">
                                    <i id="<%= item.Cells[0].Text %>" class="icon-arrow-up" style="font-size: 10px;"></i>
                                </td>

                                <td class="text-center desplegar" style="text-align: left;">Riesgo  <%= item.Cells[1].Text %>
                                </td>
                            </tr>
                            <tr hidden class="MedidasGeneralesRiesgo Contenido_<%= item.Cells[0].Text %>">
                                <td colspan="2">
                                    <table class="MedidasGeneralesRiesgo_<%=item.Cells[0].Text%>  table-bordered" style="background-color: lavenderblush; width: 100%;">
                                        <tbody>
                                            <%foreach (GridViewRow apar in DatosApartados.Rows)
                                                {%>
                                            <tr class="MedidasGeneralesRiesgo">

                                                <%bool contiene = false; %>
                                                <%foreach (GridViewRow medidaRiesgo in DatosMedidasRiesgo.Rows)
                                                    { %>
                                                <%if (medidaRiesgo.Cells[4].Text == item.Cells[0].Text && medidaRiesgo.Cells[6].Text == apar.Cells[0].Text)
                                                        {
                                                            contiene = true;
                                                        }
                                                    } %>


                                                <%if (contiene)
                                                    {%>
                                                <td class="text-center">
                                                    <%=apar.Cells[1].Text %>
                                                </td>

                                                <%}%>
                                                <td colspan="4">
                                                    <%foreach (GridViewRow medidaRiesgo in DatosMedidasRiesgo.Rows)
                                                        { %>

                                                    <%if (medidaRiesgo.Cells[4].Text == item.Cells[0].Text && medidaRiesgo.Cells[6].Text == apar.Cells[0].Text)
                                                        {

                                                    %>
                                                    <table class="table table-bordered">
                                                        <tr>

                                                            <td width="80%" style="max-width: 300px; overflow: auto"><%
                                                                                                                         string[] stringSeparators = new string[] { "[SALTO]" };
                                                                                                                         string[] lines = medidaRiesgo.Cells[2].Text.Split(stringSeparators, StringSplitOptions.None);
                                                                                                                         if (medidaRiesgo.Cells[2].Text.Contains("[SALTO]"))
                                                                                                                         {
                                                                                                                             int contador = 1;
                                                                                                                             foreach (string s in lines)
                                                                                                                             {
                                                                                                                                 if (contador == 1)
                                                                                                                                 {%>
                                                                <%=s%><br />
                                                                <% }
                                                                    else
                                                                    {%>
                                                                <%=" - " +s%><br />
                                                                <%

                                                                    } %>
                                                                <%contador++;
                                                                        }
                                                                    }
                                                                    else
                                                                    {%>
                                                                <%=medidaRiesgo.Cells[2].Text %>
                                                                <%} %>
                                                            </td>
                                                            <td>
                                                                <% var imagen = medidaRiesgo.Cells[5].Text;
                                                                    if (imagen != null && imagen != "" && imagen != "&nbsp;")
                                                                    {%>
                                                                <img id="IMG_<%=medidaRiesgo.Cells[0].Text%>" style="width: 40px; height: 40px;" src="<%=medidaRiesgo.Cells[5].Text %>" action="GuardarImagen" method="post" enctype="multipart/form-data" attributoprueba="imagen"></td>

                                                            <%}
                                                                else
                                                                {%>

                                                            <%} %>
                                                </td>

                                                <% if (medidaRiesgo.Cells[7].Text != "0")
                                                    { %>
                                                <td class="text-center">
                                                    <label for="file_<%=medidaRiesgo.Cells[0].Text%>">Subir Icono</label>
                                                    <input id="file_<%=medidaRiesgo.Cells[0].Text%>" type="file" name="img"
                                                        multiple accept="image/*" mdriesgo="<%=medidaRiesgo.Cells[0].Text%>" class="file"
                                                        onchange="file_changed(this);" onclick="this.value=null;ocultar(this);"></td>
                                                <%--accept=".ico,.png,.jpeg" --%>
                                                <td>
                                                    <button id="Subir_<%=medidaRiesgo.Cells[0].Text%>" style="display: none;" class="subirImagen" mdriesgo="<%=medidaRiesgo.Cells[0].Text%>">Subir</button></td>
                                                <td>
                                                    <%if (medidaRiesgo.Cells[7].Text != "0")
                                                        { %>
                                                    <a class="EditarRiesgoMedida" medida="<%=medidaRiesgo.Cells[0].Text%>"><i class="icon-pencil" style="color: dodgerblue;"></i></a>
                                                    <%} %>
                                                </td>

                                                <td width="5%" class="text-center">
                                                    <a class="EliminarRiesgoMedida" mdriesgo="<%=medidaRiesgo.Cells[0].Text%>"><i class="icon-remove" style="color: red;"></i></a>
                                                </td>
                                                <%}
                                                    else
                                                    {%>
                                                <td class="text-center"></td>
                                                <td></td>
                                                <td width="5%" class="text-center"></td>
                                                <%} %>
                                            </tr>
                                    </table>
                                    <%  }
                                        } %>

                                </td>
                            </tr>
                            <tr class="MedidasGeneralesRiesgo">

                                <%bool contieneimg = false; %>
                                <%foreach (GridViewRow medidaRiesgo in DatosMedidasRiesgoImagen.Rows)
                                    { %>
                                <%if (medidaRiesgo.Cells[4].Text == item.Cells[0].Text && medidaRiesgo.Cells[6].Text == apar.Cells[0].Text)
                                        {
                                            contieneimg = true;
                                        }
                                    } %>


                                <%if (contieneimg)
                                    {%>
                                <td class="text-center">
                                    <%=apar.Cells[1].Text %> (IMÁGEN)
                                </td>

                                <%}%>
                                <td colspan="4">
                                    <%foreach (GridViewRow medidaRiesgo in DatosMedidasRiesgoImagen.Rows)
                                        { %>

                                    <%if (medidaRiesgo.Cells[4].Text == item.Cells[0].Text && medidaRiesgo.Cells[6].Text == apar.Cells[0].Text)
                                        {

                                    %>
                                    <table class="table table-bordered">
                                        <tr>
                                            <td width="20%" class="text-center">
                                                <img id="IMG_G_<%=medidaRiesgo.Cells[0].Text%>" style="width: 100px; height: 100px;"
                                                    src="<%=medidaRiesgo.Cells[5].Text %>"
                                                    action="GuardarImagen" method="post" enctype="multipart/form-data"></td>
                                            <td width="40%" style="max-width: 300px; overflow: auto"><%
                                                                                                         string[] stringSeparators = new string[] { "[SALTO]" };
                                                                                                         string[] lines = medidaRiesgo.Cells[2].Text.Split(stringSeparators, StringSplitOptions.None);
                                                                                                         if (medidaRiesgo.Cells[2].Text.Contains("[SALTO]"))
                                                                                                         {
                                                                                                             int contador = 1;
                                                                                                             foreach (string s in lines)
                                                                                                             {
                                                                                                                 if (contador == 1)
                                                                                                                 {%>
                                                <%=s%><br />
                                                <% }
                                                    else
                                                    {%>
                                                <%=" - " +s%><br />
                                                <%

                                                    } %>
                                                <%contador++;
                                                        }
                                                    }
                                                    else
                                                    {%>
                                                <%=medidaRiesgo.Cells[2].Text %>
                                                <%} %>
                                                                                               
                                                                               

                                            </td>
                                            <% if (medidaRiesgo.Cells[7].Text != "0")
                                                { %>
                                            <td class="text-center">
                                                <label for="fileGrande_<%=medidaRiesgo.Cells[0].Text%>">Subir Imagen Grande</label>
                                                <input id="fileGrande_<%=medidaRiesgo.Cells[0].Text%>" type="file" name="img"
                                                    multiple accept="image/*" mdriesgo="<%=medidaRiesgo.Cells[0].Text%>" class="fileGrande"
                                                    onchange="file_changed(this);" onclick="this.value=null;ocultar(this);"></td>
                                            <td>
                                                <button id="SubirGrande_<%=medidaRiesgo.Cells[0].Text%>" style="display: none;" class="subirImagenGrande" mdriesgo="<%=medidaRiesgo.Cells[0].Text%>">Subir</button></td>
                                            <td>
                                            <td>
                                                <a class="EditarRiesgoMedida" medida="<%=medidaRiesgo.Cells[0].Text%>"><i class="icon-pencil" style="color: dodgerblue;"></i></a>
                                            </td>
                                            <td width="5%" class="text-center">
                                                <a class="EliminarRiesgoMedida" mdriesgo="<%=medidaRiesgo.Cells[0].Text%>"><i class="icon-remove" style="color: red;"></i></a>
                                            </td>
                                            <%}
                                                else
                                                {%>

                                            <td width="5%" class="text-center"></td>
                                            <%} %>
                                        </tr>
                                    </table>
                                    <%

                                            }
                                        } %>

                                </td>
                            </tr>
                            <%} %>
                        </tbody>
                    </table>
                    </td>



                            </tr>

                            <tr hidden class="RegistroNuevo_<%=item.Cells[0].Text%>">
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>

                    <tr hidden class="Contenido_<%= item.Cells[0].Text %>">
                        <td colspan="6">
                            <div hidden style="text-align: right" class="Contenido_<%= item.Cells[0].Text %>" riesgo="<%= item.Cells[0].Text %>">
                                <%--  <a title="Anadir Varias" class="AddVariasMedidasRiesgo btn btn-primary run-first">Añadir Varias Medidas</a>--%>
                                <a title="Anadir Medida General Riesgo" class="AddMedidaGeneral btn btn-primary run-first">Añadir Medida General</a>
                                <a title="Añadir Imagen" class="AddMedidaGeneralImagen btn btn-primary run-first">Añadir Medida General con Imagen Grande</a>
                            </div>
                        </td>
                    </tr>


                    <% foreach (GridViewRow situ in DatosSituaciones.Rows)
                        {
                            if (item.Cells[0].Text == situ.Cells[3].Text)
                            {
                    %>
                    <tr hidden class="Contenido_<%= item.Cells[0].Text %>">
                        <td colspan="3">

                            <table class="table" id="<%=situ.Cells[2].Text %>" situacion="<%=situ.Cells[0].Text%>">
                                <thead>
                                    <tr style="background-color: beige;" class="SituacionesRiesgo">
                                        <th width="5%" class="text-center desplegar">
                                            <i id="<%= item.Cells[0].Text %>_<%= situ.Cells[0].Text %>" class="icon-arrow-up" style="font-size: 10px;"></i>
                                        </th>

                                        <th class="task-desc" style="text-align: center;" width="5%">

                                            <%if (listaParametrica.Where(x => x.id_situacion == int.Parse(situ.Cells[0].Text) && x.activo == true && x.id_centro == int.Parse(hiCentro.Value)).Count() > 0)
                                                {%>
                                            <input checked class="form-check-input" type="checkbox" value="" id="<%="chk_" + situ.Cells[0].Text%>" onclick="CheckSituacion(this);" />
                                            <%}
                                                else
                                                { %>
                                            <input class="form-check-input" type="checkbox" value="" id="<%="chk_" + situ.Cells[0].Text%>" onclick="CheckSituacion(this);" />
                                            <% }%>

                                        </th>
                                        <th colspan="5" class="desplegar" style="font-weight: normal!important; font-size: 14px;">Situacion de Riesgo <%= item.Cells[0].Text%>.<%=situ.Cells[2].Text %>  </th>
                                        <th width="5%" class="text-center">
                                            <%--<a class="EliminarSituacion" situacion="<%=situ.Cells[0].Text%>"><i class="icon-remove" style="color: red;"></i></a>--%>
                                        </th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>

                                    <%--             <tr hidden class="ContenidoSituacion_<%= item.Cells[0].Text%>_<%=situ.Cells[0].Text%>">
                                            <td colspan="3">
                                                                                            </td>

                                        </tr>--%>
                                    <% foreach (GridViewRow medida in DatosMedidas.Rows)
                                        {
                                            if (situ.Cells[0].Text == medida.Cells[3].Text)
                                            {
                                                List<MIDAS.Models.submedidas_preventivas> listaSubMedidas = MIDAS.Models.Datos.ListarSubMedidas(int.Parse(medida.Cells[0].Text));

                                    %>
                                    <tr hidden class="MedidasSituacion ContenidoSituacion_<%= item.Cells[0].Text%>_<%=situ.Cells[0].Text%>" situacion="<%=situ.Cells[0].Text%>" riesgo="<%=item.Cells[0].Text%>" id="Medida_<%= item.Cells[0].Text%>_<%=situ.Cells[0].Text%>_<%=medida.Cells[0].Text %>" style="background-color: whitesmoke;">

                                        <td width="5%" style="text-align: center;" class="text-center">
                                            <%if (listaParametrica.Where(x => x.id_medida == int.Parse(medida.Cells[0].Text) && x.activo == true && x.id_centro == int.Parse(hiCentro.Value)).Count() > 0)
                                                {%>
                                            <input checked class="form-check-input" type="checkbox" id="<%="chk_" +  medida.Cells[0].Text %> " onclick="ModificaPadre(this);" />
                                            <%}
                                                else
                                                { %>
                                            <input class="form-check-input" type="checkbox" id="<%="chk_" +  medida.Cells[0].Text %>" onclick="ModificaPadre(this);" />
                                            <% }%>
                                        </td>



                                        <% if (MIDAS.Models.Datos.EsImagenGrandeMedida(int.Parse(medida.Cells[0].Text)) == false)
                                            {%>
                                        <td colspan="3" style="font-size: 14px; max-width: 300px; overflow: auto"><%=medida.Cells[2].Text %>
                                            <%foreach (MIDAS.Models.submedidas_preventivas submedidas in listaSubMedidas)
                                                {
                                            %><br />
                                            - <%=submedidas.descripcion %><%
                                                                              }

                                            %>
                                        </td>
                                        <td>
                                            <%var imagen = MIDAS.Models.Datos.ObtenerImagenSituacionMedida(int.Parse(medida.Cells[0].Text)); %>

                                            <%if (imagen != null && imagen != "")%>
                                            <%{ %>
                                            <img id="IMG_<%=medida.Cells[0].Text%>" style="width: 40px; height: 40px;" src="<%=  imagen%>" action="GuardarImagen" method="post" enctype="multipart/form-data" atributomedida="med">
                                            <%}
                                                else { }%>
                                        </td>
                                        <td class="text-center">
                                            <%if (medida.Cells[4].Text != "0")
                                                {%>
                                            <input id="file_<%=medida.Cells[0].Text%>" type="file" name="img" multiple accept="image/*" mdsituacion="<%=medida.Cells[0].Text%>" class="file" onclick="this.value=null;ocultar(this);" atributo="hola">
                                            <%}
                                                else { }%>
                                        </td>

                                        <%}
                                            else
                                            { %>
                                        <td colspan="2">
                                            <img id="IMG_<%=medida.Cells[0].Text%>" style="width: 400px; height: 400px;" src="<%= MIDAS.Models.Datos.ObtenerImagenSituacionMedida(int.Parse(medida.Cells[0].Text)) %>" action="GuardarImagen" method="post" enctype="multipart/form-data">
                                        </td>
                                        <td style="max-width: 300px; overflow: auto" colspan="3"><%
                                                                                                     string[] stringSeparators = new string[] { "[SALTO]" };
                                                                                                     string[] lines = medida.Cells[2].Text.Split(stringSeparators, StringSplitOptions.None);
                                                                                                     if (medida.Cells[2].Text.Contains("[SALTO]"))
                                                                                                     {
                                                                                                         int contador = 1;
                                                                                                         foreach (string s in lines)
                                                                                                         {
                                                                                                             if (contador == 1)
                                                                                                             {%>
                                            <%=s%><br />
                                            <% }
                                                else
                                                {%>
                                            <%=" -" +s%><br />
                                            <%

                                                } %>
                                            <%contador++;
                                                    }
                                                }
                                                else
                                                {%>
                                            <%=medida.Cells[2].Text %>
                                            <%} %>
                                                                                               
                                                                               

                                        </td>
                                        <%}%>
                                        <% if (MIDAS.Models.Datos.EsImagenGrandeMedida(int.Parse(medida.Cells[0].Text)) == true)
                                            {%>
                                        <td class="text-center">

                                            <%if (medida.Cells[4].Text != "0")
                                                {%>
                                            <input id="fileGrande_<%=medida.Cells[0].Text%>" type="file" name="img" multiple accept="image/*" mdsituacionGrande="<%=medida.Cells[0].Text%>" class="file" onclick="this.value=null;ocultar(this);" >
                                            <%}
                                                else { }%>
                                        </td>

                                        <%} %>

                                        <%--accept=".ico,.png,.jpeg" --%>
                                        <td>
                                            <button id="Subir_<%=medida.Cells[0].Text%>" style="display: none;" class="subirImagen" mdsituacion="<%=medida.Cells[0].Text%>" >Subir</button>
                                            <button id="SubirGrande_<%=medida.Cells[0].Text%>" style="display: none;" class="subirImagen" mdsituacionGrande="<%=medida.Cells[0].Text%>" >Subir</button></td>

                                        <td width="5%" class="text-center">
                                            <%if (medida.Cells[4].Text != "0")
                                                { %>
                                            <a class="EditarMedida" medida="<%=medida.Cells[0].Text%>"><i class="icon-pencil" style="color: dodgerblue;"></i></a>
                                            <%} %>
                                        </td>
                                        <td width="5%" class="text-center">
                                            <%if (medida.Cells[4].Text != "0")
                                                { %>
                                            <a class="EliminarMedida" medida="<%=medida.Cells[0].Text%>"><i class="icon-remove" style="color: red;" tooltip=""></i></a>
                                            <%} %>
                                        </td>
                                    </tr>
                                    <% }
                                        } %>
                                </tbody>
                            </table>

                            <div hidden style="text-align: right; width: 95%; margin-top: 10px;" class="ContenidoSituacion_<%= item.Cells[0].Text%>_<%=situ.Cells[0].Text%>">
                                <a title="Guardar medidas seleccionadas" style="text-align: initial;" class="GuardarMedidasSeleccionadas btn btn-primary run-first" riesgosituaciontabla="<%=int.Parse(item.Cells[0].Text)%>_<%= int.Parse(situ.Cells[0].Text) %>" onclick="GuardarMedidas(this);">Guardar medidas seleccionadas</a>
                                <a title="Anadir Medida" class="AddMedida btn btn-primary run-first" style="display: none;">Añadir Medida</a>

                                <a title="Anadir Medida" class="AddVariasMedidas btn btn-primary run-first">Añadir Varias Medidas</a>

                                <a title="Anadir Imagen Grande" class="AddImagenGrandeMedida btn btn-primary run-first">Anadir Imagen Grande</a>

                                <a title="Sub Medidas" class="SubMedidas btn btn-primary run-first">Añadir Medida</a>
                            </div>

                            <% }


                                }%>

                            
                        </td>
                    </tr>
                    </tbody>

                    </table>
                    <%--<div style="text-align: right; margin-top: 10px" hidden class="Contenido_<%= item.Cells[0].Text %>">
                        <a title="Anadir Situacion a Riesgo" class="AddSituacion btn btn-primary run-first">Añadir Situación</a>
                    </div>--%>
                </div>
                <div style="text-align: right; width: 100%">
                    <%--  <a href="/evr/Riesgos/seleccionar_tecnologia" class="btn btn-primary run-first">Crear Matriz de Riesgos </a>--%>
                </div>
                <%
                    }%>
            </div>
        </center>
    </div>
    <div style="text-align: right; width: 100%">
        <%--  <a href="/evr/Riesgos/seleccionar_tecnologia" class="btn btn-primary run-first">Crear Matriz de Riesgos </a>--%>
        <a href="/evr/Home/principal" title="Volver" class="btn btn-primary run-first">Guardar</a>
    </div>
    <p>
        <br />
    </p>
    <%--Modal para editar medidas_preventivas--%>

    <div class="container py-4">

        <div class="modal fade" id="mi-modal" data-backdrop="static">
            <div class="modal-dialog" style="overflow-y: scroll; max-height: 85%; margin-top: 50px; margin-bottom: 50px;">
                <div class="modal-content">

                    <div class="modal-header">
                        <h5 class="modal-title">Editar medida</h5>
                        <button class="btn btn-close" data-dismiss="modal">X</button>
                    </div>

                    <div class="modal-body">

                        <br />
                        <label>
                            DESCRIPCION DE LA MEDIDA:</label>
                        <textarea rows="6"
                            cols="50"
                            style="resize: none;"
                            placeholder="descripcion"
                            name="descripcion" type="text"
                            value="" id="descModal" class="form-control"></textarea>

                    </div>

                    <div class="modal-footer">
                        <button class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                        <button class="btn btn-primary" onclick="guardarMedidaAjax()">Guardar</button>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <%--Modal para editar riesgo_medidas--%>
    <div class="container py-4">

        <div class="modal fade" id="mi-modal-riesgo" data-backdrop="static">
            <div class="modal-dialog" style="overflow-y: scroll; max-height: 85%; margin-top: 50px; margin-bottom: 50px;">
                <div class="modal-content">

                    <div class="modal-header">
                        <h5 class="modal-title">Editar riesgo medida</h5>
                        <button class="btn btn-close" data-dismiss="modal">X</button>
                    </div>

                    <div class="modal-body">

                        <br />
                        <label>
                            DESCRIPCION DE LA MEDIDA:</label>
                        <textarea rows="6"
                            cols="50"
                            style="resize: none;"
                            placeholder="descripcion"
                            name="descripcion" type="text"
                            value="" id="descModalRiesgo" class="form-control"></textarea>

                    </div>

                    <div class="modal-footer">
                        <button class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                        <button class="btn btn-primary" onclick="guardarRiesgoMedidaAjax()">Guardar</button>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left"></div>
    </div>
    <!-- /footer -->


</asp:Content>

