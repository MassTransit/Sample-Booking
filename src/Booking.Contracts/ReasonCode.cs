namespace Booking.Contracts
{
    public struct ReasonCode
    {
        public readonly int Code;
        public readonly string Text;

        public ReasonCode(int code, string text)
        {
            Code = code;
            Text = text;
        }
    }
}