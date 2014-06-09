using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Learnosity;

namespace LearnosityWebApp
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["service"] != null) {
                // Request defaults - will be overridden from a sample `services` file
                string service  = null;
                string[] security = null;
                string secret   = null;
                Dictionary<> request  = null;
                string action   = null;

                bool foundFile = true;
                if (foundFile) {

                    //require_once __DIR__ . '/services/' . $_GET['service'] . '.php';
                    include 

                    if ($_GET['service'] !== 'schemas') {
                        // Instantiate the Init class to generate initialisation data
                        Init = new Init(service, security, secret, request, action);
                        requestPacket = Init->generate();
                    }
                }
            }
        }
    }
}