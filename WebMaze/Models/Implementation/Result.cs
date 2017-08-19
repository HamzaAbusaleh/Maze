using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMaze.Models.Implementation
{
    public class Result
    {
        public string ErrorMessage { get; set; }    
        public bool IsSuccessfull { get; set; }
    }

    public class Result<T>:Result
    {
       public T Data { get; set; }
    }
}
