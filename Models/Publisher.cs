
using
System.ComponentModel.DataAnnotations.Schema;
namespace Baciu_Dora_Lab2.Models
{
    [Table("Publishers")]
    public class Publisher
    {
        
        public int ID { get; set; }
        public string PublisherName { get; set; }
        public ICollection<Book>? Books { get; set; }  //navigation property 
    }
}
