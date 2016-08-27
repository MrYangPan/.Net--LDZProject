using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AF.Core.Data;
using AF.Data.DbContext;
using AF.Domain.Domain.TiMus;

namespace AF.Services.Question
{
    public class QuestionService : IQuestionService
    {

        #region Fields

        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly IRepository<ShushiTrack> _tmShushiRepository;

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionService" /> class.
        /// </summary>
        /// <param name="dataProvider">The data provider.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="tmShushiRepository">The tm shushi repository.</param>
        public QuestionService(IDataProvider dataProvider, IDbContext dbContext,
            IRepository<ShushiTrack> tmShushiRepository)
        {
            _dataProvider = dataProvider;
            _dbContext = dbContext;
            _tmShushiRepository = tmShushiRepository;
        }

        #endregion

        /// <summary>
        /// Gets the tm shushi track by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TmShushiTrack.</returns>
        public virtual ShushiTrack GetTmShushiTrackById(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return null;
            }
            return _tmShushiRepository.GetById(id);
        }

        /// <summary>
        /// Gets the tm shushi track by identifier.
        /// </summary>
        /// <param name="timuId">The timu identifier.</param>
        /// <param name="trackId">The track identifier.</param>
        /// <param name="trackconten">The trackconten.</param>
        /// <returns>
        /// TmShushiTrack.
        /// </returns>
        public virtual ShushiTrack UpdateTmShushiTrack(string timuId, string trackId, string trackconten)
        {
            var track = GetTmShushiTrackById(trackId);
            if (track == null)
            {
                track = new ShushiTrack()
                {
                    Id = Guid.NewGuid(),
                    TmId = Guid.Parse(timuId),
                    Track = trackconten
                };
                _tmShushiRepository.Insert(track);
            }
            else
            {
                track.TmId = Guid.Parse(timuId);
                track.Track = trackconten;
                _tmShushiRepository.Update(track);
            }
            return track;
        }
    }
}
