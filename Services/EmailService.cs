using System;
using FluentEmail.Core;
using IdentityManager.Models;

namespace IdentityManager.Services
{
	public class EmailService :IEmailService
	{
        private readonly IFluentEmail _fluentEmail;
        private readonly IFluentEmailFactory _fluentEmailFactory;
		public EmailService(IFluentEmail fluentEmail, IFluentEmailFactory fluentEmailFactory)
		{
            _fluentEmail = fluentEmail ?? throw new ArgumentNullException(nameof(fluentEmail));
            _fluentEmailFactory = fluentEmailFactory ?? throw new ArgumentNullException(nameof(fluentEmailFactory));
		}

        public async Task Send(EmailMetadata emailMetaData)
        {
            await _fluentEmail.To(emailMetaData.ToAddress)
                .Subject(emailMetaData.Subject)
                .Body(emailMetaData.Body)
                .SendAsync();
        }

        public async Task SendUsingTemplateFromFile(EmailMetadata emailMetadata, ApplicationUser user, string templateFile)
        {
            await _fluentEmail.To(emailMetadata.ToAddress)
                .Subject(emailMetadata.Subject)
                .UsingTemplateFromFile(templateFile, user)
                .SendAsync();
        }

        public async Task SendUsingTemplate(EmailMetadata emailMetadata, string template,ApplicationUser user)
        {
            await _fluentEmail.To(emailMetadata.ToAddress)
                .Subject(emailMetadata.Subject)
                .UsingTemplate(template, user)
                .SendAsync();
        }
        public async Task SendWithAttachment(EmailMetadata emailMetadata)
        {
            await _fluentEmail.To(emailMetadata.ToAddress)
                .Subject(emailMetadata.Subject)
                .AttachFromFilename(emailMetadata.AttachmentPath,
                    attachmentName: Path.GetFileName(emailMetadata.AttachmentPath))
                .Body(emailMetadata.Body)
                .SendAsync();
        }

        public async Task SendMultiple(List<EmailMetadata> emailsMetadata)
        {
            foreach (var item in emailsMetadata)
            {
                await _fluentEmailFactory
                    .Create()
                    .To(item.ToAddress)
                    .Subject(item.Subject)
                    .Body(item.Body)
                    .SendAsync();
            }
        }
    }
}

