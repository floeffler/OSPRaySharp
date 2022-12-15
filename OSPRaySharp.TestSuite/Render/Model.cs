using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OSPRay.TestSuite.Render
{
    internal abstract class Model
    {
        public const int ALL_STATES_BIT = -1;

        private int changes;
        private int suspendUpdate = 0;

        public Model()
        {
        }

        public bool UpdateSuspended => suspendUpdate > 0;

        public virtual void Refresh()
        {
            NotifyChangedAll();
        }

        public void SuspendUpdate()
        {
            Interlocked.Increment(ref suspendUpdate);
        }

        public void ResumeUpdate()
        {
            if (Interlocked.Decrement(ref suspendUpdate) < 0)
                throw new InvalidOperationException("Invalid Suspend/Resume call detected.");
        }

        /// <summary>
        /// Notify that the whole entity has be changed.
        /// </summary>
        protected void NotifyChangedAll()
        {
            NotifyChanged(ALL_STATES_BIT);
        }

        /// <summary>
        /// Notify that the model has been changed. The provided change bit identifies the change.
        /// </summary>
        /// <param name="changeBit">the change bit</param>
        protected void NotifyChanged(int stateBit)
        {
            Debug.Assert(stateBit != 0);

            // thread safe add state bit
            int currentChanges = changes;
            do
            {
                if ((currentChanges & stateBit) == stateBit)
                {   // bit already set, no change needed
                    break;
                }
            }
            while (currentChanges != (currentChanges = Interlocked.CompareExchange(ref changes, currentChanges | stateBit, currentChanges)));
        }

        internal virtual void Update(RenderContext renderContext)
        {
            if (UpdateSuspended)
                return;

            // check for model changes
            var stateChanges = Interlocked.Exchange(ref changes, 0);
            if (stateChanges == 0)
                return;

            // update
            UpdateCore(renderContext, stateChanges);
        }

        protected abstract void UpdateCore(RenderContext renderContext, int stateChanges);
    }
}
