using RabbitMQ.Models;
using RabbitMQ.Repository;
using System.Web.Mvc;


namespace RabbitMQ.Controllers
{
    public class DefaultController : Controller
    {
        IRepository queueProcessor;
        protected IRepository CsharpRepository;

        public DefaultController(IRepository csharpRepository)
        {
            CsharpRepository = csharpRepository;
        }

        public ActionResult Index()
        {
            System.Web.HttpContext.Current.Session["MessagesQuantity"] = 0;
            var model = new MainViewModel
            {
                Clients = CsharpRepository.GetClients()
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult SendMessages(string client1, string client2, string filter, bool isTime)
        {
            if (ModelState.IsValid)
                queueProcessor = new CsharpRepository();
            return Json(queueProcessor.Send(filter, isTime), JsonRequestBehavior.AllowGet);
        }

    }
}