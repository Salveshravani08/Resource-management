using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Mycollectionproject.Models;


namespace Mycollectionproject.Controllers
{
    public class HomeController1 : Controller
    {

        private readonly HttpClient _client;
        public HomeController1(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("MyAPIClient");
        }
        

 
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var collection = await _client
                 .GetFromJsonAsync<List<Collectionform>>("api/ValuesControllerapi/Items");
            return View(collection);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _client.GetFromJsonAsync<Collection>($"//api/ValuesControllerapi/Items/{id}");
            if (item == null) return NotFound();
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Collection collection)
        {
            var response = await _client.PostAsJsonAsync("api/ValuesControllerapi/Items", collection);
          
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                
                return Ok(new { message = content.Trim('"') }); 
            }
            else
            {
                return BadRequest(new { message = content });
            }
            
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, Collection collection)
        {
            var response = await _client.PutAsJsonAsync($"api/ValuesControllerapi/Items/{id}", collection);
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, error);
            return View(collection);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _client.DeleteAsync($"api/ValuesControllerapi/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Item deleted successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete item." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Search(string title, string description)
        {
            var items = await _client.GetFromJsonAsync<List<Collection>>(
                $"api/Items/search?title={Uri.EscapeDataString(title ?? "")}&description={Uri.EscapeDataString(description ?? "")}"
            );
            return View("SearchResults", items);
        }
        [HttpGet]
        public async Task<IActionResult> SearchByTag( string tag_name)
        {
            var items = await _client.GetFromJsonAsync<List<Collection>>(
                $"api/Items/search/tag_name?tag_name={Uri.EscapeDataString(tag_name ?? "")}"
            );
            return View("SearchResults", items);
        }


    }
}


    

