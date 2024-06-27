using System.ComponentModel.DataAnnotations;

namespace MailSender.Models
{
    public class EmailViewModel
    {
        [Required]
        [Display(Name = "Получатели")]
        public string Recipients { get; set; }

        [Required]
        [Display(Name = "Тема")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Сообщение")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Display(Name = "Вложения")]
        public List<IFormFile> Attachments { get; set; }
    }
}
