using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpamClassificationSystem.src.models;

namespace SpamClassificationSystem.src.interfaces
{
    public interface IReader
    {
        DataSet Read(string path);
    }
}