namespace TaskHub.Business.Models.Custum
{
    public static class CustomHttpErrorNumber<T> where T : class
    {
        public static GenericResponse<T> success = new GenericResponse<T>() { errorNumber = 0, value = "success" };
        public static GenericResponse<T> unauthorized = new GenericResponse<T>() { errorNumber = 1, value = "unauthorized" };
        public static GenericResponse<T> badCredentials = new GenericResponse<T>() { errorNumber = 2, value = "bad credentials" };
        public static GenericResponse<T> accessDenied = new GenericResponse<T>() { errorNumber = 3, value = "access denied" };
        public static GenericResponse<T> notfound = new GenericResponse<T>() { errorNumber = 4, value = "notfound" };
        public static GenericResponse<T> serverError = new GenericResponse<T>() { errorNumber = 5, value = "internal server error" };
        public static GenericResponse<T> conflict = new GenericResponse<T>() { errorNumber = 5, value = "conflict" };
    }
}
