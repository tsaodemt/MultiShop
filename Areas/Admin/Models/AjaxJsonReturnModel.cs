using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace MultiShop.Areas.Admin.Models
{
    public class AjaxJsonReturnModel
    {
        public string ReturnStatus;
        public dynamic Data { get; set; }
        public List<string> Messages { get; set; }
        public string RedirectLink { get; set; }
    }
}