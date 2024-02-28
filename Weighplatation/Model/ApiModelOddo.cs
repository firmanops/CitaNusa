using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weighplatation.Model
{
    public class ApiModelOddo
    {

    }
    public class parent
    {
        public string jsonrpc { get; set; }
        public parentsub param { get; set; }
    }

    public class parentsub
    {
        public string service { get; set; }
        public string method { get; set; }
        public string[] args { get; set; }
    }
    public class argsmodel
    {
        public string cgg_dev { get; set; }
        public int id { get; set; }
        public string admin { get; set; }
        public string plantweighbridge { get; set; }
        public string create { get; set; }
        public receiptmodel lsreceiptmodel { get; set; }
        public context contexts { get; set; }
    }
    public class receiptmodel
    {
        public string partner_id { get; set; }
        public string partner_branch_id { get; set; }
        public DateTime second_date { get; set; }
        public string name { get; set; }       
        public string contract_id { get; set; }
        public string product_id { get; set; }
        public string first_weight { get; set; }
        public string first_net_weight { get; set; }
        public string second_weight { get; set; }
        public string second_net_weight { get; set; }
        public string weight_deduction { get; set; }
        public string wb_type { get; set; }
        public string trx_type { get; set; }
        public string state { get; set; }
        public List<receiptdetail> plant_weighbridge_line { get; set; }
    }


    public class receiptHeader {
        public string partner_id { get; set; }
        public string partner_branch_id { get; set; }
        public string first_date { get; set; }
        public string second_date { get; set; }
        public string name { get; set; }
        public string contract_id { get; set; }
        public string product_id { get; set; }
        public string first_weight { get; set; }
        public string first_net_weight { get; set; }
        public string second_weight { get; set; }
        public string second_net_weight { get; set; }
        public string weight_deduction { get; set; }
        public string wb_type { get; set; }
        public string trx_type { get; set; }
        public string state { get; set; }
        public string vehicle_id { get; set; }
        public string transporter_id { get; set; }
        public string[] plant_weighbridge_line { get; set; }
        public string[] weighbridge_grade_line { get; set; }        
    }

    public class despactHeader
    {
        public string partner_id { get; set; }
        public string partner_branch_id { get; set; }
        public string first_date { get; set; }
        public string second_date { get; set; }
        public string name { get; set; }
        public string contract_id { get; set; }
        public string product_id { get; set; }
        public string first_weight { get; set; }
        public string first_net_weight { get; set; }
        public string second_weight { get; set; }
        public string second_net_weight { get; set; }
        public string weight_deduction { get; set; }
        public string wb_type { get; set; }
        public string trx_type { get; set; }
        public string state { get; set; }
        public string vehicle { get; set; }
        public string transporter { get; set; }
        public string driver { get; set; }
        public string reference { get; set; }

        public string[] weighbridge_quality_line { get; set; }       
    }
    public class receiptdetail
    {
        public List<string> blocks { get; set; }
        public List<string> grading { get; set; }
    }

    public class ApiBlock
    {
        public string block_id { get; set; }
        public string year_of_planting { get; set; }
        public string divisi { get; set; }
        public string bunches { get; set; }
        public string lose_fruit { get; set; }    
    }

    

    public class ApiGrading
    {
        public string grade_id { get; set; }  
        public string Quantity { get; set; }
    }

    public class ApiQualitiy
    {
        public string ffa { get; set; }
        public string dobi { get; set; }
        public string moisture { get; set; }
        public string impurities { get; set; }
        public string no_segel_1 { get; set; }
        public string no_segel_2 { get; set; }
    }

    public class context {
        public string unit_code { get; set; }
    }

    public class ResponseModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string contract_date { get; set; }
        public string start_contract { get; set; }
        public string expired_date { get; set; }
        public double qty { get; set; }
        public List<object> product_id { get; set; }
        public double unit_price { get; set; }
        public string contract_type { get; set; }
        public List<object> partner_id { get; set; }
    }

    public class ResponseBPModel
    {
            public int id { get; set; }
            public string name { get; set; }
            public string company_type { get; set; }
            public string street { get; set; }
            public string street2 { get; set; }
            public string city { get; set; }         
            public string zip { get; set; }
            public string vat { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public int customer_rank { get; set; }
            public int supplier_rank { get; set; }
    }
}