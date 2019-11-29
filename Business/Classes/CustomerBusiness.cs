using Business.Infrastructures;
using Business.Interfaces;
using Entity;
using Entity.DatabaseModels;
using Entity.Infrastructures;
using Entity.PostModel;
using Entity.ViewModels;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Classes
{
    public class CustomerBusiness :BaseBusiness, ICustomerBusiness
    {
        protected readonly ICustomerRepository _rpCustomer;
        protected readonly IDoitacRepository _rpDoitac;
        public CustomerBusiness(CurrentProcess process, 
            IDoitacRepository doitacRepository,
            ICustomerRepository customerRepository):base(null,process)
        {
            _rpCustomer = customerRepository;
            _rpDoitac = doitacRepository;
        }

        public async Task<bool> AddNote(CustomerNote note)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Create(CustomerModel model)
        {
            if(model ==null)
            {
                AddError(errors.invalid_data);
                return 0;
            }
            if(string.IsNullOrWhiteSpace(model.FullName))
            {
                AddError(errors.missing_name);
                return 0;
            }
            if (string.IsNullOrWhiteSpace(model.Cmnd))
            {
                AddError(errors.missing_cmnd);
                return 0;
            }
            if (model.CheckDate == null)
            {
                AddError(errors.mising_checkdate);
                return 0;
            }
            var customer = new Customer
            {
                FullName = model.FullName,
                CheckDate = model.CheckDate.Value,
                Cmnd = model.Cmnd,
                CICStatus = 0,
                LastNote = model.Note,
                CreatedBy = _process.User.Id,
                Gender = model.Gender
            };
            int id = await _rpCustomer.Create(customer);
            if (id > 0)
            {
                if (!string.IsNullOrWhiteSpace(model.Note))
                {
                    var note = new CustomerNote
                    {
                        Note = model.Note,
                        CustomerId = id,
                        CreatedBy = customer.CreatedBy
                    };
                    await _rpCustomer.AddNote(note);
                }

                return id;
            }
            return 0;
        }

        public async Task<bool> DeleteCustomerCheck(int customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> GetById(int customerId)
        {
            if(customerId<=0)
            {
                AddError(errors.invalid_id);
                return null;
            }
            var result = await _rpCustomer.GetById(customerId);
            return result;
        }
        public async Task<List<OptionSimpleWithIsSelect>> GetPartner(int customerId)
        {
            var customerCheck = await _rpCustomer.GetCustomerCheckByCustomerId(customerId);
            var partners = await _rpDoitac.GetListForCheckCustomerDuplicate();
            if (partners == null)
                return new List<OptionSimpleWithIsSelect>();
            foreach (var item in partners)
            {
                item.IsSelect = customerCheck.Contains(item.Id);
            }
            return partners;
        }
        public async Task<List<int>> GetCustomerCheckByCustomerId(int customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CustomerNoteViewModel>> GetNoteByCustomerId(int customerId)
        {
            if (customerId <= 0)
            {
                AddError(errors.invalid_id);
                return null;
            }
            var result = await _rpCustomer.GetNoteByCustomerId(customerId);
            return result;
        }

        public async Task<(List<Customer> datas,int totalRecord)> Gets(string freeText, int page =0, int limit = 10)
        {
            BusinessExtension.ProcessPaging(ref page, ref limit);
            var totalRecord = await _rpCustomer.Count(freeText);
            if(totalRecord <=0)
            {
                return (null, 0);
            }
            var datas = await _rpCustomer.Gets(freeText, page, limit);
            return (datas, totalRecord);
        }

        public async Task<bool> InserCustomerCheck(CustomerCheck check)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(CustomerEditModel model)
        {
            if (model == null || model.Customer == null)
            {
                AddError(errors.invalid_data);
                return false;
            }
            var match = string.Empty;
            var notmatch = string.Empty;
            if (model.Partners != null)
            {
                match = string.Join(", ", model.Partners.Where(p => p.IsSelect == true).Select(p => p.Name).ToArray());
                notmatch = string.Join(", ", model.Partners.Where(p => !p.IsSelect).Select(p => p.Name).ToArray());
            }

            var customer = new Customer
            {
                Id = model.Customer.Id,
                FullName = model.Customer.FullName,
                CheckDate = model.Customer.CheckDate.Value,
                Cmnd = model.Customer.Cmnd,
                CICStatus = model.Customer.CICStatus,
                LastNote = model.Customer.Note,
                UpdatedBy = _process.User.Id,
                Gender = model.Customer.Gender,
                MatchCondition = match,
                NotMatch = notmatch
            };

            var result = await _rpCustomer.Update(customer);
            if (!result)
                return false;
            if (!string.IsNullOrWhiteSpace(model.Customer.Note))
            {
                var note = new CustomerNote
                {
                    Note = model.Customer.Note,
                    CustomerId = customer.Id,
                    CreatedBy = customer.UpdatedBy
                };
                await _rpCustomer.AddNote(note);
            }
            if (model.Partners ==null || !model.Partners.Any())
            {
                return true;
            }
            var deleteCustomerCheck = await _rpCustomer.DeleteCustomerCheck(customer.Id);
            if (deleteCustomerCheck)
            {
                var selectedPartner = model.Partners.Where(p => p.IsSelect == true).ToList();
                if (selectedPartner.Any())
                {
                    foreach (var item in selectedPartner)
                    {
                        var partner = new CustomerCheck
                        {
                            CustomerId = customer.Id,
                            PartnerId = item.Id,
                            Status = 1,
                            CreatedBy = customer.UpdatedBy
                        };
                        await _rpCustomer.InserCustomerCheck(partner);
                    }
                }
            }
            return true;
        }
    }
}
