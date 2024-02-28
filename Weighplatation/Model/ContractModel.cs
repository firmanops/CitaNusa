using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weighplatation.Model
{
    public class ContractModel
	{
    
	public string ContractNo { get; set; }
	public DateTime ContractDate	{ get; set; }
	public DateTime ExpDate			{ get; set; }
	public string	ProductCode		{ get; set; }
	public string	BPCode			{ get; set; }
	public double	Qty				{ get; set; }
	public double	Toleransi		{ get; set; }
	public double	UnitPrice		{ get; set; }
	public double	PremiumPrice	{ get; set; }
	public double PPN				{ get; set; }
	public double	FinalUnitPrice	{ get; set; }
	public double	TotalPrice		{ get; set; }
	public double	DespatchQty		{ get; set; }
	public string	DeliveryStatus	{ get; set; }
	public int		oddoid { get; set; }
	public int partner_id { get; set; }
	public string RefNo { get; set; }
	}	
}