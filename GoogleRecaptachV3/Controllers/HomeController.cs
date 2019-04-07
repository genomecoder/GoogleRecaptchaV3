//THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//Atuhor: Todd Hang <tqhang@gmail.com>
//For questions of how the code is implemented, please email me at tqhang@gmail.com

using System;
using System.Data;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Timers;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Configuration;
using Newtonsoft.Json;

namespace GoogleRecaptachV3.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult Index(string tx_card)
        {
            string token = Request["captcha"];
            CaptchaResponseV3 response = ValidateCaptchaV3(token);

            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            string ac = controllerName + "/" + actionName;

            //when response is false check for the error message
            if (response.Success && response.Score >= 0.7 && response.Action == ac && tx_card.Trim() != "")
            {
                //recaptcha is good.  add code to process with the 
                var Message = "<div style=\"margin: -7px 0px; color:#973B28; \">Recaptcha Verified Successfully</div>";
                ViewBag.Response = Message;
                return View();
            }
            else
            {
                //recaptcha response is suspicious
                var Message = "<div style=\"margin: -7px 0px; color:#973B28; \">Recaptcha NOT Verified</div>";
                ViewBag.Response = Message;

                return View();
            }
        }

        public static CaptchaResponseV3 ValidateCaptchaV3(string token)
        {
            string secret = "Enter Your Secret Key Here";
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, token));
            return JsonConvert.DeserializeObject<CaptchaResponseV3>(jsonResult.ToString());
        }
    }

    public class CaptchaResponseV3
    {
        [Newtonsoft.Json.JsonProperty("success")]
        public Boolean Success { get; set; }

        [Newtonsoft.Json.JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }

        [Newtonsoft.Json.JsonProperty("challenge_ts")]
        public DateTime Challene_ts { get; set; }

        [Newtonsoft.Json.JsonProperty("hostname")]
        public string HostName { get; set; }

        [Newtonsoft.Json.JsonProperty("score")]
        public Double Score { get; set; }

        [Newtonsoft.Json.JsonProperty("action")]
        public String Action { get; set; }
    }
}