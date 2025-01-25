using System.ComponentModel.DataAnnotations;

namespace BookStoreAdminApplication.Models
{
    public class Book
    {
        public string? BookName { get; set; }
        public string? BookDescription { get; set; }
        public string? BookImage { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Rating { get; set; }
    }
}
