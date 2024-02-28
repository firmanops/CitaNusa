using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Weighplatation.Model;
using Weighplatation.Repository;
namespace Weighplatation.View
{
    public partial class WBBussinesPartner : System.Web.UI.Page
    {
        private static Random random = new Random();
        public ApiOddoRepo apiOddoRepo = new ApiOddoRepo();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void WriteLogFile(string message)
        {
            StreamWriter sw = null;
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogGetDataOddo.txt", true);
            sw.WriteLine($"{DateTime.Now.ToString()} : {message}");
            sw.Flush();
            sw.Close();
        }
        void GetBusinessToOddo()
        {
            try
            {
                BusinessRepo businessrepo = new BusinessRepo();

                string url = ConfigurationManager.AppSettings["endpointoddo"].ToString();
                string dboddo = ConfigurationManager.AppSettings["dboddo"].ToString();
                string methododdo = ConfigurationManager.AppSettings["methododdo"].ToString();
                string pass = ConfigurationManager.AppSettings["passoddo"].ToString();
                bool checkinet = apiOddoRepo.CheckForInternetConnection(1000, url.Replace("/jsonrpc", ""));
                if (checkinet)
                {
                    parentsub _parentsub = new parentsub();
                    _parentsub.service = "object";
                    _parentsub.method = methododdo;

                    string[] args = new string[] { dboddo, "2", pass, "res.partner", "search_read", @"[[[""id"", ""not in"", []]]],{
                                                    ""fields"" : [""id"", ""name"", ""company_type"", ""street"", ""street2"", ""city"", 
                                                    ""zip"", ""vat"",""phone"",""email"",""customer_rank"",""supplier_rank""]}" };
                    _parentsub.args = args;
                    parent _parent = new parent { jsonrpc = "2.0", param = _parentsub };

                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string output = jss.Serialize(_parent);
                    List<string> ls = new List<string>();
                    ls.Add(output);

                    string json = JsonConvert.SerializeObject(ls);
                    string jsonend = (json.Replace("\\", "").ToString().Replace("param", "params").Replace(@"]}""]}}", "]}]}}").Replace(@"""[[[", "[[[").Replace(@"""{""jsonrpc""", @"{""jsonrpc""").Replace(@"]}]}}""", "]}]}}"));
                    jsonend = jsonend.Substring(1, jsonend.Length - 1);
                    jsonend = jsonend.Remove(jsonend.Length - 1);
                    jsonend = jsonend.Replace(@"rn", "").ToString().Trim();

                    WriteLogFile($"Body Jason To Oddo = {jsonend}");
                    WriteLogFile($"URL Oddo = {url}");

                    StringContent sc = new StringContent(jsonend, Encoding.UTF8, "application/json");


                    using (var client = new HttpClient())
                    {
                        WriteLogFile($"Send Request Data Business.");
                        HttpClient c = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
                        var x = c.PostAsync(url, sc).Result;
                        WriteLogFile($"Cek Status : {x.StatusCode}");
                        if (x.IsSuccessStatusCode)
                        {
                            WriteLogFile($"Jika Sukses");
                            var result = x.Content.ReadAsStringAsync();
                            string bodyresult = result.Result;
                            WriteLogFile($"Hasil Response : {bodyresult}");
                            dynamic data = JObject.Parse(bodyresult);
                            Console.WriteLine(data.result);
                            WriteLogFile("Convert Json To String ");
                            var jsonresult = JsonConvert.SerializeObject(data.result);

                            WriteLogFile($"Result Convert : {jsonresult}");

                            List<ResponseBPModel> listResult = new List<ResponseBPModel>();

                            JArray jsonResponse = JArray.Parse(jsonresult);

                            WriteLogFile($"jsonResponse Convert");

                            foreach (var item in jsonResponse)
                            {
                                
                                WriteLogFile($"item : {item.ToString()}");
                                JObject jRaces = (JObject)item;
                                ResponseBPModel rowsResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseBPModel>(jRaces.ToString());
                                string jsonrowsResult = JsonConvert.SerializeObject(rowsResult);
                                WriteLogFile($"jsonrowsResult : {jsonrowsResult}");

                                listResult.Add(rowsResult);
                            }

                            string jsonlistResult = JsonConvert.SerializeObject(listResult);

                            WriteLogFile($"Result jsonlistResult : {jsonlistResult}");


                            foreach (var item in listResult)
                            {
                                WriteLogFile($"Convert To Table Model");
                                BusinessModel businessModel = new BusinessModel();
                                WriteLogFile($"1 : {item.id}");                               
                                businessModel.oddoid = int.Parse(item.id.ToString());
                                WriteLogFile($"2");
                                businessModel.BPName = item.name.ToString();
                                WriteLogFile($"3");
                                //businessModel.BPType = item.company_type == "person" ? "1" : "1";
                                WriteLogFile($"4");
                                businessModel.Address1 =item.street;
                                WriteLogFile($"5");
                                businessModel.Address2 =item.street2;                              
                                WriteLogFile($"6");
                                businessModel.City = item.city;
                                WriteLogFile($"7");
                                businessModel.Province = "";
                                WriteLogFile($"8");
                                businessModel.Postalcode = item.zip;
                                WriteLogFile($"9");
                                businessModel.TaxID = "";
                                WriteLogFile($"10");
                                businessModel.Phone = "";
                                WriteLogFile($"11");
                                businessModel.Email = item.email;
                                WriteLogFile($"12");
                                businessModel.PICName = "";
                                WriteLogFile($"13");
                                businessModel.Active = true;
                                WriteLogFile($"14");
                                businessModel.bplogo =0;
                                WriteLogFile($"15");
                                if (item.company_type == "company" && item.customer_rank > 0)
                                {
                                    businessModel.BPType = "2";
                                }
                                else if(item.company_type == "company" && item.supplier_rank > 0)
                                {
                                    businessModel.BPType = "1";
                                }
                                Random generator = new Random();
                                int r = generator.Next(1000, 9999);
                                string vBPCode = "";
                                vBPCode = "BP" + r.ToString();
                                businessModel.BPCode = RandomString(5);

                                WriteLogFile($"Save Data Business");
                                int checkContract = businessrepo.CheckDuplicateBusiness(item.id.ToString());
                                if (checkContract == 0) { 

                                    if (item.company_type != "person")
                                    {
                                        bool resultinsert = businessrepo.InsertBusiness(businessModel);
                                        if (resultinsert)
                                        {
                                            MessageSuccess(this, "success", "Success!");
                                            WriteLogFile($"The Business data has been successfully withdrawn");                                           
                                        }
                                        else
                                        {
                                            MessageError(this, "Business Data failed to withdraw", "");
                                            WriteLogFile("Business Data failed to withdraw");
                                        }

                                    }
                                }                            
                            }
                        }
                        else
                        {
                            WriteLogFile("Failed Api");
                            var result = x.Content.ReadAsStringAsync();
                            string bodyresult = result.Result;
                            dynamic data = JObject.Parse(bodyresult);
                            Console.WriteLine(data.error.data.message);
                            string msg = data.error.data.message;
                            WriteLogFile($"Web Api called : Api Data {msg}");
                        }
                    }
                }


            }
            catch (Exception err)
            {
                WriteLogFile($"Web Api called : Api Data { err.Message}");
            }
        }

        protected void MessageSuccess(Control Control, string Message, string Title = "Alert", string callback = "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "swal.fire('" + Title + "','" + Message + "','success');", true);
        }

        protected void MessageError(Control Control, string Message, string Title = "Alert", string callback = "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "swal.fire('" + Title + "','" + Message + "','error');", true);
        }

        protected void MessageWarning(Control Control, string Message, string Title = "Alert", string callback = "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "warningalert();", true);
        }

        protected void btnBusiness_Click(object sender, EventArgs e)
        {
            GetBusinessToOddo();
            Response.Redirect("/View/WBBussinesPartner.aspx");
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

 

   
}