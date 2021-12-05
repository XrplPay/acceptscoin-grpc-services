using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcceptsCoin.Services.CoreServer.Domain;
using AcceptsCoin.Services.CoreServer.Domain.Models;

namespace AcceptsCoin.Services.CoreServer.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategory();
    }
}
