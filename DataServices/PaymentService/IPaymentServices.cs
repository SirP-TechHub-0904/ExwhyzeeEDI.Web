using ExwhyzeeEDI.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExwhyzeeEDI.Web.DataServices.PaymentService
{
   public interface IPaymentServices
    {
        Task<string> Create(Payment model);
        Task<Payment> Get(int? id);
        Task<string> Edit(Payment models);
        Task<string> Delete(int? id);
        Task<List<Payment>> List();
    }
}
