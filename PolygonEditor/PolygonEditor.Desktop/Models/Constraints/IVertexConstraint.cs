using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Desktop.Models.Constraints
{
    public interface IVertexConstraint
    {
        bool IsVertexInvolved(Vertex vertex);

        bool IsContraintValid();

        bool TryRepairConstraint(Polygon polygon);
    }
}
