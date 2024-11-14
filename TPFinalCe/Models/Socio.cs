using System.ComponentModel.DataAnnotations;

namespace TPFinalCe.Models
{
    public class Socio
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }
        public int DNI { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }
        public string? Foto { get; set; }

        [Display(Name = "Fecha de inscripción")]
        [DataType(DataType.Date)]
        public DateTime? FechaAlta { get; set; }

        // Relaciones
        [Display(Name = "Disciplina")]
        public int? DisciplinaId { get; set; }
        public Disciplina? Disciplina { get; set; }

        public int? EstadoId { get; set; }
        public Estado? Estado { get; set; }

        public int? BeneficiosId { get; set; }
        public Beneficios? Beneficios { get; set; }

        public List<Cuota>? Cuotas { get; set; }

        public void ActualizarEstadoYBeneficios()
        {
            if (Cuotas != null && Cuotas.Any())
            {
                if (Cuotas.Any(c => !c.Pagada && c.FechaVencimiento < DateTime.Now))
                {
                    EstadoId = 2; 
                    BeneficiosId = 1; // Sin beneficios
                }
                else if (Cuotas.Any(c => !c.Pagada && c.FechaVencimiento >= DateTime.Now))
                {
                    EstadoId = 1; 
                    BeneficiosId = 2; // Beneficios en riesgo
                }
                else
                {
                    EstadoId = 1; 
                    BeneficiosId = 3; // Beneficios activos
                }
            }
        }
    }
}
