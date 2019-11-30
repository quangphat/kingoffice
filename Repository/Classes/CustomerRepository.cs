using Dapper;
using Entity.DatabaseModels;
using Entity.Infrastructures;
using Entity.ViewModels;
using Microsoft.Extensions.Configuration;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Classes
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<int> Create(Customer customer)
        {
            var p = AddOutputParam("id");
            p.Add("fullname", customer.FullName);
            p.Add("checkdate", customer.CheckDate);
            p.Add("cmnd", customer.Cmnd);
            p.Add("createdtime", DateTime.Now);
            p.Add("status", customer.CICStatus);
            p.Add("note", customer.LastNote);
            p.Add("createdby", customer.CreatedBy);
            p.Add("gender", customer.Gender);
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_InsertCustomer", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("id");
            }
               
        }
        public async Task<bool> AddNote(CustomerNote note)
        {
            string sql = $"insert into CustomerNote(CustomerId, Note,CreatedTime,CreatedBy) values (@customerId,@note,@createdTime,@createdBy)";
            using (var con = GetConnection())
            {
                await con.ExecuteAsync(sql, new
                {
                    customerId = note.CustomerId,
                    note = note.Note,
                    createdTime = DateTime.Now,
                    createdBy = note.CreatedBy
                }, commandType: CommandType.Text);
                return true;
            }
            
        }
        public async Task<bool> Update(Customer customer)
        {
            var p = new DynamicParameters();
            p.Add("id", customer.Id);
            p.Add("fullname", customer.FullName);
            p.Add("checkdate", customer.CheckDate);
            p.Add("cmnd", customer.Cmnd);
            p.Add("status", customer.CICStatus);
            p.Add("note", string.IsNullOrWhiteSpace(customer.LastNote) ? null : customer.LastNote);
            p.Add("gender", customer.Gender);
            p.Add("match", customer.MatchCondition);
            p.Add("notmatch", customer.NotMatch);
            p.Add("updatedtime", DateTime.Now);
            p.Add("updatedby", customer.CreatedBy);
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_UpdateCustomer", p, commandType: CommandType.StoredProcedure);
                return true;
            }
            
        }
        public async Task<Customer>  GetById(int customerId)
        {
            string sql = $"select * from Customer where Id = @id";
            using (var con = GetConnection())
            {
                var customer = await con.QueryFirstOrDefaultAsync<Customer>(sql, new
                {
                    id = customerId
                }, commandType: CommandType.Text);
                return customer;
            }
           
        }
        public async Task<bool> DeleteCustomerCheck(int customerId)
        {
           string sql = $"delete  CustomerCheck where CustomerId = @CustomerId";
            using (var con = GetConnection())
            {
                await con.ExecuteAsync(sql, new
                {
                    CustomerId = customerId
                }, commandType: CommandType.Text);
                return true;
            }
            
        }
        public async Task<List<int>> GetCustomerCheckByCustomerId(int customerId)
        {
            string sql = $"select PartnerId from CustomerCheck where CustomerId = @CustomerId";
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<int>(sql, new
                {
                    CustomerId = customerId
                }, commandType: CommandType.Text);
                return result.ToList();
            }
            
        }
        public async Task<bool> InserCustomerCheck(CustomerCheck check)
        {
            var p = AddOutputParam("id");
            p.Add("customerId", check.CustomerId);
            p.Add("partnerId", check.PartnerId);
            p.Add("status", check.Status);
            p.Add("createdtime", DateTime.Now);
            p.Add("createdby", check.CreatedBy);
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_InserCustomerCheck", p, commandType: CommandType.StoredProcedure);
                return true;
            }
            
        }
        public async Task<int> Count(string freeText)
        {
            var p = new DynamicParameters();
            if (string.IsNullOrWhiteSpace(freeText))
                freeText = "";
            p.Add("freeText", freeText);
            using (var con = GetConnection())
            {
                var total = await con.ExecuteScalarAsync<int>("sp_CountCustomer", p, commandType: CommandType.StoredProcedure);
                return total;
            }
            
        }
        public async Task<List<Customer>> Gets(
            string freeText,
            int page,
            int limit)
        {
            int offset = (page - 1) * limit;
            if (string.IsNullOrWhiteSpace(freeText))
                freeText = "";
            var p = new DynamicParameters();
            p.Add("freeText", freeText);
            p.Add("offset", offset);
            p.Add("limit", limit);
            using (var con = GetConnection())
            {
                var results = await con.QueryAsync<Customer>("sp_GetCustomer", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            
        }
        public async Task<List<CustomerNoteViewModel>> GetNoteByCustomerId(int customerId)
        {
            var p = new DynamicParameters();
            p.Add("customerId", customerId);
            using (var con = GetConnection())
            {
                var results = await con.QueryAsync<CustomerNoteViewModel>("sp_GetNotesByCustomerId", p, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            
        }
    }
}
