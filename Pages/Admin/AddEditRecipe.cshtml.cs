using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopsyTurvyCakes.Models;
using Microsoft.AspNetCore.Http;


namespace TopsyTurvyCakes.Pages.Admin {
    [Authorize]
    public class AddEditRecipeModel : PageModel {
        [FromRoute]
        public long? Id { get; set; }

        public bool IsNewRecipe{
            get { return Id == null; }
        }

        [BindProperty]
        public IFormFile tempPic { get; set; }

        private IRecipesService recipesService;

        [BindProperty]
        public Recipe Recipe { get; set; } // expose the Recipe class we'll be using later on
        public async Task OnGetAsync(){
            //var recipesService = new RecipesService();
            Recipe = recipesService.Find(Id.GetValueOrDefault()) ?? new Recipe();

        }

        [BindProperty]
        public IFormFile Image { get; set; }

        public async Task<IActionResult> OnPostAsync() {
            if(!ModelState.IsValid){
                return Page();
            }

            var modifiedRecipe = recipesService.Find(Id.GetValueOrDefault()) ?? new Recipe();

            modifiedRecipe.Name = Recipe.Name;
            modifiedRecipe.Description = Recipe.Description;
            modifiedRecipe.Directions = Recipe.Directions;
            modifiedRecipe.Ingredients = Recipe.Ingredients;

            modifiedRecipe.SetImage(Image); 


            await recipesService.SaveAsync(modifiedRecipe);
            return RedirectToPage("/Recipe", new { id = modifiedRecipe.Id });
        }

        public async Task<IActionResult> OnPostDelete() {
            await recipesService.DeleteAsync(Id.Value);
            return RedirectToPage("/Index");
        }

        public AddEditRecipeModel(IRecipesService recipesService){
            this.recipesService = recipesService;
        }
    }
}
