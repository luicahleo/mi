<%@ Page Title="" Language="C#" MaintainScrollPositionOnPostback="true" MasterPageFile="~/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">    
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    //List<MIDAS.Models.tareas> listaTareas = new List<MIDAS.Models.tareas>();
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    Hashtable _scheduleData;
    int consulta = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            if (Session["usuario"] != null || Session["CentralElegida"] == null)
            {
                user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
            }

            if (Session["CentralElegida"] != null)
            {
                centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
            }

            _scheduleData = GetSchedule();


            //calendario.OtherMonthDayStyle.BackColor = System.Drawing.Color.LightGray;          
            //calendario.FirstDayOfWeek =  FirstDayOfWeek.Monday;
            //calendario.NextPrevFormat = NextPrevFormat.ShortMonth;
            //calendario.TitleFormat = TitleFormat.MonthYear;
            //calendario.ShowGridLines = true;
            //calendario.DayStyle.HorizontalAlign = HorizontalAlign.Left;
            //calendario.DayStyle.VerticalAlign = VerticalAlign.Top;
            //calendario.DayStyle.Height = 75;
            //calendario.DayStyle.Width = 100;

            //listaTareas = (List<MIDAS.Models.Tareas>)ViewData["tareas"];

            //if (listaTareas != null)
            //{               
                
            //}
            //else
            //{

            //}
        }                                              

    }

    private Hashtable GetSchedule()
    {
        Hashtable schedule = new Hashtable();

        schedule["11/05/2018"] = "Reunión de MIDAS";

        return schedule;
    }

    protected void calendario_DayRender(object sender, DayRenderEventArgs e)
    {
        if (_scheduleData[e.Day.Date.ToShortDateString()] != null)
        {
            Literal lit = new Literal();
            lit.Text = "<br/>";
            e.Cell.Controls.Add(lit);
            
            Label lbl = new Label();
            lbl.Text = (string)_scheduleData[e.Day.Date.ToShortDateString()];
            lbl.Font.Size = new FontUnit(FontSize.Small);
            lbl.ForeColor = System.Drawing.Color.Blue;
            e.Cell.Controls.Add(lbl);
        }
    }

    void MonthChange(Object sender, MonthChangedEventArgs e)
    {

        if (e.NewDate.Month > e.PreviousDate.Month)
        {
           
        }
        else
        {

        }

    }


</script>

<asp:Content ID="headEditarObjetivo" ContentPlaceHolderID="head" runat="server">
    <title>Midas - Gestor de tareas </title>
    <script type="text/javascript">
        $(document).ready(function () {
                       

        });        
       
    </script>
    <style type="text/css">
        .button-next:hover 
        {
            background-color: #e6e6e6;
        }
        
        .button-prev:hover 
        {
            background-color: #e6e6e6;
        }
    </style>

  <%--  <link href="/Content/fullcalendar.css" rel="stylesheet" type="text/css" />--%>
    <link rel="stylesheet" href="https://fullcalendar.io/releases/fullcalendar/3.9.0/fullcalendar.min.css"/>
    

</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <table width="100%">
        <tr>
            <td>
                <div class="page-header">
        <div class="page-title">
            <h3>
                Gestor de tareas<asp:Label runat="server" ID="lblNombreProc"></asp:Label></h3>
        </div>
    </div>
            </td>
            <td style="float: right; padding-top:10px">
               <table style="float:right; border:1px solid #0555FA">
                    <tr>
                        <td style="padding-left:15px; padding-right:15px; padding: 5px">
                        <center>
                            <label>Objetivo</label>
                            <div style="width:30px; height:10px; background-color:#0555FA"></div> 
                        </center>
                        </td>
                        <td style="padding-left:15px; padding-right:15px; padding: 5px">
                        <center>
                            <label>Accion objetivo</label>
                            <div style="width:30px; height:10px; background-color:#41b9e6"></div> 
                        </center>
                        </td>
                        <td style="padding-left:15px; padding-right:15px; padding: 5px">
                        <center>
                            <label>Acción mejora</label>
                            <div style="width:30px; height:10px; background-color:#008c5a"></div> 
                        </center>
                        </td>

                        <td style="padding-left:15px; padding-right:15px; padding: 5px">
                        <center>
                            <label>Acción inmediata</label>
                            <div style="width:30px; height:10px; background-color:#55be5a"></div> 
                        </center>
                        </td>
                        <td style="padding-left:15px; padding-right:15px; padding: 5px">
                        <center>
                            <label>Desp. acción mejora</label>
                            <div style="width:30px; height:10px; background-color:#38d130"></div> 
                        </center>
                        </td>
                        <td style="padding-left:15px; padding-right:15px; padding: 5px">
                        <center>
                            <label>Emergencia</label>
                            <div style="width:30px; height:10px; background-color:#ff0f64"></div> 
                        </center>
                        </td>
                        <td style="padding-left:15px; padding-right:15px; padding: 5px">
                        <center>
                            <label>Reunión</label>
                            <div style="width:30px; height:10px; background-color:#ff5a0f"></div> 
                        </center>
                        </td>
                    </tr>
                </table>
            </td>

        </tr>
    </table>
    
    
    <br />
    
    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form id="Form1" runat="server">
    
   <%-- <asp:Calendar ID="calendario" OnDayRender="calendario_DayRender" OnVisibleMonthChanged="MonthChange" Width="100%" Height="400px" Font-Size="Larger" runat="server">
    </asp:Calendar>--%>

<%--    @{
     ViewBag.Title = "TestFullCal";
     Layout = "~/Views/Shared/_Layout.cshtml";
    }--%>

    <div id="calendar"></div>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#calendar').fullCalendar({
                theme: false,
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay',                    
                },
                buttonText: {
                    today: 'Hoy',
                    month: 'Mes',
                    week: 'Semana',
                    day: 'Día',
                    list: 'Lista'
                },
                editable: false,
                monthNames: ['Enero','Febrero','Marzo','Abril','Mayo','Junio','Julio','Agosto','Septiembre','Octubre','Noviembre','Diciembre'],
                monthNamesShort: ['Ene','Feb','Mar','Abr','May','Jun','Jul','Ago','Sep','Oct','Nov','Dic'],
                dayNames: ['Domingo','Lunes','Martes','Miércoles','Jueves','Viernes','Sábado'],
                dayNamesShort: ['Dom','Lun','Mar','Mié','Jue','Vie','Sáb'],
                events: "/GestorTareas/GetEvents/",
                height: 500
            });
        });
    </script>

    <br />

    </form>
    <!-- /form vertical (default) -->
    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left">
        </div>
    </div>
    <!-- /footer -->
</asp:Content>
