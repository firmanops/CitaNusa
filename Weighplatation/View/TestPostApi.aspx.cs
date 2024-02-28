using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Weighplatation.View
{
    public partial class TestPostApi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            var url = "http://49.50.10.55:8020/jsonrpc";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";

                var data = @"{
                              ""jsonrpc"": ""2.0"",
                              ""params"": {
                                            ""service"":""object"",
                                ""method"":""execute"",
                                ""args"":[""plantation_dev"",2,""admin"",""plant.weighbridge"",""create"",[{
                                ""name"" : ""WEIGHBRIDGE 001"",
                                ""wb_type"" : ""receipt""
                                }]]
    
                              }
                                    }";

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

            Console.WriteLine(httpResponse.StatusCode);
        }
    }
}