using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Web;

namespace BtcProApp.Models
{
    public class Email
    {
        private string _from;
        private string _to;
        private string _cc;
        private string _bcc;
        private string _subject;
        private string _body;
        private ArrayList _attachments;
        //private string _host = "";
        MailMessage mail = new MailMessage();
        SmtpClient smtp = new SmtpClient();

        public Email(Hashtable hs_data)
        {
            if (hs_data.Count > 0)
            {
                //_host = dhelp.return_mail_server();
                _from = hs_data["FROM"].ToString();
                if (hs_data["TO"] != null)
                    _to = hs_data["TO"].ToString();
                if (hs_data["CC"] != null)
                    _cc = hs_data["CC"].ToString();
                if (hs_data["BCC"] != null)
                    _bcc = hs_data["BCC"].ToString();
                _subject = hs_data["SUBJECT"].ToString();
                _body = hs_data["BODY"].ToString();
                _attachments = (ArrayList)hs_data["ATTACHMENT"];
            }
        }
        public bool SendEMail()
        {
            bool rtn = false;
            string msg = "1";
            msg = checkAllEmailAddress();

            if (msg != "1")
            {
                return false;

            }
            mail.From = new MailAddress(_from);

            IndividualToAdd(ref mail, _to);
            if (string.IsNullOrEmpty(_cc) == false)
                IndividualCCAdd(ref mail, _cc);
            if (string.IsNullOrEmpty(_bcc) == false)
                IndividualBCCAdd(ref mail, _bcc);

            mail.Subject = _subject;
            mail.IsBodyHtml = true;
            mail.Body = _body;

            if (((_attachments != null)))
            {
                foreach (string attach_loopVariable in _attachments)
                {
                    string attachmentPath = attach_loopVariable;

                    System.Net.Mail.Attachment inline = new System.Net.Mail.Attachment(attachmentPath);
                    inline.ContentDisposition.Inline = true;
                    inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                    //inline.ContentId = contentID;
                    inline.ContentType.MediaType = "image/png";
                    inline.ContentType.Name = Path.GetFileName(attachmentPath);
                    mail.Attachments.Add(inline);
                }
            }

            try
            {
                smtp.Send(mail);
                //string token = "message";
                //smtp.SendAsync(mail, token);
                smtp = null;
                foreach (System.Net.Mail.Attachment aAttach in mail.Attachments)
                {
                    aAttach.Dispose();
                }
                mail.Attachments.Dispose();
                mail.Dispose();
                mail = null;
                rtn = true;
            }
            catch(Exception e)
            {

                rtn = false;
                //throw ex;

            }
            return rtn;

        }



        private bool IsValidEmail(string email)
        {
            string[] arr = (email + ";").ToString().Split(';');
            Regex expression = new Regex("\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}");
            int i = 0;
            bool valid = false;

            for (i = 0; i <= arr.Length - 2; i++)
            {
                string address = arr[i].ToString();
                if (!string.IsNullOrEmpty(address))
                {
                    if ((expression.IsMatch(address)))
                    {
                        valid = true;
                    }
                    else
                    {
                        valid = false;
                        return valid;
                    }
                }
            }
            return valid;
        }
        private string checkAllEmailAddress()
        {
            string msg = "1";
            if (!IsValidEmail(_from))
            {
                msg = "Invalid From Email Address";
            }
            if (!IsValidEmail(_to))
            {
                msg = "Invalid To Email Address";
            }
            if (string.IsNullOrEmpty(_cc) == false && !IsValidEmail(_cc))
            {
                msg = "Invalid CC Email Address";
            }
            if (string.IsNullOrEmpty(_bcc) == false && !IsValidEmail(_bcc))
            {
                msg = "Invalid BCC Email Address";
            }
            return msg;
        }
        static void IndividualToAdd(ref MailMessage m, string str)
        {
            string[] arr = str.Split(';');
            int i = 0;

            for (i = 0; i <= arr.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(arr[i]))
                    m.To.Add(arr[i]);
            }
        }
        static void IndividualCCAdd(ref MailMessage m, string str)
        {
            string[] arr = str.Split(';');
            int i = 0;

            for (i = 0; i <= arr.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(arr[i]))
                    m.CC.Add(arr[i]);
            }
        }
        static void IndividualBCCAdd(ref MailMessage m, string str)
        {
            string[] arr = str.Split(';');
            int i = 0;

            for (i = 0; i <= arr.Length - 1; i++)
            {
                if (!string.IsNullOrEmpty(arr[i]))
                    m.Bcc.Add(arr[i]);
            }
        }


    }
}