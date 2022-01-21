using Kean.Infrastructure.Soap;
using Kean.Presentation.Rest.Soaps.Contracts;
using System;

namespace Kean.Presentation.Rest.Soaps.Services
{
    [Route("soap/test")]
    public class MyService : MyServiceContract
    {
        public void DoWork(MyRequestContract request)
        {
            throw new NotImplementedException();
        }
    }
}
