using System;
using IdentityManager.Models;
using IdentityManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{
	public class EmailController:Controller
	{
		private readonly IEmailService _emailService;
		public EmailController(IEmailService emailService)
		{
			_emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
		}

		[HttpGet]
		public async Task<IActionResult> SendSingleEmail() {
			try
			{
            EmailMetadata emailMetadata = new("1208484996@qq.com",
            "FluentEmail Test email",
            "This is a test email from FluentEmail.");
            await _emailService.Send(emailMetadata);
            return View();

			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}

        }
        [HttpGet("razortemplate")]
        public async Task<IActionResult> SendEmailWithRazorTemplate()
        {
            ApplicationUser model = new() {
                Email = "1208484996@qq.com"
            };
            EmailMetadata emailMetadata = new(model.Email,
                "FluentEmail test email with razor template");
            var template = "Dear <b>@Model.Name</b>, </br>" +
            "Thank you for being an esteemed <b>@Model.MemberType</b> member.";
            await _emailService.SendUsingTemplate(emailMetadata, template, model);
            return Ok();
        }

        [HttpGet("razortemplatefromfile")]
        public async Task<IActionResult> SendEmailWithRazorTemplateFromFile()
        {
            ApplicationUser model = new() {
                Name = "John Doe",
                Email = "1208484996@qq.com", 
            };

            EmailMetadata emailMetadata = new(model.Email,
                "FluentEmail test email with razor template file");

            var templateFile = $"{Directory.GetCurrentDirectory()}/../Templates/EmailTemplate.cshtml";

            await _emailService.SendUsingTemplateFromFile(emailMetadata, model, templateFile);

            return Ok();
        }

        [HttpGet("withattachment")]
        public async Task<IActionResult> SendEmailWithAttachment()
        {
            EmailMetadata emailMetadata = new("ziyucrc@gmail.com",
                "FluentEmail Test email",
                "This is a test email from FluentEmail.",
                $"{Directory.GetCurrentDirectory()}/Test.txt");

            await _emailService.SendWithAttachment(emailMetadata);

            return Ok();
        }
        [HttpGet("multipleemail")]
        public async Task<IActionResult> SendMultipleEmails()
        {
            List<ApplicationUser> users = new()
    {
        new(){
            Name = "John Doe", Email = "ziyucrc@gmail.com", 
        },
        new(){
            Name = "Jane Doe", Email = "1208484996@qq.com"
        }
    };

            List<EmailMetadata> emailsMetadata = new();

            foreach (var user in users)
            {
                EmailMetadata emailMetadata = new(user.Email,
                    "FluentEmail Test email",
                    "This is a test email from FluentEmail.");

                emailsMetadata.Add(emailMetadata);
            }

            await _emailService.SendMultiple(emailsMetadata);

            return Ok();
        }
    }
}

