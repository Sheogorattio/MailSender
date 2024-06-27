using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using MailSender.Models;

namespace MailSender.Controllers
{
    public class EmailController : Controller
    {
        [HttpGet]
        public IActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(EmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("TestName", "Не скажу"));

                var recipients = model.Recipients.Split(';').Select(r => r.Trim()).ToList();
                foreach (var recipient in recipients)
                {
                    message.To.Add(new MailboxAddress(recipient, recipient));
                }

                message.Subject = model.Subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = model.Body
                };

                if (model.Attachments != null && model.Attachments.Count > 0)
                {
                    foreach (var attachment in model.Attachments)
                    {
                        if (attachment.Length > 0)
                        {
                            using (var stream = new MemoryStream())
                            {
                                await attachment.CopyToAsync(stream);
                                bodyBuilder.Attachments.Add(attachment.FileName, stream.ToArray());
                            }
                        }
                    }
                }

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {

                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("Не скажу", "Не скажу");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return RedirectToAction("SendEmail", new { message = "Email отправлен успешно!" });
            }

            return View(model);
        }
    }
}
