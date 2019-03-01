using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.ApiModels;
using AutoMapper;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SecretSanta.Web.Controllers
{
    public class UsersController : Controller
    {
        private IHttpClientFactory ClientFactory { get; }
        private IMapper Mapper { get; }

        public UsersController(IHttpClientFactory clientFactory, IMapper mapper)
        {
            ClientFactory = clientFactory;
            Mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                ViewBag.Users = await secretSantaClient.GetAllUsersAsync();
            }
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserViewModel viewModel)
        {
            IActionResult result = View();

            if (ModelState.IsValid)
            {
                using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    try
                    {
                        var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                        await secretSantaClient.CreateUserAsync(Mapper.Map<UserInputViewModel>(viewModel));

                        result = RedirectToAction(nameof(Index));
                    }
                    catch(SwaggerException se)
                    {
                        ViewBag.ErrorMessage = se.Message;
                    }
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid Model State. Must have a First Name.";
            }

            return result;
        }

        [HttpGet]
        public IActionResult Update(int userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserViewModel viewModel)
        {
            IActionResult result = View();

            if(ModelState.IsValid)
            {
                using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    try
                    {
                        var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                        await secretSantaClient.UpdateUserAsync(viewModel.Id, Mapper.Map<UserInputViewModel>(viewModel));

                        result = RedirectToAction(nameof(Index));
                    }
                    catch (SwaggerException se)
                    {
                        ViewBag.ErrorMessage = se.Message;
                    }
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid Model State. Must have a First Name.";
            }

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int userId)
        {
            IActionResult result = View();

            if(userId > 0)
            {
                if (ModelState.IsValid)
                {
                    using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                    {
                        try
                        {
                            var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                            await secretSantaClient.DeleteUserAsync(userId);
                            result = RedirectToAction(nameof(Index));
                        }
                        catch (SwaggerException se)
                        {
                            ViewBag.ErrorMessage = se.Message;
                        }
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid Model State. Must have a First Name.";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "User ID must be greater than 0.";
            }

            return result;
        }
    }
}
