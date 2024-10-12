using StudentInformationSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.BusinessLayer.Repository
{
    public interface IPaymentRepository
    {
        Payment GetPaymentById(int id);
        IEnumerable<Payment> GetAllPayments();
        int AddPayment(Payment payment);
        void UpdatePayment(Payment payment);
        void DeletePayment(int id);
    }
}
