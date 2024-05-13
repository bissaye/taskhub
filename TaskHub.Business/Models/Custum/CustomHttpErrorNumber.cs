namespace TaskHub.Business.Models.Custum
{
    public static class CustomHttpErrorNumber
    {
        public static GenericResponse success = new GenericResponse() { errorNumber = 0, value = "success", detail = "" };
        public static GenericResponse unauthorized = new GenericResponse() { errorNumber = 1, value = "unauthorized", detail = "" };
        public static GenericResponse badCredentials = new GenericResponse() { errorNumber = 2, value = "bad credentials", detail = "" };
        public static GenericResponse accessDenied = new GenericResponse() { errorNumber = 3, value = "access denied", detail = "" };
        public static GenericResponse notfound = new GenericResponse() { errorNumber = 4, value = "notfound", detail = "" };
        public static GenericResponse serverError = new GenericResponse() { errorNumber = 5, value = "internal server error", detail = "" };
        public static GenericResponse conflict = new GenericResponse() { errorNumber = 5, value = "conflict", detail = "" };
    }
}
