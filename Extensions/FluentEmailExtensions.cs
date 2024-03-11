using System;
using System.Net;
using System.Net.Mail;

namespace IdentityManager.Extensions
{
	public static class FluentEmailExtensions
	{
		public static void AddFluentEmail(this IServiceCollection services, ConfigurationManager configuration)
		{
            var emailSettings = configuration.GetSection("EmailSettings");
            var defaultFromEmail = emailSettings["Mail"];
            var host = emailSettings["Host"];
            var port = emailSettings.GetValue<int>("Port");
            var userName = emailSettings["DisplayName"];
            var password = emailSettings["Password"];
            services.AddFluentEmail(defaultFromEmail)
                .AddSmtpSender(new System.Net.Mail.SmtpClient(host, port) {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(userName, password)
                })
                .AddRazorRenderer();
        }
	}
}

