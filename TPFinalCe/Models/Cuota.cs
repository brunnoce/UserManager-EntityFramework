using System.ComponentModel.DataAnnotations;

namespace TPFinalCe.Models
{
    public class Cuota
    {
        public int Id { get; set; }
        [Required]
        public int SocioId { get; set; }
        public Socio Socio { get; set; }
        [Required]
        public int Monto { get; set; }
        public DateTime EmisionCuota { get; set; }

        [Display(Name = "Fecha de vencimiento")]
        public DateTime FechaVencimiento { get; set; }

        public bool Pagada { get; set; } 
    }
}
