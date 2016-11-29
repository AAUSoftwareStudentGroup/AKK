using System;
using System.Collections.Generic;
using System.Linq;

namespace AKK.Models.Repositories
{
    public class ImageRepository : DbSetRepository<Image> {
        private MainDbContext _dbContext;

        public ImageRepository(MainDbContext dbContext) : base(dbContext.Images, dbContext) {
            _dbContext = dbContext;
        }

        public override Image Find(Guid Id) {
            var image = _dbContext.Images.FirstOrDefault(d => d.Id == Id);
            image.Holds = _dbContext.Holds.Where(h => h.ImageId == Id).ToList();
            return image;
        }

        public override IEnumerable<Image> GetAll() {
            return _dbContext.Images;
        }
    }
}
