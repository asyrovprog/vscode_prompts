using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Lab.Iter10;

// Helper class to store messages with priority and sequence
public class PriorityMessage<T> : IComparable<PriorityMessage<T>>
{
    public int Priority { get; }
    public T Value { get; }
    public long Sequence { get; }

    public PriorityMessage(int priority, T value, long sequence)
    {
        Priority = priority;
        Value = value;
        Sequence = sequence;
    }

    // Sort by priority descending, then by sequence ascending (FIFO within same priority)
    public int CompareTo(PriorityMessage<T>? other)
    {
        if (other == null) return 1;
        
        // Higher priority first (descending)
        int priorityCompare = other.Priority.CompareTo(Priority);
        if (priorityCompare != 0) return priorityCompare;
        
        // Within same priority, FIFO (ascending sequence)
        return Sequence.CompareTo(other.Sequence);
    }
}

public class PriorityBufferBlock<T> : IPropagatorBlock<(int Priority, T Value), T>
{
    private readonly SortedSet<PriorityMessage<T>> _priorityBuffer = new();
    private readonly List<ITargetBlock<T>> _targets = new();
    private readonly TaskCompletionSource _completion = new();
    private readonly object _lock = new();
    private bool _decliningPermanently = false;
    private long _nextMessageId = 0;
    private long _sequenceNumber = 0;

    // === ITargetBlock Implementation ===
    
    public DataflowMessageStatus OfferMessage(
        DataflowMessageHeader messageHeader,
        (int Priority, T Value) messageValue,
        ISourceBlock<(int Priority, T Value)>? source,
        bool consumeToAccept)
    {
        // TODO[N1]: Implement OfferMessage with Priority Storage
        // 
        // [YOUR CODE GOES HERE]
        //
        // Requirements:
        // 1. Validate messageHeader.IsValid (throw ArgumentException if invalid)
        // 2. Check _decliningPermanently flag under lock
        //    - Return DataflowMessageStatus.DecliningPermanently if true
        // 3. Handle consumeToAccept protocol:
        //    - If consumeToAccept && source != null:
        //      * Call source.ConsumeMessage(messageHeader, this, out bool consumed)
        //      * Assign result to messageValue
        //      * Return DataflowMessageStatus.NotAvailable if !consumed
        // 4. Create PriorityMessage<T> with:
        //    - messageValue.Priority
        //    - messageValue.Value
        //    - _sequenceNumber++ (increment for FIFO within same priority)
        // 5. Add to _priorityBuffer under lock
        // 6. Return DataflowMessageStatus.Accepted
        //
        // See REF.md for detailed hints.
        
        throw new NotImplementedException("TODO[N1] - see README section 'TODO N1 – Implement OfferMessage with Priority Storage'");
    }

    // === ISourceBlock Implementation ===
    
    public IDisposable LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
    {
        if (target == null) throw new ArgumentNullException(nameof(target));

        lock (_lock)
        {
            _targets.Add(target);
        }

        // Don't propagate immediately on link - wait for Complete()
        // This allows messages to buffer first

        return new Unlinker(() =>
        {
            lock (_lock)
            {
                _targets.Remove(target);
            }
        });
    }

    public T? ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
    {
        messageConsumed = false;
        return default;
    }

    public bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target) => false;

    public void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target) { }

    // === IDataflowBlock Implementation ===
    
    public Task Completion => _completion.Task;

    public void Complete()
    {
        // TODO[N3]: Implement Complete() with Graceful Shutdown
        //
        // [YOUR CODE GOES HERE]
        //
        // Requirements:
        // 1. Set _decliningPermanently = true under lock
        // 2. Call PropagateMessages() to drain buffer
        //
        // See REF.md for detailed hints.
        
        throw new NotImplementedException("TODO[N3] - see README section 'TODO N3 – Implement Complete, Fault, and CheckCompletion'");
    }

    public void Fault(Exception exception)
    {
        // TODO[N3]: Implement Fault() with Exception Propagation
        //
        // [YOUR CODE GOES HERE]
        //
        // Requirements:
        // 1. Set _decliningPermanently = true under lock
        // 2. Call _completion.TrySetException(exception)
        // 3. Copy _targets to array under lock
        // 4. Call target.Fault(exception) on each target
        //
        // See REF.md for detailed hints.
        
        throw new NotImplementedException("TODO[N3] - see README section 'TODO N3 – Implement Complete, Fault, and CheckCompletion'");
    }

    // === Helper Methods ===
    
    private void PropagateMessages()
    {
        // TODO[N2]: Implement PropagateMessages for Priority Sending
        //
        // [YOUR CODE GOES HERE]
        //
        // Requirements:
        // 1. Use while(true) loop to process all messages
        // 2. Get highest priority message (_priorityBuffer.First()) under lock
        //    - Break if buffer empty
        // 3. Copy _targets to array under lock
        // 4. Loop through targets and offer message:
        //    - Create unique header with Interlocked.Increment(ref _nextMessageId)
        //    - Call target.OfferMessage(header, messageToSend.Value, this, consumeToAccept: false)
        //    - If Accepted: remove from buffer under lock, break inner loop
        // 5. If no target accepted (offered == false), break outer loop
        // 6. Call CheckCompletion() at end
        //
        // See REF.md for detailed hints.
        
        throw new NotImplementedException("TODO[N2] - see README section 'TODO N2 – Implement PropagateMessages for Priority Sending'");
    }

    private void CheckCompletion()
    {
        // TODO[N3]: Implement CheckCompletion() Helper
        //
        // [YOUR CODE GOES HERE]
        //
        // Requirements:
        // 1. Check if should complete: _decliningPermanently && _priorityBuffer.Count == 0
        // 2. If shouldComplete && !_completion.Task.IsCompleted:
        //    - Call _completion.TrySetResult()
        //    - Copy _targets to array under lock
        //    - Call target.Complete() on each target
        //
        // See REF.md for detailed hints.
        
        throw new NotImplementedException("TODO[N3] - see README section 'TODO N3 – Implement Complete, Fault, and CheckCompletion'");
    }

    private class Unlinker : IDisposable
    {
        private readonly Action _unlinkAction;
        public Unlinker(Action unlinkAction) => _unlinkAction = unlinkAction;
        public void Dispose() => _unlinkAction();
    }
}
