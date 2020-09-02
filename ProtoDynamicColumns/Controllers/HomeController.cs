using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProtoDynamicColumns.Models;

namespace ProtoDynamicColumns.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _HostingEnv;

        public HomeController(IHostingEnvironment hostingEnviornment)
        {
            _HostingEnv = hostingEnviornment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("GetJsonData")]
        [HttpGet]
        public IActionResult GetJsonData()
        {
            var json="";
            var Status = false;
            try
            {
                string rootPath = _HostingEnv.WebRootPath;
                json =System.IO.File.ReadAllText(rootPath + "/TableData.json");
                Status = true;
            }
            catch(Exception ex)
            {
                Status = false;
                string msg = ex.Message.ToString();
            }
            return Json(new { data = json, status = Status });
        }

        [HttpPost]
        public IActionResult UpdateTableJson(List<TableJsonVM> jsonObj)
        {
            var _status = false;
            var _msg = "";
            try
            {
                
                var _roothpath = _HostingEnv.WebRootPath;
                var _json =System.IO.File.ReadAllText(_roothpath + "/TableData.json");
                dynamic dataObj = JsonConvert.DeserializeObject(_json);
                for(int i=0;i<jsonObj.Count;i++)
                {
                    dataObj["columns"][i]["visible"] = jsonObj[i].visible;
                }
                string updatedJson = JsonConvert.SerializeObject(dataObj, Formatting.Indented);
                System.IO.File.WriteAllText(_roothpath + "/TableData.json", updatedJson);
                _status = true;
                _msg = "Settings updated successfully";
            }
            catch(Exception ex)
            {
                _status = false;
                _msg = ex.Message.ToString();
            }
            return Json(new { status = _status,message= _msg });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
