﻿using System;

namespace SkyNinja.Core.Exceptions
{
    public class InternalException: Exception
    {
        protected InternalException(string message) : base(message)
        {
            // Do nothing.
        }
    }
}
