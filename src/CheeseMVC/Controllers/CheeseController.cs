using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class CheeseController : Controller
    {
        private readonly CheeseDbContext context;

        public CheeseController(CheeseDbContext dbContext)
        {
            this.context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IList<Cheese> cheeses = context.Cheeses.Include(c => c.Category).ToList();


            return View(cheeses);
        }

        public IActionResult Add()
        {
            AddCheeseViewModel addCheeseViewModel =
                new AddCheeseViewModel(context.Categories.ToList());
            return View(addCheeseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddCheeseViewModel addCheeseViewModel)
        {
            if (ModelState.IsValid)
            {
                // Add the new cheese to my existing cheeses
                CheeseCategory newCheeseCategory =
                    context.Categories.Single(c => c.ID == addCheeseViewModel.CategoryID);

                Cheese newCheese = AddCheeseViewModel.CreateCheese(addCheeseViewModel, newCheeseCategory);
                context.Cheeses.Add(newCheese);
                context.SaveChanges();

                return Redirect("/");
            }

            return View(addCheeseViewModel);
        }

        public IActionResult Remove()
        {
            ViewBag.title = "Remove Cheeses";
            ViewBag.cheeses = context.Cheeses.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] cheeseIds)
        {
            foreach (int cheeseId in cheeseIds)
            {
                Cheese theCheese = context.Cheeses.Single(c => c.ID == cheeseId);
                context.Cheeses.Remove(theCheese);
            }
            context.SaveChanges();

            return Redirect("/");
        }

        public IActionResult Edit(int cheeseId)
        {
            Cheese cheeseEdit = context.Cheeses.Single(c => c.ID == cheeseId);

            EditCheeseViewModel editCheeseVM = EditCheeseViewModel.EditCheese(cheeseEdit);
            return View(editCheeseVM);
        }

        [HttpPost]
        public IActionResult Edit(EditCheeseViewModel editCheeseViewModel)
        {
            if (ModelState.IsValid)
            {
                // Update cheese name and descriptions
                Cheese updatedCheese = context.Cheeses.Single(c => c.ID == editCheeseViewModel.ID);
                context.Cheeses.Update(updatedCheese);

                // TODO: Add edit for categories
                updatedCheese.Name = editCheeseViewModel.Name;
                updatedCheese.Description = editCheeseViewModel.Description;
                updatedCheese.Rating = editCheeseViewModel.Rating;

                context.SaveChanges();
                return Redirect("/");
            }
            else
            {
                return View(editCheeseViewModel);
            }
        }
    }
}