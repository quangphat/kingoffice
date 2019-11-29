using Entity.DatabaseModels;
using Entity.PostModel;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICustomerBusiness
    {
        Task<List<OptionSimpleWithIsSelect>> GetPartner(int customerId);
        Task<int> Create(CustomerModel model);
        Task<bool> AddNote(CustomerNote note);
        Task<bool> Update(CustomerEditModel customer);
        Task<Customer> GetById(int customerId);
        Task<bool> DeleteCustomerCheck(int customerId);
        Task<List<int>> GetCustomerCheckByCustomerId(int customerId);
        Task<bool> InserCustomerCheck(CustomerCheck check);
        Task<(List<Customer> datas, int totalRecord)> Gets(string freeText, int page = 0, int limit = 10);
        Task<List<CustomerNoteViewModel>> GetNoteByCustomerId(int customerId);
    }
}
