using Golf.Tournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Controllers
{
    [ErrorHandler]
    public class BaseController : Controller
    {
        protected GolfLoader loader;

        public BaseController(string apiUrl)
        {
            loader = new GolfLoader(apiUrl);
        }

        public BaseController()
            : this("http://localhost:8080/api/")
        {
        }
    }
}