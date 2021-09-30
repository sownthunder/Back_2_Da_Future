namespace Blartenix
{
    /// <summary>
    /// Blartenix static class with global IBlartenixLogger instance
    /// </summary>
    public static class BlartenixLogger
    {
        /// <summary>
        /// Blartenix Global IBlartenixLogger instance.
        /// Initialize this if you want to trace the internal methods execution of all Blartenix library modules.
        /// </summary>
        internal static IBlartenixLogger GlobalInstance { get; set; }
    }
}
