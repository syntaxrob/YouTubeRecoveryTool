
using System.ComponentModel.DataAnnotations;

namespace dotYT3.WebFramework.Models
{
    public class HomeIndexViewModel
    {
        [Required]
        public string DestinationFolder { get; set; }

        [Required]
        public string YoutubeUrl { get; set; }

        [Required]
        public string RecoverySelection { get; set; }
    }
}
