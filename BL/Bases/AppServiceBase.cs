using AutoMapper;
using BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Bases
{
    public class AppServiceBase : IDisposable
    {
        protected IUnitOfWork TheUnitOfWork { get; set; }
        protected readonly IMapper Mapper; 

        public AppServiceBase(IUnitOfWork theUnitOfWork, IMapper mapper)
        {
            TheUnitOfWork = theUnitOfWork;
            Mapper = mapper;
        }

        public void Dispose()
        {
            TheUnitOfWork.Dispose();
        }
    }
}
