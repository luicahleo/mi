<%@ Import Namespace="System" %>
<%@ Import Namespace="MIDAS.Models" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<script runat="server">    
    VISTA_ObtenerUsuario user = new VISTA_ObtenerUsuario();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {
            user = Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

            if (ViewData["listaActividad"] != null)
            {
                DatosListaActividad.DataSource = ViewData["listaActividad"];
                DatosListaActividad.DataBind();
            }
            if (ViewData["listaCentroTrabajo"] != null)
            {
                DatosListaCentroTrabajo.DataSource = ViewData["listaCentroTrabajo"];
                DatosListaCentroTrabajo.DataBind();
            }



            if (Session["listaPersonasFiltradas"] != null && Session["listaPersonasFiltradas"] is List<personas>)
            {
                List<personas> lista = (List<personas>)Session["listaPersonasFiltradas"];
                if (lista.Count == 0)
                {
                    // la variable de sesión no está vacía pero la lista está vacía
                }
                else
                {
                    DatosListaPerfilRiesgo.DataSource = Session["listaPersonasFiltradas"];
                    DatosListaPerfilRiesgo.DataBind();

                    Session.Remove("listaPersonasFiltradas");

                }
            }
            else
            {
                // la variable de sesión está vacía o no es una lista de personas
            }
        }

        if (TempData["Notification"] != null)
        {
            var tempData = TempData["Notification"].ToString();
            var lista = tempData.Split(',');

            string js = string.Format("MostrarMensaje('{0}','{1}','{2}');", lista[0], lista[1], lista[2]);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", js, true);
        }
    }

</script>

<asp:Content ID="versionesHead" ContentPlaceHolderID="head" runat="server">
    <title>DIMAS</title>
</asp:Content>

<asp:Content ID="pedidoContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="<%=ResolveClientUrl("~/ext/js/Personas/analisis_datos.js") %>"></script>
    <link href="<%=ResolveClientUrl("~/ext/css/Personas/analisis_datos.css") %>" rel="stylesheet" />





    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>Analisis de datos <small>Analisis de datos asociado a Maestro excel de Personas.</small></h3>
        </div>
    </div>
    <!-- /page header -->

    <form id="form1" runat="server">

        <asp:ScriptManager runat="server"></asp:ScriptManager>

        <asp:HiddenField ID="hdPerfilRiesgo" runat="server" ClientIDMode="Static"></asp:HiddenField>

        <asp:GridView ID="DatosListaActividad" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosListaCentroTrabajo" runat="server" Visible="false">
        </asp:GridView>
        <asp:GridView ID="DatosListaPerfilRiesgo" runat="server" Visible="false">
        </asp:GridView>


        <div class="form-group row">

            <select id="actividad" name="actividad" multiple="multiple" class="filter-multi-select-actividad">
                <% foreach (GridViewRow listaActividad in DatosListaActividad.Rows)
                    { %>
                <option value="<%=listaActividad.Cells[0].Text %>"><%= listaActividad.Cells[0].Text %></option>
                <% } %>
            </select>
            <select id="centroTrabajo" name="centroTrabajo" multiple="multiple" class="filter-multi-select-actividad">
                <% foreach (GridViewRow listaCentroTrabajo in DatosListaCentroTrabajo.Rows)
                    { %>
                <option value="<%=listaCentroTrabajo.Cells[0].Text %>"><%= listaCentroTrabajo.Cells[0].Text %></option>
                <% } %>
            </select>

            <button id="btnAnalizar" class="btn btn-outline-primary ml-1">Analizar</button>
        </div>

        <%-- aqui se rendetiza los resultados --%>
        <div id="container">
        </div>

        <div id="divButtonGuardar" style="display: flex; justify-content: flex-end;">
            <button id="btnGuardar" class="btn btn-primary" onclick="guardarSeleccionados()">Guardar Seleccionados</button>
        </div>
        
    </form>
    <!-- /footer -->

</asp:Content>
