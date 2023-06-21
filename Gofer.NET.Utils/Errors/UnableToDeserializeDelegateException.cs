﻿using Gofer.NET.Errors;
using System;

namespace Gofer.NET.Utils.Errors
{
  public class UnableToDeserializeDelegateException : Exception, IHelpfulException
  {
    public string HelpText => "The enqueued delegate was unable be deserialized. \n" +
                              "This usually happens when you've enqueued an unsupported method type. \n" +
                              "Currently only static void methods on a class may be enqueued. \n" +
                              "Lambdas or instance methods, for example, are not supported.";
  }
}