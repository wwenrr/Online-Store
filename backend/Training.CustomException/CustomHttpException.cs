namespace Training.CustomException
{
    public class CustomHttpException : Exception
    {
        public int code {  get; set; }

        public CustomHttpException(string message, int code) : base(message) {
            this.code = code;
        }
    }
}
