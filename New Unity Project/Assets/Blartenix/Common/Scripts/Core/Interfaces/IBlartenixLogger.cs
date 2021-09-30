using System.Runtime.CompilerServices;

namespace Blartenix
{
    /// <summary>
    /// Blartenix logger interface for logging classes.
    /// Implement this interface for initializing any BlartenixLogger IBlartenixLogger instances.
    /// </summary>
    public interface IBlartenixLogger
    {
        /// <summary>
        /// Log a message keeping track of caller member, the source file and the code line where this method was called.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="type">Message log type. Info by default</param>
        /// <param name="callMember">Caller member. Automatically setted, don't send when calling the method.</param>
        /// <param name="file">Source file where the method was called. Automatically setted, don't send when calling the method.</param>
        /// <param name="codeLine">Code line where thw mtdhos was called. Automatically setted, don't send when calling the method.</param>
        void Log(string message, BlartenixLogType type = BlartenixLogType.Info, [CallerMemberName] string callMember = null, [CallerFilePath] string file = null, [CallerLineNumber] int codeLine = -1);

        /// <summary>
        /// For showing a simple message.
        /// </summary>
        /// <param name="message">Message to show.</param>
        /// <param name="type">Message type. Info by default.</param>
        void DisplayMessage(string message, BlartenixLogType type = BlartenixLogType.Info);
    }
}
