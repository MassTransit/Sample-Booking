namespace Booking.Activities.FetchAvatar
{
    public interface FetchAvatarActivitySettings
    {
        /// <summary>
        /// The folder where avatars are to be cached
        /// </summary>
        string CacheFolderPath { get; }
    }
}