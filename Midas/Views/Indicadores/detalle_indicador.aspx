<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<script runat="server">      
    MIDAS.Models.VISTA_ObtenerUsuario user = new MIDAS.Models.VISTA_ObtenerUsuario();
    MIDAS.Models.indicadores oIndicador;
    MIDAS.Models.indicadores_imputacion oImputacion;
    MIDAS.Models.centros centroseleccionado = new MIDAS.Models.centros();
    MIDAS.Models.usuario_centros permisos = new MIDAS.Models.usuario_centros();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["usuario"] != null)
        {
            user = MIDAS.Models.Datos.ObtenerUsuarioActual(Session["usuario"].ToString());
        }

        if (Session["CentralElegida"] != null)
        {
            centroseleccionado = MIDAS.Models.Datos.ObtenerCentroPorID(int.Parse(Session["CentralElegida"].ToString()));
        }

        if (user.perfil == 2)
            permisos = MIDAS.Models.Datos.ObtenerPermisos(user.idUsuario, centroseleccionado.id);
        else
        {
            permisos.idusuario = user.idUsuario;
            permisos.idcentro = centroseleccionado.id;
            permisos.permiso = true;
        }

        for (int i = 2008; i <= DateTime.Now.Year; i++)
        {
            ListItem itemAnio = new ListItem();
            itemAnio.Value = i.ToString();
            itemAnio.Text = i.ToString();

            ddlAnio.Items.Insert(0, itemAnio);
        }

        if (ViewData["tecnologias"] != null)
        {
            ddlTecnologia.DataSource = ViewData["tecnologias"];
            ddlTecnologia.DataValueField = "id";
            ddlTecnologia.DataTextField = "nombre";
            ddlTecnologia.DataBind();
        }
        ListItem tecnologiaVacio = new ListItem();
        tecnologiaVacio.Value = "0";
        tecnologiaVacio.Text = "---";
        ddlTecnologia.Items.Insert(0, tecnologiaVacio);


        if (!IsPostBack)
        {
            oIndicador = (MIDAS.Models.indicadores)ViewData["EditarIndicador"];
            if (oIndicador != null)
            {
                hdnIdIndicador.Value = oIndicador.Id.ToString();
                txtNombre.Text = oIndicador.Nombre;
                txtDescripcion.Text = oIndicador.Descripcion;
                txtMetodo.Text = oIndicador.MetodoMedicion;
                txtUnidad.Text = oIndicador.Unidad;
                ddlTecnologia.SelectedValue = oIndicador.tecnologia.ToString();
                ddlPeriodicidad.SelectedValue = oIndicador.Periodicidad;

                if (ViewData["EditarImputacion"] != null)
                {
                    oImputacion = (MIDAS.Models.indicadores_imputacion)ViewData["EditarImputacion"];
                    ddlAnio.SelectedValue = oImputacion.anio.ToString();
                }

                if (oIndicador.ValorNumerico == false)
                {
                    //DESACTIVAR CAMPOS MEDICION
                    txtMed1.ReadOnly = true;
                    txtMed2.ReadOnly = true;
                    txtMed3.ReadOnly = true;
                    txtMed4.ReadOnly = true;
                    txtMed5.ReadOnly = true;
                    txtMed6.ReadOnly = true;
                    txtMed7.ReadOnly = true;
                    txtMed8.ReadOnly = true;
                    txtMed9.ReadOnly = true;
                    txtMed10.ReadOnly = true;
                    txtMed11.ReadOnly = true;
                    txtMed12.ReadOnly = true;
                    //OPERACION
                    MIDAS.Models.indicadores_hojadedatos_valores operador1 = new MIDAS.Models.indicadores_hojadedatos_valores();
                    MIDAS.Models.indicadores_hojadedatos_valores operador2 = new MIDAS.Models.indicadores_hojadedatos_valores();
                    MIDAS.Models.indicadores_hojadedatos_valores operador3 = new MIDAS.Models.indicadores_hojadedatos_valores();
                    if (oIndicador.Operador1 != null)
                    {
                        operador1 = MIDAS.Models.Datos.ObtenerParametroInd(int.Parse(oIndicador.Operador1.ToString()), int.Parse(ddlAnio.SelectedValue), centroseleccionado.id);
                    }
                    else
                    {
                        if (oIndicador.Operador1Constante != null)
                        {
                            operador1.valor1 = oIndicador.Operador1Constante;
                            operador1.valor2 = oIndicador.Operador1Constante;
                            operador1.valor3 = oIndicador.Operador1Constante;
                            operador1.valor4 = oIndicador.Operador1Constante;
                            operador1.valor5 = oIndicador.Operador1Constante;
                            operador1.valor6 = oIndicador.Operador1Constante;
                            operador1.valor7 = oIndicador.Operador1Constante;
                            operador1.valor8 = oIndicador.Operador1Constante;
                            operador1.valor9 = oIndicador.Operador1Constante;
                            operador1.valor10 = oIndicador.Operador1Constante;
                            operador1.valor11 = oIndicador.Operador1Constante;
                            operador1.valor12 = oIndicador.Operador1Constante;
                        }
                    }
                    if (oIndicador.Operador2 != null)
                    {
                        operador2 = MIDAS.Models.Datos.ObtenerParametroInd(int.Parse(oIndicador.Operador2.ToString()), int.Parse(ddlAnio.SelectedValue), centroseleccionado.id);
                    }
                    else
                    {
                        if (oIndicador.Operador2Constante != null)
                        {
                            operador2.valor1 = oIndicador.Operador2Constante;
                            operador2.valor2 = oIndicador.Operador2Constante;
                            operador2.valor3 = oIndicador.Operador2Constante;
                            operador2.valor4 = oIndicador.Operador2Constante;
                            operador2.valor5 = oIndicador.Operador2Constante;
                            operador2.valor6 = oIndicador.Operador2Constante;
                            operador2.valor7 = oIndicador.Operador2Constante;
                            operador2.valor8 = oIndicador.Operador2Constante;
                            operador2.valor9 = oIndicador.Operador2Constante;
                            operador2.valor10 = oIndicador.Operador2Constante;
                            operador2.valor11 = oIndicador.Operador2Constante;
                            operador2.valor12 = oIndicador.Operador2Constante;
                        }
                    }
                    if (oIndicador.Operador3 != null)
                    {
                        operador3 = MIDAS.Models.Datos.ObtenerParametroInd(int.Parse(oIndicador.Operador3.ToString()), int.Parse(ddlAnio.SelectedValue), centroseleccionado.id);
                    }
                    else
                    {
                        if (oIndicador.Operador3Constante != null)
                        {
                            operador3.valor1 = oIndicador.Operador3Constante;
                            operador3.valor2 = oIndicador.Operador3Constante;
                            operador3.valor3 = oIndicador.Operador3Constante;
                            operador3.valor4 = oIndicador.Operador3Constante;
                            operador3.valor5 = oIndicador.Operador3Constante;
                            operador3.valor6 = oIndicador.Operador3Constante;
                            operador3.valor7 = oIndicador.Operador3Constante;
                            operador3.valor8 = oIndicador.Operador3Constante;
                            operador3.valor9 = oIndicador.Operador3Constante;
                            operador3.valor10 = oIndicador.Operador3Constante;
                            operador3.valor11 = oIndicador.Operador3Constante;
                            operador3.valor12 = oIndicador.Operador3Constante;
                        }
                    }

                    if (oIndicador.Operacion1 != null && operador1 != null && operador2 != null)
                    {
                        txtMed1.ReadOnly = true;
                        txtMed2.ReadOnly = true;
                        txtMed3.ReadOnly = true;
                        txtMed4.ReadOnly = true;
                        txtMed5.ReadOnly = true;
                        txtMed6.ReadOnly = true;
                        txtMed7.ReadOnly = true;
                        txtMed8.ReadOnly = true;
                        txtMed9.ReadOnly = true;
                        txtMed10.ReadOnly = true;
                        txtMed11.ReadOnly = true;
                        txtMed12.ReadOnly = true;

                        decimal resultado = 0;
                        switch (oIndicador.Operacion1.Trim())
                        {
                            case ("+"):
                                if (operador1.valor1 != null && operador2.valor1 != null)
                                    resultado = decimal.Parse(operador1.valor1.ToString()) + decimal.Parse(operador2.valor1.ToString());
                                break;
                            case ("-"):
                                if (operador1.valor1 != null && operador2.valor1 != null)
                                    resultado = decimal.Parse(operador1.valor1.ToString()) - decimal.Parse(operador2.valor1.ToString());
                                break;
                            case ("x"):
                                if (operador1.valor1 != null && operador2.valor1 != null)
                                    resultado = decimal.Parse(operador1.valor1.ToString()) * decimal.Parse(operador2.valor1.ToString());
                                break;
                            case ("/"):
                                if (operador1.valor1 != null && operador2.valor1 != null && operador2.valor1 != 0)
                                    resultado = decimal.Parse(operador1.valor1.ToString()) / decimal.Parse(operador2.valor1.ToString());
                                break;
                        }
                        if (oIndicador.Periodicidad == "Mensual")
                            txtMed1.Text = resultado.ToString("F");

                        if (oIndicador.Operacion2 != null && operador3 != null)
                        {
                            switch (oIndicador.Operacion2.Trim())
                            {
                                case ("+"):
                                    if (operador3.valor1 != null)
                                        resultado = resultado + decimal.Parse(operador3.valor1.ToString());
                                    break;
                                case ("-"):
                                    if (operador3.valor1 != null)
                                        resultado = resultado - decimal.Parse(operador3.valor1.ToString());
                                    break;
                                case ("x"):
                                    if (operador3.valor1 != null)
                                        resultado = resultado * decimal.Parse(operador3.valor1.ToString());
                                    break;
                                case ("/"):
                                    if (operador3.valor1 != null && operador3.valor1 != 0)
                                        resultado = resultado / decimal.Parse(operador3.valor1.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual")
                                txtMed1.Text = resultado.ToString("F");

                        }


                        resultado = 0;
                        switch (oIndicador.Operacion1.Trim())
                        {
                            case ("+"):
                                if (operador1.valor2 != null && operador2.valor2 != null)
                                    resultado = decimal.Parse(operador1.valor2.ToString()) + decimal.Parse(operador2.valor2.ToString());
                                break;
                            case ("-"):
                                if (operador1.valor2 != null && operador2.valor2 != null)
                                    resultado = decimal.Parse(operador1.valor2.ToString()) - decimal.Parse(operador2.valor2.ToString());
                                break;
                            case ("x"):
                                if (operador1.valor2 != null && operador2.valor2 != null)
                                    resultado = decimal.Parse(operador1.valor2.ToString()) * decimal.Parse(operador2.valor2.ToString());
                                break;
                            case ("/"):
                                if (operador1.valor2 != null && operador2.valor2 != null && operador2.valor2 != 0)
                                    resultado = decimal.Parse(operador1.valor2.ToString()) / decimal.Parse(operador2.valor2.ToString());
                                break;
                        }
                        if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                            txtMed2.Text = resultado.ToString("F");
                        if (oIndicador.Operacion2 != null && operador3 != null)
                        {
                            switch (oIndicador.Operacion2.Trim())
                            {
                                case ("+"):
                                    if (operador3.valor2 != null)
                                        resultado = resultado + decimal.Parse(operador3.valor2.ToString());
                                    break;
                                case ("-"):
                                    if (operador3.valor2 != null)
                                        resultado = resultado - decimal.Parse(operador3.valor2.ToString());
                                    break;
                                case ("x"):
                                    if (operador3.valor2 != null)
                                        resultado = resultado * decimal.Parse(operador3.valor2.ToString());
                                    break;
                                case ("/"):
                                    if (operador3.valor2 != null && operador3.valor2 != 0)
                                        resultado = resultado / decimal.Parse(operador3.valor2.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                                txtMed2.Text = resultado.ToString("F");

                        }


                        resultado = 0;
                        switch (oIndicador.Operacion1.Trim())
                        {
                            case ("+"):
                                if (operador1.valor3 != null && operador2.valor3 != null)
                                    resultado = decimal.Parse(operador1.valor3.ToString()) + decimal.Parse(operador2.valor3.ToString());
                                break;
                            case ("-"):
                                if (operador1.valor3 != null && operador2.valor3 != null)
                                    resultado = decimal.Parse(operador1.valor3.ToString()) - decimal.Parse(operador2.valor3.ToString());
                                break;
                            case ("x"):
                                if (operador1.valor3 != null && operador2.valor3 != null)
                                    resultado = decimal.Parse(operador1.valor3.ToString()) * decimal.Parse(operador2.valor3.ToString());
                                break;
                            case ("/"):
                                if (operador1.valor3 != null && operador2.valor3 != null && operador2.valor3 != 0)
                                    resultado = decimal.Parse(operador1.valor3.ToString()) / decimal.Parse(operador2.valor3.ToString());
                                break;
                        }
                        if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Trimestral")
                            txtMed3.Text = resultado.ToString("F");
                        if (oIndicador.Operacion2 != null && operador3 != null)
                        {
                            switch (oIndicador.Operacion2.Trim())
                            {
                                case ("+"):
                                    if (operador3.valor3 != null)
                                        resultado = resultado + decimal.Parse(operador3.valor3.ToString());
                                    break;
                                case ("-"):
                                    if (operador3.valor3 != null)
                                        resultado = resultado - decimal.Parse(operador3.valor3.ToString());
                                    break;
                                case ("x"):
                                    if (operador3.valor3 != null)
                                        resultado = resultado * decimal.Parse(operador3.valor3.ToString());
                                    break;
                                case ("/"):
                                    if (operador3.valor3 != null && operador3.valor3 != 0)
                                        resultado = resultado / decimal.Parse(operador3.valor3.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Trimestral")
                                txtMed3.Text = resultado.ToString("F");

                        }


                        resultado = 0;
                        switch (oIndicador.Operacion1.Trim())
                        {
                            case ("+"):
                                if (operador1.valor4 != null && operador2.valor4 != null)
                                    resultado = decimal.Parse(operador1.valor4.ToString()) + decimal.Parse(operador2.valor4.ToString());
                                break;
                            case ("-"):
                                if (operador1.valor4 != null && operador2.valor4 != null)
                                    resultado = decimal.Parse(operador1.valor4.ToString()) - decimal.Parse(operador2.valor4.ToString());
                                break;
                            case ("x"):
                                if (operador1.valor4 != null && operador2.valor4 != null)
                                    resultado = decimal.Parse(operador1.valor4.ToString()) * decimal.Parse(operador2.valor4.ToString());
                                break;
                            case ("/"):
                                if (operador1.valor4 != null && operador2.valor4 != null && operador2.valor4 != 0)
                                    resultado = decimal.Parse(operador1.valor4.ToString()) / decimal.Parse(operador2.valor4.ToString());
                                break;
                        }
                        if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                            txtMed4.Text = resultado.ToString("F");
                        if (oIndicador.Operacion2 != null && operador3 != null)
                        {
                            switch (oIndicador.Operacion2.Trim())
                            {
                                case ("+"):
                                    if (operador3.valor4 != null)
                                        resultado = resultado + decimal.Parse(operador3.valor4.ToString());
                                    break;
                                case ("-"):
                                    if (operador3.valor4 != null)
                                        resultado = resultado - decimal.Parse(operador3.valor4.ToString());
                                    break;
                                case ("x"):
                                    if (operador3.valor4 != null)
                                        resultado = resultado * decimal.Parse(operador3.valor4.ToString());
                                    break;
                                case ("/"):
                                    if (operador3.valor4 != null && operador3.valor4 != 0)
                                        resultado = resultado / decimal.Parse(operador3.valor4.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                                txtMed4.Text = resultado.ToString("F");

                        }


                        resultado = 0;
                        switch (oIndicador.Operacion1.Trim())
                        {
                            case ("+"):
                                if (operador1.valor5 != null && operador2.valor5 != null)
                                    resultado = decimal.Parse(operador1.valor5.ToString()) + decimal.Parse(operador2.valor5.ToString());
                                break;
                            case ("-"):
                                if (operador1.valor5 != null && operador2.valor5 != null)
                                    resultado = decimal.Parse(operador1.valor5.ToString()) - decimal.Parse(operador2.valor5.ToString());
                                break;
                            case ("x"):
                                if (operador1.valor5 != null && operador2.valor5 != null)
                                    resultado = decimal.Parse(operador1.valor5.ToString()) * decimal.Parse(operador2.valor5.ToString());
                                break;
                            case ("/"):
                                if (operador1.valor5 != null && operador2.valor5 != null && operador2.valor5 != 0)
                                    resultado = decimal.Parse(operador1.valor5.ToString()) / decimal.Parse(operador2.valor5.ToString());
                                break;
                        }
                        if (oIndicador.Periodicidad == "Mensual")
                            txtMed5.Text = resultado.ToString("F");
                        if (oIndicador.Operacion2 != null && operador3 != null)
                        {
                            switch (oIndicador.Operacion2.Trim())
                            {
                                case ("+"):
                                    if (operador3.valor5 != null)
                                        resultado = resultado + decimal.Parse(operador3.valor5.ToString());
                                    break;
                                case ("-"):
                                    if (operador3.valor5 != null)
                                        resultado = resultado - decimal.Parse(operador3.valor5.ToString());
                                    break;
                                case ("x"):
                                    if (operador3.valor5 != null)
                                        resultado = resultado * decimal.Parse(operador3.valor5.ToString());
                                    break;
                                case ("/"):
                                    if (operador3.valor5 != null && operador3.valor5 != 0)
                                        resultado = resultado / decimal.Parse(operador3.valor5.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual")
                                txtMed5.Text = resultado.ToString("F");

                        }



                        resultado = 0;
                        switch (oIndicador.Operacion1.Trim())
                        {
                            case ("+"):
                                if (operador1.valor6 != null && operador2.valor6 != null)
                                    resultado = decimal.Parse(operador1.valor6.ToString()) + decimal.Parse(operador2.valor6.ToString());
                                break;
                            case ("-"):
                                if (operador1.valor6 != null && operador2.valor6 != null)
                                    resultado = decimal.Parse(operador1.valor6.ToString()) - decimal.Parse(operador2.valor6.ToString());
                                break;
                            case ("x"):
                                if (operador1.valor6 != null && operador2.valor6 != null)
                                    resultado = decimal.Parse(operador1.valor6.ToString()) * decimal.Parse(operador2.valor6.ToString());
                                break;
                            case ("/"):
                                if (operador1.valor6 != null && operador2.valor6 != null && operador2.valor6 != 0)
                                    resultado = decimal.Parse(operador1.valor6.ToString()) / decimal.Parse(operador2.valor6.ToString());
                                break;
                        }
                        if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral" || oIndicador.Periodicidad == "Trimestral" || oIndicador.Periodicidad == "Semestral")
                            txtMed6.Text = resultado.ToString("F");
                        if (oIndicador.Operacion2 != null && operador3 != null)
                        {
                            switch (oIndicador.Operacion2.Trim())
                            {
                                case ("+"):
                                    if (operador3.valor6 != null)
                                        resultado = resultado + decimal.Parse(operador3.valor6.ToString());
                                    break;
                                case ("-"):
                                    if (operador3.valor6 != null)
                                        resultado = resultado - decimal.Parse(operador3.valor6.ToString());
                                    break;
                                case ("x"):
                                    if (operador3.valor6 != null)
                                        resultado = resultado * decimal.Parse(operador3.valor6.ToString());
                                    break;
                                case ("/"):
                                    if (operador3.valor6 != null && operador3.valor6 != 0)
                                        resultado = resultado / decimal.Parse(operador3.valor6.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral" || oIndicador.Periodicidad == "Trimestral" || oIndicador.Periodicidad == "Semestral")
                                txtMed6.Text = resultado.ToString("F");

                        }


                        resultado = 0;
                        switch (oIndicador.Operacion1.Trim())
                        {
                            case ("+"):
                                if (operador1.valor7 != null && operador2.valor7 != null)
                                    resultado = decimal.Parse(operador1.valor7.ToString()) + decimal.Parse(operador2.valor7.ToString());
                                break;
                            case ("-"):
                                if (operador1.valor7 != null && operador2.valor7 != null)
                                    resultado = decimal.Parse(operador1.valor7.ToString()) - decimal.Parse(operador2.valor7.ToString());
                                break;
                            case ("x"):
                                if (operador1.valor7 != null && operador2.valor7 != null)
                                    resultado = decimal.Parse(operador1.valor7.ToString()) * decimal.Parse(operador2.valor7.ToString());
                                break;
                            case ("/"):
                                if (operador1.valor7 != null && operador2.valor7 != null && operador2.valor7 != 0)
                                    resultado = decimal.Parse(operador1.valor7.ToString()) / decimal.Parse(operador2.valor7.ToString());
                                break;
                        }
                        if (oIndicador.Periodicidad == "Mensual")
                            txtMed7.Text = resultado.ToString("F");
                        if (oIndicador.Operacion2 != null && operador3 != null)
                        {
                            switch (oIndicador.Operacion2.Trim())
                            {
                                case ("+"):
                                    if (operador3.valor7 != null)
                                        resultado = resultado + decimal.Parse(operador3.valor7.ToString());
                                    break;
                                case ("-"):
                                    if (operador3.valor7 != null)
                                        resultado = resultado - decimal.Parse(operador3.valor7.ToString());
                                    break;
                                case ("x"):
                                    if (operador3.valor7 != null)
                                        resultado = resultado * decimal.Parse(operador3.valor7.ToString());
                                    break;
                                case ("/"):
                                    if (operador3.valor7 != null && operador3.valor7 != 0)
                                        resultado = resultado / decimal.Parse(operador3.valor7.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual")
                                txtMed7.Text = resultado.ToString("F");
                        }


                        resultado = 0;
                        switch (oIndicador.Operacion1.Trim())
                        {
                            case ("+"):
                                if (operador1.valor8 != null && operador2.valor8 != null)
                                    resultado = decimal.Parse(operador1.valor8.ToString()) + decimal.Parse(operador2.valor8.ToString());
                                break;
                            case ("-"):
                                if (operador1.valor8 != null && operador2.valor8 != null)
                                    resultado = decimal.Parse(operador1.valor8.ToString()) - decimal.Parse(operador2.valor8.ToString());
                                break;
                            case ("x"):
                                if (operador1.valor8 != null && operador2.valor8 != null)
                                    resultado = decimal.Parse(operador1.valor8.ToString()) * decimal.Parse(operador2.valor8.ToString());
                                break;
                            case ("/"):
                                if (operador1.valor8 != null && operador2.valor8 != null && operador2.valor8 != 0)
                                    resultado = decimal.Parse(operador1.valor8.ToString()) / decimal.Parse(operador2.valor8.ToString());
                                break;
                        }
                        if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                            txtMed8.Text = resultado.ToString("F");
                        if (oIndicador.Operacion2 != null && operador3 != null)
                        {
                            switch (oIndicador.Operacion2.Trim())
                            {
                                case ("+"):
                                    if (operador3.valor8 != null)
                                        resultado = resultado + decimal.Parse(operador3.valor8.ToString());
                                    break;
                                case ("-"):
                                    if (operador3.valor8 != null)
                                        resultado = resultado - decimal.Parse(operador3.valor8.ToString());
                                    break;
                                case ("x"):
                                    if (operador3.valor8 != null)
                                        resultado = resultado * decimal.Parse(operador3.valor8.ToString());
                                    break;
                                case ("/"):
                                    if (operador3.valor8 != null && operador3.valor8 != 0)
                                        resultado = resultado / decimal.Parse(operador3.valor8.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                                txtMed8.Text = resultado.ToString("F");
                        }


                        resultado = 0;
                        switch (oIndicador.Operacion1.Trim())
                        {
                            case ("+"):
                                if (operador1.valor9 != null && operador2.valor9 != null)
                                    resultado = decimal.Parse(operador1.valor9.ToString()) + decimal.Parse(operador2.valor9.ToString());
                                break;
                            case ("-"):
                                if (operador1.valor9 != null && operador2.valor9 != null)
                                    resultado = decimal.Parse(operador1.valor9.ToString()) - decimal.Parse(operador2.valor9.ToString());
                                break;
                            case ("x"):
                                if (operador1.valor9 != null && operador2.valor9 != null)
                                    resultado = decimal.Parse(operador1.valor9.ToString()) * decimal.Parse(operador2.valor9.ToString());
                                break;
                            case ("/"):
                                if (operador1.valor9 != null && operador2.valor9 != null && operador2.valor9 != 0)
                                    resultado = decimal.Parse(operador1.valor9.ToString()) / decimal.Parse(operador2.valor9.ToString());
                                break;
                        }
                        if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Trimestral")
                            txtMed9.Text = resultado.ToString("F");
                        if (oIndicador.Operacion2 != null && operador3 != null)
                        {
                            switch (oIndicador.Operacion2.Trim())
                            {
                                case ("+"):
                                    if (operador3.valor9 != null)
                                        resultado = resultado + decimal.Parse(operador3.valor9.ToString());
                                    break;
                                case ("-"):
                                    if (operador3.valor9 != null)
                                        resultado = resultado - decimal.Parse(operador3.valor9.ToString());
                                    break;
                                case ("x"):
                                    if (operador3.valor9 != null)
                                        resultado = resultado * decimal.Parse(operador3.valor9.ToString());
                                    break;
                                case ("/"):
                                    if (operador3.valor9 != null && operador3.valor9 != 0)
                                        resultado = resultado / decimal.Parse(operador3.valor9.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Trimestral")
                                txtMed9.Text = resultado.ToString("F");
                        }


                        resultado = 0;
                        switch (oIndicador.Operacion1.Trim())
                        {
                            case ("+"):
                                if (operador1.valor10 != null && operador2.valor10 != null)
                                    resultado = decimal.Parse(operador1.valor10.ToString()) + decimal.Parse(operador2.valor10.ToString());
                                break;
                            case ("-"):
                                if (operador1.valor10 != null && operador2.valor10 != null)
                                    resultado = decimal.Parse(operador1.valor10.ToString()) - decimal.Parse(operador2.valor10.ToString());
                                break;
                            case ("x"):
                                if (operador1.valor10 != null && operador2.valor10 != null)
                                    resultado = decimal.Parse(operador1.valor10.ToString()) * decimal.Parse(operador2.valor10.ToString());
                                break;
                            case ("/"):
                                if (operador1.valor10 != null && operador2.valor10 != null && operador2.valor10 != 0)
                                    resultado = decimal.Parse(operador1.valor10.ToString()) / decimal.Parse(operador2.valor10.ToString());
                                break;
                        }
                        if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                            txtMed10.Text = resultado.ToString("F");
                        if (oIndicador.Operacion2 != null && operador3 != null)
                        {
                            switch (oIndicador.Operacion2.Trim())
                            {
                                case ("+"):
                                    if (operador3.valor10 != null)
                                        resultado = resultado + decimal.Parse(operador3.valor10.ToString());
                                    break;
                                case ("-"):
                                    if (operador3.valor10 != null)
                                        resultado = resultado - decimal.Parse(operador3.valor10.ToString());
                                    break;
                                case ("x"):
                                    if (operador3.valor10 != null)
                                        resultado = resultado * decimal.Parse(operador3.valor10.ToString());
                                    break;
                                case ("/"):
                                    if (operador3.valor10 != null && operador3.valor10 != 0)
                                        resultado = resultado / decimal.Parse(operador3.valor10.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral")
                                txtMed10.Text = resultado.ToString("F");
                        }


                        resultado = 0;
                        switch (oIndicador.Operacion1.Trim())
                        {
                            case ("+"):
                                if (operador1.valor11 != null && operador2.valor11 != null)
                                    resultado = decimal.Parse(operador1.valor11.ToString()) + decimal.Parse(operador2.valor11.ToString());
                                break;
                            case ("-"):
                                if (operador1.valor11 != null && operador2.valor11 != null)
                                    resultado = decimal.Parse(operador1.valor11.ToString()) - decimal.Parse(operador2.valor11.ToString());
                                break;
                            case ("x"):
                                if (operador1.valor11 != null && operador2.valor11 != null)
                                    resultado = decimal.Parse(operador1.valor11.ToString()) * decimal.Parse(operador2.valor11.ToString());
                                break;
                            case ("/"):
                                if (operador1.valor11 != null && operador2.valor11 != null && operador2.valor11 != 0)
                                    resultado = decimal.Parse(operador1.valor11.ToString()) / decimal.Parse(operador2.valor11.ToString());
                                break;
                        }
                        if (oIndicador.Periodicidad == "Mensual")
                            txtMed11.Text = resultado.ToString("F");
                        if (oIndicador.Operacion2 != null && operador3 != null)
                        {
                            switch (oIndicador.Operacion2.Trim())
                            {
                                case ("+"):
                                    if (operador3.valor11 != null)
                                        resultado = resultado + decimal.Parse(operador3.valor11.ToString());
                                    break;
                                case ("-"):
                                    if (operador3.valor11 != null)
                                        resultado = resultado - decimal.Parse(operador3.valor11.ToString());
                                    break;
                                case ("x"):
                                    if (operador3.valor11 != null)
                                        resultado = resultado * decimal.Parse(operador3.valor11.ToString());
                                    break;
                                case ("/"):
                                    if (operador3.valor11 != null && operador3.valor11 != 0)
                                        resultado = resultado / decimal.Parse(operador3.valor11.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual")
                                txtMed11.Text = resultado.ToString("F");
                        }


                        resultado = 0;
                        switch (oIndicador.Operacion1.Trim())
                        {
                            case ("+"):
                                if (operador1.valor12 != null && operador2.valor12 != null)
                                    resultado = decimal.Parse(operador1.valor12.ToString()) + decimal.Parse(operador2.valor12.ToString());
                                break;
                            case ("-"):
                                if (operador1.valor12 != null && operador2.valor12 != null)
                                    resultado = decimal.Parse(operador1.valor12.ToString()) - decimal.Parse(operador2.valor12.ToString());
                                break;
                            case ("x"):
                                if (operador1.valor12 != null && operador2.valor12 != null)
                                    resultado = decimal.Parse(operador1.valor12.ToString()) * decimal.Parse(operador2.valor12.ToString());
                                break;
                            case ("/"):
                                if (operador1.valor12 != null && operador2.valor12 != null && operador2.valor12 != 0)
                                    resultado = decimal.Parse(operador1.valor12.ToString()) / decimal.Parse(operador2.valor12.ToString());
                                break;
                        }
                        if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral" || oIndicador.Periodicidad == "Trimestral" || oIndicador.Periodicidad == "Semestral" || oIndicador.Periodicidad == "Anual")
                            txtMed12.Text = resultado.ToString("F");
                        if (oIndicador.Operacion2 != null && operador3 != null)
                        {
                            switch (oIndicador.Operacion2.Trim())
                            {
                                case ("+"):
                                    if (operador3.valor12 != null)
                                        resultado = resultado + decimal.Parse(operador3.valor12.ToString());
                                    break;
                                case ("-"):
                                    if (operador3.valor12 != null)
                                        resultado = resultado - decimal.Parse(operador3.valor12.ToString());
                                    break;
                                case ("x"):
                                    if (operador3.valor12 != null)
                                        resultado = resultado * decimal.Parse(operador3.valor12.ToString());
                                    break;
                                case ("/"):
                                    if (operador3.valor12 != null && operador3.valor12 != 0)
                                        resultado = resultado / decimal.Parse(operador3.valor12.ToString());
                                    break;
                            }
                            if (oIndicador.Periodicidad == "Mensual" || oIndicador.Periodicidad == "Bimestral" || oIndicador.Periodicidad == "Trimestral" || oIndicador.Periodicidad == "Semestral" || oIndicador.Periodicidad == "Anual")
                                txtMed12.Text = resultado.ToString("F");
                        }

                    }
                    else
                    {
                        if (operador1 != null)
                        {
                            if (operador1.valor1 != null)
                                txtMed1.Text = operador1.valor1.ToString();
                            if (operador1.valor2 != null)
                                txtMed2.Text = operador1.valor2.ToString();
                            if (operador1.valor3 != null)
                                txtMed3.Text = operador1.valor3.ToString();
                            if (operador1.valor4 != null)
                                txtMed4.Text = operador1.valor4.ToString();
                            if (operador1.valor5 != null)
                                txtMed5.Text = operador1.valor5.ToString();
                            if (operador1.valor6 != null)
                                txtMed6.Text = operador1.valor6.ToString();
                            if (operador1.valor7 != null)
                                txtMed7.Text = operador1.valor7.ToString();
                            if (operador1.valor8 != null)
                                txtMed8.Text = operador1.valor8.ToString();
                            if (operador1.valor9 != null)
                                txtMed9.Text = operador1.valor9.ToString();
                            if (operador1.valor10 != null)
                                txtMed10.Text = operador1.valor10.ToString();
                            if (operador1.valor11 != null)
                                txtMed11.Text = operador1.valor11.ToString();
                            if (operador1.valor12 != null)
                                txtMed12.Text = operador1.valor12.ToString();
                        }
                    }



                }
            }

            if (ViewData["EditarImputacion"] != null)
            {
                oImputacion = (MIDAS.Models.indicadores_imputacion)ViewData["EditarImputacion"];
                ddlAnio.SelectedValue = oImputacion.anio.ToString();
                #region switchtablas

                tablaMeses.Visible = true;
                if (oIndicador.ValorNumerico == true)
                {
                    txtMed1.Text = oImputacion.Valoracion1.ToString();
                    txtMed2.Text = oImputacion.Valoracion2.ToString();
                    txtMed3.Text = oImputacion.Valoracion3.ToString();
                    txtMed4.Text = oImputacion.Valoracion4.ToString();
                    txtMed5.Text = oImputacion.Valoracion5.ToString();
                    txtMed6.Text = oImputacion.Valoracion6.ToString();
                    txtMed7.Text = oImputacion.Valoracion7.ToString();
                    txtMed8.Text = oImputacion.Valoracion8.ToString();
                    txtMed9.Text = oImputacion.Valoracion9.ToString();
                    txtMed10.Text = oImputacion.Valoracion10.ToString();
                    txtMed11.Text = oImputacion.Valoracion11.ToString();
                    txtMed12.Text = oImputacion.Valoracion12.ToString();
                }

                switch (oIndicador.Periodicidad)
                {
                    case "Mensual":
                        tablaMeses.Visible = true;
                        txtRef1.Text = oImputacion.ValorReferencia1.ToString();
                        txtRef2.Text = oImputacion.ValorReferencia2.ToString();
                        txtRef3.Text = oImputacion.ValorReferencia3.ToString();
                        txtRef4.Text = oImputacion.ValorReferencia4.ToString();
                        txtRef5.Text = oImputacion.ValorReferencia5.ToString();
                        txtRef6.Text = oImputacion.ValorReferencia6.ToString();
                        txtRef7.Text = oImputacion.ValorReferencia7.ToString();
                        txtRef8.Text = oImputacion.ValorReferencia8.ToString();
                        txtRef9.Text = oImputacion.ValorReferencia9.ToString();
                        txtRef10.Text = oImputacion.ValorReferencia10.ToString();
                        txtRef11.Text = oImputacion.ValorReferencia11.ToString();
                        txtRef12.Text = oImputacion.ValorReferencia12.ToString();

                        txtCalc1.ReadOnly = true;
                        txtCalc2.ReadOnly = true;
                        txtCalc3.ReadOnly = true;
                        txtCalc4.ReadOnly = true;
                        txtCalc5.ReadOnly = true;
                        txtCalc6.ReadOnly = true;
                        txtCalc7.ReadOnly = true;
                        txtCalc8.ReadOnly = true;
                        txtCalc9.ReadOnly = true;
                        txtCalc10.ReadOnly = true;
                        txtCalc11.ReadOnly = true;
                        txtCalc12.ReadOnly = true;
                        txtCalc1.Text = oImputacion.ValorCalculado1.ToString();
                        txtCalc2.Text = oImputacion.ValorCalculado2.ToString();
                        txtCalc3.Text = oImputacion.ValorCalculado3.ToString();
                        txtCalc4.Text = oImputacion.ValorCalculado4.ToString();
                        txtCalc5.Text = oImputacion.ValorCalculado5.ToString();
                        txtCalc6.Text = oImputacion.ValorCalculado6.ToString();
                        txtCalc7.Text = oImputacion.ValorCalculado7.ToString();
                        txtCalc8.Text = oImputacion.ValorCalculado8.ToString();
                        txtCalc9.Text = oImputacion.ValorCalculado9.ToString();
                        txtCalc10.Text = oImputacion.ValorCalculado10.ToString();
                        txtCalc11.Text = oImputacion.ValorCalculado11.ToString();
                        txtCalc12.Text = oImputacion.ValorCalculado12.ToString();
                        break;
                    case "Bimestral":
                        tablaMeses.Visible = true;
                        txtRef1.Enabled = false;
                        txtRef2.Text = oImputacion.ValorReferencia2.ToString();
                        txtRef3.Enabled = false;
                        txtRef4.Text = oImputacion.ValorReferencia4.ToString();
                        txtRef5.Enabled = false;
                        txtRef6.Text = oImputacion.ValorReferencia6.ToString();
                        txtRef7.Enabled = false;
                        txtRef8.Text = oImputacion.ValorReferencia8.ToString();
                        txtRef9.Enabled = false;
                        txtRef10.Text = oImputacion.ValorReferencia10.ToString();
                        txtRef11.Enabled = false;
                        txtRef12.Text = oImputacion.ValorReferencia12.ToString();

                        txtCalc1.Enabled = false;
                        txtCalc2.ReadOnly = true;
                        txtCalc2.Text = oImputacion.ValorCalculado2.ToString();
                        txtCalc3.Enabled = false;
                        txtCalc4.ReadOnly = true;
                        txtCalc4.Text = oImputacion.ValorCalculado4.ToString();
                        txtCalc5.Enabled = false;
                        txtCalc6.ReadOnly = true;
                        txtCalc6.Text = oImputacion.ValorCalculado6.ToString();
                        txtCalc7.Enabled = false;
                        txtCalc8.ReadOnly = true;
                        txtCalc8.Text = oImputacion.ValorCalculado8.ToString();
                        txtCalc9.Enabled = false;
                        txtCalc10.ReadOnly = true;
                        txtCalc10.Text = oImputacion.ValorCalculado10.ToString();
                        txtCalc11.Enabled = false;
                        txtCalc12.ReadOnly = true;
                        txtCalc12.Text = oImputacion.ValorCalculado12.ToString();
                        break;
                    case "Trimestral":
                        tablaMeses.Visible = true;
                        txtRef1.Enabled = false;
                        txtRef2.Enabled = false;
                        txtRef3.Text = oImputacion.ValorReferencia3.ToString();
                        txtRef4.Enabled = false;
                        txtRef5.Enabled = false;
                        txtRef6.Text = oImputacion.ValorReferencia6.ToString();
                        txtRef7.Enabled = false;
                        txtRef8.Enabled = false;
                        txtRef9.Text = oImputacion.ValorReferencia9.ToString();
                        txtRef10.Enabled = false;
                        txtRef11.Enabled = false;
                        txtRef12.Text = oImputacion.ValorReferencia12.ToString();

                        txtCalc1.Enabled = false;
                        txtCalc2.Enabled = false;
                        txtCalc3.ReadOnly = true;
                        txtCalc3.Text = oImputacion.ValorCalculado3.ToString();
                        txtCalc4.Enabled = false;
                        txtCalc5.Enabled = false;
                        txtCalc6.ReadOnly = true;
                        txtCalc6.Text = oImputacion.ValorCalculado6.ToString();
                        txtCalc7.Enabled = false;
                        txtCalc8.Enabled = false;
                        txtCalc9.ReadOnly = true;
                        txtCalc9.Text = oImputacion.ValorCalculado9.ToString();
                        txtCalc10.Enabled = false;
                        txtCalc11.Enabled = false;
                        txtCalc12.ReadOnly = true;
                        txtCalc12.Text = oImputacion.ValorCalculado12.ToString();
                        break;
                    case "Cuatrimestral":
                        tablaMeses.Visible = true;
                        txtRef1.Enabled = false;
                        txtRef2.Enabled = false;
                        txtRef3.Enabled = false;
                        txtRef4.Text = oImputacion.ValorReferencia4.ToString();
                        txtRef5.Enabled = false;
                        txtRef6.Enabled = false;
                        txtRef7.Enabled = false;
                        txtRef8.Text = oImputacion.ValorReferencia8.ToString();
                        txtRef9.Enabled = false;
                        txtRef10.Enabled = false;
                        txtRef11.Enabled = false;
                        txtRef12.Text = oImputacion.ValorReferencia12.ToString();

                        txtCalc1.Enabled = false;
                        txtCalc2.Enabled = false;
                        txtCalc3.Enabled = false;
                        txtCalc4.ReadOnly = true;
                        txtCalc4.Text = oImputacion.ValorCalculado4.ToString();
                        txtCalc5.Enabled = false;
                        txtCalc6.Enabled = false;
                        txtCalc7.Enabled = false;
                        txtCalc8.ReadOnly = true;
                        txtCalc8.Text = oImputacion.ValorCalculado8.ToString();
                        txtCalc9.Enabled = false;
                        txtCalc10.Enabled = false;
                        txtCalc11.Enabled = false;
                        txtCalc12.ReadOnly = true;
                        txtCalc12.Text = oImputacion.ValorCalculado12.ToString();
                        break;
                    case "Semestral":
                        tablaMeses.Visible = true;
                        txtRef1.Enabled = false;
                        txtRef2.Enabled = false;
                        txtRef3.Enabled = false;
                        txtRef4.Enabled = false;
                        txtRef5.Enabled = false;
                        txtRef6.Text = oImputacion.ValorReferencia6.ToString();
                        txtRef7.Enabled = false;
                        txtRef8.Enabled = false;
                        txtRef9.Enabled = false;
                        txtRef10.Enabled = false;
                        txtRef11.Enabled = false;
                        txtRef12.Text = oImputacion.ValorReferencia12.ToString();

                        txtCalc1.Enabled = false;
                        txtCalc2.Enabled = false;
                        txtCalc3.Enabled = false;
                        txtCalc4.Enabled = false;
                        txtCalc5.Enabled = false;
                        txtCalc6.ReadOnly = true;
                        txtCalc6.Text = oImputacion.ValorCalculado6.ToString();
                        txtCalc7.Enabled = false;
                        txtCalc8.Enabled = false;
                        txtCalc9.Enabled = false;
                        txtCalc10.Enabled = false;
                        txtCalc11.Enabled = false;
                        txtCalc12.ReadOnly = true;
                        txtCalc12.Text = oImputacion.ValorCalculado12.ToString();
                        break;
                    case "Anual":
                        tablaMeses.Visible = true;
                        txtRef1.Enabled = false;
                        txtRef2.Enabled = false;
                        txtRef3.Enabled = false;
                        txtRef4.Enabled = false;
                        txtRef5.Enabled = false;
                        txtRef6.Enabled = false;
                        txtRef7.Enabled = false;
                        txtRef8.Enabled = false;
                        txtRef9.Enabled = false;
                        txtRef10.Enabled = false;
                        txtRef11.Enabled = false;
                        txtRef12.Text = oImputacion.ValorReferencia12.ToString();

                        txtCalc1.Enabled = false;
                        txtCalc2.Enabled = false;
                        txtCalc3.Enabled = false;
                        txtCalc4.Enabled = false;
                        txtCalc5.Enabled = false;
                        txtCalc6.Enabled = false;
                        txtCalc7.Enabled = false;
                        txtCalc8.Enabled = false;
                        txtCalc9.Enabled = false;
                        txtCalc10.Enabled = false;
                        txtCalc11.Enabled = false;
                        txtCalc12.ReadOnly = true;
                        txtCalc12.Text = oImputacion.ValorCalculado12.ToString();
                        break;
                }
                #endregion

            }
        }

        if (Session["EdicionIndicadorMensaje"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-success', header: '" + Session["EdicionIndicadorMensaje"].ToString() + "' });", true);
            Session["EdicionIndicadorMensaje"] = null;
        }

        if (Session["EdicionIndicadorError"] != null)
        {
            Page.ClientScript.RegisterStartupScript(typeof(string), "jgrowlnotify", "$.jGrowl('', { theme: 'growl-error', header: '" + Session["EdicionIndicadorError"].ToString() + "' });", true);
            Session["EdicionIndicadorError"] = null;
        }


    }


</script>
<asp:Content ID="headEditarPedido" ContentPlaceHolderID="head" runat="server">
    <title>Midas-Indicador </title>
    <script type="text/javascript">


        $(document).ready(function () {

            $("form").submit(function () {
                var val = $("input[type=submit][clicked=true]").attr("id")

                if (val == "GuardarIndicador")
                    $("#hdFormularioEjecutado").val("GuardarIndicador");
                if (val == "btnImprimir")
                    $("#hdFormularioEjecutado").val("btnImprimir");

            });

            $("form input[type=submit]").click(function () {
                $("input[type=submit]", $(this).parents("form")).removeAttr("clicked");
                $(this).attr("clicked", "true");
            });          


        });

        
       
    </script>
</asp:Content>
<asp:Content ID="contentEditarPedido" ContentPlaceHolderID="MainContent" runat="server">
    <input id="HiddenResultado" type="hidden" value="<%= Session["resultado"]%>" />
    <!-- Page header -->
    <div class="page-header">
		<div class="page-title">
            <h3>
                Edición de Indicador
            </h3>
        </div> 
    </div>

    <!-- /page header -->
    <!-- Form vertical (default) -->
    <form role="form" action="#" runat="server">
    <input id="hdFormularioEjecutado" name="hdFormularioEjecutado" type="hidden" value="Entro" />
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-search"></i>Datos generales</h6>
        </div>
        <div class="panel-body">
        <asp:HiddenField runat="server" ID="hdnIdIndicador" />
            <table style="width:100%">
                <tr>
                    <td class="form-group" style="width: 100%">

                            <label>
                                Nombre</label>
                        <asp:TextBox ID="txtNombre" ReadOnly="true" Width="98%" runat="server" class="form-control"></asp:TextBox>

                        </td>    
                    </tr>
                </table>
                <br />
            <table style="width:100%">
                <tr>
                    
                            <td style="width:40%">
                                    <label>
                                    Método de medición</label>
                                    <asp:TextBox ID="txtMetodo" ReadOnly="true" Width="95%" runat="server" class="form-control"></asp:TextBox>
                            </td>  
                            <td style="width:20%">
                                    <label>Periodicidad</label>
                                    <asp:DropDownList ReadOnly="true" Width="95%" CssClass="form-control" ID="ddlPeriodicidad" runat="server"> 
                                                            <asp:ListItem Value="Mensual" Text="Mensual"></asp:ListItem>
                                                            <asp:ListItem Value="Bimestral" Text="Bimestral"></asp:ListItem>
                                                            <asp:ListItem Value="Trimestral" Text="Trimestral"></asp:ListItem>
                                                            <asp:ListItem Value="Cuatrimestral" Text="Cuatrimestral"></asp:ListItem>
                                                            <asp:ListItem Value="Semestral" Text="Semestral"></asp:ListItem>
                                                            <asp:ListItem Value="Anual" Text="Anual"></asp:ListItem>
                                    </asp:DropDownList>  
                            </td>             
                            <td class="form-group" style="width: 10%">

                                <label>Unidad</label>
                                <asp:TextBox ReadOnly="true" ID="txtUnidad" Width="90%" runat="server" class="form-control"></asp:TextBox>

                                </td>   
                                <td style="width:30%">
                                    <label>Tecnología</label>
                                    <asp:DropDownList ReadOnly="true" Width="98%" CssClass="form-control" ID="ddlTecnologia" runat="server"> 
                                                            </asp:DropDownList>  
                            </td>                  
                    </tr>
                    <tr>
                        <td colspan="4">
                        <br />
                                <label>Descripción</label>
                                <asp:TextBox ID="txtDescripcion" ReadOnly="true"  TextMode="MultiLine" Rows="5" runat="server" class="form-control"></asp:TextBox>

                        </td>
                    </tr>
              </table>
            <br />
        </div>
    </div>

    

    <% if (oImputacion != null)
       { %>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <i class="icon-rulers"></i>Medición</h6>
        </div>
        <div class="panel-body">
            <div class="form-group">
                <table style="width:40%">
                    <tr>
                        <td style="width:30%" class="form-group">
                            <label>Año de medición</label>
                            <asp:DropDownList AutoPostBack="true" runat="server" ID="ddlAnio" class="form-control" Width="90%">
                            </asp:DropDownList>
                        </td>
                        <td  style="width:70%" class="form-group">
                        <label>Operación</label>
                                    <asp:DropDownList Width="98%" CssClass="form-control" ID="ddlOperacion" runat="server"> 
                                        <asp:ListItem Value="Computo" Text="Cómputo"></asp:ListItem>
                                        <asp:ListItem Value="Acumulado" Text="Acumulado"></asp:ListItem>
                                        <asp:ListItem Value="AcumuladoPeriodo" Text="Acumulado período"></asp:ListItem>
                                        <asp:ListItem Value="Promedio" Text="Promedio"></asp:ListItem>
                                        <asp:ListItem Value="PromedioPeriodo" Text="Promedio Período"></asp:ListItem>
                                                            </asp:DropDownList>  
                        </td>
                    </tr>
                </table>
                
            </div>
            <table runat="server" visible="false" width="100%" id="tablaMeses">
                <tr>
                    <td style="width:7%">
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Enero</label>
                        </center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Febrero</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Marzo</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Abril</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Mayo</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Junio</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Julio</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Agosto</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Septiembre</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Octubre</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Noviembre</label></center>
                    </td>
                    <td style="width:7%">
                    <center>
                        <label>Diciembre</label></center>
                    </td>
                </tr>

                <tr>
                    <td style="width:7%">
                    <center>
                        <label>Referencia</label>
                    </center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtRef1" Width="70%" runat="server" class="form-control"></asp:TextBox>
                        </center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtRef2" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtRef3" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtRef4" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtRef5" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtRef6" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtRef7" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtRef8" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtRef9" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtRef10" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtRef11" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtRef12" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                </tr>
                <tr>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <label>Medición</label>
                    </center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed1" Width="70%" runat="server" class="form-control"></asp:TextBox>
                        </center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed2" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed3" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed4" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed5" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed6" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed7" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed8" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed9" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed10" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed11" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtMed12" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                </tr>
                
                <tr>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <label>Operación</label>
                    </center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtCalc1" Width="70%" runat="server" class="form-control"></asp:TextBox>
                        </center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtCalc2" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtCalc3" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtCalc4" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtCalc5" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtCalc6" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtCalc7" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtCalc8" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtCalc9" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtCalc10" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtCalc11" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                    <td style="width:7%; padding-top:10px">
                    <center>
                        <asp:TextBox ID="txtCalc12" Width="70%" runat="server" class="form-control"></asp:TextBox></center>
                    </td>
                </tr>
            </table>

            

            <br />

        </div>
    </div>
   <% } %>
    <!-- /modal with table -->
    <div class="form-actions text-right">
        <% if (oImputacion != null)
           { %>
        <input id="btnImprimir" type="submit" value="Exportar" class="btn btn-primary run-first" />
        <% } %>
		<%
            if (permisos.permiso == true) 
            {
        %>
        <input id="GuardarIndicador" type="submit" value="Guardar datos" class="btn btn-primary run-first" />
        <% } %>
        <a href="/evr/indicadores/gestion_indicadores" title="Volver" class="btn btn-primary run-first">Volver</a>
    </div>
    </form>
    <!-- /form vertical (default) -->
    <!-- Footer -->
    <div class="footer clearfix">
        <div class="pull-left">
        </div>
    </div>
    <!-- /footer -->
</asp:Content>
