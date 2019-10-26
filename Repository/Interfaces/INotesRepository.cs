using Entity.DatabaseModels;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface INotesRepository
    {
        Task<bool> Add(GhichuModel model);
    }
}

