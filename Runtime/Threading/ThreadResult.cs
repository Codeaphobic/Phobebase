using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Phobebase.Threading
{
    // A Janky way to get around object being a nullable type
    public struct ThreadResult
    {
        public bool complete { get; set; }
        public object result { get; set; }

        public ThreadResult(bool complete, object result)
        {
            this.complete = complete;
            this.result = result;
        }
    }
}