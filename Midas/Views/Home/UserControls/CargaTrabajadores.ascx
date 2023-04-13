<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        DatosCargaArchivo2.DataSource = ViewData["archivos2"];
        DatosCargaArchivo2.DataBind();
    }
</script>

    <!-- Tasks table -->
				        	<div class="block">
							    <form id="UploadForm1" action="/Home/CargaDatos/2" method="post" enctype="multipart/form-data" class="block validate"> 
									<div class="form-group">
										<div class="row">
											<div class="col-md-4">
												<label>Seleccionar archivo:</label>
			                                    <input type="file" class="styled" id="file2" name="file2" />
			                                    <%= Html.ValidationMessage("file2")%>
			                                    <br />
			                                    <input type="submit" value="Cargar archivo" class="btn btn-primary run-first" style="margin-top:10px"/>
											</div>
											<div class="col-md-8">

											</div>
										</div>
									</div>
								</form>
                        </div>
                        <div class="block">
                            <form id="form2" runat="server">
                                <asp:GridView ID="DatosCargaArchivo2" runat="server" Visible="False">
                                </asp:GridView>
                            </form>
                        </div>
                        
                        <div class="block">
				           <div class="datatableOrden">
				                <table class="table table-bordered">
				                    <thead>
				                        <tr>
				                            <th>Nombre Fichero</th>
				                            <th>Usuario Creador</th>
				                            <th>Estado</th>
				                            <th class="task-date-added">Fecha de carga</th>
				                        </tr>
				                    </thead>
				                    <tbody>
		
		                              <% foreach (GridViewRow item in DatosCargaArchivo2.Rows)
                                         { %>
        
				                        <tr>
				                            <td class="task-desc"><%= item.Cells[1].Text %></td>
				                            <td><%= item.Cells[5].Text %></td>
				                            <td><%= item.Cells[3].Text %></td>
				                            <td class="text-center"><%= item.Cells[2].Text %></td>
				                        </tr>
				                        
				                      <% } %>
				                    </tbody>
				                </table>
				            </div>
				        </div>
				        <!-- /tasks table -->