using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model
{
    public class ClientDetail
    {
        public List<ClientInfo> Clients { get; set; }
        public long RowCount { get; set; }
    }
}
