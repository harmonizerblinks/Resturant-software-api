using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resturant.Repository;
using Resturant.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.SignalR;
using Resturant.Services;
using System;

namespace Resturant.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public IHubContext<OrderHub> _order;
        //private IMyServices _get;
        private IOrderRepository _orderRepository;
        private IOrderListRepository _orderlistRepository;
        private readonly ITellerRepository _tellerRepository;
        private readonly ITransitRepository _transitRepository;
        private readonly ITransactionRepository _transactionRepository;

        public OrderController(IOrderRepository orderRepository, IOrderListRepository orderlistRepository,
            IHubContext<OrderHub> order, /*IMyServices get,*/ ITransitRepository transitRepository,
            ITransactionRepository transactionRepository, ITellerRepository tellerRepository)
        {
            //_get = get;
            _order = order;
            _orderRepository = orderRepository;
            _tellerRepository = tellerRepository;
            _transitRepository = transitRepository;
            _transactionRepository = transactionRepository;
            _orderlistRepository = orderlistRepository;
        }

        // GET Order
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var order = _orderRepository.GetAll();
            var orderlist = _orderlistRepository.GetAll();

            return Ok(order);
        }

        // GET Order
        [HttpGet("list")]
        public async Task<IActionResult> GetOrderlist()
        {
            var orderlist = _orderlistRepository.GetAll();

            return Ok(orderlist);
        }

        [HttpGet("list/{id}")]
        public async Task<IActionResult> GetOrderlistByOrderId([FromRoute] int id)
        {
            var orderlist = _orderlistRepository.GetAll().Where(l=>l.OrderId == id);

            return Ok(orderlist);
        }

        // GET Order
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAsync(id);

            if (order != null)
            {
                return Ok(order.Result);
            }
            else
                return BadRequest();
        }

        // GET Order/Code/ORD344
        [HttpGet("Code/{code}")]
        public async Task<IActionResult> GetOrderByCode([FromRoute] string code)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAll().Where(c => c.OrderNo == code).FirstOrDefault();

            return Ok(order);
        }
        
        // GET Order/Code/ORD344
        [HttpGet("Status/{status}")]
        public async Task<IActionResult> GetOrderByStatus([FromRoute] string status)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAll().Where(c => c.Status.ToLower() == status.ToLower()).FirstOrDefault();

            return Ok(order);
        }

        // GET Order/Summary/userid
        [HttpGet("Summary/{id}/{date}")]
        public async Task<IActionResult> GetOrderSummary([FromRoute] string id, DateTime date)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var order = _orderRepository.GetTodayOrders(date).Where(c => c.UserId == id);
            var now = date;

            var sum = _tellerRepository.Query().Where(t => t.Id.Equals(id)).FirstOrDefault();
            if (sum == null) return BadRequest("You Are not A Valid Teller");
            var cash = new
            {
                Opening = _transactionRepository.Query().Where(c => c.Type == "Credit" && c.NominalId == sum.NominalId && c.TellerId == sum.TellerId && c.Date < now).Select(a => a.Amount).Sum()
                                    - _transactionRepository.Query().Where(c => c.Type == "Debit" && c.NominalId == sum.NominalId && c.TellerId == sum.TellerId && c.Date.Date < now).Select(a => a.Amount).Sum(),
                Credit = _transactionRepository.Query().Where(c => c.Type == "Credit" && c.NominalId == sum.NominalId && c.TellerId == sum.TellerId && c.Date.Date == now).Select(a => a.Amount).Sum(),
                Debit = _transactionRepository.Query().Where(c => c.Type == "Debit" && c.NominalId == sum.NominalId && c.TellerId == sum.TellerId && c.Date.Date == now).Select(a => a.Amount).Sum(),
                Balance = _transactionRepository.Query().Where(c => c.Type == "Credit" && c.NominalId == sum.NominalId && c.TellerId == sum.TellerId && c.Date.Date == now).Select(a => a.Amount).Sum()
                                    - _transactionRepository.Query().Where(c => c.Type == "Debit" && c.NominalId == sum.NominalId && c.TellerId == sum.TellerId && c.Date.Date == now).Select(a => a.Amount).Sum()
            };

            return Ok(new { order, cash });
        }

        // GET Order/cancel/5
        [HttpGet("Cancel/{code}")]
        public async Task<IActionResult> CancelOrder([FromRoute] int code)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAll().Where(c => c.OrderId == code).FirstOrDefault();
            order.Status = "Cancel";
            await _orderRepository.UpdateAsync(order);
            //var _get = new MyServices();
            //await _get.RefreshOrder();

            return Ok(order);
        }

        // GET Order/confirm/5
        [HttpGet("Accept/{code}")]
        public async Task<IActionResult> AcceptOrder([FromRoute] int code)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAll().Where(c => c.OrderId == code).FirstOrDefault();
            order.Status = "In-Process";
            await _orderRepository.UpdateAsync(order);
            //await _get.RefreshOrder();

            return Ok(order);
        }
        
        // GET Order/confirm/5
        [HttpGet("Confirm/{code}")]
        public async Task<IActionResult> ConfirmOrder([FromRoute] int code)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAll().Where(c => c.OrderId == code).FirstOrDefault();
            order.Status = "Confirm";
            await _orderRepository.UpdateAsync(order);
            //await _get.RefreshOrder();

            return Ok(order);
        }

        // GET Order/confirm/5
        [HttpGet("Ready/{code}")]
        public async Task<IActionResult> FinishOrder([FromRoute] int code)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAll().Where(c => c.OrderId == code).FirstOrDefault();
            order.Status = "Ready";
            await _orderRepository.UpdateAsync(order);
            //await _get.RefreshOrder();
            return Ok(order);
        }

        // GET Order/confirm/5
        [HttpGet("Delivered/{code}")]
        public async Task<IActionResult> DeliverOrder([FromRoute] int code)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.GetAll().Where(c => c.OrderId == code).FirstOrDefault();
            order.Status = "Delivered";
            await _orderRepository.UpdateAsync(order);
            //await _get.RefreshOrder();
            return Ok(order);
        }
        
        // POST Order
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var teller = _tellerRepository.Query().Where(i => i.Id == value.UserId).FirstOrDefault();
            if (teller == null) return BadRequest("You Are not allowed to place Order");
            var transit = _transitRepository.Query().Where(i => i.Method.ToLower() == value.Method.ToLower()).FirstOrDefault();
            if (transit == null) return BadRequest("There is no Tansit Nominal for Payment Method");

            //value.OrderNo = await _get.GetCode("Order");
            var tell = new Transaction() { TransCode = value.OrderNo, Amount = value.Total, Method = value.Method,
                Source = "Order", Type = "Credit", NominalId = teller.NominalId, TellerId = teller.TellerId,
                Reference = $"Payment For Order {value.OrderNo}", UserId = value.UserId, Date = value.Date };

            var trans = new Transaction() { TransCode = value.OrderNo, Amount = value.Total, Method = value.Method,
                Source = "Order", Type = "Debit", NominalId = transit.NominalId, TellerId = teller.TellerId,
                Reference = $"Payment For Order {value.OrderNo}", UserId = value.UserId, Date = value.Date };

            await _transactionRepository.InsertAsync(tell);
            await _transactionRepository.InsertAsync(trans);

            value.TransactionId = tell.TransactionId;
            await _orderRepository.InsertAsync(value);
            
            return Created($"order/{value.OrderId}", value);
        }

        // PUT Order
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] Order value, [FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != value.OrderId) return BadRequest();

            await _orderRepository.UpdateAsync(value);

            return Ok(value);
        }

        // DELETE Order
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var order = _orderRepository.DeleteAsync(id);

            return Ok(order.Result);
        }


    }
}