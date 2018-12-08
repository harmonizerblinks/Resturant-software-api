using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Resturant.Models;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Resturant.Repository;
using Microsoft.AspNetCore.SignalR;
using Hangfire;

namespace Resturant.Services
{
    public class MyServices //: IMyServices
    {
        protected AppDbContext _context { get; set; }
        //protected ISmsRepository _smsRepository;
        //protected ISmsApiRepository _smsapiRepository;
        //protected IOrderRepository _orderRepository;
        //protected ISequenceRepository _sequenceRepository;
        protected IHubContext<OrderHub> _order;
        
        public async Task<string> GetCode(string type)
        {
            var sequence = await _context.Sequence.FirstOrDefaultAsync(a => a.Name.ToLower() == type.ToLower());
            //var sequence = await _sequenceRepository.Query().FirstOrDefaultAsync(a => a.Name.ToLower() == type.ToLower());

            if (sequence != null)
            {
                string transcodenum = (sequence.Counter).ToString();
                int padnum = sequence.Length - transcodenum.Length;
                string number = transcodenum.PadLeft(padnum + 1, '0');
                string code = sequence.Prefix + number;

                sequence.Counter += 1;
                //await _sequenceRepository.UpdateAsync(sequence);
                _context.Entry(sequence).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return code;
            }
            return null;
        }

        public async Task<Sms> Sms(Order order)
        {
            var config = _context.SmsApi.LastOrDefault(a => a.Status.ToLower().Contains("active") && a.Default == true);
            //var config =_smsapiRepository.Query().LastOrDefault(a => a.Status.ToLower().Contains("active") && a.Default == true);
            if (config == null)
            {
                return null;
            }
            else
            {
                var sms = new Sms();
                sms.UserId = order.UserId; sms.Date = DateTime.UtcNow; sms.Mobile = order.Mobile;
                sms.Message = $"You are Welcome to {config.Name}, your Order Number is {order.OrderNo}. We are Glad {order.FullName} to serve you.";

                try
                {
                    var httpClient = new HttpClient();
                    StringBuilder sb = new StringBuilder();
                    sb.Append(config.Url).Append("?&username=").Append(config.Username)
                        .Append("&password=").Append(config.Password).Append("&source=").Append(config.SenderId)
                        .Append("&destination=").Append(sms.Mobile).Append("&message=").Append(sms.Message);
                    var json = await httpClient.GetStringAsync(sb.ToString());
                    var smsresponse = JsonConvert.DeserializeObject<SmsResponse>(json);
                    sms.Code = smsresponse.Code; sms.Response = smsresponse.Message;
                    //await _smsRepository.InsertAsync(sms);
                    _context.Sms.Add(sms);
                    await _context.SaveChangesAsync();

                    await _order.Clients.All.SendAsync("order", order);
                    //await RefreshOrder();

                    return sms;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public async Task<string> RefreshOrder()
        {
            //var pending = _orderRepository.GetAll().Where(a => a.Status.ToLower() == "pending" && a.Status.ToLower() == "inprogress")
            //    .Include(l => l.Orderlist.Select(f=>f.Food));
            //var ready = _orderRepository.GetAll().Where(a => a.Status.ToLower() == "ready").Include(l => l.Orderlist.Select(f => f.Food));
            var pending = _context.Order.Where(a => a.Status.ToLower() == "pending" && a.Status.ToLower() == "inprogress")
                .Include(l => l.Orderlist.Select(f => f.Food));
            var ready = _context.Order.Where(a => a.Status.ToLower() == "ready").Include(l => l.Orderlist.Select(f => f.Food));

            await _order.Clients.All.SendAsync("Send", "Hello From Hangfire");
            await _order.Clients.All.SendAsync("pending", pending);
            await _order.Clients.All.SendAsync("ready", ready);

            //await Clients.All.SendAsync("Send", message);
            var ok = "Ok";
            return ok;
        }
        

    }
}
