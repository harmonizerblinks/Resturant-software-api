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
    public class Sequences
    {
        private readonly AppDbContext _context;
        //private readonly ISequenceRepository _sequenceRepository;
        //private readonly ISmsRepository _smsRepository;
        //private readonly ISmsApiRepository _smsapiRepository;
        public IHubContext<OrderHub> _order;

        private Sequences(AppDbContext context /*, IHubContext<OrderHub> order /*ISequenceRepository sequenceRepository,
            ISmsApiRepository smsapiRepository, ISmsRepository smsRepository*/ )
        {
            //_sequenceRepository = sequenceRepository;
            //_smsapiRepository = smsapiRepository;
            //_smsRepository = smsRepository;
            //_order = order;
            _context = context;
        }

        public async Task<string> GetCode(string type)
        {
            var sequence = await _context.Sequence.FirstOrDefaultAsync(a => a.Name.ToLower() == type.ToLower());
            //var sequence = await _sequenceRepository.Query().FirstOrDefaultAsync(a => a.Name.ToLower() == type.ToLower());

            if (sequence != null)
            {
                //string code = sequence.Prefix + sequence.Counter + sequence.Suffix;
                //int codeint = Convert.ToInt32(sequence.Counter); codeint++;
                string transcodenum = (sequence.Counter).ToString();
                //int length = sequence.Length;
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

        public async Task RefreshOrder()
        {
            var pending = _context.Order.Where(a => a.Status.ToLower() == "pending" && a.Status.ToLower() == "inprogress")
                .Include(l => l.Orderlist.Select(f=>f.Food));
            var ready = _context.Order.Where(a => a.Status.ToLower() == "ready").Include(l => l.Orderlist.Select(f => f.Food));

            await _order.Clients.All.SendAsync("Send", "Hello From Hangfire");
            await _order.Clients.All.SendAsync("pending", pending);
            await _order.Clients.All.SendAsync("ready", ready);

            //await Clients.All.SendAsync("Send", message);
        }

        public class SmsResponse
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }

        // POST: api/Other
        //public async Task<decimal> TopUp(TopUp payment)
        //{
        //    var topup = _context.Transaction.Where(a => a.HubCode == payment.TransactionId || a.TransCode == payment.ClientReference && a.Status == "Pending").FirstOrDefault();
        //    if (topup == null) return 0;

        //    var user = _context.User.Where(a => a.UserId == topup.UserId).FirstOrDefault();
        //    decimal charge = 0; user.Balance += payment.Amount - charge;

        //    topup.Sms = false; topup.Status = "Successful";
        //    topup.Amount = payment.Amount; topup.ChargeAmount = payment.Charges;
        //    topup.ModifiedDate = DateTime.UtcNow;

        //    _context.Entry(topup).State = EntityState.Modified;
        //    _context.Entry(user).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();

        //    return user.Balance;
        //}

    }
}
