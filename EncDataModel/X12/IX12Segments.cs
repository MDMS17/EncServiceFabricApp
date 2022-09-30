using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncDataModel.X12
{
    public interface IX12Segments
    {
        bool Valid();
        string ToString();
    }
}
