using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace SendMail
{
    class SendMail
    {
        //现在权限设置原因 有些邮件服务器已经不能这样发邮件了
        public static void Main()
        {
            SmtpClient sc = new SmtpClient();
            sc.Host = "mail.163.com";
            MailMessage mm = new MailMessage();
            mm.Sender = new MailAddress("abtree@163.com", "abtree");
            mm.From = mm.Sender;
            mm.To.Add(new MailAddress("abtree@qq.com", "abtree"));
            //抄送
            //mm.CC.Add(new MailAddress("abtree123@gmail.com", "abtree"));
            mm.Subject = "The latest chapter";
            mm.Body = "<b>Here you can put a long message</b>";
            mm.IsBodyHtml = true;
            mm.Priority = MailPriority.High;
            //附件
            //Attachment att = new Attachment("attach.zip", MediaTypeNames.Application.Zip);
            //mm.Attachments.Add(att);
            sc.Send(mm);
        } 
    }
}
