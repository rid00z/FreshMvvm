//===============================================================================
// TinyIoc
//
// An easy to use, hassle free, Inversion of Control Container for small projects
// and beginners alike.
//
// https://github.com/grumpydev/TinyIoc
//===============================================================================
// Copyright © Steven Robbins.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace FreshTinyIoc
{
    using System;

    public class TinyIocResolutionException : Exception
    {
        private const string ERROR_TEXT = "Resolve failed: {0}";
        private const string ERROR_TEXT_WITHINNER = "Resolve failed: {0} - Reason: {1}";

        public TinyIocResolutionException(Type type)
            : base(String.Format(ERROR_TEXT, type.Name))
        {
        }

        public TinyIocResolutionException(Type type, Exception innerException)
            : base(String.Format(ERROR_TEXT_WITHINNER, type.Name, innerException.Message), innerException)
        {
        }
    }
}
