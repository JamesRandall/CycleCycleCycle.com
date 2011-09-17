using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using CycleCycleCycle.Models;
using CycleCycleCycle.Models.Repositories;
using CycleCycleCycle.Services;
using CycleCycleCycle.ViewModels;

namespace CycleCycleCycle.Controllers
{   
    public class RouteController : BaseController
    {
        private readonly IRouteService _routeService;
        private readonly IFavouriteService _favouriteService;

        public RouteController(IAccountService accountService, IRouteService routeService, IFavouriteService favouriteService)
            : base(accountService)
        {
            _routeService = routeService;
            _favouriteService = favouriteService;
        }

        //
        // GET: /Route/

        public ViewResult Index()
        {
            TopRoutes topRoutes = _routeService.TopRoutes(Account);
            return View(topRoutes);
        }

        //
        // GET: /Route/Details/5

        public ViewResult Details(int id)
        {
            bool isFavourited;
            bool isOwner;

            Route route = _routeService.Details(
                id,
                Request.IsAuthenticated ? (int?)Account.AccountID : null,
                out isFavourited,
                out isOwner);

            ViewBag.IsFavourited = isFavourited;
            ViewBag.IsOwner = isOwner;
            
            return View(route);
        }

        //
        // GET: /Route/PointDetails/5

        public JsonResult PointDetails(string sidx, string sord, int page, int rows, int entityId)
        {
            int totalPages;
            int totalRecords;
            var routePoints = _routeService.RoutePoints(entityId, sidx, sord, page, rows, out totalPages,
                                                        out totalRecords);

            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = (
                           from rp in routePoints
                           select new
                           {
                               id = rp.RoutePointID,
                               cell = new string[]
                                                                        {
                                                                            rp.TimeRecorded.ToString(),
                                                                            rp.Elevation.ToString(),
                                                                            rp.Latitude.ToString(),
                                                                            rp.Longitude.ToString()
                                                                        }
                           }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        
        //
        // GET: /Route/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            bool isFavourite;
            bool isOwner;
            Route route = _routeService.Details(id, Account.AccountID, out isFavourite, out isOwner);
            if (!isOwner)
            {
                return RedirectToAction("Unauthorised", "Home");
            }
            return View(route);
        }

        //
        // POST: /Route/Edit/5

        [HttpPost]
        [Authorize]
        public ActionResult Edit(Route route)
        {
            if (ModelState.IsValid)
            {
                Route updatedRoute;
                try
                {
                    updatedRoute= _routeService.Update(route, Account.AccountID);
                }
                catch (SecurityException)
                {
                    return RedirectToAction("Unauthorised", "Home");
                }
                
                return RedirectToAction("Details", new { id = updatedRoute.RouteID});
            }
            return View();
        }

        //
        // GET: /Route/Upload
        [Authorize]
        public ActionResult Upload()
        {
            return View();
        }

        //
        // POST: /Route/Upload
        [HttpPost]
        [Authorize]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            try
            {
                XDocument xmlDocument = XDocument.Load(file.InputStream);
                Route route = _routeService.CreateFromXDocument(xmlDocument, Account.AccountID);
                return RedirectToAction("Details", new {id = route.RouteID});
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Uploaded file is not a valid GPX file");
                return View();
            }
        }

        //
        // GET: /Route/HeightMapImage
        public FileResult HeightMapImage(int id, int width, int height)
        {
            Bitmap bitmap = _routeService.HeightMapImage(id, width, height);
            MemoryStream memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);

            return File(memoryStream.ToArray(), "image/png");
        }

        public ActionResult Find()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Find(RouteSearch search)
        {
            try
            {
                string name;
                search.Results = _routeService.FindRoute(search.SearchText, 10, out name);
                search.SearchedLocation = name;
            }
            catch(Exception)
            {
                ModelState.AddModelError("", "An error has occurred, unable to retrieve search results.");
            }
            
            return View(search);
        }

        [HttpPost]
        [Authorize]
        public JsonResult Favourite(int id)
        {
            _favouriteService.Favourite(Account.AccountID, id);
            return Json(true);
        }

        [HttpPost]
        [Authorize]
        public JsonResult Unfavourite(int id)
        {
            _favouriteService.Unfavourite(Account.AccountID, id);
            return Json(true);
        }

        public FileResult Download(int id)
        {
            string filename;
            FileResult result = new FileStreamResult(_routeService.Download(1, out filename), "application/xml");
            result.FileDownloadName = filename;
            return result;
        }
    }
}

