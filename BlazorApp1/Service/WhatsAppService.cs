using System.Text;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using BlazorApp1.Modals;
using Microsoft.JSInterop;
namespace BlazorApp1.Service
{
    public class WhatsAppService
    {
        //    private readonly string accountSid = "ACa00010f93e55d2da33a34e16ce2d0245";
        //    private readonly string authToken = "1c090444ae0d65d6cd854f22e4a4a07e";
        //    private readonly string fromNumber = "whatsapp:+14155238886"; // ✅ CORRECT from Sandbox                                                                      // Twilio Sandbox number

        //    public WhatsAppService()
        //    {
        //        TwilioClient.Init(accountSid, authToken);
        //    }

        //    public async Task SendWhatsAppMessage(string toMobile, string message)
        //    {
        //        var to = new PhoneNumber("whatsapp:+91" + toMobile);

        //        var messageResult = await MessageResource.CreateAsync(
        //            to: to,
        //            from: new PhoneNumber(fromNumber),
        //            body: message
        //        );
        //    }

        //    public string GenerateOrderSummary(List<Modals.ProductVM> cartItems, decimal total)
        //    {
        //        var sb = new StringBuilder();
        //        sb.AppendLine("🛒 *Order Summary*");

        //        foreach (var item in cartItems)
        //        {
        //            sb.AppendLine($"\n🔸 {item.Name}");
        //            sb.AppendLine($"Qty: {item.Quantity}");
        //            sb.AppendLine($"Price: ₹{item.Price}");
        //            sb.AppendLine($"Subtotal: ₹{item.Price * item.Quantity}");
        //        }

        //        sb.AppendLine($"\n💰 *Order Total:* ₹{total}");
        //        sb.AppendLine("🚚 Delivery: To be calculated");
        //        sb.AppendLine($"\n📅 Date: {DateTime.Now:dd MMM yyyy}");

        //        return sb.ToString();
        //    }

        private readonly IJSRuntime _jsRuntime;
        public WhatsAppService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private readonly string whatsappNumber = "919319980035"; // Your business number here

        public string GenerateOrderSummary(List<ProductVM> cartItems, decimal cartTotal, string customerName, string customerMobile, string customerAddress)
        {
            var sb = new StringBuilder();

            sb.AppendLine("🌟 *Hi, I want to place an order:*");
            sb.AppendLine();
            sb.AppendLine("🛒 *Order Summary:*");
            sb.AppendLine("-----------------------------");

            foreach (var item in cartItems)
            {
                sb.AppendLine($"• *{item.Name}*");
                sb.AppendLine($"  Quantity: {item.Quantity}");
                sb.AppendLine($"  Price: ₹{item.Price} | Subtotal: ₹{item.Price * item.Quantity}");
                sb.AppendLine();
            }

            sb.AppendLine("-----------------------------");
            sb.AppendLine($"🧾 *Total Amount:* ₹{cartTotal}");
            sb.AppendLine();
            sb.AppendLine("👤 *Customer Details:*");
            sb.AppendLine($"• Name: {customerName}");
            sb.AppendLine($"• Mobile: {customerMobile}");
            sb.AppendLine($"• Address: {customerAddress}");
            sb.AppendLine();
            sb.AppendLine("✅ Please confirm the order.");

            return sb.ToString();
        }


        public async Task SendWhatsAppMessage(string message)
        {
            var encodedMessage = Uri.EscapeDataString(message);
            var url = $"https://wa.me/{whatsappNumber}?text={encodedMessage}";

            await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
        }

    }
}

