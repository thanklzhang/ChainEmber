public class ServiceResponse
{
    public ErrorCode err;
    public static ServiceResponse New(ErrorCode err)
    {
        return new ServiceResponse()
        {
            err = err,
        };
    }
    public static ServiceResponse Success => New(ErrorCode.Success);
}

public class LoginResponse : ServiceResponse
{
} 