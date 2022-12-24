using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNFree
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Created { get; set; }
        public bool IsPremium { get; set; }
    }
}
