using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace *********.**********.Threading
{

   ///<summary>
   ///  The exception that is thrown when a state transition is invalid.
   ///</summary>
   ///<remarks>
   ///  The <b>InvalidTransitionException</b> class is thrown when a state transition
   ///  is invalid.
   ///</remarks>
   [Serializable]
   public class InvalidTransitionException : Exception 
   {

      ///<summary>
      ///  Initializes a new instance of the <see cref="InvalidTransitionException"/> class.
      ///</summary>
      ///<remarks>
      ///  This constructor initializes the <see cref="Exception.Message"/> property of the new 
      ///  instance to a system-supplied message that describes the error and takes into 
      ///  account the current system culture.
      ///</remarks>
      public InvalidTransitionException () : base() 
      {} 

      /// <summary>
      ///   Initializes a new instance of the <see cref="InvalidTransitionException"/> class with a specified error message.
      /// </summary>
      /// <param name="obj">
      ///   The object whose state can not be changed.
      /// </param>
      /// <param name="currentState">
      ///   Its current state.
      /// </param>
      /// <param name="nextState">
      ///   The invalid state.
      /// </param>
      /// <remarks>
      ///   This constructor initializes the <see cref="Exception.Message"/> property of the new 
      ///   instance to the string "Can not transition '<i>obj</i> from state <i>curr</i> to <i>next</i>.".
      /// </remarks>
      public InvalidTransitionException(object obj, object currentState, object nextState) :
         this(obj, currentState, nextState, null)
      {
      }

      /// <summary>
      ///   Initializes a new instance of the <see cref="InvalidTransitionException"/> class with a specified error message and
      ///   inner <see cref="Exception"/>.
      /// </summary>
      /// <param name="obj">
      ///   The object whose state can not be changed.
      /// </param>
      /// <param name="currentState">
      ///   Its current state.
      /// </param>
      /// <param name="nextState">
      ///   The invalid state.
      /// </param>
      /// <param name="innerException">
      ///   The <see cref="Exception"/> that is the cause of the current exception.
      /// </param>
      /// <remarks>
      ///   This constructor initializes the <see cref="Exception.Message"/> property of the new 
      ///   instance to the string "Can not transition '<i>obj</i> from state <i>prev</i> to <i>next</i>.".
      /// </remarks>
      public InvalidTransitionException(object obj, object currentState, object nextState, Exception innerException) :
         base (String.Format(CultureInfo.InvariantCulture, "Can not transition '{0}' from state {1} to {2}.", 
               obj, currentState, nextState), innerException)
      {
      }

      ///<summary>
      ///  Initializes a new instance of the <see cref="InvalidTransitionException"/> class with serialized data.
      ///</summary>
      ///<param name="info">
      ///  The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
      ///</param>
      ///<param name="context">
      ///  The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
      ///</param>
      ///<remarks>
      ///  This constructor is called during deserialization to reconstitute the 
      ///  exception object transmitted over a stream. 
      ///</remarks>
      protected InvalidTransitionException(SerializationInfo info, StreamingContext context) 
         : base(info,context)
      {
      }
   
   }
}

