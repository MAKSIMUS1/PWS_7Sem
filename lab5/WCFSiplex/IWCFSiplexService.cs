using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFSiplex
{
    [ServiceContract]
    public interface IWCFSiplexService
    {
        [OperationContract]
        int Add(int x, int y);

        [OperationContract]
        string Concat(string s, double d);

        [OperationContract]
        A Sum(A a1, A a2);
    }

    [DataContract]
    public class A
    {
        [DataMember]
        public string s { get; set; }

        [DataMember]
        public int k { get; set; }

        [DataMember]
        public float f { get; set; }
    }
}
