using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        private static readonly Dictionary<int, int> Bracket = new();

        private static readonly Dictionary<int, int> ClosingBracket = new();

        private static readonly Stack<int> chars = new Stack<int>();

        public static void BodyLoop(IVirtualMachine vm)
        {
            for (int i = 0; i < vm.Instructions.Length; i++)
            {
                var bracket = vm.Instructions[i];
                switch (bracket)
                {
                    case '[':
                        chars.Push(i);
                        break;
                    case ']':
                        ClosingBracket[i] = chars.Peek();
                        Bracket[chars.Pop()] = i;
                        break;
                }
            }
        }

        public static void RegisterTo(IVirtualMachine vm)
        {
            BodyLoop(vm);
            vm.RegisterCommand('[',
                b =>
                {
                    if (b.Memory[b.MemoryPointer] == 0)
                    {
                        b.InstructionPointer = Bracket[b.InstructionPointer];
                    }
                });
            vm.RegisterCommand(']', b =>
            {
                if (b.Memory[b.MemoryPointer] != 0)
                {
                    b.InstructionPointer = ClosingBracket[b.InstructionPointer];
                }
            });
        }
    }
}