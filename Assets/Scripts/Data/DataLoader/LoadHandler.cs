using System;

namespace TestApp.Data
{
    public class LoadHandler<T>
    {
        public T Result { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDone { get; private set; }
        public string ErrorMessage { get; private set; }
        public float Progress { get; private set; }

        public event Action<T> Success;
        public event Action<Exception> Error;
        public event Action<float> ProgressChanged;

        public void SetProgress(float value)
        {
            Progress = value;
            ProgressChanged?.Invoke(Progress);
        }

        public void SetSuccess(T result)
        {
            Result = result;

            IsCompleted = true;
            IsDone = true;

            Success?.Invoke(result);
        }

        public void SetError(Exception e)
        {
            ErrorMessage = e.Message;
            IsCompleted = true;
            Error?.Invoke(e);
        }
    }
}