using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using MIDAS.Helpers;

using static MIDAS.Models.Enum;

namespace MIDAS.Controllers
{
	public class BaseController : Controller
	{

		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public void Alert(string message, NotificationType notificationType, int tiempoDuracion)
		{
			var msg = message + "," + notificationType + "," + tiempoDuracion;
			//var msg = "<script language='javascript'>Swal.fire('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "</script>";
			//var msg = "<script language='javascript'>Swal.fire({title:'',text: '" + message + "',type:'" + notificationType + "',allowOutsideClick: false,allowEscapeKey: false,allowEnterKey: false})" + "</script>";
			TempData["Notification"] = msg;
		}

		/// </summary>
		/// <param name="message">The message to display to the user.</param>
		/// <param name="notifyType">The type of notification to display to the user: Success, Error or Warning.</param>
		public void Message(string message, NotificationType notifyType)
		{
			TempData["Notification2"] = message;

			switch (notifyType)
			{
				case NotificationType.success:
					TempData["NotificationCSS"] = "alert-box success";
					break;
				case NotificationType.error:
					TempData["NotificationCSS"] = "alert-box errors";
					break;
				case NotificationType.warning:
					TempData["NotificationCSS"] = "alert-box warning";
					break;

				case NotificationType.info:
					TempData["NotificationCSS"] = "alert-box notice";
					break;
			}
		}

		public string CleanInput(string strIn)
		{
			// Replace invalid characters with empty strings.
			try
			{
				return Regex.Replace(strIn, @"[^\w\.@-]", " ",
									 RegexOptions.None, TimeSpan.FromSeconds(1.5));
			}
			// If we timeout when replacing invalid characters,
			// we should return Empty.
			catch (RegexMatchTimeoutException)
			{
				return String.Empty;
			}
		}
	}
}