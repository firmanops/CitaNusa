using System;
using System.Collections.Generic;
using System.Messaging;
using System.Web.UI;
using Weighplatation.Model;
using Weighplatation.Repository;
using DevExpress.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Text;
using System.Configuration;
using System.IO;
using System.Net.Http;

namespace Weighplatation.View
{
    public partial class Contract : System.Web.UI.Page
    {
        public ContractRepo contractRepo = new ContractRepo();
      
        public ApiOddoRepo apiOddoRepo = new ApiOddoRepo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAllContract();
            }
            else
            {
                ContractModel lt = new ContractModel();
                lt = Session["Data"] as ContractModel;
                Grid.DataSource = lt;

            }           
        }

        void GetAllContract()
        {
            List<ContractModel> contractModels = new List<ContractModel>();
            contractModels = contractRepo.GetAllContract();
            Grid.DataSource = contractModels;
            Session["Data"] = contractModels;
            Grid.DataBind();
            //NewContract.ShowOnPageLoad = false;

        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("/View/ContractNew.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<ContractModel> contractModel = new List<ContractModel>();
            contractModel = contractRepo.GetByFilterContractNo(txtSearch.Text);
            Grid.DataSource = contractModel;
            Session["Data"] = contractModel;
            Grid.DataBind();
        }

        protected void btCancel_Click(object sender, EventArgs e)
        {
            Console.WriteLine("");
        }
        void lockobject() {
            //txtTotalPrice.Enabled = false;
            //txtTotalNetWB.Enabled = false;
            //txtDeliveryStatus.Enabled = false;
            //txtFinalPrice.Enabled = false;
        }
        void unlockobject()
        {
            //txtTotalPrice.Enabled = true;
            //txtTotalNetWB.Enabled = true;
            //txtDeliveryStatus.Enabled = true;
            //txtFinalPrice.Enabled = true;
        }
    
        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
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

      
        protected void hyperLink_Init(object sender, EventArgs e)
        {
            //ASPxHyperLink hyperLink = sender as ASPxHyperLink;
            //string keyValue = grid.GetRowValues(e.VisibleIndex, "EmployeeID").ToString();
            //grid.JSProperties["cpKeyValue"] = keyValue;

            //var index = (hyperLink.NamingContainer as GridViewDataItemTemplateContainer).VisibleIndex;
            //hyperLink.JSProperties["cpContainerIndex"] = index;
        }

        protected void Grid_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            //ASPxGridView grids = (ASPxGridView)sender;
            //string keyValue = grids.GetRowValues(e.VisibleIndex, "ContractNo").ToString();
            //Grid.JSProperties["cpKeyValue"] = keyValue;
           
            //NewContract.ShowOnPageLoad = true;




        }

        private void GetContractByNo() {
            try
            {
                ContractModel contract = new ContractModel();
                contract = contractRepo.GetByContractNo("");
            }
            catch (Exception)
            {

                throw;
            }
           

        }

        protected void btnContract_Click(object sender, EventArgs e)
        {
            GetContractToOddo();
            Response.Redirect("/View/Contract.aspx");
            //GetAllContract();
            //NewContract.ShowOnPageLoad = false;
        }

        public void WriteLogFile(string message)
        {
            StreamWriter sw = null;
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogGetDataOddo.txt", true);
            sw.WriteLine($"{DateTime.Now.ToString()} : {message}");
            sw.Flush();
            sw.Close();
        }

        void GetContractToOddo()
        {
            try
            {
                //ContractRepo contractRepo = new ContractRepo();

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

                    string[] args = new string[] { dboddo, "2", pass, "plant.contract", "search_read", @"[[[""state"", ""="", ""in_progress""], [""id"", ""not in"", [4,5]]]],{
                                                    ""fields"" : [""name"", ""contract_date"", ""start_contract"", ""expired_date"", ""qty"", ""product_id"", 
                                                    ""qty"", ""unit_price"", ""contract_type"",""partner_id""]}" };
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
                        WriteLogFile($"Send Request Data Contract.");
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

                            List<ResponseModel> listResult = new List<ResponseModel>();

                            JArray jsonResponse = JArray.Parse(jsonresult);

                            WriteLogFile($"jsonResponse Convert");

                            foreach (var item in jsonResponse)
                            {
                                WriteLogFile($"item : {item.ToString()}");
                                JObject jRaces = (JObject)item;
                                ResponseModel rowsResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseModel>(jRaces.ToString());
                                string jsonrowsResult = JsonConvert.SerializeObject(rowsResult);
                                WriteLogFile($"jsonrowsResult : {jsonrowsResult}");

                                listResult.Add(rowsResult);
                            }

                            string jsonlistResult = JsonConvert.SerializeObject(listResult);

                            WriteLogFile($"Result jsonlistResult : {jsonlistResult}");


                            foreach (var item in listResult)
                            {
                                //if (item.name.Trim() == "SPKJ/2022/10/004")
                                //{
                                //    Console.Write("XXX");
                                //}

                                WriteLogFile($"Convert To Table Model");
                                ContractModel contractRow = new ContractModel();
                                WriteLogFile($"1 : {item.id}");
                                WriteLogFile($"2 : {item.name}");
                                contractRow.oddoid = int.Parse(item.id.ToString());
                                WriteLogFile($"2");
                                contractRow.ContractNo = item.name.ToString();
                                WriteLogFile($"3");
                                contractRow.ContractDate = Convert.ToDateTime(item.contract_date);
                                WriteLogFile($"4");
                                contractRow.ExpDate = Convert.ToDateTime(item.expired_date.ToString());
                                WriteLogFile($"5");
                                contractRow.Qty = double.Parse(item.qty.ToString());                           
                                var jsonprod = JsonConvert.SerializeObject(item.product_id);
                                JArray jsonprodResponse = JArray.Parse(jsonprod);
                                string codeProd = jsonprodResponse[1].ToString();
                                WriteLogFile($"6");
                                contractRow.ProductCode = codeProd.Substring(1, 3);
                                WriteLogFile($"7");
                                contractRow.UnitPrice = double.Parse(item.unit_price.ToString());
                                WriteLogFile($"8");
                                var jsonbp = JsonConvert.SerializeObject(item.partner_id);
                                JArray jsonbpResponse = JArray.Parse(jsonbp);
                                string codebp = jsonbpResponse[0].ToString();
                                contractRow.BPCode = apiOddoRepo.GetUnitByCode(int.Parse(codebp));
                                WriteLogFile($"9");
                                contractRow.PPN = 11;
                                WriteLogFile($"10");
                                contractRow.Toleransi = 0;
                                WriteLogFile($"11");
                                contractRow.PremiumPrice = 0;
                                WriteLogFile($"12");
                                contractRow.TotalPrice = contractRow.Qty * contractRow.UnitPrice;
                                WriteLogFile($"13");
                                contractRow.FinalUnitPrice = (contractRow.TotalPrice * contractRow.PPN / 100) + contractRow.TotalPrice;
                                WriteLogFile($"14");
                                contractRow.DespatchQty = 0;
                                WriteLogFile($"15");
                                contractRow.DeliveryStatus = "F";
                                

                                WriteLogFile($"Save Data Contract");
                                int checkContract = contractRepo.CheckDuplicateContract(item.name.Trim());
                                //if (item.name.Trim() == "SPKJ/2022/10/004")
                                //{
                                //    Console.Write("XXX");
                                //}
                                if (checkContract == 0) {
                                    bool resultinsert = contractRepo.InsertContract(contractRow);
                                    if (resultinsert)
                                    {
                                        MessageSuccess(this, "The contract data has been successfully withdrawn", "Success", "");
                                        WriteLogFile($"The contract data has been successfully withdrawn");

                                    }
                                    else
                                    {
                                        MessageError(this, "Contract Data failed to withdraw", "");
                                        WriteLogFile("Contract Data failed to withdraw");
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

        protected void Grid_DataBinding(object sender, EventArgs e)
        {
            List<ContractModel> lt = new List<ContractModel>();
            lt = Session["Data"] as List<ContractModel>;
            Grid.DataSource = lt;
        }
    }
}