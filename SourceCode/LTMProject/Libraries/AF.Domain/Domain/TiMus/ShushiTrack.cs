using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core;

namespace AF.Domain.Domain.TiMus
{
    /// <summary>
    /// 竖式表
    /// </summary>
    public class ShushiTrack : BaseEntity
    {
        /// <summary>
        /// 竖式id
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public new Guid Id { get; set; }

        /// <summary>
        /// 题目id
        /// </summary>
        /// <value>
        /// The tm identifier.
        /// </value>
        public Guid TmId { get; set; }

        /// <summary>
        /// 竖式
        /// </summary>
        /// <value>
        /// The track.
        /// </value>
        public string Track { get; set; }
    }
}
