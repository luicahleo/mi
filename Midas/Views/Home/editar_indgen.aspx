<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">
  
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            MIDAS.Models.indicador oIndicador = (MIDAS.Models.indicador)ViewData["EditarIndGen"];
            if (oIndicador != null)
            {
                hdnIdInd.Value = oIndicador.id.ToString();
                ddlCriterio.SelectedValue = oIndicador.criterio.ToString();
                ddlSubcriterio.SelectedValue = oIndicador.subcriterio.ToString();
                txtIndicador.Text = oIndicador.indic.ToString();
                txtTitulo.Text = oIndicador.titulo.ToString();
                txtEscalaInicio.Text = oIndicador.escala_inicio.ToString();
                txtEscalaFin.Text = oIndicador.escala_fin.ToString();
                txtDescripcion.Text = oIndicador.descripcion.ToString();
                txtReferencia.Text = oIndicador.vref.ToString();
                txtObjetivo.Text = oIndicador.vobj.ToString();
                ViewData["idIndicador"] = oIndicador.id;


            }
            else
            {
                hdnIdInd.Value = "0";
            }


            if (Session["EdicionIndicadorMensaje"] != null)
            {
                MIDAS.Models.indicador indErroneo = (MIDAS.Models.indicador)Session["IndicadorErroneo"];

                hdnIdInd.Value = indErroneo.id.ToString();
                ddlCriterio.Text = indErroneo.criterio.ToString();
                ddlSubcriterio.Text = indErroneo.subcriterio.ToString();
                txtIndicador.Text = indErroneo.indic.ToString();
                txtTitulo.Text = indErroneo.titulo.ToString();
                txtEscalaInicio.Text = indErroneo.escala_inicio.ToString();
                txtEscalaFin.Text = indErroneo.escala_fin.ToString();
                txtDescripcion.Text = indErroneo.descripcion.ToString();
                txtReferencia.Text = indErroneo.vref.ToString();
                txtObjetivo.Text = indErroneo.vobj.ToString();
                Session.Remove("IndicadorErroneo");


                Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionIndicadorMensaje"].ToString() + "' });", true);
                Session["EdicionIndicadorMensaje"] = null;
            }
        }
        
      
        
        

    }
    
    
</script>

<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
	<title>Novotec-Usuario </title>
	
     <script type="text/javascript">


    $(document).ready(function() {
    
             $("form").submit(function() { 
                    var val = $("input[type=submit][clicked=true]").attr("id")

                    if (val == "GuardarIndicador")
                        $("#hdFormularioEjecutado").val("GuardarIndicador");
            });
            
            $("form input[type=submit]").click(function() {
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
					<h3>Indicador <small>Edición del indicador</small></h3>
				</div>
			</div>
			<!-- /page header -->




			<!-- Form vertical (default) -->
						<form role="form" action="#" runat="server">
						    <input id="hdFormularioEjecutado" name ="hdFormularioEjecutado" type="hidden" value="Entro"/>
							<div class="panel panel-default">
								<div class="panel-heading"><h6 class="panel-title"><i class="icon-page-break"></i>Datos del indicador</h6></div>
								<div class="panel-body">
									<table width="100%">
                                        <asp:HiddenField runat="server" ID="hdnIdInd" />
										<tr>
											<td style="padding:10px"  class="form-group">
												<label>CRITERIO:</label>
												<asp:DropDownList class="form-control" runat="server" ID="ddlCriterio">                                           
                                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                                    </asp:DropDownList>
											</td>
											<td  style="padding:10px" class="form-group">
												<label>SUBCRITERIO:</label>
												<asp:DropDownList class="form-control" runat="server" ID="ddlSubcriterio">                                           
                                                        <asp:ListItem Text="A" Value="A"></asp:ListItem>
                                                        <asp:ListItem Text="B" Value="B"></asp:ListItem>
                                                    </asp:DropDownList>
											</td>

                                            <td  style="padding:10px" class="form-group">
												<label>INDICADOR:</label>
													<asp:TextBox ID="txtIndicador" runat="server" class="form-control" ></asp:TextBox>
											</td>

                                            </tr>
                                            <tr>
											<td colspan="2"  style="padding:10px; width:50%" class="form-group">
                                                <label>TITULO:</label>
												<asp:TextBox style="text-align:center" ID="txtTitulo" CssClass="form-control" runat="server" class="form-control" ></asp:TextBox>
											</td>
                                            <td style="padding:10px" class="form-group">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                        <label>ESCALA INICIO:</label>
												<asp:TextBox ID="txtEscalaInicio" Width="50%" runat="server" class="form-control" ></asp:TextBox>
                                                        </td>
                                                        <td>
                                                         <label>ESCALA FIN:</label>
												<asp:TextBox ID="txtEscalaFin" Width="50%" runat="server" class="form-control" ></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <label>REFERENCIA:</label>
												            <asp:TextBox ID="txtReferencia" Width="50%" runat="server" class="form-control" ></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <label>OBJETIVO:</label>
												            <asp:TextBox ID="txtObjetivo" Width="50%" runat="server" class="form-control" ></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
												
											</td>
 
										</tr>
										<tr>
                                            <td colspan="3"  style="padding:10px" class="form-group">
                                                <label>DESCRIPCIÓN:</label>
												<asp:TextBox TextMode="MultiLine" Rows="6" style="text-align:center" ID="txtDescripcion" CssClass="form-control" runat="server" ></asp:TextBox>
											</td>
                                        </tr>
								
                                </table>
                                    								

								</div>


							</div>

                 
          
            
            

			<!-- /modal with table -->
							

                            <div class="form-actions text-right">
                                        <input id="GuardarIndicador" type="submit" value="Guardar datos" class="btn btn-primary run-first">                                              
                                        <a href="/evr/Home/IndGenericos" title="Volver" class="btn btn-primary run-first">Volver</a>
									</div>
                         </form>				
						<!-- /form vertical (default) -->
		



			<!-- Footer -->
			<div class="footer clearfix">
				<div class="pull-left"></div>
			</div>
			<!-- /footer -->



</asp:Content>
