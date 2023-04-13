<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
  
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MIDAS.Models.localidad oLocalidad = (MIDAS.Models.localidad)ViewData["EditarLocalidad"];


            int idCCAA = 0;
            int idPais = 0;
            if (oLocalidad != null)
            {
                idCCAA = ((MIDAS.Models.provincia)MIDAS.Models.Datos.ObtenerProvincia(oLocalidad.id_provincia).First()).id_comunidad_autonoma;
                idPais = ((MIDAS.Models.comunidad_autonoma)MIDAS.Models.Datos.ObtenerCCAA(idCCAA).First()).id_pais;

            }
            if (ViewData["pais"] != null)
            {
                ddlPais.DataSource = ViewData["pais"];
                ddlPais.DataValueField = "id_pais";
                ddlPais.DataTextField = "nombre_pais";
                ddlPais.DataBind();

                ListItem item = new ListItem();
                item.Value = "0";
                item.Text = "Seleccione un país";
                ddlPais.Items.Insert(0, item);

                ddlPais.SelectedIndex = 0;

                ddlPais.SelectedValue = idPais.ToString();
            }

            if (ViewData["comunidades"] != null)
            {
                gvCCAA.DataSource = ViewData["comunidades"];
                gvCCAA.DataBind();

                

                hdCCAA.Value = idCCAA.ToString();


            }


            if (ViewData["provincias"] != null)
            {
                gvProvincias.DataSource = ViewData["provincias"];
                gvProvincias.DataBind();

                if (oLocalidad != null)
                {
                    hdProvincia.Value = oLocalidad.id_provincia.ToString();
                }
            }

            if (oLocalidad != null)
            {
                hdnIdLocalidad.Value = oLocalidad.id_consecutivo.ToString();
                txtDescripcion.Text = oLocalidad.nombre_localidad;

                ViewData["idLocalidad"] = oLocalidad.id_consecutivo;


            }
            
            if (Session["EdicionLocalidadMensaje"] != null)
            {
                MIDAS.Models.localidad LocalidadErroneo = (MIDAS.Models.localidad)Session["LocalidadErroneo"];
                hdnIdLocalidad.Value = LocalidadErroneo.id_consecutivo.ToString();
                if (LocalidadErroneo.id_provincia != null && LocalidadErroneo.id_provincia != 0)
                {
                    idCCAA = ((MIDAS.Models.provincia)MIDAS.Models.Datos.ObtenerProvincia(LocalidadErroneo.id_provincia).First()).id_comunidad_autonoma;
                    idPais = ((MIDAS.Models.comunidad_autonoma)MIDAS.Models.Datos.ObtenerCCAA(idCCAA).First()).id_pais;
                }
                ddlPais.SelectedValue = idPais.ToString();
                
                hdCCAA.Value = idCCAA.ToString();
                hdProvincia.Value = LocalidadErroneo.id_provincia.ToString();
                txtDescripcion.Text = LocalidadErroneo.nombre_localidad;
                Session.Remove("LocalidadErroneo");

                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionLocalidadMensaje"].ToString() + "' });", true);
                Session["EdicionLocalidadMensaje"] = null;
            }
        }
               

    }
    
    
</script>

