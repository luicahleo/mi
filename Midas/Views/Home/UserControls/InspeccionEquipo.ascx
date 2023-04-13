<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

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
				<%--<div class="row">
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
				</div>--%>
				<div class="row">
					<div class="col-md-4">
                        <br />
                        <input type="submit" id="ExportarDatos" value="Exportar datos" class="btn btn-primary run-first" style="margin-top:10px"/>
                    </div>				
				</div>
			</div>
		</form>
</div>