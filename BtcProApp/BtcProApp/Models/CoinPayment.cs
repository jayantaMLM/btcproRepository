using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace BtcProApp.Models
{
    public class CoinPayments
    {
        private string s_privkey = "";
        private string s_pubkey = "";
        private static readonly Encoding encoding = Encoding.UTF8;

        public CoinPayments(string privkey, string pubkey)
        {
            s_privkey = privkey;
            s_pubkey = pubkey;
            if (s_privkey.Length == 0 || s_pubkey.Length == 0)
            {
                throw new ArgumentException("Private or Public Key is empty");
            }
        }

        public Dictionary<string, object> CallAPI(string cmd, string username, string packagename, double investmentAmt, string cointype, SortedList<string, string> parms = null)
        {
            if (parms == null)
            {
                parms = new SortedList<string, string>();
            }
            parms["version"] = "1";
            parms["key"] = s_pubkey;
            parms["cmd"] = cmd;
            //parms["txid"] = "CPBF0T9M9AZ3RSADV9V4LGD4DD";
            parms["amount"] = investmentAmt.ToString(); 
            if (packagename=="Ethereum" || packagename=="Ethereum Plus")
                {
                parms["currency1"] = "ETH";
                }
            else
                {
                parms["currency1"] = "USD";
                }
           
            if (cointype == "BTC") { parms["currency2"] = "BTC"; }
            if (cointype == "ETH") { parms["currency2"] = "ETH"; }
            if (cointype == "LTC") { parms["currency2"] = "LTC"; }
            if (cointype == "DASH") { parms["currency2"] = "DASH"; }
            parms["buyer_name"] = username;
            parms["item_name"] = packagename;
            parms["ipn_url"] = "https://btcpro.co/Home/ipn";



            string post_data = "";
            foreach (KeyValuePair<string, string> parm in parms)
            {
                if (post_data.Length > 0) { post_data += "&"; }
                post_data += parm.Key + "=" + Uri.EscapeDataString(parm.Value);
            }

            byte[] keyBytes = encoding.GetBytes(s_privkey);
            byte[] postBytes = encoding.GetBytes(post_data);
            var hmacsha512 = new System.Security.Cryptography.HMACSHA512(keyBytes);
            string hmac = BitConverter.ToString(hmacsha512.ComputeHash(postBytes)).Replace("-", string.Empty);

            // do the post:
            System.Net.WebClient cl = new System.Net.WebClient();
            cl.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            cl.Headers.Add("HMAC", hmac);
            cl.Encoding = encoding;

            var ret = new Dictionary<string, object>();
            try
            {
                string resp = cl.UploadString("https://www.coinpayments.net/api.php", post_data);
                var decoder = new System.Web.Script.Serialization.JavaScriptSerializer();
                ret = decoder.Deserialize<Dictionary<string, object>>(resp);
            }
            catch (System.Net.WebException e)
            {
                ret["error"] = "Exception while contacting CoinPayments.net: " + e.Message;
            }
            catch (Exception e)
            {
                ret["error"] = "Unknown exception: " + e.Message;
            }
            return ret;
        }
    }
}