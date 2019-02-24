using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Areas.Zadania.Models
{
    public class ToDoModel
    {
        [Key]
        public int Identifier { get; set; }
        [Required(ErrorMessage = "Wprowadź temat")]
        [StringLength(255)]
        public string Temat { get; set; }
        [Required]
        [StringLength(255)]
        public string Czynnosc { get; set; }
        [StringLength(255)]
        public string Opis { get; set; }
        [Required]
        
        public int Status { get; set; }
        [Required]
        public int Priorytet { get; set; }
        [Required]
        [Display(Name ="Procent zakończenia")]
        public int ProcentZakonczenia { get; set; } 
        
        [Required, Display(Name = "Data rozpoczęcia"), DataType(DataType.Date)]
        public DateTime DataRozpoczecia { get; set; }
        [Required, Display(Name = "Data zakończenia"), DataType(DataType.Date)]
        
        public DateTime DataZakonczenia { get; set; }
    }
}