<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
	<title>Midas - Localidad </title>
	
     <script type="text/javascript">


         $(document).ready(function () {

             if ($("#ctl00_MainContent_hdCCAA").val() != '0') {
                 var table = document.getElementById('ctl00_MainContent_gvCCAA');

                 if (table != null) {
                 var rowLength = table.rows.length;

                 

                     $("#ctl00_MainContent_ddlComunidad").empty()
                     $("#ctl00_MainContent_ddlProvincia").empty()
                     $("#ctl00_MainContent_ddlLocalidad").empty()

                     $("#ctl00_MainContent_ddlComunidad").append($("<option>" + 'Seleccione una comunidad' + "</option>").val('0').html('Seleccione una comunidad'));
                     $("#ctl00_MainContent_ddlProvincia").append($("<option>" + 'Seleccione una provincia' + "</option>").val('0').html('Seleccione una provincia'));
                     $("#ctl00_MainContent_ddlLocalidad").append($("<option>" + 'Seleccione una localidad' + "</option>").val('0').html('Seleccione una localidad'));

                     for (var i = 0; i < rowLength; i += 1) {
                         var row = table.rows[i];

                         //your code goes here, looping over every row.
                         //cells are accessed as easy


                         if ($("#ctl00_MainContent_ddlPais").val() == row.cells[1].innerHTML) {
                             $("#ctl00_MainContent_ddlComunidad").append($("<option>" + row.cells[2].innerHTML + "</option>").val(row.cells[0].innerHTML).html(row.cells[2].innerHTML));
                             //do something with every cell here
                         }
                     }

                     $("#ctl00_MainContent_ddlComunidad").val($("#ctl00_MainContent_hdCCAA").val());
                 }
             }

             if ($("#ctl00_MainContent_hdProvincia").val() != '0') {
                 var table = document.getElementById('ctl00_MainContent_gvProvincias');

                 if (table != null) {
                     var rowLength = table.rows.length;

                     $("#ctl00_MainContent_ddlProvincia").empty()
                     $("#ctl00_MainContent_ddlLocalidad").empty()

                     $("#ctl00_MainContent_ddlProvincia").append($("<option>" + 'Seleccione una provincia' + "</option>").val('0').html('Seleccione una provincia'));
                     $("#ctl00_MainContent_ddlLocalidad").append($("<option>" + 'Seleccione una localidad' + "</option>").val('0').html('Seleccione una localidad'));

                     for (var i = 0; i < rowLength; i += 1) {
                         var row = table.rows[i];

                         //your code goes here, looping over every row.
                         //cells are accessed as easy


                         if ($("#ctl00_MainContent_ddlComunidad").val() == row.cells[1].innerHTML) {
                             $("#ctl00_MainContent_ddlProvincia").append($("<option>" + row.cells[2].innerHTML + "</option>").val(row.cells[0].innerHTML).html(row.cells[2].innerHTML));
                             //do something with every cell here
                         }
                     }

                     $("#ctl00_MainContent_ddlProvincia").val($("#ctl00_MainContent_hdProvincia").val());
                 }
             }


             $("form").submit(function () {
                 var val = $("input[type=submit][clicked=true]").attr("id")

                 if (val == "GuardarSector")
                     $("#hdFormularioEjecutado").val("GuardarCCAA");
             });

             $("form input[type=submit]").click(function () {
                 $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                 $(this).attr("clicked", "true");
             });

             $("#<%= ddlPais.ClientID %>").change(function () {
                 //hide social worker and sponsor stuff
                 //$("#hdFormularioEjecutado").val("ddlComunidad");

                 var table = document.getElementById('ctl00_MainContent_gvCCAA');
                 if (table != null) {
                 var rowLength = table.rows.length;

                 

                     $("#ctl00_MainContent_ddlComunidad").empty()
                     $("#ctl00_MainContent_ddlProvincia").empty()
                     $("#ctl00_MainContent_ddlLocalidad").empty()

                     $("#ctl00_MainContent_ddlComunidad").append($("<option>" + 'Seleccione una comunidad' + "</option>").val('0').html('Seleccione una comunidad'));
                     $("#ctl00_MainContent_ddlProvincia").append($("<option>" + 'Seleccione una provincia' + "</option>").val('0').html('Seleccione una provincia'));
                     $("#ctl00_MainContent_ddlLocalidad").append($("<option>" + 'Seleccione una localidad' + "</option>").val('0').html('Seleccione una localidad'));


                     for (var i = 0; i < rowLength; i += 1) {
                         var row = table.rows[i];

                         //your code goes here, looping over every row.
                         //cells are accessed as easy


                         if ($("#ctl00_MainContent_ddlPais").val() == row.cells[1].innerHTML) {
                             $("#ctl00_MainContent_ddlComunidad").append($("<option>" + row.cells[2].innerHTML + "</option>").val(row.cells[0].innerHTML).html(row.cells[2].innerHTML));
                             //do something with every cell here
                         }
                     }
                 }
             });

             $("#<%= ddlComunidad.ClientID %>").change(function () {
                 //hide social worker and sponsor stuff
                 //$("#hdFormularioEjecutado").val("ddlComunidad");

                 var table = document.getElementById('ctl00_MainContent_gvProvincias');
                 if (table != null) {
                 var rowLength = table.rows.length;
                 
                     $("#ctl00_MainContent_ddlProvincia").empty()
                     $("#ctl00_MainContent_ddlLocalidad").empty()

                     $("#ctl00_MainContent_ddlProvincia").append($("<option>" + 'Seleccione una provincia' + "</option>").val('0').html('Seleccione una provincia'));
                     $("#ctl00_MainContent_ddlLocalidad").append($("<option>" + 'Seleccione una localidad' + "</option>").val('0').html('Seleccione una localidad'));

                     for (var i = 0; i < rowLength; i += 1) {
                         var row = table.rows[i];

                         //your code goes here, looping over every row.
                         //cells are accessed as easy


                         if ($("#ctl00_MainContent_ddlComunidad").val() == row.cells[1].innerHTML) {
                             $("#ctl00_MainContent_ddlProvincia").append($("<option>" + row.cells[2].innerHTML + "</option>").val(row.cells[0].innerHTML).html(row.cells[2].innerHTML));
                             //do something with every cell here
                         }
                     }
                 }
             });

             $("#<%= ddlProvincia.ClientID %>").change(function () {
                 //hide social worker and sponsor stuff


                 var table = document.getElementById('ctl00_MainContent_gvLocalidades');
                 if (table != null) {
                 var rowLength = table.rows.length;
                 
                     $("#ctl00_MainContent_ddlLocalidad").empty()

                     $("#ctl00_MainContent_ddlLocalidad").append($("<option>" + 'Seleccione una localidad' + "</option>").val('0').html('Seleccione una localidad'));

                     for (var i = 0; i < rowLength; i += 1) {
                         var row = table.rows[i];

                         //your code goes here, looping over every row.
                         //cells are accessed as easy


                         if ($("#ctl00_MainContent_ddlProvincia").val() == row.cells[1].innerHTML) {
                             $("#ctl00_MainContent_ddlLocalidad").append($("<option>" + row.cells[3].innerHTML + "</option>").val(row.cells[0].innerHTML).html(row.cells[3].innerHTML));
                             //do something with every cell here
                         }
                     }
                 }
             });

         });

       
    </script>

