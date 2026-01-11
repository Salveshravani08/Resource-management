using System.ComponentModel.DataAnnotations;

namespace Mycollectionproject.Models
{
    public class Collectionform
    {
        public int id { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }


        public String? Tag { get; set; }
        
        public string imageUrl { get; set; }
        public string Link { get; set; }
        public DateTime Created_at { get; set; }
        public int Fk_id { get; set; }
    }
}
