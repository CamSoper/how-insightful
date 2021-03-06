﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MoviePortal.Models;

namespace MoviePortal.Pages.Movies
{
    #region snippet_newProps
    public class IndexModel : PageModel
    {
        private readonly MoviePortal.Models.MoviePortalContext _context;

        public IndexModel(MoviePortal.Models.MoviePortalContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get; set; }
        public SelectList Genres { get; set; }
        public string MovieGenre { get; set; }
        #endregion

        // Requires using Microsoft.AspNetCore.Mvc.Rendering;
        public async Task OnGetAsync(string movieGenre, string searchString)
        {
            await Search(movieGenre, searchString);
        }

        public PartialViewResult OnGetMovieTable(string movieGenre, string searchString)
        {
            Search(movieGenre, searchString).Wait();
            return new PartialViewResult
            {
                ViewName = "_MovieList",
                ViewData = new ViewDataDictionary<List<Movie>>(ViewData, Movie)
            };
        }

        private async Task Search(string movieGenre, string searchString)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            var movies = from m in _context.Movie
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            Movie = await movies.ToListAsync();
        }
    }
}