</asp:Content>

<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">


            <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>"/>
            
			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>País <small>Edición de la localidad</small></h3>
				</div>
			</div>
			<!-- /page header -->




			<!-- Form vertical (default) -->
						<form role="form" action="#" runat="server">

                          <asp:GridView ID="gvCCAA" runat="server" style="display:none;">
                            </asp:GridView>
                            <asp:HiddenField ID="hdCCAA" value="0" runat="server" />
                                <asp:GridView ID="gvProvincias" runat="server" style="display:none;">
                            </asp:GridView>
                            <asp:HiddenField ID="hdProvincia" value="0" runat="server" />

						    <input id="hdFormularioEjecutado" name ="hdFormularioEjecutado" type="hidden" value="Entro"/>
							<div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-page-break"></i>Datos del pais</h6></div>
								<div class="panel-body">
									<div class="row">
                                        <asp:HiddenField runat="server" ID="hdnIdLocalidad" />
                                        <table width="100%">
                                            <tr>
                                                <td style="width:50%; padding:10px" class="form-group">
                                                    <label>PAÍS:</label>
												    <asp:TextBox Visible="false" ID="txtCodPais" Enabled="false" runat="server" class="form-control" ></asp:TextBox>
                                                    <asp:DropDownList class="form-control" runat="server" ID="ddlPais">
                                                     </asp:DropDownList>
                                                </td>
                                                <td style="width:50%; padding:10px" class="form-group">
                                                    <label>COMUNIDAD AUTÓNOMA:</label>
                                                    <asp:DropDownList class="form-control" runat="server" ID="ddlComunidad">
                                                     </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:50%; padding:10px" class="form-group">
                                                    <label>PROVINCIA:</label>
                                                    <asp:DropDownList class="form-control" runat="server" ID="ddlProvincia">
                                                     </asp:DropDownList>                                                
                                                </td>
                                                <td style="width:50%; padding:10px" class="form-group">
                                                    <label>NOMBRE:</label>
													<asp:TextBox ID="txtDescripcion" runat="server" class="form-control" ></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
										<div style="width:50%" class="col-md-4">
											<div  class="form-group">
												
											</div>
											<div class="form-group">
												
											</div>

                                            </div>

										
									</div>

                                    								

								</div>


							</div>

			<!-- /modal with table -->
							

                            <div class="form-actions text-right">
                                        <input id="GuardarCCAA" type="submit" value="Guardar datos" class="btn btn-primary run-first">                                               
                                        <a href="/evr/Home/Territorios" title="Volver" class="btn btn-primary run-first">Volver</a>
									</div>
                         </form>				
						<!-- /form vertical (default) -->
		



			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->



</asp:Content>
