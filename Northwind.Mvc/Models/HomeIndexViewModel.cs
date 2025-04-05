﻿using PraktiskaAppar;

namespace Northwind.Mvc.Models
{
    public record HomeIndexViewModel
    (
        int VisitorCount,
            IList<Category> Categories,
            IList<Product> Products,
            IList<Product> ProductCategory
    );

}
