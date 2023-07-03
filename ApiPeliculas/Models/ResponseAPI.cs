using System.Net;

namespace ApiPeliculas.Models
{
    public class ResponseAPI
    {
        public ResponseAPI()
        {
            ErrorMessages = new List<string>();
        }

        public List<string> ErrorMessages { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess { get; set; } = true;

        public object Result { get; set; }
    }
}
