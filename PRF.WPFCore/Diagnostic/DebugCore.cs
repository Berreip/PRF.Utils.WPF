using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PRF.WPFCore.Diagnostic
{
    public static class DebugCore
    {
        private static Action<AssertionFailedResult> _assertionFailedCallBack;

        public static void SetAssertionFailedCallback(Action<AssertionFailedResult> assertionFailedCallBack)
        {
            _assertionFailedCallBack = assertionFailedCallBack;
        }

        private static void FailCore(string stackTrace, string message, string errorSource)
        {
            if (_assertionFailedCallBack != null)
            {
                // use the call back to provide specific behaviour
                _assertionFailedCallBack(new AssertionFailedResult(stackTrace, message, errorSource));
                return;
            }

            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            else
            {
                // In Core, we do not show a dialog.
                // Fail in order to avoid anyone catching an exception and masking
                // an assert failure.
                var ex = new NotSupportedException(string.Join(Environment.NewLine, new []{message, stackTrace, errorSource}));
                Environment.FailFast(ex.Message, ex);
            }
        }
        
        
        [Conditional("DEBUG")]
        public static void Fail(string message, [CallerMemberName] string methodSource = "")
        {
            FailCore(GetStackTrace(), message, methodSource);
        }
        
        [Conditional("DEBUG")]
        public static void Assert(bool condition, string message, [CallerMemberName] string methodSource = "")
        {
            if (!condition)
            {
                Fail(message, methodSource);
            }
        }

        private static string GetStackTrace()
        {
            try
            {
                return new StackTrace(0, true).ToString();
            }
            catch
            {
                return "";
            }
        }
    }

    public sealed class AssertionFailedResult
    {
        public string StackTrace { get; }
        public string Message { get; }
        public string MethodSource { get; }

        public AssertionFailedResult(string stackTrace, string message, string methodSource)
        {
            StackTrace = stackTrace;
            Message = message;
            MethodSource = methodSource;
        }
    }
}