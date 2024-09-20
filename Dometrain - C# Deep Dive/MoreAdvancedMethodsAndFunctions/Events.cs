﻿public sealed class Events
{
    public void RunExample()
    {
        // events and event handlers in C# are a way that we can have
        // delegates be called by "subscribing" them to events.
        // This allows us to create a mechanism where we can
        // notify other components in our system about changes
        // they care about.

        // Event handlers will get executed in the same order in
        //   which they are chained on to the event.

        // in the most common scenarios, events use an EventHandler delegate, which accepts
        // a type called EventArgs. The delegate signature is:
        //public delegate void EventHandler<TEventArgs>(object sender, TEventArgs e)

        // the "sender" parameter is the object that raised the event
        // the "e" parameter is an instance of the EventArgs class

        EventSource source = new EventSource();

        // we hook up a new handler with +=
        source.SourceChanged += Source_SourceChanged;

        // this will cause the event to be raised
        source.RaiseEvent("Hello, world!");

        // we can remove the event with -=
        // This is safe to run on a source that doesn't have any events assigned on it
        source.SourceChanged -= Source_SourceChanged;

        // but why the += and -= syntax? why aren't
        // we just assigning the handler?
        // well... we can hook up chains of handlers!
        source.SourceChanged += Source_SourceChanged1;
        source.SourceChanged += Source_SourceChanged2;
        source.RaiseEvent("Hello, world! From Nick");

        // What gets output here is:

        //  Sender: EventSource
        //  Message: Hello, world!
        //  This is the first handler!
        //  Sender: EventSource
        //  Message: Hello, world! From Nick
        //  This is the second handler!
        //  Sender: EventSource
        //  Message: Hello, world! From Nick

        void Source_SourceChanged(object? sender, MessageEventArgs e)
        {
            Console.WriteLine($"Sender: {sender}");
            Console.WriteLine($"Message: {e.Message}");
        }

        void Source_SourceChanged1(object? sender, MessageEventArgs e)
        {
            Console.WriteLine("This is the first handler!");
            Console.WriteLine($"Sender: {sender}");
            Console.WriteLine($"Message: {e.Message}");
        }

        void Source_SourceChanged2(object? sender, MessageEventArgs e)
        {
            Console.WriteLine("This is the second handler!");
            Console.WriteLine($"Sender: {sender}");
            Console.WriteLine($"Message: {e.Message}");
        }

        // Events are ultimately just a different way of using
        // delegates and callbacks! We're telling the source
        // of the event that we want them to call our method
        // (the handler) when something happens.

        #region Beware of Memory Leaks

        // NOTE: complex systems with events can lead to memory leaks
        // if you don't unsubscribe from events when you're done with them
        // because the event source will keep a reference to the handler
        // and the handler will keep a reference to the event source
        // and the garbage collector won't be able to clean them up.
        // If your event subscriptions live for the lifetime of the
        // application, then this isn't really a problem.

        #endregion
    }

    #region Creating our own EventArgs

     // let's create our own EventArgs class for sending a message!
   public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

    #endregion

    public class EventSource
    {
        // this declares the event, and the type of the event
        // but nobody outside of this class can raise the event
        // directly by accessing this
        public event EventHandler<MessageEventArgs> SourceChanged;

        public void RaiseEvent(string message)
        {
            #region Checking for null - old ways..

            SourceChanged.Invoke(this, new MessageEventArgs(message));

            // but we need to check for null!
            if (SourceChanged != null)
            {
                SourceChanged(this, new MessageEventArgs(message));
                // This is the same as SourceChanged.Invoke(..)
                // Slight possibility of a race condition, if the source changes between checking and invoking
            }

            // arguably there could be a race condition here (above) so:
            var handler = SourceChanged;
            if (handler != null)
            {
                handler(this, new MessageEventArgs(message));
            }

            #endregion  

            // but that can be greatly simplified to the following:
            SourceChanged?.Invoke(this, new MessageEventArgs(message));
        }
    }
}