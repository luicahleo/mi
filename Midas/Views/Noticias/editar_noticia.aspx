<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" ValidateRequest="false" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<script runat="server">
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.noticias oNoticia = new MIDAS.Models.noticias();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            oNoticia = (MIDAS.Models.noticias)ViewData["noticia"];

            if (Session["usuario"] != null)
            {
                if (Session["CentralElegida"] != null)
                {
                    centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
                }

                user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());

                if (centroseleccionado.tipo != 4)
                {
                    lblNombreOrg.Text = centroseleccionado.nombre;
                }
                else
                {
                    lblNombreOrg.Text = "Todas las centrales";
                }
            }
            
            if (oNoticia != null)
            {
                txtTexto.Text = oNoticia.texto;
                txtTitulo.Text = oNoticia.titulo;
                Session["noticia"] = oNoticia.id;
                if (oNoticia.cabecera != null)
                    imgCabecera.ImageUrl = "http://novotecsevilla.westeurope.cloudapp.azure.com/evr/cabeceras" + "/" + oNoticia.id + "/" + oNoticia.cabecera;
            }

            if (Session["EdicionNoticiaError"] != null)
            {
                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionNoticiaError"].ToString() + "' });", true);
                Session["EdicionNoticiaError"] = null;
            } 
        }             

    }
    
    
</script>

<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
	<title>Compass - Proceso </title>

    <script type="text/javascript">
         $(document).ready(function () {

             $("form").submit(function () {
                 var val = $("input[type=submit][clicked=true]").attr("id")

                 if (val == "GuardarNoticia")
                     $("#hdFormularioEjecutado").val("GuardarNoticia");

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
					<h3>Noticia - <asp:Label runat="server" ID="lblNombreOrg"></asp:Label> <small>Esta noticia aparecerá en la pantalla principal</small></h3>
				</div>
			</div>
			<!-- /page header -->

     
            

			<!-- Form vertical (default) -->
						<form method="post" enctype="multipart/form-data" role="form" action="#" runat="server">
                           
                           

						    <input id="hdFormularioEjecutado" name ="hdFormularioEjecutado" type="hidden" value="Entro"/>
							<div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-pencil"></i>Editar Noticia</h6></div>
                                <div class="panel-body">
                                    
                                    <table width="100%">
                                        <tr>
                                            <td style="width:80%">
                                            <label>Titulo (*)</label>
                                            <asp:TextBox Width="98%" CssClass="form-control" ID="txtTitulo" runat="server"></asp:TextBox>
                                            </td>
                                            <td style="width:20%">
                                                <label>Fecha de expiración</label>
                                     <asp:TextBox CssClass="form-control" ID="txtExpira" Width="100%" TextMode="Date" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>                                    

                                     <br /><br />
                                    
                                    <table width="100%">
                                        <tr>
                                            <td style="width:75%">
                                                <label>Cuerpo de la noticia (*)</label>
                                                <asp:TextBox ID="txtTexto" Rows="15" TextMode="MultiLine" runat="server" class="form-control"></asp:TextBox>
                                            </td>
                                            <td style="width:25%; padding-left:10px"">
                                               <input type="file" id="fileCabecera" name="file" style="margin-top:10px; " /></center>
                                               <br />
                                               <div style="border:1px solid blue; min-height:120px; min-width:100px; max-width:260px">
                                                    <% if (oNoticia != null && oNoticia.cabecera != null)
                                                       { %>
                                                        <asp:Image style="max-width:255px" ID="imgCabecera"  runat="server" />
                                                    <% } %>
                                               </div>
                                            </td>
                                            
                                        </tr>
                                    </table>
                                    
                                    <br />
			                    </div>  

							</div>

                             
                           



                            
                            <div class="form-actions text-right">
                              
										<input id="GuardarNoticia" type="submit" value="Actualizar Noticia" class="btn btn-primary run-first"/>                                      
                                        <a href="/evr/Home/principal" class="btn btn-primary" >Volver</a></>
									</div>
                                    
                         </form>				
						<!-- /form vertical (default) -->
		



			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->



</asp:Content>
