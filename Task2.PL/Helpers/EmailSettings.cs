using System.Net;
using System.Net.Mail;
using Task2.DAL.Models;

namespace Task2.PL.Helpers
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var Client = new SmtpClient("smtp.gmail.com",587);
			Client.EnableSsl= true;
			Client.Credentials = new NetworkCredential("norhansalemabdelhameedmohamed@gmail.com", "hsfqpcbjrrdtoslt");
			Client.Send("norhansalemabdelhameedmohamed@gmail.com", email.To, email.Subject, email.Body);


		}
	}
}
