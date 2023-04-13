<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MIDAS.Models.organizacion oOrganizacion = (MIDAS.Models.organizacion)ViewData["EditarOrganizacion"];

            
            
                if (ViewData["sectores"] != null)
                {
                    ddlSector.DataSource = ViewData["sectores"];
                    ddlSector.DataValueField = "id";
                    ddlSector.DataTextField = "descripcion";
                    ddlSector.DataBind();
                }

                if (ViewData["numempleados"] != null)
                {
                    ddlNumEmpleados.DataSource = ViewData["numempleados"];
                    ddlNumEmpleados.DataValueField = "id";
                    ddlNumEmpleados.DataTextField = "descripcion";
                    ddlNumEmpleados.DataBind();
                }

                if (ViewData["volumenes"] != null)
                {
                    ddlVolumen.DataSource = ViewData["volumenes"];
                    ddlVolumen.DataValueField = "id";
                    ddlVolumen.DataTextField = "descripcion";
                    ddlVolumen.DataBind();
                }

                if (ViewData["paises"] != null)
                {
                    ddlPais.DataSource = ViewData["paises"];
                    ddlPais.DataValueField = "id_pais";
                    ddlPais.DataTextField = "nombre_pais";
                    ddlPais.DataBind();

                    ListItem item = new ListItem();
                    item.Value = "0";
                    item.Text = "Seleccione un país";
                    ddlPais.Items.Insert(0, item);

                    if (Session["paisselec"] != null)
                    {
                        ddlPais.SelectedValue = Session["paisselec"].ToString();
                    }

                }

                if (ViewData["comunidades"] != null)
                {
                    gvCCAA.DataSource = ViewData["comunidades"];
                    gvCCAA.DataBind();


                    hdCCAA.Value = oOrganizacion.com_aut.ToString();

                    //if (Session["comunidadselec"] != null)
                    //{
                    //    hdCCAA.Value = Session["comunidadselec"].ToString();
                    //    Session.Remove("comunidadselec");
                    //}
                }


                if (ViewData["provincias"] != null)
                {
                    gvProvincias.DataSource = ViewData["provincias"];
                    gvProvincias.DataBind();

                    hdProvincia.Value = oOrganizacion.provincia.ToString();

                    //if (Session["provinciaselec"] != null)
                    //{
                    //    hdProvincia.Value = Session["provinciaselec"].ToString();
                    //    Session.Remove("provinciaselec");
                    //}
                }

                if (ViewData["localidades"] != null)
                {
                    gvLocalidades.DataSource = ViewData["localidades"];
                    gvLocalidades.DataBind();

                    hdLocalidad.Value = oOrganizacion.localidad.ToString();

                    //if (Session["localidadselec"] != null)
                    //{
                    //    hdLocalidad.Value = Session["localidadselec"].ToString();
                    //    Session.Remove("localidadselec");
                    //}
                }                


                if (oOrganizacion != null)
                {
                    txtCodOrg.Text = oOrganizacion.cod_organizacion;
                    txtNombre.Text = oOrganizacion.nombre_organizacion;
                    lblNombreOrg.Text = oOrganizacion.nombre_organizacion;
                    ddlSector.SelectedValue = oOrganizacion.sector.ToString();
                    if (oOrganizacion.pais != null)
                        ddlPais.SelectedValue = oOrganizacion.pais.ToString();
                    if (Session["comunidadselec"] == null && oOrganizacion.com_aut != null)
                        ddlComunidad.SelectedValue = oOrganizacion.com_aut.ToString();
                    if (Session["provinciaselec"] == null && oOrganizacion.provincia != null)
                        ddlProvincia.SelectedValue = oOrganizacion.provincia.ToString();
                    if (oOrganizacion.localidad != null)
                        ddlLocalidad.SelectedValue = oOrganizacion.localidad.ToString();

                    ddlNumEmpleados.SelectedValue = oOrganizacion.num_empleados.ToString();
                    ddlTipo.SelectedValue = oOrganizacion.tipo_org.ToString();
                    ddlVolumen.SelectedValue = oOrganizacion.volumen_fact.ToString();

                    ddlEFQM.SelectedValue = oOrganizacion.efqm.ToString();
                    if (ddlEFQM.SelectedValue == "0")
                    {
                        txtAnio.Enabled = false;
                        txtAnio.Text = string.Empty;
                    }
                    else
                    {
                        txtAnio.Text = oOrganizacion.efqm_desde.ToString();    
                        
                    }                    

                    txtPersContacto.Text = oOrganizacion.nombre_contacto;
                    txtEmail.Text = oOrganizacion.email_contacto;
                    txtTelefono.Text = oOrganizacion.telefono_contacto;
                    hdnColor1.Value = oOrganizacion.color1;
                    hdnColor2.Value = oOrganizacion.color2;
                    hdnColor3.Value = oOrganizacion.color3;
                    txtResponsable.Text = oOrganizacion.email_resp;
                    txtCargo.Text = oOrganizacion.cargo_contacto;
                    txtTelefono2.Text = oOrganizacion.telefono2_contacto;
                    txtWeb.Text = oOrganizacion.web;
                    txtNombreResponsable.Text = oOrganizacion.nombre_responsable;
                    
                    MIDAS.Models.cuestionario licencia = new MIDAS.Models.cuestionario();

                       licencia = MIDAS.Models.Datos.ObtenerLicencia(int.Parse(Session["idUsuario"].ToString()));

                        
                       if (licencia!=null && (licencia.organizacionvalidada == false || licencia.organizacionvalidada == null))
                       {
                           txtFechaCumplimentacion.Text = DateTime.Now.Date.ToString().Replace(" 0:00:00", "");
                       }
                       else
                       {
                           txtFechaCumplimentacion.Text = oOrganizacion.fecha_cumplimiento.ToString().Replace(" 0:00:00", "");
                       }
                       
                       user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
                       if (user.id_organizacion != null && (Session["permisoOrg"] != null && Session["permisoOrg"].ToString()!="2"))
                       {
                           ddlSector.Enabled = false;
                           ddlPais.Enabled = false;
                           ddlComunidad.Enabled = false;
                           ddlProvincia.Enabled = false;
                           ddlLocalidad.Enabled = false;
                           ddlNumEmpleados.Enabled = false;
                           ddlTipo.Enabled = false;
                           ddlVolumen.Enabled = false;
                           ddlEFQM.Enabled = false;
                           txtAnio.Enabled = false;
                           txtPersContacto.Enabled = false;
                           txtEmail.Enabled = false;
                           txtTelefono.Enabled = false;
                           txtTelefono2.Enabled = false;
                           txtResponsable.Enabled = false;
                           txtWeb.Enabled = false;
                           txtCargo.Enabled = false;
                           txtNombreResponsable.Enabled = false;
                           
                       }

                }
            
                 
                
               
            


        }

        if (Session["orgErronea"] != null)
        {
            MIDAS.Models.organizacion org = new MIDAS.Models.organizacion();

            org = (MIDAS.Models.organizacion)Session["orgErronea"];

            ddlSector.SelectedValue = org.sector.ToString();
            if (org.pais != null)
                ddlPais.SelectedValue = org.pais.ToString();
            if (org.com_aut != null)
                hdCCAA.Value = org.com_aut.ToString();
            if (org.provincia != null)
                hdProvincia.Value = org.provincia.ToString();
            if (org.localidad != null)
               hdLocalidad.Value = org.localidad.ToString();

            ddlNumEmpleados.SelectedValue = org.num_empleados.ToString();
            ddlTipo.SelectedValue = org.tipo_org.ToString();
            ddlVolumen.SelectedValue = org.volumen_fact.ToString();

            ddlEFQM.SelectedValue = org.efqm.ToString();
            if (ddlEFQM.SelectedValue == "0")
            {
                txtAnio.Enabled = false;
                txtAnio.Text = string.Empty;
            }
            else
            {
                txtAnio.Text = org.efqm_desde.ToString();

            }

            txtPersContacto.Text = org.nombre_contacto;
            txtEmail.Text = org.email_contacto;
            txtTelefono.Text = org.telefono_contacto;

            Session.Remove("orgErronea");
        }

        if (Session["EdicionOrganizacionMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionOrganizacionMensaje"].ToString() + "' });", true);
            Session["EdicionOrganizacionMensaje"] = null;
        }
        
        

    }
    
    
