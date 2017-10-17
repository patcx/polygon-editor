using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonEditor.Desktop.Models.Constraints
{
    public class VerticalEdgeConstraint : IVertexConstraint
    {
        public bool IsVertexInvolved(Vertex vertex)
        {
            throw new NotImplementedException();
        }

        public bool IsContraintValid()
        {
            throw new NotImplementedException();
        }

        public bool TryRepairConstraint(Polygon polygon)
        {
            throw new NotImplementedException();
        }
    }
}
