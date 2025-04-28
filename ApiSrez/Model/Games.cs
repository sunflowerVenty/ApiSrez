using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSrez.Model
{
    public class Games
    {
        [Key]
        public int id_Game { get; set; }
        public string Name { get; set; }
    }
}
