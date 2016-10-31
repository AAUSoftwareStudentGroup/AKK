namespace AKK.Classes.ApiResponses {
    public class ApiSuccessResponse : ApiResponse {
        
        public override object Value {
            get {
                return new {success = true, data=Data};
            } 
        }
        
        public ApiSuccessResponse (object data) : base(true)
        {
            Data = data;
        }
    }
}