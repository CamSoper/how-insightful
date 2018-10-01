using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieNewsService.Services
{
    
    public class RandomService
    {
        Random _rnd;

        public RandomService()
        {
            _rnd = new Random();
        }

        public int GetRandom(int Low, int High)
        {
            return _rnd.Next(Low, High);
        }

    }
}
