<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
  
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MIDAS.Models.provincia oProvincia = (MIDAS.Models.provincia)ViewData["EditarProvincia"];


            int idPais = 0;
            if (oProvincia != null)
            {
                idPais = ((MIDAS.Models.comunidad_autonoma)MIDAS.Models.Datos.ObtenerCCAA(oProvincia.id_comunidad_autonoma).First()).id_pais;
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

                
                if (oProvincia != null)
                    hdCCAA.Value = oProvincia.id_comunidad_autonoma.ToString();


            }


            if (oProvincia != null)
            {
                hdnIdProvinciad.Value = oProvincia.id_provincia.ToString();
                txtDescripcion.Text = oProvincia.nombre_provincia;

                ViewData["idProvincia"] = oProvincia.id_provincia;


            }
            
            if (Session["EdicionProvinciaMensaje"] != null)
            {
                MIDAS.Models.provincia ProvinciaErroneo = (MIDAS.Models.provincia)Session["ProvinciaErroneo"];
                hdnIdProvinciad.Value = ProvinciaErroneo.id_provincia.ToString();
                if (ProvinciaErroneo.id_comunidad_autonoma != null && ProvinciaErroneo.id_comunidad_autonoma != 0)
                {
                    idPais = ((MIDAS.Models.comunidad_autonoma)MIDAS.Models.Datos.ObtenerCCAA(ProvinciaErroneo.id_comunidad_autonoma).First()).id_pais;
                }
                ddlPais.SelectedValue = idPais.ToString();

                hdCCAA.Value = ProvinciaErroneo.id_comunidad_autonoma.ToString();
                txtDescripcion.Text = ProvinciaErroneo.nombre_provincia;
                Session.Remove("ProvinciaErroneo");

                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionProvinciaMensaje"].ToString() + "' });", true);
                Session["EdicionProvinciaMensaje"] = null;
            }
        }
               

    }
    
    
</script>

<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
	<title>Midas - Provincia </title>
	
     <script type="text/javascript">


         $(document).ready(function () {

             if ($("#ctl00_MainContent_hdCCAA").val() != '0') {
                 var table = document.getElementById('ctl00_MainContent_gvCCAA');

                 if (table != null) {
                 var rowLength = table.rows.length;

                 

                     $("#ctl00_MainContent_ddlComunidad").empty()

                     $("#ctl00_MainContent_ddlComunidad").append($("<option>" + 'Seleccione una comunidad' + "</option>").val('0').html('Seleccione una comunidad'));

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

                     $("#ctl00_MainContent_ddlComunidad").append($("<option>" + 'Seleccione una comunidad' + "</option>").val('0').html('Seleccione una comunidad'));

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
             

         });

       
    </script>

</asp:Content>

<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">


            <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>"/>
            
			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>País <small>Edición de la provincia</small></h3>
				</div>
			</div>
			<!-- /page header -->




			<!-- Form vertical (default) -->
						<form role="form" action="#" runat="server">

                          <asp:GridView ID="gvCCAA" runat="server" style="display:none;">
                            </asp:GridView>
                            <asp:HiddenField ID="hdCCAA" value="0" runat="server" />

						    <input id="hdFormularioEjecutado" name ="hdFormularioEjecutado" type="hidden" value="Entro"/>
							<div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-page-break"></i>Datos del pais</h6></div>
								<div class="panel-body">
									<div class="row">
                                        <asp:HiddenField runat="server" ID="hdnIdProvinciad" />
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
                                                <label>NOMBRE:</label>
													<asp:TextBox ID="txtDescripcion" runat="server" class="form-control" ></asp:TextBox>                             
                                                </td>
                                                <td style="width:50%; padding:10px" class="form-group">
                                                    
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
