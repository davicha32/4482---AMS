//Base code provided by Jon Holmes
//https://github.com/ISU-INFO4482-Spring19/PeerVal/blob/0e8ccf6201d8ddb010901441797605f9ca5c96c1/PeerVal/PeerVal/Controllers/BaseController.cs

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AMS.Data;
using AMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AMS.Controllers
{
    /// <summary>
    /// This controller is utilized by just about all of the other controllers to provide some baseline functionality
    /// It tracks sessions, who is logged into the system, and who the current user is. 
    /// </summary>
    public class BaseController : Controller
    {
        public BaseController()
        {

        }

        public string Get(string key)
        {
            string sObject = HttpContext.Session.GetString(key);
            return sObject;
        }
        public T Get<T>(string key)
        {
            //_context = context;
            if (HttpContext.Session.Keys.Contains(key))
            {
                string sObject = HttpContext.Session.GetString(key);
                return JsonConvert.DeserializeObject<T>(sObject);
            }
            else
            {
                return default(T);
            }
        }
        public void Set(string key, object obj)
        {
            String jsonString = JsonConvert.SerializeObject(obj);
            HttpContext.Session.SetString(key, jsonString);
        }

        internal User CurrentUser
        {
            get
            {
                User u = Get<User>("CurrentUser");
                if (u == null) u = new User() { FirstName = "Anonymous" };
                return u;
            }
            set
            {
                Set("CurrentUser", value);
            }
        }

        [DefaultValue(false)]
        internal bool LoggedIn
        {
            get
            {
                bool LoggedIn = Get<bool>("LoggedIn");
                if (LoggedIn == true)
                {
                    return true;
                }
                else return false;
            }
            set
            {
                Set("LoggedIn", value);
            }
        }

        /// <summary>
        /// Used for ajax [GET] to get all Assets from the database
        /// </summary>
        /// <returns>A list of Assets</returns>
        public ActionResult getAllAssets()
        {
            List<Asset> sl = DAL.GetAllAssets();

            return Json(sl.Select(x => new
            {
                assetID = x.ID,
                assetNumber = x.InventoryNumber,
                assetDevice = x.Model.Device.Name,
                assetBrand = x.Model.Device.Brand.Name,
                assetModel = x.Model.Name
            }).ToList());
        }

        /// <summary>
        /// Used for ajax [GET] to get all Assets from the database that match search in inventorynumber, brand, device, or model name
        /// </summary>
        /// <returns>A list of Assets</returns>
        public ActionResult getAssetsLike(string search)
        {
            List<Asset> sl = DAL.GetAssetsLike(search);

            return Json(sl.Select(x => new
            {
                assetID = x.ID,
                assetNumber = x.InventoryNumber,
                assetDevice = x.Model.Device.Name,
                assetBrand = x.Model.Device.Brand.Name,
                assetModel = x.Model.Name
            }).ToList());
        }

        /// <summary>
        /// Used for ajax [GET] to get all brands from the database
        /// </summary>
        /// <returns>A list of brands</returns>
        public ActionResult getAllBrands()
        {
            List<Brand> bl = new List<Brand>(DAL.GetAllBrands());

            return Json(bl.Select(x => new
            {
                brandID = x.ID,
                brandName = x.Name
            }).ToList());
        }

        /// <summary>
        /// Used for ajax [GET] to get all Users from the database
        /// </summary>
        /// <returns>A list of Users</returns>
        public ActionResult getAllUsers()
        {
            List<User> bl = new List<User>(DAL.GetUsers());

            return Json(bl.Select(x => new
            {
                UserID = x.ID,
                UserFirstName = x.FirstName,
                UserLastName = x.LastName
            }).ToList());
        }

        /// <summary>
        /// Used for ajax [GET] to get all Locations from the database
        /// </summary>
        /// <returns>A list of Locations</returns>
        public ActionResult getAllLocations()
        {
            List<Location> bl = new List<Location>(DAL.GetAllLocations());

            return Json(bl.Select(x => new
            {
                locationID = x.ID,
                locationName = x.Name
            }).ToList());
        }

        /// <summary>
        /// Used for ajax [GET] to get all States from the database
        /// </summary>
        /// <returns>A list of States</returns>
        public ActionResult getAllStates()
        {
            List<State> sl = DAL.GetAllStates();

            return Json(sl.Select(x => new
            {
                stateID = x.ID,
                stateName = x.Name
            }).ToList());
        }

        /// <summary>
        /// Used for ajax [GET] to get all Statuses from the database
        /// </summary>
        /// <returns>A list of Statuses</returns>
        public ActionResult getAllStatuses()
        {
            List<Status> sl = DAL.GetAllStatuses();

            return Json(sl.Select(x => new
            {
                statusID = x.ID,
                statusName = x.Condition
            }).ToList());
        }

        /// <summary>
        /// Used for ajax [GET] to get all Categories from the database
        /// </summary>
        /// <returns>A list of Categories</returns>
        public ActionResult getAllCategories()
        {
            List<Category> sl = DAL.GetAllCategories();

            return Json(sl.Select(x => new
            {
                categoryID = x.ID,
                categoryName = x.Name
            }).ToList());
        }

        /// <summary>
        /// Used for ajax [post] to get all models from the database based on brandID
        /// </summary>
        /// <returns>A list of Models</returns>
        [HttpPost]
        public ActionResult getModelsByDeviceID(int id)
        {
            List<Model> ml = DAL.GetModelsByDeviceID(id);

            return Json(ml.Select(x => new
            {
                modelID = x.ID,
                modelName = x.Name
            }).ToList());
        }

        /// <summary>
        /// Used for ajax [post] to get all models from the database based on brandID
        /// </summary>
        /// <returns>A list of Models</returns>
        [HttpPost]
        public ActionResult getDevicesByBrandID(int id)
        {
            List<Device> ml = DAL.GetDevicesByBrandID(id);

            return Json(ml.Select(x => new
            {
                deviceID = x.ID,
                deviceName = x.Name
            }).ToList());
        }
        /// <summary>
        /// These methods below are all post methods that add new objects into the database.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult AddBrand(string Name)
        {
            Brand b = new Brand();
            b.Name = Name;

            DAL.AddBrand(b);

            List<Brand> bl = new List<Brand>();
            bl.Add(b);

            return Json(bl.Select(x => new
            {
                brandName = x.Name
            }).ToList());
        }

        [HttpPost]
        public ActionResult AddDevice(string Name, int brandID)
        {
            Device d = new Device();
            d.Name = Name;
            d.BrandID = brandID;

            DAL.AddDevice(d);

            List<Device> dl = new List<Device>();
            dl.Add(d);

            return Json(dl.Select(x => new
            {
                deviceName = x.Name,
                deviceBrandID = x.ID
            }).ToList());
        }

        [HttpPost]
        public ActionResult AddModel(string Name, int id)
        {
            Model m = new Model();
            m.Name = Name;
            m.DeviceID = id;

            DAL.AddModel(m);

            List<Model> ml = new List<Model>();
            ml.Add(m);

            return Json(ml.Select(x => new
            {
                ModelName = x.Name,
                ModelDeviceID = x.ID
            }).ToList());
        }

        [HttpPost]
        public ActionResult AddState(string Name)
        {
            State s = new State();
            s.Name = Name;

            DAL.AddState(s);

            List<State> sl = new List<State>();
            sl.Add(s);

            return Json(sl.Select(x => new
            {
                stateName = x.Name
            }).ToList());
        }

        [HttpPost]
        public ActionResult AddLocation(Location LocationModel)
        {
            DAL.AddLocation(LocationModel);
            return null;
        }

        [HttpPost]
        public ActionResult GetBrandByName(string BrandNameModel)
        {
            int BrandID = DAL.GetBrandByName(BrandNameModel);
            return Json(BrandID);
        }

        [HttpPost]
        public ActionResult GetDeviceByName(string DeviceNameModel)
        {
            int DeviceID = DAL.GetDeviceByName(DeviceNameModel);
            return Json(DeviceID);
        }

        [HttpPost]
        public ActionResult ReturnAsset(int id)
        {
            int assetID = DAL.ReturnAsset(id);
            return Json(assetID);
        }

        public ActionResult getCurrentUser()
        {
            User u = CurrentUser;
            return Json(u);
        }
    }
}