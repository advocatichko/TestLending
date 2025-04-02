using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestLending.Models
{
    [Table("CRM_Lead", Schema = "CRM")]
    public class CrmLead
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле 'П.І.Б.' є обов'язковим.")]
        [StringLength(200, ErrorMessage = "Довжина 'П.І.Б.' не може перевищувати 200 символів.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле 'Email' є обов'язковим.")]
        [StringLength(100, ErrorMessage = "Довжина 'Email' не може перевищувати 100 символів.")]
        [EmailAddress(ErrorMessage = "Некоректний формат 'Email'.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Довжина 'Повідомлення' не може перевищувати 500 символів.")]
        public string? Message { get; set; } = string.Empty; // Установлено значение по умолчанию

        [StringLength(20, ErrorMessage = "Довжина 'Номер телефону' не може перевищувати 20 символів.")]
        public string? Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string? Source { get; set; }

        public bool IsProcessed { get; set; } = false;
    }
}
