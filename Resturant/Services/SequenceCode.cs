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

namespace Resturant.Services
{
    public class SequenceCode
    {
        private readonly AppDbContext _context;

        public SequenceCode(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<string> GetCode(string type)
        {
            var sequence = await _context.Sequence.FirstOrDefaultAsync(a => a.Name.ToLower() == type.ToLower());

            if (sequence != null)
            {
                //string code = sequence.Prefix + sequence.Counter + sequence.Suffix;
                //int codeint = Convert.ToInt32(sequence.Counter); codeint++;
                string transcodenum = (sequence.Counter).ToString();
                int length = sequence.Length;
                int padnum = length - transcodenum.Length;
                string number = transcodenum.PadLeft(padnum + 1, '0');
                string code = sequence.Prefix + number;

                sequence.Counter += 1;
                _context.Entry(sequence).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return code;
            }
            return null;
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
