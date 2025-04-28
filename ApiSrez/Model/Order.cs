using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSrez.Model
{
    public class Order
    {
        [Key]
        public int id_Order { get; set; }
        public DateTime TimeStart { get; set;}
        public DateTime TimeFinish { get; set; }

        [Required]
        [ForeignKey("Users")]
        public int User_id { get; set; }

        [Required]
        [ForeignKey("Games")]
        public int Game_id { get; set; }
    }
}
