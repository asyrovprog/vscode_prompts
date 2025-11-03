using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Lab.Iter06
{
    public record Event(string Category, string Severity, string Message);

    public class EventRouter
    {
        private int _securityCount = 0;
        private int _performanceCount = 0;
        private int _generalCount = 0;
        private int _auditCount = 0;

        public int SecurityCount => _securityCount;
        public int PerformanceCount => _performanceCount;
        public int GeneralCount => _generalCount;
        public int AuditCount => _auditCount;

        // TODO[N1]: Implement Predicate-Based Routing Logic
        // [YOUR CODE GOES HERE]
        private Predicate<Event> SecurityCriticalPredicate => throw new NotImplementedException("TODO[N1]");

        private Predicate<Event> PerformancePredicate => throw new NotImplementedException("TODO[N1]");

        private Predicate<Event> GeneralPredicate => throw new NotImplementedException("TODO[N1]");

        private Predicate<Event> AuditPredicate => throw new NotImplementedException("TODO[N1]");

        private BufferBlock<Event>? _source;
        private ActionBlock<Event>? _securityHandler;
        private ActionBlock<Event>? _performanceHandler;
        private ActionBlock<Event>? _generalHandler;
        private ActionBlock<Event>? _auditLog;

        // TODO[N2]: Build Complete Pipeline with Audit Tap
        public void BuildPipeline()
        {
            // [YOUR CODE GOES HERE]
            throw new NotImplementedException("TODO[N2]");
        }

        public void PostEvent(Event evt)
        {
            if (_source == null)
                throw new InvalidOperationException("Pipeline not built. Call BuildPipeline() first.");
            
            _source.Post(evt);
        }

        public void CompleteSource()
        {
            _source?.Complete();
        }

        // TODO[N3]: Verify Pipeline Completion
        public async Task<bool> WaitForCompletion()
        {
            // [YOUR CODE GOES HERE]
            throw new NotImplementedException("TODO[N3]");
        }
    }
}
