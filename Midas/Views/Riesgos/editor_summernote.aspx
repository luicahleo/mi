<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" ValidateRequest="false" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>



<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] != null)
        {

            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());


            //DatosCentro.DataSource =ViewData["informacion_centro"] ;
            //DatosCentro.DataBind();
            //string datos = "";
            //foreach (GridViewRow item in DatosCentro.Rows)
            //{
            //    datos += item.Cells[1].Text;
            //}
            if(ViewData["informacion_centro"] != null)
            {
                Content.Text = ViewData["informacion_centro"].ToString();

                LiteralControl datos = new LiteralControl();
                datos.Text = ViewData["informacion_centro"].ToString();
                descripcion.Controls.Add(datos);
            }



        }

        if (Session["EliminarSistema"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EliminarSistema"].ToString() + "' });", true);
            Session["EliminarSistema"] = null;
        }
    }

</script>

<asp:Content ID="versionesHead" ContentPlaceHolderID="head" runat="server">
    <title>DIMAS</title>
</asp:Content>

<asp:Content  ContentPlaceHolderID="MainContent" runat="server">
   <!DOCTYPE html>
<html lang="en">

    <!-- Page header -->
    <div class="page-header">
        <div class="page-title">
            <h3>Edición Información<small>Conjunto de matrices</small></h3>
        </div>
    </div>

    <!-- /page header -->
    <form role="form" action="#" runat="server" method="post">
        <asp:GridView ID="DatosCentro" runat="server" Visible="false">
        </asp:GridView>
        <div class="block" id="Lectura">
          
            <div style="width: 95%;    " >
                <div style="border: blue 2px inset;">
                <asp:Panel ID="descripcion" runat="server" style="margin:22px;">
                </asp:Panel>
                </div> 
                </br>
                <div style="text-align: right">


                    <a id="Editar" value="Editar" class="btn btn-primary run-first">Editar</a>

                    <a href="/evr/Home/principal" title="Volver" class="btn btn-primary run-first">Volver</a>
                </div>
            </div>
             
        </div>

        <!-- Tasks table -->
        
            <center>
                <div style="width: 95%;" class="">
      
                   <textarea id="txtTest" runat="server"></textarea>
                    <asp:TextBox ID="Content" runat="server" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
                </div>
            </center>
            <div style="text-align: right">

                <br />
                <input id="GuardarArea" type="submit" value="Guardar" class="btn btn-primary run-first" name="submit">
                <%-- <a  href="/evr/Riesgos/seleccionar_tecnologia" class="btn btn-primary run-first">Guardar</a>--%>
                   <a id="CancelarEdicion" value="Editar" class="btn btn-primary run-first">Cancelar Edición</a>
               
            </div>
     
        <!-- /tasks table -->


        <p>
            <br />
        </p>
        <!-- Footer -->
    </form>
    <div class="footer clearfix">
        <div class="pull-left"></div>
    </div>
    <!-- /footer -->

    


    <script type="text/javascript">
       
        $("txtTest").summernote({
            height: 300,                 // set editor height  
            minHeight: null,             // set minimum height of editor  
            maxHeight: null,             // set maximum height of editor  
            focus: true,                  // set focus to editable area after initializing summernote  
            callbacks: {
                onImageUpload: function (files) {
                    for (let i = 0; i < files.length; i++) {
                        UploadImage(files[i]);
                    }
                }
            }
        });
       
        $(document).ready(function () {

            

            function UploadImage(file) {
                var url = "";

                formData = new FormData();
                formData.append("aUploadedFile", file);
                $.ajax({
                    type: "POST",
                    url: url,
                    data: formData,
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function (FileUrl) {
                        // alert(FileUrl);  
                        var imgNode = document.createElement("img");
                        imgNode.src = FileUrl;
                        $("#Content").summernote("insertNode", imgNode);
                    },
                    error: function (data) {
                        alert(data.responseText);
                    }
                });
            }

        });
        $('#Editar').on('click', function () {
            $('#Edicion').show();
            $('#Lectura').hide();
        });
        $('#CancelarEdicion').on('click', function () {
            $('#Lectura').show();
            $('#Edicion').hide();

        });
        function obtenerDocumento() {
            var iframe = frames["txt_desc_designEditor"];
            if (iframe.contentDocument) {
                doc = iframe.contentWindow.document;
            } else {
                doc = iframe.contentWindow.document;
            }
            var resulhtml = doc.body.innerHTML;
            var resultext = doc.body.innerText;
        }


    </script>
    </html>
</asp:Content>
