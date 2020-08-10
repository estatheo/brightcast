using System.Collections.Generic;
using Brightcast.Controls.Grid;
using Brightcast.Model;

namespace Brightcast.Client.Data
{
    /// <summary>
    /// Result from query request.
    /// </summary>
    public class QueryResult
    {
        /// <summary>
        /// New <see cref="PageHelper"/> information.
        /// </summary>
        public PageHelper PageInfo { get; set; }

        /// <summary>
        /// A page of <see cref="ICollection{Contact}"./>
        /// </summary>
        public ICollection<Contact> Contacts { get; set; }
    }
}
