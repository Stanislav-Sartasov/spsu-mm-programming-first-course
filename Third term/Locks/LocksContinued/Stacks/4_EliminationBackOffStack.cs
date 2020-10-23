using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocksContinued.Stacks
{
    public class EliminationBackoffStack<T> : LockFreeStack<T> {
        const int Capacity = ...;
        const int Timeout = ...;
        EliminationArray<T> eliminationArray = new EliminationArray<T>(Capacity);

        public override void Push(T value)
        {
            Node node = new Node(value);
            while (true)
            {
                if (TryPush(node))
                {
                    return;
                }
                else
                    try
                    {
                        T otherValue = eliminationArray.Visit(value, Timeout);
                        if (otherValue.Equals(default(T)))
                        {
                            // some timeout policy actions
                            return; // exchanged with pop
                        }
                    }
                    catch (TimeoutException ex)
                    {
                        // some timeout policy acions
                    }
            }
        }


        public override T Pop()
        {
            while (true)
            {
                Node returnNode = TryPop();
                if (returnNode != null)
                {
                    return returnNode.Value;
                }
                else try
                    {
                        T otherValue = eliminationArray.Visit(default(T), Timeout);
                        if (otherValue != null)
                        {
                            // some timeout policy actions
                            return otherValue;
                        }
                    }
                    catch (TimeoutException ex)
                    {
                        // some timeout policy actions
                    }
            }
        }
    }
}
