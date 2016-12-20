namespace AKK.Controllers.ApiResponses {
    //The object which is returned if the request failed
    //This object also contains an error message
    public class ApiErrorResponse<T> : ApiResponse<T> {
        
        public string ErrorMessage {get; set;}

        public override object Value {
            get {
                return new {success = false, message=ErrorMessage};
            } 
        }
        
        public ApiErrorResponse (string errorMessage) : base(false)
        {
            ErrorMessage = errorMessage;
            Data = default(T);
        }
    }
}