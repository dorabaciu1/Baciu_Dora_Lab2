using Baciu_Dora_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baciu_Dora_Lab2.Pages.Books
{
    public class IndexModel : PageModel
    {
        private readonly Baciu_Dora_Lab2.Data.Baciu_Dora_Lab2Context _context;

        public IndexModel(Baciu_Dora_Lab2.Data.Baciu_Dora_Lab2Context context)
        {
            _context = context;
        }


        public IList<Book> Book { get; set; }
        public BookData BookD { get; set; }
        public int BookID { get; set; }
        public int CategoryID { get; set; }

        [BindProperty(SupportsGet = true)]

        public string TitleSort { get; set; }
        public string AuthorSort { get; set; }


        public string CurrentFilter { get; set; }
        public string AuthorFilter { get; set; }

        public SelectList AuthorNames { get; set; }
        public async Task OnGetAsync(int? id, int? categoryID, string sortOrder, string searchString)
        {
            // 1. Lista pentru dropdown (AuthorNames) - o lasam simpla sau o comentam daca da eroare
            // Daca Author e string, nu mai putem face selectia complexa de sus, deci o simplificam:
            AuthorNames = new SelectList(await _context.Book.Select(x => x.Author).Distinct().ToListAsync());

            BookD = new BookData();
            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            AuthorSort = sortOrder == "author" ? "author_desc" : "author";
            CurrentFilter = searchString;

            // 2. Luam cartile FARA .Include(b => b.Author) pentru ca Author e string acum
            BookD.Books = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                .ThenInclude(b => b.Category)
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .ToListAsync();

            // 3. Cautare simplificata (fara FirstName/LastName)
            if (!String.IsNullOrEmpty(searchString))
            {
                BookD.Books = BookD.Books.Where(s => s.Author.Contains(searchString)
                                                  || s.Title.Contains(searchString));
            }

            // 4. Selectie ID
            if (id != null)
            {
                BookID = id.Value;
                Book book = BookD.Books.Where(i => i.ID == id.Value).Single();
                BookD.Categories = book.BookCategories.Select(s => s.Category);
            }

            // 5. Sortare simplificata (Codul de mai sus)
            switch (sortOrder)
            {
                case "title_desc":
                    BookD.Books = BookD.Books.OrderByDescending(s => s.Title);
                    break;
                case "author_desc":
                    BookD.Books = BookD.Books.OrderByDescending(s => s.Author);
                    break;
                case "author":
                    BookD.Books = BookD.Books.OrderBy(s => s.Author);
                    break;
                default:
                    BookD.Books = BookD.Books.OrderBy(s => s.Title);
                    break;
            }
        }
    }
}
    