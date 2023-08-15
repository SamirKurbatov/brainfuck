using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace func.brainfuck
{
    public class Constant
    {
        static readonly char[] chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890".ToCharArray();

        public static void GetConstants(IVirtualMachine vm)
        {
            foreach (var ch in chars)
            {
                vm.RegisterCommand(ch, b =>
                {
                    b.Memory[b.MemoryPointer] = (byte)ch;
                });
            }
        }
    }

    public class BrainfuckBasicCommands
    {
        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand('.',
                b =>
                {
                    write((char)b.Memory[b.MemoryPointer]);
                });
            vm.RegisterCommand('+', b =>
            {
                b.Memory[b.MemoryPointer]++;
            });
            vm.RegisterCommand('-', b =>
            {
                b.Memory[b.MemoryPointer]--;
            });
            vm.RegisterCommand(',', b =>
            {
                var inputChar = (char)read();
                b.Memory[b.MemoryPointer] = (byte)inputChar;
            });
            vm.RegisterCommand('>', b =>
            {
                if (b.MemoryPointer >= b.Memory.Length - 1)
                    b.MemoryPointer = 0;
                else
                    b.MemoryPointer++;
            });
            vm.RegisterCommand('<', b =>
            {
                if (b.MemoryPointer <= 0)
                    b.MemoryPointer = b.Memory.Length - 1;
                else
                    b.MemoryPointer--;
            });

            Constant.GetConstants(vm);
        }
    }
}