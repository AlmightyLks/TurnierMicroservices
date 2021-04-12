using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedTypes.Models
{
    public class DatabaseObject
    {
        private int _id;
        public int Id { get => _id; set => _id = value; }
    }
}
