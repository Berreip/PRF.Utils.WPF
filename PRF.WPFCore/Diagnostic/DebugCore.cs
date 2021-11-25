using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PRF.WPFCore.Diagnostic
{
    public static class DebugCore
    {
        private static Func<AssertionFailedResult, AssertionResponse> _assertionFailedCallBack;

        public static void SetAssertionFailedCallback(Func<AssertionFailedResult, AssertionResponse> assertionFailedCallBack)
        {
            _assertionFailedCallBack = assertionFailedCallBack;
        }

        private static void FailCore(string stackTrace, string message, string errorSource)
        {
            if (_assertionFailedCallBack != null)
            {
                // use the call back to provide specific behaviour
                var response = _assertionFailedCallBack(new AssertionFailedResult(stackTrace, message, errorSource));
                switch (response)
                {
                    case AssertionResponse.Ignore:
                        return;
                    case AssertionResponse.Debug:
                        if (Debugger.IsAttached)
                        {
                            Debugger.Break();
                        }
                        else
                        {
                            Debugger.Launch();
                        }
                        return;
                    default:
                    // ReSharper disable once RedundantCaseLabel
                    case AssertionResponse.TerminateProcess:
                        Environment.FailFast("Exiting from assertion terminate process");
                        return;
                }
            }

            // default behavior if no callback
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            else
            {
                // In Core, we do not show a dialog.
                // Fail in order to avoid anyone catching an exception and masking
                // an assert failure.
                var ex = new NotSupportedException(string.Join(Environment.NewLine, new[] { message, stackTrace, errorSource }));
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
        public string SourceMethod { get; }

        public AssertionFailedResult(string stackTrace, string message, string sourceMethod)
        {
            StackTrace = stackTrace;
            Message = message;
            SourceMethod = sourceMethod;
        }
    }

    public enum AssertionResponse
    {
        Ignore,
        Debug,
        TerminateProcess
    }
}