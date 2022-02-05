using System;
using System.Linq;
using System.Threading.Tasks;
using AcceptsCoin.Services.PosServer.Domain.Interfaces;
using AcceptsCoin.Services.PosServer.Domain.Models;
using AcceptsCoin.Services.PosServer.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AcceptsCoin.Services.PosServer.Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InvoiceGrpcService : InvoiceAppService.InvoiceAppServiceBase
    {
        private readonly ILogger<InvoiceGrpcService> _logger;
        private IInvoiceRepository _InvoiceRepository;
        public InvoiceGrpcService(ILogger<InvoiceGrpcService> logger, IInvoiceRepository InvoiceRepository)
        {
            _logger = logger;
            _InvoiceRepository = InvoiceRepository;
        }

        private Guid getUserId(ServerCallContext context)
        {
            return Guid.Parse(context.GetHttpContext().User.Identity.Name);
        }
        private string getPartnetId(ServerCallContext context)
        {
            return "bff3b2dd-e89d-46fc-a868-aab93a3efbbe";
        }
        public override async Task<InvoiceListGm> GetAll(InvoiceQueryFilter request, ServerCallContext context)
        {
            InvoiceListGm response = new InvoiceListGm();

            IQueryable<Invoice> query = _InvoiceRepository.GetQuery();


            response.CurrentPage = request.PageId;
            response.ItemCount = await _InvoiceRepository.GetCount(query);
            response.PageCount = (response.ItemCount / request.PageSize) + 1;


            var Invoices = from Invoice in await _InvoiceRepository.GetAll(query, request.PageId, request.PageSize)
                           select new InvoiceGm()
                           {
                               Id = Invoice.InvoiceId.ToString(),
                               Amount = Invoice.Amount,
                               Currency = Invoice.Currency,
                               CustomerEmail = Invoice.CustomerEmail,
                               NotificationEmail = Invoice.NotificationEmail,
                               NotificationUrl = Invoice.NotificationUrl,
                               OrderId = Invoice.OrderId.HasValue ? Invoice.OrderId.Value.ToString() : "",
                           };
            response.Items.AddRange(Invoices.ToArray());
            return await Task.FromResult(response);
        }


        public override async Task<InvoiceGm> GetById(InvoiceIdFilter request, ServerCallContext context)
        {
            var Invoice = await _InvoiceRepository.Find(request.InvoiceId);
            var searchedInvoice = new InvoiceGm()
            {
                Id = Invoice.InvoiceId.ToString(),
                Amount = Invoice.Amount,
                Currency = Invoice.Currency,
                CustomerEmail = Invoice.CustomerEmail,
                NotificationEmail = Invoice.NotificationEmail,
                NotificationUrl = Invoice.NotificationUrl,
                OrderId = Invoice.OrderId.HasValue ? Invoice.OrderId.Value.ToString() : "",
            };
            return await Task.FromResult(searchedInvoice);
        }

        public override async Task<InvoiceGm> Post(InvoiceGm request, ServerCallContext context)
        {

            var InvoiceAdded = new Invoice()
            {
                InvoiceId = Guid.NewGuid(),
                OrderId = request.OrderId == "" ? Guid.Parse(request.OrderId) : null,
                Amount = request.Amount,
                Currency = request.Currency,
                CustomerEmail = request.CustomerEmail,
                NotificationEmail = request.NotificationEmail,
                NotificationUrl = request.NotificationUrl,
                StoreId = Guid.Parse(request.StoreId),
                CreatedById = getUserId(context),
                CreatedDate = DateTime.Now,
                Published = false,
                Deleted = false,
            };

            var invoice = await _InvoiceRepository.Add(InvoiceAdded);

            var response = new InvoiceGm()
            {
                Id = invoice.InvoiceId.ToString(),
                Amount = invoice.Amount,
                Currency = invoice.Currency,
                CustomerEmail = invoice.CustomerEmail,
                NotificationEmail = invoice.NotificationEmail,
                NotificationUrl = invoice.NotificationUrl,
                OrderId = invoice.OrderId.HasValue ? invoice.OrderId.Value.ToString() : "",
            };
            return await Task.FromResult(response);
        }


        public override async Task<InvoiceGm> Put(InvoiceGm request,
           ServerCallContext context)
        {
            Invoice Invoice = await _InvoiceRepository.Find(request.Id);
            if (Invoice == null)
            {
                return await Task.FromResult<InvoiceGm>(null);
            }


            Invoice.Amount = request.Amount;
            Invoice.Currency = request.Currency;
            Invoice.CustomerEmail = request.CustomerEmail;
            Invoice.NotificationEmail = request.NotificationEmail;
            Invoice.NotificationUrl = request.NotificationUrl;
            Invoice.UpdatedById = getUserId(context);
            Invoice.UpdatedDate = DateTime.Now;

            var invoice = await _InvoiceRepository.Update(Invoice);

            return await Task.FromResult<InvoiceGm>(new InvoiceGm()
            {
                Id = Invoice.InvoiceId.ToString(),
                Amount = invoice.Amount,
                Currency = invoice.Currency,
                CustomerEmail = invoice.CustomerEmail,
                NotificationEmail = invoice.NotificationEmail,
                NotificationUrl = invoice.NotificationUrl,
                OrderId = invoice.OrderId.HasValue ? invoice.OrderId.Value.ToString() : "",
            });
        }


        public override async Task<EmptyInvoice> UpdatePublishStatus(InvoiceIdFilter request,
           ServerCallContext context)
        {
            Invoice Invoice = await _InvoiceRepository.Find(request.InvoiceId);
            if (Invoice == null)
            {
                return await Task.FromResult<EmptyInvoice>(null);
            }

            Invoice.Published = !Invoice.Published;
            Invoice.UpdatedById = getUserId(context);
            Invoice.UpdatedDate = DateTime.Now;

            await _InvoiceRepository.Update(Invoice);
            return await Task.FromResult<EmptyInvoice>(new EmptyInvoice());
        }



        public override async Task<EmptyInvoice> Delete(InvoiceIdFilter request, ServerCallContext context)
        {
            Invoice Invoice = await _InvoiceRepository.Find(request.InvoiceId);
            if (Invoice == null)
            {
                return await Task.FromResult<EmptyInvoice>(null);
            }

            await _InvoiceRepository.Delete(Invoice);
            return await Task.FromResult<EmptyInvoice>(new EmptyInvoice());
        }

        public override async Task<EmptyInvoice> SoftDelete(InvoiceIdFilter request, ServerCallContext context)
        {
            Invoice Invoice = await _InvoiceRepository.Find(request.InvoiceId);

            if (Invoice == null)
            {
                return await Task.FromResult<EmptyInvoice>(null);
            }

            Invoice.Deleted = true;
            Invoice.UpdatedById = getUserId(context);
            Invoice.UpdatedDate = DateTime.Now;

            await _InvoiceRepository.Update(Invoice);
            return await Task.FromResult<EmptyInvoice>(new EmptyInvoice());
        }
        public override async Task<EmptyInvoice> SoftDeleteCollection(InvoiceDeleteCollectionGm request, ServerCallContext context)
        {

            foreach (var item in request.Items)
            {
                Invoice Invoice = await _InvoiceRepository.Find(item.InvoiceId);

                if (Invoice == null)
                {
                    return await Task.FromResult<EmptyInvoice>(null);
                }

                Invoice.Deleted = true;
                Invoice.UpdatedById = getUserId(context);
                Invoice.UpdatedDate = DateTime.Now;

                await _InvoiceRepository.Update(Invoice);
            }

            return await Task.FromResult<EmptyInvoice>(new EmptyInvoice());
        }
    }
}
