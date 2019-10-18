using System;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class Order
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public int Type { get; set; }

        [DataMember]
        public int Price { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}\tType:{Type}\tPrice:{Price}";
        }

        public static Order Create()
        {
            return new Order()
            {
                Id = new Random().Next(1000),
                Price = new Random().Next(1000),
                Type = 1
            };
        }
    }
}