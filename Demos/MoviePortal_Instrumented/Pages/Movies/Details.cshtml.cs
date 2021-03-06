﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoviePortal.Models;

namespace MoviePortal.Pages.Movies
{
    public class DetailsModel : PageModel
    {
        private readonly MoviePortal.Models.MoviePortalContext _context;
        private Dictionary<string, string> _emojis;
        public string Emoji { get; set; }

        public DetailsModel(MoviePortal.Models.MoviePortalContext context)
        {
            _context = context;
            InitEmojiList();
        }

        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.Movie.FirstOrDefaultAsync(m => m.ID == id);
            Emoji = _emojis[Movie.Genre.ToLower()];

            if (Movie == null)
            {
                return NotFound();
            }
            return Page();
        }

        private void InitEmojiList()
        {
            _emojis = new Dictionary<string, string>
            {
                { "fantasy", "&#x1F680;" },
                { "western", "&#x1F920;" },
                { "comedy", "&#x1F923;" }
            };
        }
    }
}
