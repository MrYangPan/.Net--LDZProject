using AF.Domain.Domain.TiMus;

namespace AF.Data.Mapping.TiMus
{
    public class TiMuImagesMap:AFEntityTypeConfiguration<TiMuImages>
    {
        public TiMuImagesMap()
        {
            this.ToTable("TMImages");
            this.HasKey(m => m.Id);
        }
    }
}
