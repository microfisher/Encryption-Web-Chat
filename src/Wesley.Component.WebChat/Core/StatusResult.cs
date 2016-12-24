using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wesley.Component.WebChat.Core
{
    public class StatusResult<T>
    {
        public int Status { get; set; } = 0;

        public T Data { get; set; } = default(T);

        public StatusResult()
        {

        }

        public StatusResult(int status, T data) {
            Status = status;
            Data = data;
        }

    }

    public class StatusResult
    {
        public int Status { get; set; } = 0;

        public object Data { get; set; } = new object();

        public StatusResult() {

        }
        public StatusResult(int status, object data)
        {
            Status = status;
            Data = data;
        }
    }
}
