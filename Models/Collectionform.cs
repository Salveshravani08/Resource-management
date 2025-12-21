using System.ComponentModel.DataAnnotations;

namespace Mycollectionproject.Models
{
    public class Collectionform
    {
        public int id { get; set; }
        
        public String Title { get; set; }
        [Required]

        public string Description { get; set; }
        [Required]

        public String Tag { get; set; }
        [Required]
        public string imageUrl { get; set; }
        public string Link { get; set; }
        public DateTime Created_at { get; set; }
        public int Fk_id { get; set; }
    }
}
