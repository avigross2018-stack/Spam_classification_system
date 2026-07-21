using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpamClassificationSystem.src.interfaces
{
    public interface ICsvReader
    {
        DataSet Read(string path);
    }
}