</script>



<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
	<title>Midas - Organización </title>

    <script type="text/javascript">
         $(document).ready(function () {

             if ($("#ctl00_MainContent_hdCCAA").val() != '0') {
                 var table = document.getElementById('ctl00_MainContent_gvCCAA');

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

             if ($("#ctl00_MainContent_hdProvincia").val() != '0') {
                 var table = document.getElementById('ctl00_MainContent_gvProvincias');

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

             if ($("#ctl00_MainContent_hdLocalidad").val() != '0') {
                 var table = document.getElementById('ctl00_MainContent_gvLocalidades');

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

                 $("#ctl00_MainContent_ddlLocalidad").val($("#ctl00_MainContent_hdLocalidad").val());
             }

             $("form").load(function () {

             });

             $("form").submit(function () {
                 var val = $("input[type=submit][clicked=true]").attr("id")

                 if (val == "GuardarOrganizacion")
                     $("#hdFormularioEjecutado").val("GuardarOrganizacion");
                 if (val == "ValidarCuestionario")
                     $("#hdFormularioEjecutado").val("ValidarCuestionario");
             });

             $("#<%= ddlPais.ClientID %>").change(function () {
                 //hide social worker and sponsor stuff
                 //$("#hdFormularioEjecutado").val("ddlComunidad");

                 var table = document.getElementById('ctl00_MainContent_gvCCAA');

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
             });

             $("#<%= ddlComunidad.ClientID %>").change(function () {
                 //hide social worker and sponsor stuff
                 //$("#hdFormularioEjecutado").val("ddlComunidad");

                 var table = document.getElementById('ctl00_MainContent_gvProvincias');

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
             });

             $("#<%= ddlProvincia.ClientID %>").change(function () {
                 //hide social worker and sponsor stuff


                 var table = document.getElementById('ctl00_MainContent_gvLocalidades');

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
             });

             $("#<%= ddlLocalidad.ClientID %>").change(function () {
                 //hide social worker and sponsor stuff
                 $("#hdFormularioEjecutado").val("ddlLocalidad");
             });

             $("#<%= ddlEFQM.ClientID %>").change(function () {
                 if ($("#<%= ddlEFQM.ClientID %>").val() == "0") {
                     $("#<%= txtAnio.ClientID %>").val("");
                     $("#<%= txtAnio.ClientID %>").prop('disabled', true);
                 }
                 else {
                     $("#<%= txtAnio.ClientID %>").prop('disabled', false);
                 }
             });

             $("#ValidarCuestionario").click(function () {
                 var returnVal = confirm("¿Está seguro que desea validar la organización? No podrá hacer modificaciones después de realizar la validación.");

                 return returnVal;
             });

             $("form input[type=submit]").click(function () {
                 $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                 $(this).attr("clicked", "true");
             });


         });
       
    </script>

</asp:Content>

<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
            <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>"/>
            
			<!-- Page header -->
			<div class="page-header">
				<div class="page-title">
					<h3>Organización - <asp:Label runat="server" ID="lblNombreOrg"></asp:Label> <small>Cumplimentar datos de la organización</small></h3>
				</div>
			</div>
			<!-- /page header -->

     
            

			<!-- Form vertical (default) -->
						<form method="post" enctype="multipart/form-data" role="form" action="#" runat="server">
                               <asp:GridView ID="gvCCAA" runat="server" style="display:none;">
                            </asp:GridView>
                            <asp:HiddenField ID="hdCCAA" value="0" runat="server" />
                                <asp:GridView ID="gvProvincias" runat="server" style="display:none;">
                            </asp:GridView>
                            <asp:HiddenField ID="hdProvincia" value="0" runat="server" />
                                <asp:GridView ID="gvLocalidades" runat="server" style="display:none;">
                            </asp:GridView>
                            <asp:HiddenField ID="hdLocalidad" value="0" runat="server" />

						    <input id="hdFormularioEjecutado" name ="hdFormularioEjecutado" type="hidden" value="Entro"/>
							<div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-page-break"></i>Datos generales</h6></div>
								<div class="panel-body">
									<div class="row">
										<div style="width:50%" class="col-md-4">
											<div  class="form-group">
												<label>CIF</label>
												<asp:TextBox Enabled="false" ID="txtCodOrg" runat="server" class="form-control" ></asp:TextBox>
											</div>
											<div class="form-group">
                                             
											</div>
                                                
                                            </div>
                                            <div style="width:50%" class="col-md-4">
											<div class="form-group">
												<label>Nombre</label>
                                                <asp:TextBox Enabled="false" ID="txtNombre" runat="server" class="form-control" ></asp:TextBox>
											</div>
											<div class="form-group">
											
											</div>
 
										</div>
										
									</div>
                                    <div class="row">
										<div style="width:50%" class="col-md-4">
											<div  class="form-group">
												<label>Sector</label>
                                                
													<asp:DropDownList class="form-control" runat="server" ID="ddlSector">                                           
                                                    </asp:DropDownList>
                                                                                  
											</div>
											<div class="form-group">
												
											</div>

                                            </div>
                                            <div style="width:50%" class="col-md-4">
											<div class="form-group">
                                                <label></label>
											</div>
											<div class="form-group">
												
											</div>
 
										</div>
										
									</div>
                                    <div class="row">
										<div style="width:50%" class="col-md-4">
											<div  class="form-group">
                                                <label>País</label>
												<asp:DropDownList class="form-control" runat="server" ID="ddlPais">                                           
                                                </asp:DropDownList>
												
											</div>
											<div class="form-group">
                                                <label>Provincia</label>
												
                                                <asp:DropDownList  class="form-control" runat="server" ID="ddlProvincia">                                           
                                                </asp:DropDownList>
												
											</div>

                                            </div>
                                            <div style="width:50%" class="col-md-4">
											<div class="form-group">
                                                <label>Comunidad Autónoma</label>
													<asp:DropDownList  class="form-control" runat="server" name="ddlComunidad" ID="ddlComunidad">                                           
                                                </asp:DropDownList>
												
											</div>
											<div class="form-group">
												<label>Localidad</label>
                                                <asp:DropDownList class="form-control" runat="server" ID="ddlLocalidad">                                           
                                                </asp:DropDownList>
												
											</div>
 
										</div>
										
									</div>

									

								</div>


							</div>

                             

                            <div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-page-break"></i>Detalle de la organización</h6></div>
								<div class="panel-body">
									<div class="row">
										<div style="width:50%" class="col-md-4">
											<div  class="form-group">
												<label>Nº de empleados</label>
												<asp:DropDownList class="form-control" runat="server" ID="ddlNumEmpleados">                                           
                                                </asp:DropDownList>
											</div>
											<div class="form-group">
												<label>Volumen de facturación</label>
													<asp:DropDownList class="form-control" runat="server" ID="ddlVolumen">                                           
                                                </asp:DropDownList>
											</div>

                                            </div>
                                            <div style="width:50%" class="col-md-4">
											<div class="form-group">
												<label>Tipo de organización</label>
                                                <asp:DropDownList class="form-control" runat="server" ID="ddlTipo">
                                                    <asp:ListItem Value="0" Text="Pública"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Privada"></asp:ListItem>   
                                                </asp:DropDownList>
											</div>
											<div class="form-group">
												<label></label>
												
											</div>
 
										</div>
										
									</div>

                                  

								</div>


							</div>


                            <div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-page-break"></i>EFQM</h6></div>
								<div class="panel-body">
									<div class="row">
										<div style="width:50%" class="col-md-4">
											<div  class="form-group">
												<label>Utiliza el Modelo EFQM de Excelencia</label>
												<asp:DropDownList class="form-control" runat="server" ID="ddlEFQM">                                           
                                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="Autoevaluación interna"></asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Reconocimiento externo Sello de Excelencia"></asp:ListItem>
                                                    <asp:ListItem Value="3" Text="Premio Europeo"></asp:ListItem>
                                                </asp:DropDownList>
											</div>
	

                                            </div>
                                            <div style="width:50%" class="col-md-4">
											<div class="form-group">
												<label>Desde (año)</label>
                                                <asp:TextBox MaxLength="4" CssClass="form-control" ID="txtAnio" runat="server" ></asp:TextBox>
											</div>
										
 
										</div>
										
									</div>

                                  

								</div>


							</div>


                            <div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-page-break"></i>Datos de contacto</h6></div>
								<div class="panel-body">
									<div class="row">
										<div style="width:50%" class="col-md-4">
											<div  class="form-group">
												<label>Persona de contacto (nombre y apellidos)</label>
												<asp:TextBox ID="txtPersContacto" runat="server" class="form-control" ></asp:TextBox>
											</div>
											<div class="form-group">
												<label>Teléfono 1</label>
													<asp:TextBox ID="txtTelefono" runat="server" class="form-control" ></asp:TextBox>
											</div>

                                            </div>
                                            <div style="width:50%" class="col-md-4">
											<div class="form-group">
												<label>Cargo</label>
                                                <asp:TextBox ID="txtCargo" runat="server" class="form-control" ></asp:TextBox>
											</div>
											<div class="form-group">
												<label>Teléfono 2</label>
													<asp:TextBox ID="txtTelefono2" runat="server" class="form-control" ></asp:TextBox>
											</div>
                                            </div>
                                            
										
									</div>

                                    <div class="row">
                                    <div style="width:50%" class="col-md-4">
                                            <div  class="form-group">
												<label>Email</label>
                                                <asp:TextBox ID="txtEmail" runat="server" class="form-control" ></asp:TextBox>
											</div>
                                            <div class="form-group">
                                                <label>Responsable</label>
                                                <asp:TextBox ID="txtNombreResponsable" runat="server" class="form-control" ></asp:TextBox>
												
											</div>
											
 
										</div>
                                    <div style="width:50%" class="col-md-4">
                                            
                                            <div class="form-group">
												<label>Página Web</label>
												<asp:TextBox style="text-align:center" ID="txtWeb" runat="server"  CssClass="form-control" ></asp:TextBox>
											</div>

                                            <div  class="form-group">
												<label>Email Responsable de Calidad</label>
                                                <asp:TextBox ID="txtResponsable" runat="server" class="form-control" ></asp:TextBox>
											</div>
											
 
										</div>
                                        
                                    </div>

                                     <div class="row">
                                    <div style="width:50%" class="col-md-4">
                                            <div  class="form-group">
												<label>Fecha de cumplimentación de datos</label>
												<asp:TextBox style="text-align:center" ReadOnly="true" ID="txtFechaCumplimentacion" runat="server"  CssClass="form-control" ></asp:TextBox>
											</div>
											<div class="form-group">
	
											</div>
 
										</div>
                                    <div style="width:50%" class="col-md-4">
                                            <div  class="form-group">

											</div>
											<div class="form-group">

											</div>
 
										</div>
                                        
                                    </div>

								</div>


							</div>
							
                            <div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-page-break"></i>Personalizacion</h6></div>
								<div class="panel-body">
                                    <div class="row">
										<div style="width:70%" class="col-md-4">
											<div  class="form-group">
												<label>Color principal</label>
												<input type="color" name="txtColor1" onchange="getElementById('ctl00_MainContent_hdnColor1').value=getElementById('txtColor1').value;" id="txtColor1" value="<%=  (string)Session["colorOrg1"] %>">
                                                <label style="margin-left:50px">Color secundario</label>
                                                <input type="color" name="txtColor2" onchange="getElementById('ctl00_MainContent_hdnColor2').value=getElementById('txtColor2').value;" id="txtColor2" value="<%=  (string)Session["colorOrg2"] %>">
                                                <label style="margin-left:50px">Color de fuente</label>
                                                <input type="color" name="txtColor3" onchange="getElementById('ctl00_MainContent_hdnColor3').value=getElementById('txtColor3').value;" id="txtColor3" value="<%=  (string)Session["colorOrg3"] %>">

                                                <asp:HiddenField ID="hdnColor1" runat="server" />
                                                <asp:HiddenField ID="hdnColor2" runat="server" />
                                                <asp:HiddenField ID="hdnColor3" runat="server" />
											</div>
											<div class="form-group">
												<label>Logo</label>													
                                                    <input type="file" name="file" />
											</div>

                                            </div>
                                            <div style="width:50%" class="col-md-4">
											<div class="form-group">
												
                                           
											</div>
                                            </div>
                                            
										
									</div>

                                </div>  
                           </div>
                           

                            
                            <div class="form-actions text-right">
                                <%
                                MIDAS.Models.cuestionario licencia = new MIDAS.Models.cuestionario();

                       licencia = MIDAS.Models.Datos.ObtenerLicencia(int.Parse(Session["idUsuario"].ToString()));

                       if (user.id_organizacion == null || (licencia != null && (licencia.organizacionvalidada == false || licencia.organizacionvalidada == null) && (Session["permisoOrg"] != null && Session["permisoOrg"].ToString() == "2")))
                       {
                                 %>
                                        <%--<input id="ValidarCuestionario" type="submit" value="Validar Datos" class="btn btn-primary">--%>
										<input id="GuardarOrganizacion" type="submit" value="Guardar datos" class="btn btn-primary run-first">
                                        <% } %>

                                        <% if (user.id_organizacion == null)
                                           { %>
                                        <a href="/evr/Home/Organizaciones" title="Volver" class="btn btn-primary run-first">Volver</a>
                                        <% }
                                           else
                                           { %>
                                                <a href="/evr/Home/Principal" title="Volver" class="btn btn-primary run-first">Volver</a>
                                           <% } %>
									</div>
                                    
                         </form>				
						<!-- /form vertical (default) -->
		



			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->



</asp:Content>
