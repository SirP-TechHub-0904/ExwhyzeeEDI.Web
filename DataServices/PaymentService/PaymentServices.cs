using ExwhyzeeEDI.Web.Models;
using ExwhyzeeEDI.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ExwhyzeeEDI.Web.DataServices.PaymentService
{
    public class PaymentServices : IPaymentServices
    {

            private ApplicationDbContext db = new ApplicationDbContext();
            public async Task<string> Create(Payment model)
            {
                try
                {
                    model.PaymentInitalizedDate = DateTime.UtcNow;
                    model.PaymentCompleteDate = DateTime.UtcNow;
                db.Payments.Add(model);
                    await db.SaveChangesAsync();
                    return "OK";
                }
                catch (Exception c)
                {

                }
                return null;
            }

            public async Task<string> Delete(int? id)
            {
                try
                {

                    var item = await db.Payments.FirstOrDefaultAsync(x => x.Id == id);

                    db.Payments.Remove(item);
                    await db.SaveChangesAsync();
                    return "OK";

                }
                catch (Exception c)
                {

                }
                return null;
            }

            public async Task<string> Edit(Payment models)
            {
                try
                {
                    db.Entry(models).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return "OK";
                }
                catch (Exception c)
                {

                }
                return null;
            }

            public async Task<Payment> Get(int? id)
            {
                var item = await db.Payments.FirstOrDefaultAsync(x => x.Id == id);
                return item;
            }

            public async Task<List<Payment>> List()
            {
                var items = db.Payments.ToListAsync();
                return await items;

            }
        }
    }