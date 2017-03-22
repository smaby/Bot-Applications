using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net;


namespace Stock.Modules
{
    public class YahooStockServiceBot
    {
        public static async Task<String> GetStockRateAsync(string stockSymbol)
        {
            try
            {
                string serviceURL = $"http://finance.yahoo.com/d/quotes.csv?s={stockSymbol}&f=sl1d1nd";
                string resultInCSV;
                String result = String.Empty;

                using (WebClient client = new WebClient())
                {
                    resultInCSV = await client.DownloadStringTaskAsync(serviceURL); 
                }
                if (resultInCSV != null )
                {
                    var firstLine = resultInCSV.Split('\n')[0];  //extract first line from the output
                    var price = firstLine.Split(',')[1];  //get the prise of the stock
                    var companyName = firstLine.Split(',')[3];

                    result = string.Format("Symbol : {0} \n Company Name : {1} \n Price : {2}", stockSymbol, companyName, price);  
                }
                else
                {
                    result = string.Format("Symbol : {0} does not exists", stockSymbol);
                }
                                
                return result;
            }
            catch(WebException ex)
            {
                throw ex;
            }
        }
    }
}