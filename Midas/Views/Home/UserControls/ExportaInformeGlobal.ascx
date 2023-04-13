<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<!-- Preguntar fecha de inicio y fecha de fin -->
<div class="block">
	    <form role="form" action="#" runat="server">
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
                        <input type="submit" value="Exportar datos" class="btn btn-primary run-first" style="margin-top:10px"/>
                    </div>				
				</div>
			</div>
		</form>
</div>