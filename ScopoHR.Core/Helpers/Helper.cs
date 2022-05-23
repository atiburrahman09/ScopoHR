using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.Helpers
{
    
    public enum LeaveApplicationStatus { Pending = 0, Approaved = 1, Rejected = 2 };
    public static class ShiftId
    {
        public static string Day = "1";
        public static string Night = "2";
        public static string SG_A = "3";
        public static string SG_B = "4";
        public static string SG_C = "5";
        public static string SG_D = "6";
        
    }
    public static class AppDefaults
    {
        public static string[] SystemUsers =
            { "superuser", "admin", "user", "director", "accounts", "dataentry" };
        public static int OverTimeStartsAfterMinutes = 29;
    }

    public static class AttendanceStatus
    {
        public const string Absent = "A";
        public const string Present = "P";
        public const string Late = "L";
        public const string ExtendedLate = "EL";
        public const string Holiday = "H";
        public const string PaidLeave = "PL";
        public const string UnpaidLeave = "UP";
    }

    public static class Page
    {
        public const int Size = 20;
    }

    public enum DocumentCategory
    {
        ProfilePhoto = 1, NomineePhoto, FingerPrint,
        Medical, AttendanceCsvFile, EmployeeCsvFile,
        CardNoMappingFile, NationalId, LeaveApplication        
    };
    
    public class MailHelpers
    {
        private string path = @"D:\test\test.xls";

        public void SendMail()
        {
            
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Atib", "atibur.rahman@yahoo.com"));
            message.To.Add(new MailboxAddress("Leton Vay", "getleton@gmail.com"));
            message.Cc.Add(new MailboxAddress("Hasib Bhaia", "arnab.sust@gmail.com"));
            message.Subject = "Test Message";

            // create our message text, just like before (except don't set it as the message.Body)
            var body = new TextPart("plain")
            {
                Text = @"Hey Rahman,

                        This is test mail Message!!!

                        -- Atib
                        "
            };

            // create an image attachment for the file located at path
            var attachment = new MimePart("excel", "xls")
            {
                ContentObject = new ContentObject(File.OpenRead(path), ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(path)
            };

            // now create the multipart/mixed container to hold the message text and the
            // image attachment
            var multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(attachment);

            // now set the multipart/mixed as the message body
            message.Body = multipart;



            SmtpClient smtp = new SmtpClient();
            smtp.Connect(
                  "smtp.mail.yahoo.com"
                , 587
                , MailKit.Security.SecureSocketOptions.StartTls
            );
            smtp.Authenticate("atibur.rahman@yahoo.com", "Password");
            smtp.Send(message);
            smtp.Disconnect(true);
        }
    }


}
