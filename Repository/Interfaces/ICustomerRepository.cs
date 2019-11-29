﻿using Entity.DatabaseModels;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ICustomerRepository
    {
        Task<int> Create(Customer customer);
        Task<bool> AddNote(CustomerNote note);
        Task<bool> Update(Customer customer);
        Task<Customer> GetById(int customerId);
        Task<bool> DeleteCustomerCheck(int customerId);
        Task<List<int>> GetCustomerCheckByCustomerId(int customerId);
        Task<bool> InserCustomerCheck(CustomerCheck check);
        Task<int> Count(string freeText);
        Task<List<Customer>> Gets(
            string freeText,
            int page,
            int limit);
        Task<List<CustomerNoteViewModel>> GetNoteByCustomerId(int customerId);
    }
}
