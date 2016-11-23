namespace AKK.Classes.ApiResponses {
    public class ApiSuccessResponse<T> : ApiResponse<T> {
        
        public override object Value {
            get {
                return new {success = true, data=Data};
            } 
        }
        
        public ApiSuccessResponse (T data) : base(true)
        {
            Data = data;
        }
    }
}