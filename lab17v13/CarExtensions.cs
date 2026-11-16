using System;
using System.Linq;

namespace lab17v13
{
    public static class CarExtensions
    {
        public static decimal TotalCarPrice(this Car car)
        {
            if (car == null)
                throw new ArgumentNullException(nameof(car));

            decimal optionsTotal = car.Options?.Sum(o => o.Price) ?? 0m;
            return car.BasePrice + optionsTotal;
        }
    }
}