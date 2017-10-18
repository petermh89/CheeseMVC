using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Models;

namespace CheeseMVC.ViewModels
{
    public class ViewMenuViewModel
    {
        public Menu Menu = new Menu();
        public IList<CheeseMenu> Items { get; set; } = new List<CheeseMenu>();

    }
}