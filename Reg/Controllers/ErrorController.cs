using Microsoft.AspNetCore.Mvc;
using Store.Helpers;


namespace Store.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error(Helpers.HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case Helpers.HttpStatusCode.NotFound:
                    ViewBag.ErrorMessage = ErrorMessages.ResourceNotFound;
                    break;
                case Helpers.HttpStatusCode.InternalServerError:
                    ViewBag.ErrorMessage = ErrorMessages.InternalServerError;
                    break;
                case Helpers.HttpStatusCode.BadRequest:
                    ViewBag.ErrorMessage = ErrorMessages.BadRequest;
                    break;
                case Helpers.HttpStatusCode.Unauthorized:
                    ViewBag.ErrorMessage = ErrorMessages.UnauthorizedAccess;
                    break;
                default:
                    ViewBag.ErrorMessage = "An error occurred.";
                    break;
            }

            return View();
        }
    }
}
