using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Baciu_Dora_Lab2.Data;
using Baciu_Dora_Lab2.Models;
using Baciu_Dora_Lab2.Models;

namespace Baciu_Dora_Lab2.Pages.Publishers
{
    public class EditModel : BookCategoriesPageModel
    {
        private readonly Baciu_Dora_Lab2.Data.Baciu_Dora_Lab2Context _context;

        public EditModel(Baciu_Dora_Lab2.Data.Baciu_Dora_Lab2Context context)
        {
            _context = context;
        }
        //.
        [BindProperty]
        public Publisher Publisher { get; set; } = default!;
        [BindProperty]
        public Book book { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           book = await _context.Book
               .Include(b => b.Publisher)
               .Include(b => b.BookCategories).ThenInclude(b => b.Category)
               .AsNoTracking()
               .FirstOrDefaultAsync(m => m.ID == id);

            var publisher =  await _context.Publisher.FirstOrDefaultAsync(m => m.ID == id);
            if (publisher == null)
            {
                return NotFound();
            }
            Publisher = publisher;
            return Page();

        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id, string[]
selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }

            //se va include Author  conform cu sarcina de la lab 2 

            var bookToUpdate = await _context.Book
                .Include(i => i.Publisher)
                .Include(i => i.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .FirstOrDefaultAsync(s => s.ID == id);
            if (bookToUpdate == null)
            {
                return NotFound();
            }
            PopulateAssignedCategoryData(_context, book);
            //se va modifica AuthorID  conform cu sarcina de la lab 2 

            if (await TryUpdateModelAsync<Book>(
                bookToUpdate,
                "book",
                i => i.Title, i => i.Author,
                 i => i.Price, i => i.PublishingDate, i => i.PublisherID))
            {
                UpdateBookCategories(_context, selectedCategories, bookToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            UpdateBookCategories(_context, selectedCategories, bookToUpdate);
            PopulateAssignedCategoryData(_context, bookToUpdate);
            return Page();
        }
    }
}