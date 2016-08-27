using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Domain.Domain.TiMus;

namespace AF.Services.Question
{
    public interface IQuestionService
    {
        /// <summary>
        /// Gets the tm shushi track by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TmShushiTrack.</returns>
        ShushiTrack GetTmShushiTrackById(string id);

        /// <summary>
        /// Gets the tm shushi track by identifier.
        /// </summary>
        /// <param name="timuId">The timu identifier.</param>
        /// <param name="trackId">The track identifier.</param>
        /// <param name="trackconten">The trackconten.</param>
        /// <returns>TmShushiTrack.</returns>
        ShushiTrack UpdateTmShushiTrack(string timuId, string trackId, string trackconten);
    }
}
