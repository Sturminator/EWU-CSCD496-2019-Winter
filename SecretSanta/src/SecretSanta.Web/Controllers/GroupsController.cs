﻿using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.ApiModels;

namespace SecretSanta.Web.Controllers
{
    public class GroupsController : Controller
    {
        private IHttpClientFactory ClientFactory { get; }

        public GroupsController(IHttpClientFactory clientFactory)
        {
            ClientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
            {
                var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                ViewBag.Groups = await secretSantaClient.GetGroupsAsync();
            }
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(GroupInputViewModel viewModel)
        {
            IActionResult result = View();

            if (ModelState.IsValid)
            {
                using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    try
                    {
                        var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                        await secretSantaClient.CreateGroupAsync(viewModel);

                        result = RedirectToAction(nameof(Index));
                    }
                    catch (SwaggerException se)
                    {
                        ViewBag.ErrorMessage = se.Message;
                    }
                }
            }
            {
                ViewBag.ErrorMessage = "Invalid Model State. Must have a Group Name.";
            }

            return result;
        }

        [HttpGet]
        public IActionResult Update(int groupId)
        {
            ViewBag.GroupId = groupId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(GroupInputViewModel viewModel, int groupId)
        {
            IActionResult result = View();

            if (ModelState.IsValid)
            {
                using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                {
                    try
                    {
                        var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                        await secretSantaClient.UpdateGroupAsync(groupId, viewModel);

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
                ViewBag.ErrorMessage = "Invalid Model State. Must have a Group Name.";
            }

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int groupId)
        {
            IActionResult result = View();

            if (groupId > 0)
            {
                if (ModelState.IsValid)
                {
                    using (var httpClient = ClientFactory.CreateClient("SecretSantaApi"))
                    {
                        try
                        {
                            var secretSantaClient = new SecretSantaClient(httpClient.BaseAddress.ToString(), httpClient);
                            await secretSantaClient.DeleteGroupAsync(groupId);
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
                    ViewBag.ErrorMessage = "Invalid Model State. Must have a Group Name.";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Group ID must be greater than 0.";
            }

            return result;
        }
    }
}