using Challenge.Core.CustomEntities;

namespace Challenge.API.Responses
{
    public class ApiResponse<T>
    {
        public ApiResponse(T data)
        {
            Data = data;
        }
        public T Data { get; set; }

        public Metadata Metadata { get; set; }

    }
}
