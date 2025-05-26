namespace GadevangTennisklub2025.Models.ViewModels
{
    public class EventViewModel
    {
        /// <summary>
        /// Denne klasse er oprettet for at vise et overblik de events en person er tilmeldt på personens
        /// profils side. Den indeholder informationer om eventets id, titel og dato/tidspunkt.
        /// </summary>
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime DateAndTime { get; set; }
    }
}
