using System.Collections.Generic; // S-ar putea să fie nevoie de asta

namespace Baciu_Dora_Lab2.Models
{
    public class Author
    {
        public int ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public ICollection<Book>? Books { get; set; }
    }
}