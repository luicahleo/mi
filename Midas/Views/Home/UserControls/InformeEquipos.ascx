<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        //Tipos de equipo
        ddlTiposEquipo.DataSource = ViewData["tiposequipo"];
        ddlTiposEquipo.DataTextField = "nombre";
        ddlTiposEquipo.DataValueField = "id";
        ddlTiposEquipo.DataBind();

        ListItem generico = new ListItem();
        generico.Value = "0";
        generico.Text = "--GENERAL--";

        ddlTiposEquipo.Items.Insert(0, generico);
    }
</script>
<script type="text/javascript">

        $(document).ready(function () {


            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "ExportarDatos")
                    $("#hdFormularioEjecutado").val("ExportarDatos");
                

            });
</script>
<!-- Preguntar fecha de inicio y fecha de fin -->
<div class="block">
	    <form role="form" action="#" runat="server">
        <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
			<div class="form-group">
                <div class="row">
                    <div class="col-md-8">
                        <div class="col-md-3">
                            <label>Código:</label>
                            <input id="txtCodigo" class="form-control" name="txtCodigo" type="text" />
                        </div>
                        <div class="col-md-8">
                            <label>Tipo:</label>
                            <asp:DropDownList Style="width: 95%" CssClass="form-control" ID="ddlTiposEquipo" Width="160px" runat="server">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <br />
				<div class="row">
					<div class="col-md-8">
					    <div class="col-md-3">
					        <label>Fecha inicio:</label>
                            <input id="FechaI" name="FechaI" type="text" class="datepicker form-control" />
                            <%= Html.ValidationMessage("FechaI")%>
					    </div>
					    <div class="col-md-3">
					        <label>Fecha fin:</label>
                            <input id="FechaF" name="FechaF" type="text" class="datepicker form-control" />
                            <%= Html.ValidationMessage("FechaF")%>
					    </div>
					</div>
				</div>
				<div class="row">
					<div class="col-md-4">
                        <br />
                        <input type="submit" id="ExportarDatos" value="Exportar datos" class="btn btn-primary run-first" style="margin-top:10px"/>
                    </div>				
				</div>
			</div>
		</form>
</div>