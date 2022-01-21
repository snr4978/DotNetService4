using System.Runtime.Serialization;

namespace Kean.Presentation.Rest.Soaps.Contracts
{
    [DataContract(Name = "MyRequest")]
    public class MyRequestContract
    {
        [DataMember]
        public string Xxx { get; set; }
    }
}
