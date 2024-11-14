namespace TPFinalCe.Models
{
    public class Disciplina
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Foto { get; set; }

        public List<Socio> Socios { get; set; }
    }
}
