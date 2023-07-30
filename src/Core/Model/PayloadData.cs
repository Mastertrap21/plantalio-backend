using System.Collections.Generic;

namespace Core.Model;

public class PayloadData : Dictionary<string, object>
    {
        public PayloadData? Parent { get; }
        
        public PayloadData()
        {
        }

        public PayloadData(IDictionary<string, object> dictionary) : base(dictionary)
        {
        }

        public PayloadData(IDictionary<string,object> dictionary, PayloadData? parent) : base(dictionary)
        {
            Parent = parent;
        }
    }