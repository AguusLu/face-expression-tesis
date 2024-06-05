using facial_expression.WEB.Models;

namespace facial_expression.WEB.Repository
{
    public interface IImageRespository
    {
        List<Expresion> GetAllImages();

    }

    public class ImageRepository : IImageRespository
    {

        private List<Expresion> _images;

        public ImageRepository(List<Expresion> images) { 
            this._images = images;
        }

        public void Init()
        {
            this._images.Clear();
        }

        public List<Expresion> GetAllImages()
        {
            return null;
        }
    }
}
