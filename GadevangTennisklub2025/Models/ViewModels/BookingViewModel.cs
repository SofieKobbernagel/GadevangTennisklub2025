namespace GadevangTennisklub2025.Models.ViewModels
{
    public class BookingViewModel
    {
        /// <summary>
        /// Denne klasse er oprettet med henblik på at vise et overblik på en persons profilside 
        /// over de bookinger som personen har lavet med informationer om navnet på den bane der er booket
        /// og navnet på den person som bookingen er lavet med.
        /// </summary>
        public Booking Booking { get; set; }
        public string CourtName { get; set; }
        public string PartnerName { get; set; }
    }

}
