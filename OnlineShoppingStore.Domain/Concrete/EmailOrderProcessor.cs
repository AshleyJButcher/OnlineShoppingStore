using OnlineShoppingStore.Domain.Abstract;
using OnlineShoppingStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingStore.Domain.Concrete
{
   public class EmailOrderProcessor : IOrderProcessor
    {

        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(emailSettings.Username,
                        emailSettings.Password);


                StringBuilder body = new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("---")
                    .AppendLine("Items:");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c})\n",
                        line.Quantity,
                        line.Product.Name,
                        subtotal);
                }
                body.AppendFormat("Total order value: {0:c}",
                    cart.CalculateTotalPrice())
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Address1)
                    .AppendLine(shippingInfo.Address2 ?? "")
                    .AppendLine(shippingInfo.Address3 ?? "")
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.State ?? "")
                    .AppendLine(shippingInfo.Country)
                    .AppendLine(shippingInfo.Postcode)
                    .AppendLine("---")
                    .AppendFormat("Gift wrap: {0}",
                        shippingInfo.GiftWrapped ? "Yes" : "No");
                MailMessage mailMessage = new MailMessage(new MailAddress(emailSettings.MailFromAddress).Address,
                    new MailAddress(emailSettings.MailToAddress).Address,
                    "New order submitted!",
                    body.ToString());

                smtpClient.Send(mailMessage);
            }
        }

    }
}
