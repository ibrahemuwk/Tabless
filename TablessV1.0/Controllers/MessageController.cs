using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TablessV1._0.Models;

namespace TablessV1._0.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        public ActionResult Index(MessageViewModel _messageViewModel)
        {
            return View(_messageViewModel);
        }
    }
}