namespace AKK.Classes.ApiResponses {
    public class ApiErrorResponse : ApiResponse {
        
        public string ErrorMessage {get; set;}

        public override object Value {
            get {
                return new {success = false, message=ErrorMessage};
            } 
        }
        
        public ApiErrorResponse (string errorMessage) : base(false)
        {
            ErrorMessage = errorMessage;
        }
    }
}