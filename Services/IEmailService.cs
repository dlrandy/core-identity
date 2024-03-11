using System;
using IdentityManager.Models;

namespace IdentityManager.Services
{
	public interface IEmailService
	{
		Task Send(EmailMetadata emailMetaData);
        Task SendUsingTemplate(EmailMetadata emailMetadata, string template, ApplicationUser user);
        Task SendUsingTemplateFromFile(EmailMetadata emailMetadata, ApplicationUser model, string templateFile);
        Task SendWithAttachment(EmailMetadata emailMetaData);
        Task SendMultiple(List<EmailMetadata> emailsMetadata);
    }
}

