namespace AKK.Controllers.ApiResponses {
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