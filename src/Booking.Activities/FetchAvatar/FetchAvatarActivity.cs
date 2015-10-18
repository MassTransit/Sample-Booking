namespace Booking.Activities.FetchAvatar
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts;
    using Contracts.Events;
    using MassTransit.Courier;
    using MassTransit.Courier.Exceptions;


    public class FetchAvatarActivity :
        ExecuteActivity<FetchAvatarArguments>
    {
        readonly FetchAvatarActivitySettings _settings;

        public FetchAvatarActivity(FetchAvatarActivitySettings settings)
        {
            _settings = settings;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<FetchAvatarArguments> context)
        {
            var emailAddress = context.Arguments.EmailAddress;
            if (string.IsNullOrWhiteSpace(emailAddress))
                throw new ActivityExecutionException("EmailAddress is required");

            var avatarName = GenerateAvatarName(emailAddress);
            var avatarAddress = GetAvatarAddress(avatarName);
            var avatarFileName = Path.Combine(_settings.CacheFolderPath, $"{avatarName}.jpg");

            using (var httpClient = new HttpClient())
            {
                var request = await httpClient.GetAsync(avatarAddress);
                if (request.IsSuccessStatusCode)
                {
                    using (var fileStream = File.Create(avatarFileName, 4096, FileOptions.Asynchronous))
                    {
                        await request.Content.CopyToAsync(fileStream);
                    }

                    return context.CompletedWithVariables(new
                    {
                        AvatarAddress = avatarAddress,
                        AvatarFileName = avatarFileName
                    });
                }

                await context.Publish<BookingRequestNotAccepted>(new RequestNotAccepted(context.Arguments, ReasonCodes.AvatarNotFound.Code,
                    $"Avatar not found: {emailAddress}"));

                return context.Terminate();
            }
        }

        Uri GetAvatarAddress(string avatarName)
        {
            var uriBuilder = new UriBuilder($"http://www.gravatar.com/avatar/{avatarName}.jpg");

            return uriBuilder.Uri;
        }

        string GenerateAvatarName(string emailAddress)
        {
            var bytes = Encoding.UTF8.GetBytes(emailAddress.Trim());

            using (var md5 = new MD5Cng())
            {
                var hash = md5.ComputeHash(bytes, 0, bytes.Length);

                var avatarName = ByteArrayToString(hash);

                return avatarName;
            }
        }

        static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);

            for (var i = 0; i < ba.Length; i++)
                hex.Append(ba[i].ToString("x2"));

            return hex.ToString();
        }


        class RequestNotAccepted :
            BookingRequestNotAccepted
        {
            readonly FetchAvatarArguments _arguments;

            public RequestNotAccepted(FetchAvatarArguments arguments, int reasonCode, string reasonText)
            {
                _arguments = arguments;
                ReasonCode = reasonCode;
                ReasonText = reasonText;
            }

            public Guid BookingRequestId => _arguments.BookingRequestId;
            public int ReasonCode { get; }
            public string ReasonText { get; }
        }
    }
}