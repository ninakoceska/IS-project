﻿using BookStore.Domain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<BookInShoppingCart>? Books { get; set; }
        public double TotalPrice { get; set; }
    }
}
