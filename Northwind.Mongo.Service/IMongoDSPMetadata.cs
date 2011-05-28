using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataServiceProvider;

namespace Northwind.Mongo.Service
{
    public interface IMongoDSPMetadata
    {
        DSPMetadata CreateMetadata();
    }
}
